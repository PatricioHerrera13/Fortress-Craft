using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Crafting/Item")]
public class ItemSO : ScriptableObject {
    public string itemName; // Nombre del ítem
    public GameObject prefab; // Prefab del ítem
}
