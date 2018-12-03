using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class DroidManager : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The player. This is basically used to make sure the droid is facing and shooting the right way.")]
   private PlayerManager m_Player;

   [SerializeField]
   [Tooltip("The prefab to use for bullets.")]
   private PlayerBullet m_BulletPrefab;
   
   [SerializeField]
   [Tooltip("How fast bullets travel.")]
   private float m_BulletSpeed = 20;

   [SerializeField]
   [Tooltip("How much damage a single bullet from the player deals to an enemy.")]
   private int m_Damage = 1;
   #endregion

   #region Private Instance Variables
   private int m_BulletDir;

   private float m_OrigXPos;
   #endregion

   #region Cached Components
   private SpriteRenderer m_Renderer;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_Renderer = GetComponent<SpriteRenderer>();

      m_OrigXPos = Mathf.Abs(transform.localPosition.x);
   }
   #endregion

   #region Main Updates
   private void Update()
   {
      m_BulletDir = m_Player.FacingDirection;
      transform.localPosition = new Vector2(-m_OrigXPos * m_BulletDir, transform.localPosition.y);
      m_Renderer.flipX = (m_BulletDir < 0) ? true : false;
   }
   #endregion
  
   #region Fire Methods
   public void StartFiring(float fireRate)
   {
      StartCoroutine(Fire(fireRate));
   }

   private IEnumerator Fire(float fireRate)
   {
      // Shoots 10 bullets really fast

      int i = 0;
      while (i++ < 10)
      {
         PlayerBullet bullet = Instantiate(m_BulletPrefab, transform.position + 0.1f * Vector3.right * m_BulletDir, Quaternion.identity);
         if (m_BulletDir < 0)
            bullet.GetComponent<SpriteRenderer>().flipX = false;
         else
            bullet.GetComponent<SpriteRenderer>().flipX = true;
         bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * m_BulletSpeed * m_BulletDir;
         bullet.Damage = m_Damage;
            SFXManager.Instance.Play("droid_shoot");
         Destroy(bullet.gameObject, 5);

         yield return new WaitForSeconds(1 / fireRate);
      }
   }
   #endregion
}
