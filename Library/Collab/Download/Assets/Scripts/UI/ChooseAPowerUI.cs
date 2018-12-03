using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ChooseAPowerUI : MonoBehaviour
{
   #region Delegates and Events
   public static event Delegates.IntDelegate AbilityChosenEvent;
   #endregion

   #region Editor Variables
   [SerializeField]
   private GameObject[] backgrounds;

   [SerializeField]
   private Button[] m_Buttons;
   #endregion

   #region Private Instance Variables
   private int m_AbilityIndex;

    
    private GameObject currBack;

   private bool[] m_AbilitiesSelected;
    #endregion

    #region First Time Initialization and Setup
    private void Awake()
   {
      m_AbilityIndex = -1;

      m_AbilitiesSelected = new bool[3];
      for (int i = 0; i < 3; i++)
         m_AbilitiesSelected[i] = false;
   }
   #endregion

   #region Button Methods
   public void DashButton()
   {
      if (m_AbilitiesSelected[0])
         return;

      m_AbilityIndex = 0;
        updateBacks();
   }

   public void TimeSlowButton()
   {
      if (m_AbilitiesSelected[1])
         return;

      m_AbilityIndex = 1;
        updateBacks();
   }

   public void DroidButton()
   {
      if (m_AbilitiesSelected[2])
         return;

      m_AbilityIndex = 2;
        updateBacks();
   }

   public void ConfirmButton()
   {
      if (m_AbilityIndex < 0)
         return;

      m_AbilitiesSelected[m_AbilityIndex] = true;
      m_Buttons[m_AbilityIndex].interactable = false;

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
