using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerGraphics))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
   #region Delegates and Events
   public static event Delegates.EmptyDelegate PlayerDiedEvent;
   #endregion

   #region Editor Variables
   [SerializeField]
   [Tooltip("This is where all of the data about the player will be stored.")]
   private PlayerData m_Data;
   #endregion

   #region Private Instance Variables
   private bool m_IsAttacking = false;

   // When this is false, buttons/input should not work
   private bool m_CanPlay;

   // Which direction the player is facing (-1 for left, 0 for stationary, 1 for right) right now
   private int m_Direction;

   // Similar to above but cannot have a value of 0
   private int m_FacingDirection;

   // Keeping track of whether the player is moving or not
   private bool m_IsMoving;

   // Keeping track of whether the player is falling or not
   private bool m_IsFalling;

   // Keeping track of whether the player is falling or not
   private bool m_IsInAir;

   private float m_FiringCooldownTimer = 0;

   private bool m_IsUsingAbility;

   private float[] m_AbilityCooldownTimer;

   public static event Delegates.IntDelegate PlayerManagerUpdateHealthEvent;
   #endregion

   #region Cached Components
   private PlayerMotor m_MotorScript;
   private PlayerAttack m_AttackScript;
   private PlayerGraphics m_GraphicsScript;
    private CapsuleCollider2D m_Collider;
   private Rigidbody2D m_RigidBody;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_MotorScript = GetComponent<PlayerMotor>();
      m_AttackScript = GetComponent<PlayerAttack>();
      m_GraphicsScript = GetComponent<PlayerGraphics>();
        m_Collider = GetComponent<CapsuleCollider2D>();
      m_RigidBody = GetComponent<Rigidbody2D>();

      m_CanPlay = true;
      m_Direction = 0;
      m_FacingDirection = 1;
      m_IsMoving = false;
      m_IsFalling = false;
      m_IsInAir = false;
      m_IsUsingAbility = false;

      m_AbilityCooldownTimer = new float[m_Data.Abilities.Length];
      for (int i = 0; i < m_AbilityCooldownTimer.Length; i++)
         m_AbilityCooldownTimer[i] = 0;

      m_Data.CurrentHealth = m_Data.MaxHealth;
   }

   private void Start()
   {
      m_Collider.enabled = true;
      m_RigidBody.gravityScale = 20;

      m_MotorScript.Setup(m_Data.JumpPower, m_Data.TimeToTop);
      m_AttackScript.Setup(m_Data.BulletPrefab, m_Data.BulletSpawnLoc, m_Data.BulletSpeed, m_Data.BulletDamage);
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      Generator.GeneratorTriggeredEvent += TurnInputOff;
      GameManager.LevelStartEvent += TurnInputOn;
      ChooseAPowerUI.AbilityChosenEvent += RemoveAbility;
   }
   #endregion

   #region Main Updates
   private void Update()
   {
      if (!m_CanPlay)
         return;

      m_Direction = (int)Input.GetAxisRaw("Horizontal");
      m_FacingDirection = (m_Direction != 0) ? m_Direction : m_FacingDirection;

      UpdateMotor();
      UpdateGraphics();
      UpdateAttack();
      UpdateAbilities();
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      Generator.GeneratorTriggeredEvent -= TurnInputOff;
      GameManager.LevelStartEvent -= TurnInputOn;
      ChooseAPowerUI.AbilityChosenEvent -= RemoveAbility;
   }
   #endregion

   #region Accessors and Mutators
   public int FacingDirection
   {
      get { return m_FacingDirection; }
   }
   
   public bool IsFalling
   {
      get { return m_IsFalling; }
      set { m_IsFalling = value; }
   }

   public int Health
   {
      get { return m_Data.CurrentHealth; }
   }
   #endregion

   #region Collision Methods
   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.collider.CompareTag("Platform") && m_IsFalling)
      {
         m_IsFalling = false;
         m_IsInAir = false;
      }
   }
   #endregion

   #region Motor Methods
   private void UpdateMotor()
   {
      // MOVEMENT
      m_MotorScript.UpdateMove(
         m_Direction * m_Data.MoveSpeed);

      // JUMPING
      if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && !m_IsInAir)
      {
         m_MotorScript.StartJump(m_Data.JumpPower);
         m_IsInAir = true;
         m_Data.JumpPS.Play();
      }
   }
   #endregion

   #region Graphics Methods
   private void UpdateGraphics()
   {
      m_GraphicsScript.SetDirection(m_Direction, m_IsInAir);
   }
   #endregion

   #region Attack Methods
   private void UpdateAttack()
   {
      if (m_Direction != 0)
      {
         Vector2 loc = m_Data.BulletSpawnLoc.transform.localPosition;
         if (m_Direction < 0 && loc.x > 0)
            loc.x = -loc.x;
         else if (m_Direction > 0 && loc.x < 0)
            loc.x = -loc.x;

         m_AttackScript.BulletDir = m_Direction;

         m_Data.BulletSpawnLoc.transform.localPosition = loc;
      }

      if (Input.GetKeyDown(KeyCode.J) && m_FiringCooldownTimer == 0 && !m_IsAttacking)
      {
         m_IsAttacking = true;
         m_AttackScript.StartFiring(m_Data.FireRate);
      }
      else if (Input.GetKeyUp(KeyCode.J) && m_FiringCooldownTimer == 0 && m_IsAttacking)
      {
         m_IsAttacking = false;
         m_AttackScript.StopFiring();
         m_FiringCooldownTimer = 1 / (m_Data.FireRate);
      }
      else if (m_FiringCooldownTimer > 0)
         m_FiringCooldownTimer -= Time.deltaTime;
      else if (m_FiringCooldownTimer < 0)
         m_FiringCooldownTimer = 0;
   }
   #endregion

   #region Health Methods
   public void TakeDamage(int amt)
   {
      m_Data.CurrentHealth -= amt;
        SFXManager.Instance.Play("player_damage");
      if (PlayerManagerUpdateHealthEvent != null)
         PlayerManagerUpdateHealthEvent(m_Data.CurrentHealth);
      if (m_Data.CurrentHealth <= 0)
         Die();
   }

   private void Die()
   {
      TurnInputOff();

      m_RigidBody.gravityScale = 0;
      m_Collider.enabled = false;

      m_GraphicsScript.Die();
      if (PlayerDiedEvent != null)
         PlayerDiedEvent();
   }
   #endregion

   #region Ability Methods
   private void UpdateAbilities()
   {
      int ind = 0;

      if (Input.GetKeyDown(KeyCode.LeftShift) && m_AbilityCooldownTimer[ind] == 0)
      {
         m_Data.Abilities[ind].Ability.Use(m_FacingDirection);
         m_AbilityCooldownTimer[ind] = m_Data.Abilities[ind].CooldownTimer;
      }
      else if (m_AbilityCooldownTimer[ind] > 0)
         m_AbilityCooldownTimer[ind] -= Time.deltaTime;
      else if (m_AbilityCooldownTimer[ind] < 0 && m_AbilityCooldownTimer[ind] > -0.5f)
         m_AbilityCooldownTimer[ind] = 0;

      ind = 1;

      if (Input.GetKeyDown(KeyCode.K) && m_AbilityCooldownTimer[ind] == 0)
      {
         m_Data.Abilities[ind].Ability.Use(m_FacingDirection);
         m_AbilityCooldownTimer[ind] = m_Data.Abilities[ind].CooldownTimer;
      }
      else if (m_AbilityCooldownTimer[ind] > 0)
         m_AbilityCooldownTimer[ind] -= Time.deltaTime;
      else if (m_AbilityCooldownTimer[ind] < 0 && m_AbilityCooldownTimer[ind] > -0.5f)
         m_AbilityCooldownTimer[ind] = 0;

      ind = 2;

      if (Input.GetKeyDown(KeyCode.L) && m_AbilityCooldownTimer[ind] == 0)
      {
         m_Data.Abilities[ind].Ability.Use(m_FacingDirection);
         m_AbilityCooldownTimer[ind] = m_Data.Abilities[ind].CooldownTimer;
      }
      else if (m_AbilityCooldownTimer[ind] > 0)
         m_AbilityCooldownTimer[ind] -= Time.deltaTime;
      else if (m_AbilityCooldownTimer[ind] < 0 && m_AbilityCooldownTimer[ind] > -0.5f)
         m_AbilityCooldownTimer[ind] = 0;
   }

   private void RemoveAbility(int ind)
   {
      Destroy(m_Data.Abilities[ind].Ability);
      m_AbilityCooldownTimer[ind] = -1;
   }
   #endregion

   #region Input Control
   private void TurnInputOn()
   {
      m_CanPlay = true;
   }

   private void TurnInputOff()
   {
      m_CanPlay = false;
      m_Direction = 0;

      UpdateMotor();
      UpdateGraphics();

      m_AttackScript.StopFiring();
   }
   #endregion
}
