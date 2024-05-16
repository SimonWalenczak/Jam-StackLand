using System;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Arme,
    Lieu,
    Ressource
}

[CreateAssetMenu(fileName = "Data", menuName = "New Element", order = 1)]
public class ElementData : ScriptableObject
{
    public ElementType Type;
    
    [field: SerializeField] public List<ElementData> ElementsRequieres;

    public float TimeToCraft;
    
    public Sprite CardSprite;
    public int GoldValue;
    public int DefValue;
    
    [field: SerializeField] public List<ElementData> RepearElement;

}
