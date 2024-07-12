using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<item> items = new List<item>();
}
