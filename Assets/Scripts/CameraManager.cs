using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    Camera cam;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("LastLevel") && SceneManager.GetActiveScene().name != PlayerPrefs.GetString("LastLevel")) SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel"));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) cam.transform.position += new Vector3(0f, -Input.mouseScrollDelta.y, 0f);
    }
}
