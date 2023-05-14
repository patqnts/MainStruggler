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
    public bool _stackable = true;
    public bool stackable
    {
        get { return _stackable; }
        set
        {
            _stackable = value;
            if (_stackable)
            {
                maxStackCount = 64; // Set the default max stack count to 64
            }
            else
            {
                maxStackCount = 1; // Set the max stack count to 1 if not stackable
            }
        }
    }
    public int maxStackCount = 1; // Default max stack count is 1


    public bool consumable;
    public bool holdable;

    [TextAreaAttribute(15, 20)]
    public string description;
    public int priceAmount;
    public MaterialRequirement[] materialRequirements;

    [Header("Both")]
    public Sprite image;
}

[System.Serializable]
public class MaterialRequirement
{
    public Sprite materialSprite;
    public int requiredAmount;
    
}

public enum ItemType
{
    BuildingBlock,
    Tool,
    Weapon,
    Currency,
    Material,
    Fairy,
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
