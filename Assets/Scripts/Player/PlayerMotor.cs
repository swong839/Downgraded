using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
   #region Private Instance Variables
   private int m_MoveDirection;

   private float m_JumpPower;

   private float m_TimeToTop;

   private float m_JumpTime;

   private Vector2 m_Velocity;

   private bool m_CanMove;
   #endregion

   #region Cached Components
   private Rigidbody2D m_RigidBody;
   private PlayerManager m_ManagerScript;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_RigidBody = GetComponent<Rigidbody2D>();
      m_ManagerScript = GetComponent<PlayerManager>();

      m_MoveDirection = 1;
      m_JumpTime = 0;
      m_Velocity = Vector2.zero;
      m_CanMove = true;
   }

   public void Setup(float jumpPower, float timeToTop)
   {
      m_JumpPower = jumpPower;
      m_TimeToTop = timeToTop;
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      DashAbility.DashAbilityUseEvent += SetCanMoveFalse;
      DashAbility.DashAbilityDoneUseEvent += SetCanMoveTrue;
   }
   #endregion

   #region Main Updates
   private void Update()
   {
      if (!m_CanMove)
         return;

      if (m_JumpTime > 0)
         m_JumpTime -= Time.deltaTime / m_TimeToTop;
      else
      {
         m_ManagerScript.IsFalling = true;
         m_JumpTime = 0;
      }
   }

   private void FixedUpdate()
   {
      if (!m_CanMove)
         return;

      Move();
      Jump();
   }
   #endregion

   #region Game Loop Updates
   private void SetCanMoveFalse()
   {
      m_CanMove = false;
   }

   private void SetCanMoveTrue()
   {
      m_CanMove = true;
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      DashAbility.DashAbilityUseEvent -= SetCanMoveFalse;
      DashAbility.DashAbilityDoneUseEvent -= SetCanMoveTrue;
   }
   #endregion

   #region Movement Methods
   public void UpdateMove(float velocity)
   {
      m_Velocity.x = velocity;
   }

   private void Move()
   {
      m_RigidBody.MovePosition(
         m_RigidBody.position + m_Velocity * Time.fixedDeltaTime);
   }
   #endregion

   #region Jumping Methods
   public void StartJump(float jumpPower)
   {
      m_JumpTime = 1;
   }

   private void Jump()
   {
      m_Velocity.y = m_JumpPower * m_JumpTime * m_JumpTime;
   }
   #endregion
}
