/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir; // direction for bullet
    public float speed; // bullet speed
    public int damage; // bullet damage
    public string targetTag; // target opponent tag
    public float distanceTraveled = 0; // what distance bullet traveled

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime; // move bullet
        distanceTraveled += (dir * speed * Time.deltaTime).magnitude; // increment traveled distance

        if (distanceTraveled > 25) GameObject.Destroy(this.gameObject); // if bullet traveled more than 25 meters destroy it
    }

    // when collider enterd
    private void OnTriggerEnter(Collider other)
    {
        // check if what was hit was bullet destination
        if (other.CompareTag(targetTag))
        {
            ISelectable targetSelectable = other.GetComponent<ISelectable>(); // get target ISelectable component

            if (targetSelectable != null) // if target had ISelectable component
            {
                targetSelectable.HP = Mathf.Max(0, targetSelectable.HP - damage); // decrement target hp by bullet damage but no less than to 0
                GameObject.FindObjectOfType<PlayerShip>().OnRefreshGUI(); // run methods stored in delegate
            }
        }

        GameObject.Destroy(this.gameObject); // destroy bullet
    }

    // put bullet in motion
    public void Shoot(Vector3 _dir, float _speed, int _damage, string _targetTag)
    {
        dir = _dir; // set direction
        speed = _speed; // set speed
        damage = _damage; // set damage
        targetTag = _targetTag; // set opponent tag
    }
}
