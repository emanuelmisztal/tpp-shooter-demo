using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public int damage;
    public string targetTag;
    public float distanceTraveled = 0;

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        distanceTraveled += (dir * speed * Time.deltaTime).magnitude;

        if (distanceTraveled > 25) GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            ISelectable targetSelectable = other.GetComponent<ISelectable>();

            if (targetSelectable != null)
            {
                targetSelectable.HP = Mathf.Max(0, targetSelectable.HP - damage);
                GameObject.FindObjectOfType<PlayerShip>().OnRefreshGUI();
            }
        }

        GameObject.Destroy(this.gameObject);
    }

    public void Shoot(Vector3 _dir, float _speed, int _damage, string _targetTag)
    {
        dir = _dir;
        speed = _speed;
        damage = _damage;
        targetTag = _targetTag;
    }
}
