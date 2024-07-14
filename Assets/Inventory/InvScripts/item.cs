using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class item : ScriptableObject
{
    public string itemName;
    public Sprite item_icon;
    public int item_amount;

    [TextArea]
    public string item_description;

    public bool equiptable; // If the item is equiptables

    public bool isPickedUp; // If the item is picked up
}
