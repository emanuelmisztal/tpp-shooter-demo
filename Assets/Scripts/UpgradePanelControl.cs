using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelControl : MonoBehaviour
{
    public void Hide()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

    public void Show()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(true);
    }

    public void OnHealthButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<ISelectable>().HP += 150;
        Hide();
        Time.timeScale = 1;
    }

    public void OnDamageButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<ISelectable>().Damage += 25;
        Hide();
        Time.timeScale = 1;
    }

    public void OnAttackSpeedButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerShip>().BulletSpeed *= 1.25f;
        Hide();
        Time.timeScale = 1;
    }
}
