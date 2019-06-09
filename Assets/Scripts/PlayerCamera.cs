/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public short sensitivity = 20; // percentage of camera movement speed
    public short borderWidth = 5; // percentage of screen border, max 100

    private float screenWidth = Screen.width; // get screen width
    private float screenHeight = Screen.height; // get screen height
    private float negativeBorder; // multiplicator of negative screen border (left for x axis and bottom for y axis)
    private float positiveBorder; // multiplicator of positive screen border (right for x axis and top for y axis)

    private void Awake()
    {
        // check if camera is set properly
        if (borderWidth > 100 || borderWidth < 1) Debug.LogError("Wrong camera setting: border width (should be between 1 and 100)");
        else
        {
            negativeBorder = borderWidth * 0.01f; // calculate negative border for given parameters
            positiveBorder = 1 - borderWidth * 0.01f; // calculate positive border for given parameters
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float xMousePosition = Input.mousePosition.x; // get mouse x position
        float yMousePosition = Input.mousePosition.y; // get mouse y position

        if (xMousePosition < 0 || xMousePosition > screenWidth || yMousePosition < 0 || yMousePosition > screenHeight) return; // if pointer is outside window

        // check if mouse is on border
        if (xMousePosition < screenWidth * negativeBorder || xMousePosition > screenWidth * positiveBorder || yMousePosition < screenHeight * negativeBorder || yMousePosition > screenHeight * positiveBorder)
        {
            if (xMousePosition < screenWidth * negativeBorder) transform.position += -transform.right * sensitivity * 0.01f; // move left
            if (xMousePosition > screenWidth * positiveBorder) transform.position += transform.right * sensitivity * 0.01f; // move right
            if (yMousePosition < screenHeight * negativeBorder) transform.position += Quaternion.Euler(-transform.rotation.eulerAngles) * -transform.forward * sensitivity * 0.01f; // move backward
            if (yMousePosition > screenHeight * positiveBorder) transform.position += Quaternion.Euler(-transform.rotation.eulerAngles) * transform.forward * sensitivity * 0.01f; // move forward
        }
    }
}
