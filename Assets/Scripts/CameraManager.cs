/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    Camera cam; // store camera

    private void OnEnable()
    {
        // check if there is saved last level
        if (PlayerPrefs.HasKey("LastLevel") && SceneManager.GetActiveScene().name != PlayerPrefs.GetString("LastLevel")) SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel")); // load last level
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null) cam = Camera.main; // if cam has no camera attached, link main camera
    }

    // Update is called once per frame
    void Update()
    {
        // check if player scrolled on mouse
        if (Input.mouseScrollDelta.y != 0) cam.transform.position += new Vector3(0f, -Input.mouseScrollDelta.y, 0f); // change camera hight
    }
}
