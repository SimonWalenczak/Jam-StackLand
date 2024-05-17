using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Boosters", order = 1)]
public class BoosterData : ScriptableObject
{
    public List<Booster> Boosters;
}

[Serializable]
public class Booster
{
    public List<ElementData> ElementsInBooster;
}