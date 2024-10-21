using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour {
    
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private float interactDistance = 3f; // Distancia de interacción
    private CraftingAnvil currentCraftingAnvil;

    private void Update() {
        // Verifica si se presiona la tecla 'X'
        if (Input.GetKeyDown(KeyCode.X)) {
            // Busca el objeto CraftingAnvil más cercano
            FindNearestCraftingAnvil();
            if (currentCraftingAnvil != null) {
                currentCraftingAnvil.NextRecipe(); // Cambia la receta al presionar 'X'
            }
        }

        // Verifica si se presiona la tecla 'C' para intentar elaborar
        if (Input.GetKeyDown(KeyCode.C) && currentCraftingAnvil != null) {
            currentCraftingAnvil.Craft(); // Intenta elaborar el objeto
        }
    }

    private void FindNearestCraftingAnvil() {
        // Usa transform.position en lugar de playerCameraTransform.position
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactDistance, interactLayerMask);
        float closestDistance = float.MaxValue;

        // Busca el CraftingAnvil más cercano
        foreach (var collider in colliders) {
            if (collider.TryGetComponent(out CraftingAnvil craftingAnvil)) {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    currentCraftingAnvil = craftingAnvil; // Actualiza el CraftingAnvil actual
                }
            }
        }

        // Si no hay CraftingAnvil cercano, establece a null
        if (closestDistance == float.MaxValue) {
            currentCraftingAnvil = null;
        }
    }
}