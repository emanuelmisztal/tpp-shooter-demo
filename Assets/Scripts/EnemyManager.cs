/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); // link to all enemies

    private static EnemyManager instance; // link to this singleton instance

    // partial interface for enemies count
    public int GetEnemyCount() { return enemies.Count; } // return how many enemies are there

    // return singleton instance
    public static EnemyManager GetInstance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<EnemyManager>(); // if no singleton exists, search for one

            if (instance == null) // if no singleton exists, create new one
            {
                GameObject newGameObj = new GameObject("EnemyManager"); // create new GameObject named EnemyManager
                instance = newGameObj.AddComponent<EnemyManager>(); // add EnemyManager script component to new object
            }

            return instance; // return this singleton instance
        }
    }

    private void Awake()
    {
        instance = this; // link this singleton to instance variable
    }

    // add enemy to list
    public void AddEnemy(Enemy _addedEnemy)
    {
        if (!enemies.Contains(_addedEnemy)) enemies.Add(_addedEnemy); // if enemy wasn't in list
    }

    // remove enemy from list
    public void RemoveEnemy(Enemy _removeEnemy)
    {
        if (enemies.IndexOf(_removeEnemy) > -1) enemies.RemoveAt(enemies.IndexOf(_removeEnemy)); // if enemy is on a list, remove it
    }

    private void Update()
    {
        /*
        if (timer < 0.5f) timer += Time.deltaTime; // if timer is less than 5 sec, increment it by delta time
        else
        {
            for (int i = 0; i < enemies.Count; i++) enemies[i].UpdateTargetPosition(); // for each enemy update it's random position
            timer = 0f; // reset timer
        }
        */
    }
}
