/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    // for:
    int HP { get; set; } // storing health points
    string Name { get; set; } // storing name
    int Damage { get; set; } // for storing damage
    string GetDesc(); // to get description
    void OnRayCastHit(); // when raycast hit
    void OnRayCastMiss(); // when raycast missed
}
