using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "BoosterData", menuName = "New Booster Data", order = 1)]
public class BoosterData : ScriptableObject
{
    [field: SerializeField] public List<ElementData> CardsInBooster;

}
