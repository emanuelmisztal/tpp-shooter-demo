using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ISelectable
{
    public int healthPoints = 100; // HP
    public string enemyName = "Red Orb"; // name
    public int damage = 5; // damage
    public Path currentPath;
    public GameObject bulletPrefab;
    public LayerMask oponentMask;

    private int pathIndex;
    private Vector3 target;
    private float speed;
    private NavMeshAgent myAgent;
    private float timer;


    public int HP
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }

    public string Name
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("MovingObjects/Bullet");
        timer = 0;
    }

    private void OnEnable()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>();
        EnemyManager.GetInstance.AddEnemy(this); // add this enemy to enemy manager
        pathIndex = 0;
        if (myAgent != null && currentPath != null && currentPath.pathPoints.Length > 0) myAgent.SetDestination(currentPath.pathPoints[pathIndex].transform.position);
    }

    private void OnDisable()
    {
        EnemyManager.GetInstance.RemoveEnemy(this); // remove this enemy from enemy manager
    }

    private void Update()
    {
        if (healthPoints <= 0) GameObject.Destroy(this.gameObject);

        // moving on path
        if (myAgent != null && currentPath != null && myAgent.isOnNavMesh && myAgent.remainingDistance < 1)
        {
            if (pathIndex < currentPath.pathPoints.Length && currentPath.pathPoints[pathIndex] != null) myAgent.SetDestination(currentPath.pathPoints[pathIndex].transform.position);
            else pathIndex = 0;
            pathIndex++;
        }

        // shooting
        if (timer >= 0.5f && Physics.Raycast(new Ray(this.transform.position, (GameObject.FindWithTag("Player").transform.position - this.transform.position).normalized), 200, oponentMask))
        {
            GameObject newBullet = GameObject.Instantiate<GameObject>(bulletPrefab);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            Vector3 dir = (GameObject.FindWithTag("Player").transform.position - this.transform.position).normalized;
            newBullet.transform.position = this.transform.position + dir * 0.6f;
            bulletScript.Shoot(dir, 10f, 10, "Player");
            newBullet.SetActive(true);
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    public void UpdateTargetPosition()
    {
        target = GameObject.FindWithTag("Player").transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // get random target position
        speed = Vector3.Distance(transform.position, target) / 5f; // calculate speed
    }

    public string GetDesc()
    {
        return "Name: " + enemyName + "\nHP: " + healthPoints + "\nDMG: " + damage;
    }

    public void OnRayCastHit() { }

    public void OnRayCastMiss() { }
}
