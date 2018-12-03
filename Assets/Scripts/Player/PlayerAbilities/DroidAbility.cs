using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidAbility : Ability
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The droid game object.")]
   private DroidManager m_Droid;
   #endregion



   #region OnDisable and Other Enders
   private void OnDisable()
   {
      Destroy(m_Droid.gameObject);
   }
   #endregion

   #region Use Methods
   public override void Use(int dir)
   {
      m_Droid.StartFiring(10);
   }

   protected override void EndUse()
   {

   }
   #endregion
}
