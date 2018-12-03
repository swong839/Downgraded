using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauseAbility : Ability
{
   #region Delegates and Events
   public static event Delegates.EmptyDelegate TimePauseAbilityUseEvent;
   public static event Delegates.EmptyDelegate TimePauseAbilityDoneUseEvent;
   #endregion
   
   #region Editor Variables
   [SerializeField]
   [Tooltip("The effect to use when slowing time.")]
   private ParticleSystem m_SlowTimePS;
   #endregion

   #region Use Methods
   public override void Use(int dir)
   {
      if (TimePauseAbilityUseEvent != null)
         TimePauseAbilityUseEvent();

      GameManager.ENEMY_TIME_SCALE *= 0.5f;
        SFXManager.Instance.Play("slow_down");

      m_SlowTimePS.Play();

      Invoke("EndUse", 5);
   }

   protected override void EndUse()
   {
      if (TimePauseAbilityDoneUseEvent != null)
         TimePauseAbilityDoneUseEvent();

      GameManager.ENEMY_TIME_SCALE *= 2f;
   }
   #endregion
}
