using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    #region Editor Variables
    //Time between basic attacks
    [SerializeField]
    float attackSpeed;

    //Time between abilities
    [SerializeField]
    float abilitySpeed;

    //Projectile Prefab for the boss
    [SerializeField]
    GameObject projectilePrefab;

    //projectile speed
    [SerializeField]
    float projectileSpeed;

    //damage per projectile
    [SerializeField]
    int projectileDamage;

    //Starting HP
    [SerializeField]
    int maxHP;

    [SerializeField]
    GameObject storyController;
    #endregion


    #region non Editor Variables
    //Keeps track of time left between attacks
    float attackTimer;

    //Keeps track of time left between abilities
    float abilityTimer;

    //Keeps track of last ability used
    float lastAbilityIndex;

    //checks if currently attacking or not
    bool canAttack;

    //Tracks current HP
    int currHP;

    //if is dead
    bool isDead;
    #endregion

    #region Cached Components
    Transform shootingOrigin;
    Animator anim;
    #endregion

    #region Instance Components
    Transform player;
    #endregion


    private void Awake()
    {
        shootingOrigin = transform.GetChild(0).transform;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        canAttack = true;
        attackTimer = attackSpeed;
        abilityTimer = abilitySpeed;

        isDead = false;
        currHP = maxHP;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if (canAttack)
        {
         if (Mathf.Abs((transform.position - player.position).magnitude) > 30)
            return;
            if (abilityTimer <= 0)
            {
                chooseAbility();
            }
            if (attackTimer <= 0)
            {
                attack();

            }
        }
        attackTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;
    }

    private void chooseAbility() {
        int randIndex = Random.Range(0, 3);
        while (randIndex == lastAbilityIndex)
        {
            randIndex = Random.Range(0, 3);
        }
        Debug.Log("ABILITIES WOOOOO");
        switch (randIndex)
        {

            case 0:
                //StartCoroutine(sprayAbility());
                StartCoroutine(sprinklerAbility());

                break;
            case 1:
                StartCoroutine(sprinklerAbility());

                break;
            case 2:
                //StartCoroutine(lineAbility());
                StartCoroutine(sprinklerAbility());

                break;
            default:
                Debug.Log("randIndex was not 0, 1, or 2");
                break;
        }
        return;
    }

    private IEnumerator sprayAbility() {
        canAttack = false;
        anim.SetBool("UsingAbility", true);
        Vector3 direction;
        Quaternion rotation;
        GameObject projectile;
        for (int i = 0; i < 3; i++)
        {
            for (int x = 1; x < 4; x++)
            {
                //Top side shot
                rotation = shootingOrigin.rotation * Quaternion.Euler(new Vector3(0, 0, i * 5 + x * 15));
                direction = rotation * Vector3.left;
                projectile = Instantiate(projectilePrefab, shootingOrigin.position, shootingOrigin.rotation) as GameObject;
                projectile.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, projectileDamage);

                //bottom side shot
                rotation = Quaternion.Euler(new Vector3(0, 0, i * 5 + x * -15)) * shootingOrigin.rotation;
                direction = rotation * Vector3.left;
                projectile = Instantiate(projectilePrefab, shootingOrigin.position, shootingOrigin.rotation) as GameObject;
                projectile.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, projectileDamage);
            }
            yield return new WaitForSeconds(.5f);
        }
        canAttack = true;
        abilityTimer = abilitySpeed;
        anim.SetBool("UsingAbility", false);

    }

    private IEnumerator sprinklerAbility() {
        canAttack = false;
        anim.SetBool("UsingAbility", true);
        Vector3 direction;
        Quaternion rotation;
        GameObject projectile;
        Quaternion baseRotation = shootingOrigin.rotation * Quaternion.Euler(new Vector3(0, 0, -60f));
        for (int i = 0; i < 12; i++)
        {
            rotation = Quaternion.Euler(new Vector3(0, 0, i * 10)) * baseRotation;
            direction = rotation * Vector3.left;
            projectile = Instantiate(projectilePrefab, shootingOrigin.position, shootingOrigin.rotation) as GameObject;
            projectile.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, projectileDamage);
            yield return new WaitForSeconds(.4f);
        }

        baseRotation = shootingOrigin.rotation * Quaternion.Euler(new Vector3(0, 0, 60f));
        for (int i = 0; i < 12; i++)
        {
            rotation = Quaternion.Euler(new Vector3(0, 0, i * -10)) * baseRotation;
            direction = rotation * Vector3.left;
            projectile = Instantiate(projectilePrefab, shootingOrigin.position, shootingOrigin.rotation) as GameObject;
            projectile.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, projectileDamage);
            yield return new WaitForSeconds(.4f);
        }
        canAttack = true;
        abilityTimer = abilitySpeed;
        anim.SetBool("UsingAbility", false);
    }

    private IEnumerator lineAbility() {
        canAttack = false;
        anim.SetBool("UsingAbility", true);
        Vector3 origin;
        GameObject projectile;
        for (int i = 1; i < 10; i++)
        {
            //Shoot topside
            origin = new Vector3(shootingOrigin.position.x, shootingOrigin.position.y + i * 1.25f, 0);
            projectile = Instantiate(projectilePrefab, origin, shootingOrigin.rotation) as GameObject;
            projectile.GetComponent<RangedProjectile>().shoot(Vector3.left * projectileSpeed, projectileDamage);

            //shoot botside
            origin = new Vector3(shootingOrigin.position.x, shootingOrigin.position.y + i * -1.25f, 0);
            projectile = Instantiate(projectilePrefab, origin, shootingOrigin.rotation) as GameObject;
            projectile.GetComponent<RangedProjectile>().shoot(Vector3.left * projectileSpeed, projectileDamage);
            yield return new WaitForSecondsRealtime(.8f);
        }

        canAttack = true;
        abilityTimer = abilitySpeed;
        anim.SetBool("UsingAbility", false);

    }

    private void attack() {
        anim.SetTrigger("Attack");
        attackTimer = attackSpeed;
        //Calculate initial direction
        Vector3 direction = player.position - shootingOrigin.position;
        direction = direction.normalized;

        //Calculate 30 deg on either side of the initial vector
        Quaternion upRotation = Quaternion.Euler(new Vector3(0, 0, -15));
        Vector3 upDirection = upRotation * direction;

        Quaternion downRotation = Quaternion.Euler(new Vector3(0, 0, 15));
        Vector3 downDirection = downRotation * direction;

        //Spawn 3 projectiles with correct speeds
        GameObject projectile1 = Instantiate(projectilePrefab, shootingOrigin.position, shootingOrigin.rotation) as GameObject;
        projectile1.GetComponent<RangedProjectile>().shoot(direction.normalized * projectileSpeed, projectileDamage);

        GameObject projectile2 = Instantiate(projectilePrefab, shootingOrigin.position, Quaternion.LookRotation(upDirection)) as GameObject;
        projectile2.GetComponent<RangedProjectile>().shoot(upDirection.normalized * projectileSpeed, projectileDamage);

        GameObject projectile3 = Instantiate(projectilePrefab, shootingOrigin.position, Quaternion.LookRotation(downDirection)) as GameObject;
        projectile3.GetComponent<RangedProjectile>().shoot(downDirection.normalized * projectileSpeed, projectileDamage);


    }


    public void takeDamage(int dmg) {
        currHP -= dmg;
        if (currHP < 0 && !isDead)
        {
            die();
        }
    }

    private void die() {
        isDead = true;
        canAttack = false;
        Debug.Log("ded");
        anim.SetTrigger("Dead");
        storyController.GetComponent<StoryController>().startStory();
        Destroy(gameObject,1f);
    }
    
}
