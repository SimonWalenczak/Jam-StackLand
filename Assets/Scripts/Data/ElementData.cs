using System;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Arme,
    LopinDeTerre,
    Foret,
    Lieu,
    Ressource
}

[CreateAssetMenu(fileName = "Data", menuName = "New Element", order = 1)]
public class ElementData : ScriptableObject
{
    public ElementType Type;
    
    [field: SerializeField] public List<ElementsRequieresForCraft> ElementsRequieresForCraft;
    
    public Sprite CardSprite;
    public int GoldValue;
    public int DefValue;
    
    [field: SerializeField] public List<ElementData> RepearElement;

    public bool Destructable;
}

[Serializable]
public class ElementsRequieresForCraft
{
    public ElementData ElementToCraft;
    public List<ElementData> ElementsRequieres;
    public int TimeToCraft;
}