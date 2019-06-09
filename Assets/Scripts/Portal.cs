using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public short nextLevelNumber;

    private EnemyManager enemyGroup;

    // Start is called before the first frame update
    void Start()
    {
        enemyGroup = GameObject.FindObjectOfType<EnemyManager>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyGroup.GetEnemyCount() == 0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LastLevel", "Level " + nextLevelNumber);
            SceneManager.LoadScene("Level " + nextLevelNumber);
        }
    }
}
