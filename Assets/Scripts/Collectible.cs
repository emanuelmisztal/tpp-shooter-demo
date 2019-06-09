/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create new enum for collectible items
public enum CollectibleType
{
    NONE, // default value
    HEALTH_PACK, // item is health pack
    DAMAGE_PACK, // item is damage pack
    UPGRADE_POINT // item is upgrade point
}

public class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType; // store what type of collectible item is it

    private float rotationX = 0; // reset rotation counter

    // Update is called once per frame
    void Update()
    {
        if (rotationX > 360) rotationX = 0; // reset rotation counter to 0 because it made a full circle
        gameObject.transform.rotation.eulerAngles.Set(rotationX += 5, 0, 0); // rotate object and increment rotation counter by 5
    }

    // when something entered collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // if player entered collider
        {
            // check what kind of item is it
            switch (collectibleType)
            {
                // it's a health pack
                case CollectibleType.HEALTH_PACK:
                    other.gameObject.GetComponent<ISelectable>().HP += 150; // increment player hp by 150
                    GameObject.Destroy(this.gameObject); // destroy this item
                    break;

                // it's a damage pack
                case CollectibleType.DAMAGE_PACK:
                    other.gameObject.GetComponent<ISelectable>().Damage += 25; // increment hero damage by 25
                    GameObject.Destroy(this.gameObject); // destroy this item
                    break;

                // it's upgrade point
                case CollectibleType.UPGRADE_POINT:
                    GameObject.FindWithTag("UPpanel").GetComponent<UpgradePanelControl>().Show(); // show upgrade panel
                    GameObject.Destroy(this.gameObject); // destroy this item
                    Time.timeScale = 0; // stop game time
                    break;
            }
        }
    }
}
