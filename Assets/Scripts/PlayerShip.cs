/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro; // for Text Mesh Pro
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour, ISelectable // implements ISelectable so it can be selected
{
    public delegate void PlayerShipDelegate(); // create a void delegate method
    public PlayerShipDelegate OnRefreshGUI; // create a delegate to collect all methods that need refresh of GUI

    public short acceleration = 1; // acceleration of ship
    public TextMeshProUGUI pointsText; // link to text with points
    public TextMeshProUGUI timeText; // link to text with time
    public int healthPoints = 1000; // HP
    public string playersShipName = "mothership"; // name
    public int damage = 50; // damage
    public LayerMask enemyMask; // mask of all hostiles that can be hit
    public GameObject bulletPrefab; // link to bullet prefab

    private int points = 0; // player points, default 0
    private Rigidbody rb; // on begining get this object's rigidbody
    private float timer; // just a timer
    private bool isSelected; // is selected flag
    private NavMeshAgent navAgent; // NavMeshAgent component
    private ISelectable attackTarget; // link to target hero should attack
    private ISelectable userForcedTarget; // link to target hero should attack selected by player
    private float reloadTimer; // when next shot can be made
    private float bulletSpeed = 10f; // speed of a bullet <=> attack speed - makes shots more accurate
    private Animator myAnimator; // Animator component
    private Vector3 prevForwardDirection; // previous walk direction
    
    // health points interface
    public int HP
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }

    // hero name interface
    public string Name
    {
        get { return playersShipName; }
        set { playersShipName = value; }
    }

    // bullet damage interface
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // bullet speed (attack speed) interface
    public float BulletSpeed
    {
        get { return bulletSpeed; }
        set { bulletSpeed = value; }
    }

    private void Start()
    {
        if (OnRefreshGUI != null) InvokeRepeating("OnRefreshGUI", 0, 0.1f); // repeat methods stored in delegate every 1/10 of second
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // get rigidbody of this game object
        isSelected = false; // reset selected flag to false
        navAgent = gameObject.GetComponent<NavMeshAgent>(); // get NavMeshAgent component
        reloadTimer = 0; // reset reload timer
        bulletPrefab = Resources.Load<GameObject>("MovingObjects/Bullet"); // load bullet prefab
        myAnimator = gameObject.GetComponentInChildren<Animator>(); // get Animator component from child
    }

    private void OnEnable()
    {
        // if this is not the first level
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
            HP = PlayerPrefs.GetInt("PlayerHP"); // get stored player HP
            Damage = PlayerPrefs.GetInt("PlayerDMG"); // get stored player damage
            BulletSpeed = PlayerPrefs.GetFloat("PlayerAttackSpeed"); // get stored player bullet speed
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("PlayerHP", HP); // save player hp
        PlayerPrefs.SetInt("PlayerDMG", Damage); // save player damage
        PlayerPrefs.SetFloat("PlayerAttackSpeed", BulletSpeed); // save player bullet speed
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) navAgent.speed = 0; // if hero is out of hp stop him

        // check if there are any methods in delegate
        if (OnRefreshGUI != null)
        {
            // check if player hit K button
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnRefreshGUI(); // run delegate
            }
        }
        else OnRefreshGUI += AddPoint; // if delegate was empty add AddPoint method, just for fun

        // check if there are NavMesAgent and Animator components
        if (navAgent != null && myAnimator != null)
        {
            float angularSpeed = Vector3.SignedAngle(this.transform.forward, prevForwardDirection, Vector3.up); // angular speed of hero
            angularSpeed *= -1; // revert angular speed
            angularSpeed = Mathf.Clamp(angularSpeed, -1f, 1f); // clamp angular speed between -1 and 1

            myAnimator.SetFloat("Speed", Mathf.Clamp(navAgent.velocity.magnitude, 0f, 1f)); // send clamped between 0 and 1 speed to animator
            myAnimator.SetFloat("AngularSpeed", angularSpeed); // send angular speed to anmiator
            myAnimator.SetInteger("Health", HP); // send hp to animator
        }

        prevForwardDirection = this.transform.forward; // set previous (now current, later previous) movement direction

        timer += Time.deltaTime; // update timer
        timeText.text = "time: " + (int)timer; // update timer text

        if (userForcedTarget != null) attackTarget = userForcedTarget; // if player selected enemy for hero to deal with

        // check if hero is not engaging any hostiles
        if (attackTarget == null && userForcedTarget == null)
        {
            Collider[] targets = Physics.OverlapSphere(gameObject.transform.position, 15f, enemyMask); // get all hostiles colliders that are in heros range
            //targets = targets.Min(x => Mathf.Abs((x.transform.position - this.transform.position).magnitude));

            // check if there are any hostiles in range
            if (targets.Length > 0)
            {
                int minDistanceIndex = 0; // initialize minimum distance index variable for sorting
                float minDistance = (targets[0].transform.position - this.transform.position).magnitude; // get distance to first hostile in array

                for (int i = 1; i < targets.Length; i++) // and the world turns around (for every hostile in range)
                {
                    float currentEnemyDistance = (targets[i].transform.position - this.transform.position).magnitude; // get the distance to enemy

                    // check if it is closer to hero than current saved closest one
                    if (currentEnemyDistance < minDistance)
                    {
                        minDistanceIndex = i; // set him as closest
                        currentEnemyDistance = minDistance; // set closest distance
                    }
                }

                attackTarget = targets[UnityEngine.Random.Range(0, targets.Length)].GetComponent<ISelectable>(); // update target enemy
            }
        }

        // check if hero reloaded and there is a target marked
        if (reloadTimer > 0.05f && attackTarget != null && attackTarget.GetType() == typeof(Enemy))
        {
            Enemy targetEnemy = (Enemy)attackTarget; // link enemy

            // check if there is an enemy and he's in range
            if (targetEnemy != null && (targetEnemy.transform.position - this.transform.position).magnitude < 15f)
            {
                GameObject newBullet = GameObject.Instantiate<GameObject>(bulletPrefab); // make new bullet from prefab
                Bullet bulletScript = newBullet.GetComponent<Bullet>(); // load bullet script
                Vector3 dir = (targetEnemy.transform.position - this.transform.position).normalized; // set direction to shoot in
                newBullet.transform.position = this.transform.position + dir * 0.6f; // set bullet position
                bulletScript.Shoot(dir, bulletSpeed, damage, "enemy"); // send the bullet in set direction
                newBullet.SetActive(true); // activate bullet after everything is set
                reloadTimer = 0; // reset reload timer
            }
            
            // check if there targer enemy is still in range
            if(targetEnemy != null && (targetEnemy.transform.position - this.transform.position).magnitude >= 5f)
            {
                NavMeshAgent myAgent = GetComponent<NavMeshAgent>(); // get hero NavMeshAgent

                // check if there is NavMeshAgent component, and hero isn't moving much
                if (myAgent != null && myAgent.velocity.magnitude < 0.1f) movePlayer(targetEnemy.transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-3f, 3f))); // move player in enemy direction
            }
        }
        else reloadTimer += Time.deltaTime; // update reload timer
    }

    // update target
    public void UpdateAttackTarget()
    {
        // set to no target
        userForcedTarget = null;
        attackTarget = null;
    }

    // update target
    public void UpdateAttackTarget(ISelectable newTarget)
    {
        if (newTarget != attackTarget) attackTarget = newTarget; // update target as provided
    }

    // move player
    public void movePlayer(Vector3 hit) { navAgent.destination = hit; } // move player to provided point

    // increment player points
    public void AddPoint() { pointsText.text = "points: " + ++points; } // add one point for player

    // selecting on raycast hit
    public void OnRayCastHit() { isSelected = !isSelected; } // change isSelected flag to opposite

    // when raycast miss
    public void OnRayCastMiss() { isSelected = false; } // change isSelected flag to not selected (false)

    // interface
    public string GetDesc()
    {
        return "Name: " + playersShipName + "\nHP: " + healthPoints + "\nDMG: " + damage; // return hero description
    }

    /* 
    //for drawing gizmo in scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 15);
    }
    */
}
