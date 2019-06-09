using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    NONE,
    HEALTH_PACK,
    DAMAGE_PACK,
    UPGRADE_POINT
}

public class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType;    

    private float rotationX = 0;

    // Update is called once per frame
    void Update()
    {
        if (rotationX > 360) rotationX = 0;
        gameObject.transform.rotation.eulerAngles.Set(++rotationX, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (collectibleType)
            {
                case CollectibleType.HEALTH_PACK:
                    other.gameObject.GetComponent<ISelectable>().HP += 150;
                    GameObject.Destroy(this.gameObject);
                    break;

                case CollectibleType.DAMAGE_PACK:
                    other.gameObject.GetComponent<ISelectable>().Damage += 25;
                    GameObject.Destroy(this.gameObject);
                    break;

                case CollectibleType.UPGRADE_POINT:
                    GameObject.FindWithTag("UPpanel").GetComponent<UpgradePanelControl>().Show();
                    GameObject.Destroy(this.gameObject);
                    Time.timeScale = 0;
                    break;
            }
        }
    }
}
