using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{



    Vector3 direction;
    int damage;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 7f);
    }

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      TimePauseAbility.TimePauseAbilityUseEvent += HalveVelocity;
      TimePauseAbility.TimePauseAbilityDoneUseEvent += DoubleVelocity;
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      TimePauseAbility.TimePauseAbilityUseEvent -= HalveVelocity;
      TimePauseAbility.TimePauseAbilityDoneUseEvent -= DoubleVelocity;
   }
   #endregion

   #region Update Velocity Methods
   private void HalveVelocity()
   {
      rb.velocity *= 0.5f;
   }

   private void DoubleVelocity()
   {
      rb.velocity *= 2;
   }
   #endregion

   public void shoot(Vector3 dir, int dmg)
   {
        rb.velocity = dir ;
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.CompareTag("Player"))
        {
         
         collision.GetComponent<PlayerManager>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
