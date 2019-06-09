using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour, ISelectable
{
    public delegate void PlayerShipDelegate();
    public PlayerShipDelegate OnRefreshGUI;

    public short acceleration = 1; // acceleration of ship
    public TextMeshProUGUI pointsText; // link to text with points
    public TextMeshProUGUI timeText; // link to text with time
    public int healthPoints = 1000; // HP
    public string playersShipName = "mothership"; // name
    public int damage = 50; // damage
    public LayerMask enemyMask;
    public GameObject bulletPrefab;

    private int points = 0; // player points, default 0
    private Rigidbody rb; // on begining get this object's rigidbody
    private float timer; // just a timer
    private bool isSelected; // is selected flag
    private NavMeshAgent navAgent; //
    private ISelectable attackTarget; //
    private ISelectable userForcedTarget;
    private float reloadTimer;
    private float bulletSpeed;
    private Animator myAnimator;
    private Vector3 prevForwardDirection;
    

    public int HP
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }

    public string Name
    {
        get { return playersShipName; }
        set { playersShipName = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float BulletSpeed
    {
        get { return bulletSpeed; }
        set { bulletSpeed = value; }
    }

    private void Start()
    {
        if (OnRefreshGUI != null) InvokeRepeating("OnRefreshGUI", 0, 0.1f);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // get rigidbody of this game object
        isSelected = false;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        reloadTimer = 0;
        bulletPrefab = Resources.Load<GameObject>("MovingObjects/Bullet");
        myAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
            HP = PlayerPrefs.GetInt("PlayerHP");
            Damage = PlayerPrefs.GetInt("PlayerDMG");
            BulletSpeed = PlayerPrefs.GetFloat("PlayerAttackSpeed");
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("PlayerHP", HP);
        PlayerPrefs.SetInt("PlayerDMG", Damage);
        PlayerPrefs.SetFloat("PlayerAttackSpeed", BulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) navAgent.speed = 0;

        if (OnRefreshGUI != null)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnRefreshGUI();
            }
        }
        else OnRefreshGUI += AddPoint;

        if (navAgent != null && myAnimator != null)
        {
            float angularSpeed = Vector3.SignedAngle(this.transform.forward, prevForwardDirection, Vector3.up);
            angularSpeed *= -1;
            angularSpeed = Mathf.Clamp(angularSpeed, -1f, 1f);

            myAnimator.SetFloat("Speed", Mathf.Clamp(navAgent.velocity.magnitude, 0f, 1f));
            myAnimator.SetFloat("AngularSpeed", angularSpeed);
            myAnimator.SetInteger("Health", HP);
        }

        prevForwardDirection = this.transform.forward;

        timer += Time.deltaTime; // update timer
        timeText.text = "time: " + (int)timer; // update timer text

        if (userForcedTarget != null) attackTarget = userForcedTarget;

        if (attackTarget == null && userForcedTarget == null)
        {
            Collider[] targets = Physics.OverlapSphere(gameObject.transform.position, 15f, enemyMask);
            //targets = targets.Min(x => Mathf.Abs((x.transform.position - this.transform.position).magnitude));
            if (targets.Length > 0)
            {
                int minDistanceIndex = 0;
                float minDistance = (targets[0].transform.position - this.transform.position).magnitude;

                for (int i = 1; i < targets.Length; i++)
                {
                    float currentEnemyDIstance = (targets[i].transform.position - this.transform.position).magnitude;
                    if (currentEnemyDIstance < minDistance)
                    {
                        minDistanceIndex = i;
                        currentEnemyDIstance = minDistance;
                    }
                }

                attackTarget = targets[UnityEngine.Random.Range(0, targets.Length)].GetComponent<ISelectable>();
            }
        }

        if (reloadTimer > 0.05f && attackTarget != null && attackTarget.GetType() == typeof(Enemy))
        {
            Enemy targetEnemy = (Enemy)attackTarget;

            if (targetEnemy != null && (targetEnemy.transform.position - this.transform.position).magnitude < 15f)
            {
                GameObject newBullet = GameObject.Instantiate<GameObject>(bulletPrefab);
                Bullet bulletScript = newBullet.GetComponent<Bullet>();
                Vector3 dir = (targetEnemy.transform.position - this.transform.position).normalized;
                newBullet.transform.position = this.transform.position + dir * 0.6f;
                bulletScript.Shoot(dir, 10f, damage, "enemy");
                newBullet.SetActive(true);
                reloadTimer = 0;
            }

            if(targetEnemy != null && (targetEnemy.transform.position - this.transform.position).magnitude >= 5f)
            {
                NavMeshAgent myAgent = GetComponent<NavMeshAgent>();
                if (myAgent != null && myAgent.velocity.magnitude < 0.1f) movePlayer(targetEnemy.transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-3f, 3f)));
            }
        }
        else reloadTimer += Time.deltaTime;
    }

    public void UpdateAttackTarget()
    {

    }

    public void UpdateAttackTarget(ISelectable newTarget)
    {
        if (newTarget != attackTarget) attackTarget = newTarget;
    }

    // move player
    public void movePlayer(Vector3 hit) { navAgent.destination = hit; }

    // increment player points
    public void AddPoint() { pointsText.text = "points: " + ++points; }

    // selecting on raycast hit
    public void OnRayCastHit() { isSelected = !isSelected; }

    // when raycast miss
    public void OnRayCastMiss() { isSelected = false; }

    // interface
    public string GetDesc()
    {
        return "Name: " + playersShipName + "\nHP: " + healthPoints + "\nDMG: " + damage;
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 15);
    }
    */
}
