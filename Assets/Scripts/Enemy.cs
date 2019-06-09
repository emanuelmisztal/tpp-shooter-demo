/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ISelectable
{
    public int healthPoints = 100; // enemy health points
    public string enemyName = "Red Orb"; // enemy name
    public int damage = 5; // enemy damage
    public Path currentPath; // enemies current path to follow
    public GameObject bulletPrefab; // link to bullet prefab
    public LayerMask oponentMask; // mask with what to be shoot by enemy

    private int pathIndex; // which checkpoint was last (or will be next i dont remember)
    private Vector3 target; // target but i dont remember which
    //private float speed; // enemy speed
    private NavMeshAgent myAgent; // NavMeshAgent component
    private float timer; // reload timer

    // interface for health points
    public int HP
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }
    // interface for name
    public string Name
    {
        get { return enemyName; }
        set { enemyName = value; }
    }
    // interface for damage
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("MovingObjects/Bullet"); // load bullet prefab
        timer = 0; // reset reload timer
    }

    private void OnEnable()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>(); // get NavMeshAgent component
        EnemyManager.GetInstance.AddEnemy(this); // add this enemy to enemy manager
        pathIndex = 0; // reset checkpoint index to 0

        // check if there is NavMeshAgent component and there is a path linked for this enemy and path is longer than 0 checkpoints
        if (myAgent != null && currentPath != null && currentPath.pathPoints.Length > 0) myAgent.SetDestination(currentPath.pathPoints[pathIndex].transform.position); // send enemy to first checkpoint
    }

    private void OnDisable()
    {
        EnemyManager.GetInstance.RemoveEnemy(this); // remove this enemy from enemy manager
    }

    private void Update()
    {
        if (healthPoints <= 0) GameObject.Destroy(this.gameObject); // if enemy health is 0 or less self destruct

        // check if there is NavMeshAgent component and there is a path linked for this enemy and he's close to his next checkpoint
        if (myAgent != null && currentPath != null && myAgent.isOnNavMesh && myAgent.remainingDistance < 1)
        {
            // check where is enemy on a path
            if (pathIndex < currentPath.pathPoints.Length && currentPath.pathPoints[pathIndex] != null) myAgent.SetDestination(currentPath.pathPoints[pathIndex].transform.position); // send enemy to next checkpoint
            else pathIndex = -1; // reset checkpoint indexing (it will be incremented to 0 in next instruction)
            pathIndex++; // increment checkpoint index
        }

        // check if enemy reloaded and raycast hit oponent
        if (timer >= 0.5f && Physics.Raycast(new Ray(this.transform.position, (GameObject.FindWithTag("Player").transform.position - this.transform.position).normalized), 200, oponentMask))
        {
            GameObject newBullet = GameObject.Instantiate<GameObject>(bulletPrefab); // create new bullet
            Bullet bulletScript = newBullet.GetComponent<Bullet>(); // add bullet script to new bullet
            Vector3 dir = (GameObject.FindWithTag("Player").transform.position - this.transform.position).normalized; // set direction for bullet
            newBullet.transform.position = this.transform.position + dir * 0.6f; // place bullet in place for shoot
            bulletScript.Shoot(dir, 10f, damage, "Player"); // shoot bullet
            newBullet.SetActive(true); // activate bullet
            timer = 0; // reset reload timer
        }

        timer += Time.deltaTime; // increment reload timer
    }

    // set new target
    public void UpdateTargetPosition()
    {
        target = GameObject.FindWithTag("Player").transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // get random target position
        //speed = Vector3.Distance(transform.position, target) / 5f; // calculate speed
    }

    // implement interface for getting desription
    public string GetDesc()
    {
        return "Name: " + enemyName + "\nHP: " + healthPoints + "\nDMG: " + damage; // return enemy description
    }

    // when raycast hit
    public void OnRayCastHit() { }

    // when raycast missed
    public void OnRayCastMiss() { }
}
