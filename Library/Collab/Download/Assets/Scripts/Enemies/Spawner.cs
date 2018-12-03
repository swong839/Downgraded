using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Spawner : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The type of enemy to spawn.")]
   private GameObject m_EnemyType;

   [SerializeField]
   private float m_PaceDist;
   #endregion

   #region Private Instance Variables
   private GameObject m_Enemy;
   #endregion

   #region First Time Initialization
   private void Start()
   {
      m_Enemy = Instantiate(m_EnemyType, transform.position, Quaternion.identity);
      m_Enemy.GetComponent<BasicEnemy>().PaceDist = m_PaceDist;
      m_Enemy.GetComponent<BasicEnemy>().locateWayPoints();
   }
   #endregion

   #region OnEnable, Setups, and Resetters
   private void OnEnable()
   {
      GameManager.LevelStartEvent += RespawnEnemy;
   }
   #endregion

   #region OnDisable and Other Enders
   private void OnDisable()
   {
      GameManager.LevelStartEvent -= RespawnEnemy;
   }
   #endregion

   #region Respawning Enemy Methods
   private void RespawnEnemy()
   {
      m_Enemy.SetActive(false);

      m_Enemy.transform.position = transform.position;

      m_Enemy.SetActive(true);
   }
   #endregion
}
