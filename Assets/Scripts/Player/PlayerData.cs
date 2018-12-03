using UnityEngine;

[System.Serializable]
public class PlayerData
{
   #region Editor Variables
   [Header("Movement")]

   [SerializeField]
   [Tooltip("How fast the player can run around.")]
   private float m_MoveSpeed = 5;

   [Space(10)]

   [Header("Jetpack")]

   [SerializeField]
   [Tooltip("How strong the player pushes off the ground when they jump.")]
   private float m_JumpPower = 20;

   [SerializeField]
   [Tooltip("How long until you reach the top of your jump (in seconds).")]
   private float m_TimeToTop = 0.4f;

   [SerializeField]
   [Tooltip("The particle system to use for flying.")]
   private ParticleSystem m_JumpPS;

   [Space(10)]

   [Header("Gun")]

   [SerializeField]
   [Tooltip("The prefab to use for bullets.")]
   private PlayerBullet m_BulletPrefab;

   [SerializeField]
   [Tooltip("Where the bullet should spawn from.")]
   private Transform m_BulletSpawnLoc;

   [SerializeField]
   [Tooltip("How many bullets are shot per second.")]
   private float m_FireRate = 3;

   [SerializeField]
   [Tooltip("How fast bullets travel.")]
   private float m_BulletSpeed = 20;

   [SerializeField]
   [Tooltip("How much damage a single bullet from the player deals to an enemy.")]
   private int m_BulletDamage = 1;

   [Space(10)]

   [Header("Health")]

   [SerializeField]
   [Tooltip("How much damage the player can take before they die.")]
   private int m_MaxHealth;

   [Space(10)]

   [Header("Abilities")]

   [SerializeField]
   [Tooltip("An array of data for abilities that the player can use.")]
   private AbilityData[] m_Abilities;
   #endregion

   #region Private Instance Variables
   private int m_CurrentHealth;
   #endregion

   #region Accessors and Mutators
   public float MoveSpeed
   {
      get { return m_MoveSpeed; }
   }

   public float JumpPower
   {
      get { return m_JumpPower; }
   }

   public float TimeToTop
   {
      get { return m_TimeToTop; }
   }

   public ParticleSystem JumpPS
   {
      get { return m_JumpPS; }
   }

   public PlayerBullet BulletPrefab
   {
      get { return m_BulletPrefab; }
   }

   public Transform BulletSpawnLoc
   {
      set { m_BulletSpawnLoc = value; }
      get { return m_BulletSpawnLoc; }
   }

   public float FireRate
   {
      get { return m_FireRate; }
   }

   public float BulletSpeed
   {
      get { return m_BulletSpeed; }
   }

   public int BulletDamage
   {
      get { return m_BulletDamage; }
   }

   public int MaxHealth
   {
      get { return m_MaxHealth; }
   }

   public int CurrentHealth
   {
      get { return m_CurrentHealth; }
      set { m_CurrentHealth = value; }
   }

   public AbilityData[] Abilities
   {
      get { return m_Abilities; }
   }
   #endregion

   [System.Serializable]
   public struct AbilityData
   {
      #region Editor Variables
      [SerializeField]
      [Tooltip("The actual ability script.")]
      private Ability m_Ability;

      [SerializeField]
      [Tooltip("How long the cooldown for this particular ability is.")]
      private float m_CooldownTimer;
      #endregion

      #region Accessors and Mutators
      public Ability Ability
      {
         get { return m_Ability; }
      }

      public float CooldownTimer
      {
         get { return m_CooldownTimer; }
      }
      #endregion
   }
}
