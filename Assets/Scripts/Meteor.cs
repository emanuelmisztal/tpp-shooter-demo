/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // when colider eneterd
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // if colided with player
        {
            collision.gameObject.GetComponent<PlayerShip>().AddPoint(); // add point for player
            Destroy(gameObject); // destroy this game object
        }
    }

    // when being hit by raycast
    public void OnRayCastHit()
    {
        GameObject.FindObjectOfType<PlayerShip>().AddPoint(); // add point for player
        Destroy(gameObject); // destroy this game object
    }
}
