using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public enum TableType { BasicTable, WoodenDispenser, IronDispenser, CraftingTable }
    public TableType tableType;

    public void Interact(PlayerInteraction player, Item item)
    {
        Debug.Log("Interacción con la mesa: " + tableType);
        switch (tableType)
        {
            case TableType.WoodenDispenser:
                if (item == null) // Si el jugador no tiene ítem
                {
                    Item woodItem = CreateItem("Wood"); // Crear ítem de madera
                    player.GrabItem(woodItem);
                }
                break;

            case TableType.IronDispenser:
                if (item == null) // Si el jugador no tiene ítem
                {
                    Item ironItem = CreateItem("Iron"); // Crear ítem de hierro
                    player.GrabItem(ironItem);
                }
                break;

            case TableType.CraftingTable:
                if (item != null) // Si el jugador tiene un ítem
                {
                    if (item.name == "Wood" || item.name == "Iron") // Verificar el ítem
                    {
                        Item craftedItem = CraftItem(item); // Lógica de fabricación
                        player.DropItem(); // Suelta el ítem usado
                        player.GrabItem(craftedItem); // Toma el nuevo ítem
                    }
                }
                break;

            case TableType.BasicTable:
                if (item != null)
                {
                    DropItemOnTable(item); // Colocar el ítem en la mesa
                    player.DropItem(); // Suelta el ítem del jugador
                }
                break;
        }
    }
    [SerializeField] private GameObject woodPrefab; // Prefab para el ítem de madera
    [SerializeField] private GameObject ironPrefab; // Prefab para el ítem de hierro

    private Item CreateItem(string itemType)
    {
        GameObject newItemPrefab = itemType == "Wood" ? woodPrefab : ironPrefab;
        GameObject newItem = Instantiate(newItemPrefab, transform.position, Quaternion.identity);
        Item item = newItem.GetComponent<Item>();
        item.name = itemType; // Asegúrate de que el nombre sea el correcto
        return item;
    }

    private Item CraftItem(Item item)
    {
        // Lógica para combinar madera y hierro
        GameObject newItem = new GameObject("Mystical"); // Crear ítem especial
        Item craftedItem = newItem.AddComponent<Item>();
        return craftedItem; // Retornar el nuevo ítem
    }

    private void DropItemOnTable(Item item)
    {
        // Implementar la lógica para colocar el ítem sobre la mesa
        // Por ejemplo, ajustar la posición del ítem
        item.transform.position = transform.position + new Vector3(0, 1, 0); // Ajustar posición
    }

    public void StartHighlight()
    {
        // Lógica para resaltar la mesa
    }

    public void StopHighlight()
    {
        // Lógica para dejar de resaltar la mesa
    }
}

