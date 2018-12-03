using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
   #region Delegates and Events
   public static event Delegates.EmptyDelegate LevelStartEvent;
   #endregion

   #region Static Variables
   public static float ENEMY_TIME_SCALE = 1f;
   #endregion

   #region Editor Variables
   [SerializeField]
   [Tooltip("The checkpoints around the map.")]
   private Transform[] m_Checkpoints;

   [SerializeField]
   [Tooltip("The UI to show the player when the player wants to decide on a power to lose.")]
   private RectTransform m_ChooseAPowerUI;

   [SerializeField]
   [Tooltip("The panel to use to fade out then in.")]
   private RectTransform m_FadePanel;

   [SerializeField]
   [Tooltip("The actual player game object.")]
   private PlayerManager m_Player;

    [SerializeField]
    private GameObject m_cam;

    [SerializeField]
    private GameObject[] m_confiners;
   #endregion

   #region Private Instance Variables
   private int m_CurrentCheckpointInd;
   #endregion

   #region First Time Initialization and Setup
   private void Awake()
   {
      m_CurrentCheckpointInd = 0;
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      Generator.GeneratorTriggeredEvent += GeneratorReached;
      ChooseAPowerUI.AbilityChosenEvent += AbilityChosen;
      PlayerManager.PlayerDiedEvent += RestartFromPreviousCheckpoint;
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      Generator.GeneratorTriggeredEvent -= GeneratorReached;
      ChooseAPowerUI.AbilityChosenEvent -= AbilityChosen;
      PlayerManager.PlayerDiedEvent -= RestartFromPreviousCheckpoint;
   }
   #endregion

   #region Level End Reached Methods
   private void GeneratorReached()
   {
      // Stop player from being able to press buttons

      m_ChooseAPowerUI.gameObject.SetActive(true);
      
      // Allow player to press buttons

      m_CurrentCheckpointInd++;
   }

   private void AbilityChosen(int abilityIndex)
   {
      StartCoroutine(Fade());
   }
   #endregion

   #region Restart Level
   private void RestartFromPreviousCheckpoint()
    {
        StartCoroutine(Fade());
    }
   #endregion

   #region Misc
   private IEnumerator Fade()
   {
      yield return new WaitForSeconds(1.4f);

      m_FadePanel.gameObject.SetActive(true);

      Color transparentBlack = Color.black;
      transparentBlack.a = 0;

      float t = 0;
      while (t < 1)
      {
         t += 0.01f;
         m_FadePanel.GetComponent<Image>().color = Color.Lerp(transparentBlack, Color.black, t);
         yield return null;
      }

      m_Player.transform.position = m_Checkpoints[m_CurrentCheckpointInd].position;
        m_cam.GetComponent<CinemachineConfiner>().enabled = false;
        m_cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = m_confiners[m_CurrentCheckpointInd].GetComponent<Collider2D>();

        yield return new WaitForSeconds(0.75f);

      while (t > 0)
      {
         t -= 0.01f;
         m_FadePanel.GetComponent<Image>().color = Color.Lerp(transparentBlack, Color.black, t);
         yield return null;
        }
        //SetUpHealth(10);

        m_FadePanel.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        m_cam.GetComponent<CinemachineConfiner>().enabled = true;

        if (LevelStartEvent != null)
         LevelStartEvent();
   }
   #endregion
}
