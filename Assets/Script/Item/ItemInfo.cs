using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemValue
{
    Low,
    Mid,
    High        
}

[CreateAssetMenu(fileName = "ItemInfo.asset", menuName = "Item/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public int itemScore;
    public string itemName;
    public ItemValue value;
    public int price;
}
