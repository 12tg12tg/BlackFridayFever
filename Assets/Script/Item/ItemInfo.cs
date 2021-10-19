using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo.asset", menuName = "Item/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public int itemScore;
    public string itemName;
}
