using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The particle system to play when a bullet collides with an alien.")]
   private ParticleSystem m_AlienCollisionPS;
   #endregion

   #region Private Instance Variables
   private int m_Damage;
   #endregion

   #region Accessors and Mutators
   public int Damage
   {
      set { m_Damage = value; }
   }
   #endregion

   #region Collision Methods
   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.collider.CompareTag("Enemy"))
      {
         m_AlienCollisionPS.transform.parent = null;
         m_AlienCollisionPS.transform.localScale = Vector3.one;
         m_AlienCollisionPS.Play();
         Destroy(m_AlienCollisionPS, 0.5f);
         collision.collider.GetComponent<BasicEnemy>().takeDamage(m_Damage);
         Destroy(gameObject);
      }
        if (collision.transform.CompareTag("Platform"))
        {
            Destroy(m_AlienCollisionPS, 0.5f);
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Boss"))
        {
            Debug.Log("hit boss");
            m_AlienCollisionPS.transform.parent = null;
            m_AlienCollisionPS.transform.localScale = Vector3.one;
            m_AlienCollisionPS.Play();
            Destroy(m_AlienCollisionPS, 0.5f);
            collision.collider.GetComponent<BossScript>().takeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
   #endregion
}
