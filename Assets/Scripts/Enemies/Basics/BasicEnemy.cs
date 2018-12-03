using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    #region editorVars
    //total hp for an enemy
    [SerializeField]
    protected int maxHP = 2; 

    //enemy movespeed
    [SerializeField]
    protected float movespeed = 3;

    //enemy damage
    [SerializeField]
    protected int damage = 1;

    //range from player that enemy can attack
    [SerializeField]
    protected float range = 3;

    //Distance on either side of the initial spawn point that the enemy paces between
    [SerializeField]
    protected float paceDistance = 5f;
    #endregion

    #region nonEditorVars
    //current hp for enemy
    int currHP;

    //layermask to keep track of the floor
    LayerMask floorLayer;
    #endregion

    #region cachedComponents
    Rigidbody2D rb;
    protected Animator anim;
   protected Collider2D m_Collider;
    #endregion

    #region cachedInstances
    protected Transform player;
    Vector3[] wayPoints = new Vector3[2];
    int currDestinationIndex;
    Vector3 currDestination;
    #endregion


    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
      m_Collider = GetComponent<Collider2D>();

        currHP = maxHP;

        floorLayer = LayerMask.NameToLayer("Floor");

        locateWayPoints();
    }

   private void OnEnable()
   {
      player = null;
      currHP = maxHP;
   }

   virtual protected void Update()
    {
      if (currHP <= 0)
         return;

      rb.velocity = Vector2.zero;
        if (player == null)
        {
            pace();
            return;
        }

        if (inRange())
        {
            attack();

        }
        else
        {
            move();
        }
    }

    virtual protected void move()
    {
        Vector3 movement = new Vector3(player.position.x - transform.position.x, 0, 0).normalized;
        movement *= movespeed * GameManager.ENEMY_TIME_SCALE * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
      if (movement.x > 0)
         anim.SetBool("FaceRight", true);
      else
         anim.SetBool("FaceRight", false);

      if (movement.x != 0)
         anim.SetTrigger("Walk");
    }

    virtual protected void pace() 
    {
        if (Mathf.Abs( currDestination.x - transform.position.x) < .1f )  
        {
            currDestinationIndex = 1 - currDestinationIndex;
            currDestination = wayPoints[currDestinationIndex];
            Debug.Log(currDestinationIndex);
            Debug.Log(currDestination);
        }
        Vector3 movement = new Vector3(currDestination.x - transform.position.x, 0, 0).normalized;
        movement *= movespeed / 2 * GameManager.ENEMY_TIME_SCALE * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

      if (movement.x > 0)
         anim.SetBool("FaceRight", true);
      else
         anim.SetBool("FaceRight", false);

      if (movement.x != 0)
         anim.SetTrigger("Walk");
   }

    virtual protected bool inRange()
    {
        return (player.position - transform.position).magnitude < range;
    }

    virtual protected void locateWayPoints( ) {
        RaycastHit2D outHit;
        Vector3 direcVect;
        bool leftDone = false;
        bool rightDone = false;
        for (float i = 0; i <= paceDistance; i+= .5f)
        {
            Debug.Log(i);
            direcVect = new Vector3(i, 0, 0);
            if (!leftDone)
            {
                //Check Left Raycast
                outHit = Physics2D.Raycast(direcVect + transform.position, Vector2.down, 3f, 1 << floorLayer);
                //Debug.Log("HIT");
                if (outHit.transform != null)
                {
                    wayPoints[0] = outHit.point;
                }
                else
                {
                    leftDone = true;
                }
            }

            if (!rightDone)
            {
                //Check Right Raycast
                //Debug.Log("HITRIGHT");
                outHit = Physics2D.Raycast(transform.position - direcVect, Vector2.down, 3f, 1 << floorLayer);
                if (outHit.transform != null)
                {
                    wayPoints[1] = outHit.point;
                }
                else
                {
                    rightDone = true;
                }
            }
        }
        currDestination = wayPoints[Random.Range(0,2)];
    }

    virtual protected void attack()
    {
        Debug.Log("implemented individually");
    }

    virtual public void buff() 
    {
        Debug.Log("Implemented Individually");
    }

    virtual public void unbuff()
    {
        Debug.Log("Implemented Individually");
    }

    virtual public void takeDamage(int amt)
    {
        currHP -= amt;
        if (currHP <= 0)
        {
            die();
        }
    }

   virtual protected void die()
   {
      m_Collider.enabled = false;
      anim.SetTrigger("Die");
      Invoke("Hide", 1);
   }

   protected void Hide()
   {
      gameObject.SetActive(false);
   }

    #region get_set
    public Transform _player
    {
        get { return _player; }
        set { player = value; }
    }

   public float PaceDist
   {
      get { return paceDistance; }
      set { paceDistance = value; }
   }
    #endregion
}
