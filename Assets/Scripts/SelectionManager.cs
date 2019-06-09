/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{
    public LayerMask mask; // layer mask
    public LayerMask playerCommandMask; // movement layer mask
    public TextMeshProUGUI descriptionText; // link to text with description

    private GameObject selected; // store selected game object

    private void Awake()
    {
        selected = null; // clear selected on awake
    }

    private void OnEnable()
    {
        PlayerShip ps = GameObject.FindObjectOfType<PlayerShip>(); // get PlayerShip script component from player object

        if (ps != null) ps.OnRefreshGUI += RefreshUnitDesription; // add RefreshUnitDesription to PlayerShip OnRefreshGUI delegate
    }

    private void OnDisable()
    {
        PlayerShip ps = GameObject.FindObjectOfType<PlayerShip>(); // get PlayerShip script component from player object

        if (ps != null) ps.OnRefreshGUI -= RefreshUnitDesription; // remove RefreshUnitDesription from PlayerShip OnRefreshGUI delegate
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0.001) return; // if time is stoped don't update

        if (Input.GetMouseButtonDown(0)) // when left mouse button is clicked
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask)) // check if raycast hit something
            {
                if (selected != null && selected == hit.collider.gameObject) // if selected the same object
                {
                    selected.GetComponent<ISelectable>().OnRayCastMiss(); // tell selected that raycast missed
                    selected = null; // unselect if raycast hit the same game object as previously selected
                    //descriptionText.text = ""; // empty description text
                }
                else // if selected new object
                {
                    selected = hit.collider.gameObject; // link game object to selected
                    selected.GetComponent<ISelectable>().OnRayCastHit(); // tell selected that raycast hit
                    //descriptionText.text = selected.GetComponent<ISelectable>().GetDesc(); // put description in text field
                }
            }
            else if(selected != null) // if nthing was selected
            {
                selected.GetComponent<ISelectable>().OnRayCastMiss(); // tell selected that raycast missed
                selected = null; // unselect if raycast hit the same game object as previously selected
               //descriptionText.text = ""; // empty description text
            }
        }

        if (Input.GetMouseButtonDown(1) && selected != null && selected.CompareTag("Player")) // when right mouse button is clicked and something is selected
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, playerCommandMask)) // check if raycast hit something
            {
                if (hit.collider.CompareTag("enemy")) selected.GetComponent<PlayerShip>().UpdateAttackTarget(hit.collider.GetComponent<ISelectable>()); // tell hero to attack selected enemy
                else if (hit.collider.CompareTag("navigation")) // if nav mesh was hit
                {
                    selected.GetComponent<PlayerShip>().movePlayer(hit.point); // tell hero to move to target destination
                    selected.GetComponent<PlayerShip>().UpdateAttackTarget(); // update forced target to null
                }
            }
        }
    }

    // refresh what to show in description
    void RefreshUnitDesription()
    {
        if (selected != null) descriptionText.text = selected.GetComponent<ISelectable>().GetDesc(); // put description in text field
        else descriptionText.text = ""; // empty description field
    }
}