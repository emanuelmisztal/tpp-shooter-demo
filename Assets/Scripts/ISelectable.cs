using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    int HP { get; set; }
    string Name { get; set; }
    int Damage { get; set; }
    string GetDesc();
    void OnRayCastHit();
    void OnRayCastMiss();
}
