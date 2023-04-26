using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    public GameObject prefab; // Add the prefab property to the Item class

    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public float weaponDamage;
    

    [Header("Only UI")]
    public bool stackable = true;
    public bool consumable;
    public bool holdable;


    [Header("Both")]
    public Sprite image;
}   
public enum ItemType
{
    BuildingBlock,
    Tool,
    Weapon,
    Currency,
    Material,
    Food,
      
}
public enum ActionType
{
    Attack,
    Interact,
    Place
}

public enum Element
{
    None,
    Flame,
    Dark,
    Light,
    Wind,
    Water
}

