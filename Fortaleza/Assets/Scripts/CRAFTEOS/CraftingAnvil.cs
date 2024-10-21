using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingAnvil : MonoBehaviour {

    [SerializeField] private Image recipeImage;
    [SerializeField] private List<CraftingRecipeSO> craftingRecipeSOList;
    [SerializeField] private BoxCollider placeItemsAreaBoxCollider;
    [SerializeField] private Transform itemSpawnPoint;

    private CraftingRecipeSO craftingRecipeSO;

    private void Awake() {
        NextRecipe(); // Inicializa la receta al comenzar
    }

    public void NextRecipe() {
        // Cambia a la siguiente receta en la lista
        if (craftingRecipeSO == null) {
            craftingRecipeSO = craftingRecipeSOList[0]; // Primera receta
        } else {
            int index = craftingRecipeSOList.IndexOf(craftingRecipeSO);
            index = (index + 1) % craftingRecipeSOList.Count; // Cicla a la siguiente receta
            craftingRecipeSO = craftingRecipeSOList[index];
        }
        
        recipeImage.sprite = craftingRecipeSO.sprite; // Actualiza la imagen de la receta
    }

    public void Craft() {
        Debug.Log("Craft");
        // Verifica ítems en el área de elaboración
        Collider[] colliderArray = Physics.OverlapBox(
            transform.position + placeItemsAreaBoxCollider.center, 
            placeItemsAreaBoxCollider.size, 
            placeItemsAreaBoxCollider.transform.rotation);
        
        List<ItemSO> inputItemList = new List<ItemSO>(craftingRecipeSO.inputItemSOList);
        List<GameObject> consumeItemGameObjectList = new List<GameObject>();

        foreach (Collider collider in colliderArray) { 
            if (collider.TryGetComponent(out ItemSOHolder itemSOHolder)) {
                if (inputItemList.Contains(itemSOHolder.itemSO)) {
                    inputItemList.Remove(itemSOHolder.itemSO); // Remueve ítem de la lista
                    consumeItemGameObjectList.Add(collider.gameObject); // Añade a la lista de consumibles
                }
            }
        }

        // Si se tienen todos los ítems necesarios
        if (inputItemList.Count == 0) {
            Debug.Log("Yes");
            GameObject spawnedItemGameObject = 
                Instantiate(craftingRecipeSO.outputItemSO.prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            Transform spawnedItemTransform = spawnedItemGameObject.transform; // Obtener el Transform del GameObject
            
            foreach (GameObject consumeItemGameObject in consumeItemGameObjectList) {
                Destroy(consumeItemGameObject); // Destruye los ítems consumidos
            }
        }
    }
}
