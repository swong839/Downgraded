using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : BasicEnemy
{



   #region Editor Variables
   //time between enemy attacks
   [SerializeField]
   protected float attackSpeed = 1.5f;

   //enemy buffed movepeed
   [SerializeField]
   protected float buffAttackSpeed = 2f;
   #endregion

   #region Non-editor variables
   //float to hold base movespeed
   float baseAttackSpeed;

   //float to keep track of attackspeed timing
   protected float attackTimer;
   #endregion

   #region Projectile Components
   [SerializeField]
   GameObject projectilePrefab;

   [SerializeField]
   float projectileSpeed;
   #endregion

   override protected void Awake()
   {
      base.Awake();
      baseAttackSpeed = attackSpeed;

   }

   override public void buff()
   {
      //Debug.Log("RANGED POWERED UP!");
      attackSpeed = buffAttackSpeed;
   }

   override public void unbuff()
   {
      //Debug.Log("RANGED POWERED DOWN!");
      attackSpeed = baseAttackSpeed;
   }

   protected override void attack()
   {
      if (attackTimer < attackSpeed)
      {
         attackTimer += Time.deltaTime * GameManager.ENEMY_TIME_SCALE;
         return;
      }
      float xComponent = player.position.x - transform.position.x;
      Vector3 direction = new Vector3(xComponent, 0f, 0f);

      if (xComponent > 0)
         anim.SetBool("FaceRight", true);
      else
         anim.SetBool("FaceRight", false);

      anim.SetTrigger("Attack");

      attackTimer = 0;

      StartCoroutine(FireAttack(direction));
   }


   private IEnumerator FireAttack(Vector3 direction)
   {
      yield return new WaitForSeconds(0.5f);

      GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
      projectile.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, damage);

   }
}
