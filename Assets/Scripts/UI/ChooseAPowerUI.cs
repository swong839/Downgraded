using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ChooseAPowerUI : MonoBehaviour
{
   #region Delegates and Events
   public static event Delegates.IntDelegate AbilityChosenEvent;
   #endregion

   #region Private Instance Variables
   private int m_AbilityIndex;

    [SerializeField]
    private GameObject[] backgrounds;

    private GameObject currBack;
    #endregion

    #region First Time Initialization and Setup
    private void Awake()
   {
      m_AbilityIndex = -1;
   }
   #endregion

   #region Button Methods
   public void DashButton()
   {
      m_AbilityIndex = 0;
        updateBacks();
   }

   public void TimeSlowButton()
   {
      m_AbilityIndex = 1;
        updateBacks();
   }

   public void DroidButton()
   {
      m_AbilityIndex = 2;
        updateBacks();
   }

   public void ConfirmButton()
   {
      if (m_AbilityIndex < 0)
         return;

      if (AbilityChosenEvent != null)
         AbilityChosenEvent(m_AbilityIndex);
      gameObject.SetActive(false);
   }

    private void updateBacks()
    {
        if (currBack != null)
        {
            currBack.SetActive(false);
        }
        currBack = backgrounds[m_AbilityIndex];
        currBack.SetActive(true);
    }
   #endregion
}
