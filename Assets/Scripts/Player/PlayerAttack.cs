using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAttack : MonoBehaviour
{
   #region Private Instance Variables
   private PlayerBullet m_BulletPrefab;

   private Transform m_BulletSpawnLoc;

   private float m_BulletSpeed;

   private int m_BulletDir;

   private int m_Damage;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_BulletDir = 1;
   }

   public void Setup(PlayerBullet bulletPrefab, Transform bulletSpawnLoc,
      float bulletSpeed, int damage)
   {
      m_BulletPrefab = bulletPrefab;
      m_BulletSpawnLoc = bulletSpawnLoc;
      m_BulletSpeed = bulletSpeed;
      m_Damage = damage;
   }
   #endregion
   
   #region Accessors and Mutators
   public int BulletDir
   {
      set { m_BulletDir = value; }
   }
   #endregion

   #region Fire Methods
   public void StartFiring(float fireRate)
   {
      StartCoroutine(Fire(fireRate));
   }

   private IEnumerator Fire(float fireRate)
   {
      while (true)
      {
         PlayerBullet bullet = Instantiate(m_BulletPrefab, m_BulletSpawnLoc.position + 0.1f * Vector3.right * m_BulletDir, Quaternion.identity);
            SFXManager.Instance.Play("shoot");
         if (m_BulletDir < 0)
            bullet.GetComponent<SpriteRenderer>().flipX = false;
         else
            bullet.GetComponent<SpriteRenderer>().flipX = true;
         bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * m_BulletSpeed * m_BulletDir;
         bullet.Damage = m_Damage;
         Destroy(bullet.gameObject, 5);

         yield return new WaitForSeconds(1 / fireRate);
      }
   }

   public void StopFiring()
   {
      StopAllCoroutines();
   }
   #endregion
}
