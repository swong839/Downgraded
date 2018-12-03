using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : BasicEnemy {

    //enemy buffed movepeed
    [SerializeField]
    protected float buffMoveSpeed = 5f;

    //float to hold base movespeed
    float baseMoveSpeed;

   [SerializeField]
   private ParticleSystem m_DeathExplosion;

    override protected void Awake()
    {
        base.Awake();
        baseMoveSpeed = movespeed;

    }

    override public void buff()
    {
        Debug.Log("BOOMER POWERED UP!");
        movespeed = buffMoveSpeed;
    }

    override public void unbuff()
    {
        Debug.Log("BOOMER POWERED DOWN!");
        movespeed = baseMoveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
         collision.collider.GetComponent<PlayerManager>().TakeDamage(damage);
         m_DeathExplosion.Play();
            SFXManager.Instance.Play("enemy_explode");
         m_DeathExplosion.transform.parent = null;
         die();
      }
    }

   protected override void die()
   {
      m_DeathExplosion.Play();
      m_DeathExplosion.transform.parent = null;
      Hide();
   }
}
