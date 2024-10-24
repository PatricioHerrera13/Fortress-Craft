using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider; // Collider del jugador
    public Collider jugadorCollider1; // Collider adicional del jugador
    public List<OrderPrefabData> itemsRequeridos; // Lista de datos de pedidos requeridos
    public float cantEntrega = 0; // Contador de entregas

    private bool jugadorDentro = false; // Variable que indica si el jugador está dentro del área

    private void OnTriggerStay(Collider other)
    {
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            jugadorDentro = true; // El jugador está dentro del área
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            jugadorDentro = false; // El jugador ha salido del área
        }
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("El jugador presionó E dentro del área.");
            Collider playerCollider = jugadorCollider != null ? jugadorCollider : jugadorCollider1;

            if (playerCollider != null)
            {
                PickUpItem playerPickUp = playerCollider.GetComponentInChildren<PickUpItem>();

                if (playerPickUp != null && playerPickUp.GetPickedPrefab() != null)
                {
                    GameObject prefabJugador = playerPickUp.GetPickedPrefab();
                    OrderPrefabData orderData = FindOrderData(prefabJugador); // Buscar el pedido relacionado

                    if (orderData != null &&
                        orderManager.EliminarPedido(orderData.orderSprite))
                    {
                        // Eliminar el prefab de las manos del jugador
                        EliminarItemDeLasManos(playerCollider);
                        // Sumar un punto a las entregas
                        cantEntrega += 1;
                        Debug.Log("¡Pedido entregado! Punto sumado.");
                    }
                    else
                    {
                        Debug.Log("Entrega errónea. No se ha sumado nada.");
                        EliminarItemDeLasManos(playerCollider); // De todas formas elimina el ítem
                    }
                }
                else
                {
                    Debug.Log("El jugador no sostiene ningún ítem.");
                }
            }
        }
    }

    private void EliminarItemDeLasManos(Collider other)
    {
        Transform hand = other.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Destroy(hand.GetChild(0).gameObject);
            Debug.Log("El objeto ha sido eliminado del jugador.");
        }
    }

    public void ActualizarItemsRequeridos()
    {
        itemsRequeridos.Clear(); // Limpiar la lista antes de actualizar
        var activeOrders = orderManager.GetActiveOrders(); // Obtener las órdenes activas

        Debug.Log("Total de órdenes activas: " + activeOrders.Count); // Verificar cuántas órdenes activas hay

        foreach (OrderPrefabData pedido in activeOrders)
        {
            itemsRequeridos.Add(pedido); // Agregar el pedido completo (datos del sprite, prefab, etc.)
            Debug.Log("Item requerido agregado: " + pedido.orderPrefab.name); // Depuración
        }

        Debug.Log("Items requeridos actualizados.");
    }

    private OrderPrefabData FindOrderData(GameObject prefab)
    {
        // Obtener el SpriteRenderer del prefab entregado
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("El prefab entregado no tiene un SpriteRenderer.");
            return null; // Retorna null si no hay un SpriteRenderer
        }

        Sprite objetoSprite = spriteRenderer.sprite; // Obtener el sprite del objeto del prefab

        // Buscar en los items requeridos si el sprite del objeto corresponde a uno de los pedidos
        foreach (OrderPrefabData order in itemsRequeridos)
        {
            // Compara con el sprite del objeto en el prefab
            if (order.objectSprite == objetoSprite)
            {
                return order; // Retorna el pedido si se encuentra una coincidencia
            }
        }
        return null; // Si no se encuentra, retorna null
    }
}
