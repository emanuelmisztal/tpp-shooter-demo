/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public short nextLevelNumber; // number of next level

    private EnemyManager enemyGroup; // link to group of all enemies (EnemyManager)

    // Start is called before the first frame update
    void Start()
    {
        enemyGroup = GameObject.FindObjectOfType<EnemyManager>(); // get EnemyManager
        gameObject.GetComponent<MeshRenderer>().enabled = false; // hide mesh
        gameObject.GetComponent<SphereCollider>().enabled = false; // turn off collider
    }

    // Update is called once per frame
    void Update()
    {
        // check if there is no more enemies on field
        if (enemyGroup.GetEnemyCount() == 0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true; // show mesh
            gameObject.GetComponent<SphereCollider>().enabled = true; // enable collider
        }
    }

    // on collision
    private void OnTriggerEnter(Collider other)
    {
        // check if player hit collider
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LastLevel", "Level " + nextLevelNumber); // update level progress in PlayerPrefs
            SceneManager.LoadScene("Level " + nextLevelNumber); // load next level
        }
    }
}
