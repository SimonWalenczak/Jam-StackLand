using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Attacks", order = 1)]
public class AttackData : ScriptableObject
{
    public List<int> Attacks;
}
