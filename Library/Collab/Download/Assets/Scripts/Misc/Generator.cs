using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class Generator : MonoBehaviour
{
   #region Events and Delegates
   public static event Delegates.EmptyDelegate GeneratorTriggeredEvent;
   #endregion

   #region Privat Instance Variables
   private bool m_WasTouchedByPlayer;
   #endregion

   #region Cached Components
   private Animator m_Animator;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_Animator = GetComponent<Animator>();

      m_WasTouchedByPlayer = false;
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      ChooseAPowerUI.AbilityChosenEvent += FillUpGenerator;
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      ChooseAPowerUI.AbilityChosenEvent -= FillUpGenerator;
   }
   #endregion

   #region Collision Methods
   private void OnTriggerEnter2D(Collider2D collider)
   {
      if (collider.CompareTag("Player") && GeneratorTriggeredEvent != null)
      {
         m_WasTouchedByPlayer = true;
         GeneratorTriggeredEvent();
      }
   }
   #endregion

   #region Animator Methods
   private void FillUpGenerator(int num)
   {
      if (m_WasTouchedByPlayer)
         Invoke("FillUp", 0.1f);
   }

   private void FillUp()
   {
      m_Animator.SetTrigger("FillUp");
   }
   #endregion
}
