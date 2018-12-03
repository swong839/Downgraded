using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class DashAbility : Ability
{
   #region Delegates and Events
   public static event Delegates.EmptyDelegate DashAbilityUseEvent;
   public static event Delegates.EmptyDelegate DashAbilityDoneUseEvent;
   #endregion

   #region Editor Variables
   [SerializeField]
   [Tooltip("The effect to use when boosting.")]
   private ParticleSystem m_DashPS;
   #endregion

   #region Private Instance Variables
   private float m_OrigDashPSX;
   #endregion

   #region Cached Components
   private Rigidbody2D m_RigidBody;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_RigidBody = GetComponent<Rigidbody2D>();
   }

   private void Start()
   {
      m_OrigDashPSX = m_DashPS.transform.localPosition.x;
   }
   #endregion

   #region Use Methods
   public override void Use(int dir)
   {
      if (DashAbilityUseEvent != null)
         DashAbilityUseEvent();
      m_RigidBody.gravityScale = 0;
      Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
      Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"), true);
      m_RigidBody.velocity = dir * Vector2.right * 10;

      m_DashPS.transform.localPosition = new Vector2(m_OrigDashPSX * dir, m_DashPS.transform.localPosition.y);
      m_DashPS.transform.localEulerAngles = new Vector3(0, 0, (dir > 0) ? 60 : 240);
      m_DashPS.Play();
        SFXManager.Instance.Play("dash");

      Invoke("EndUse", 0.2f);
   }

   protected override void EndUse()
   {
      if (DashAbilityDoneUseEvent != null)
         DashAbilityDoneUseEvent();
      m_RigidBody.gravityScale = 20;
      Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
      Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"), false);
   }
   #endregion
}
