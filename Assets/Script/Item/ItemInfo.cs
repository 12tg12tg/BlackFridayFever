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
    public ItemValue value;
    public PoolTag poolTag;
    public int price;
}
