using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerGraphics : MonoBehaviour
{
   #region Private Instance Variables
   private int m_LastDir;
   #endregion

   #region Cached Components
   private Animator m_Animator;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_Animator = GetComponent<Animator>();
      m_LastDir = 1;
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   public void ResetToIdle()
   {
      m_Animator.SetTrigger("Reset");
   }
   #endregion

   #region Direction Methods
   public void SetDirection(int dir, bool isInAir)
   {
      if (isInAir)
         m_Animator.SetFloat("RunSpeed", 0);
      else
         m_Animator.SetFloat("RunSpeed", Mathf.Abs(dir));

      m_Animator.SetBool("IsInAir", isInAir);

      if (dir != 0 && dir != m_LastDir)
      {
         m_LastDir = dir;
         m_Animator.SetBool("FaceRight", (dir == 1) ? true : false);
      }
   }
   #endregion

   #region Death Animation Methods
   public void Die()
   {
      m_Animator.SetTrigger("Die");
   }
   #endregion
}
