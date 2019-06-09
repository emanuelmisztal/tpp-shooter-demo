/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelControl : MonoBehaviour
{
    // hide upgrade panel
    public void Hide()
    {
        // activate all upgrade buttons
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

    // show upgrade panel
    public void Show()
    {
        // deactivate all upgrade buttons
        foreach (Transform child in transform) child.gameObject.SetActive(true);
    }

    // when hp upgrade button is clicked
    public void OnHealthButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<ISelectable>().HP += 150; // increment player hp by 150
        Hide(); // hide panel
        Time.timeScale = 1; // set time scale back to normal
    }

    // when damage upgrade button is clicked
    public void OnDamageButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<ISelectable>().Damage += 25; // increment player damage by 25
        Hide(); // hide panel
        Time.timeScale = 1; // set time scale back to normal
    }

    // when attack speed upgrade button is clicked
    public void OnAttackSpeedButtonClick()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerShip>().BulletSpeed *= 1.25f; // increment player attack speed by 25%
        Hide(); // hide panel
        Time.timeScale = 1; // set time scale back to normal
    }
}
