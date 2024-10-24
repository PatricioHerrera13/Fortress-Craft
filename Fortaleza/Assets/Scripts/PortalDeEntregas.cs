using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider; // Collider del jugador
    public Collider jugadorCollider1; // Collider adicional del jugador
    public List<GameObject> itemsRequeridos; // Lista de prefabs requeridos
    public float cantEntrega = 0; // Contador de entregas

    private void OnTriggerStay(Collider other)
    {
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("El jugador presionó E.");
                PickUpItem playerPickUp = other.GetComponentInChildren<PickUpItem>();

                if (playerPickUp != null && playerPickUp.GetPickedPrefab() != null)
                {
                    GameObject prefabJugador = playerPickUp.GetPickedPrefab();

                    // Buscar si hay un pedido que coincida con el prefab del jugador
                    OrderPrefabData pedidoCorrespondiente = BuscarPedidoPorPrefab(prefabJugador);

                    if (pedidoCorrespondiente != null && orderManager.EliminarPedido(pedidoCorrespondiente.orderSprite))
                    {
                        // Eliminar el prefab de las manos del jugador
                        EliminarItemDeLasManos(other);

                        // Sumar un punto a las entregas
                        cantEntrega += 1;
                        Debug.Log("¡Pedido entregado! Punto sumado.");
                    }
                    else
                    {
                        Debug.Log("Prefab Incorrecto, No se ha sumado nada");
                        EliminarItemDeLasManos(other);
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

    private OrderPrefabData BuscarPedidoPorPrefab(GameObject prefab)
    {
        // Recorremos la lista de órdenes y comparamos el prefab con los prefabs de las órdenes activas
        foreach (OrderPrefabData pedido in orderManager.orderPrefabList)
        {
            // Buscamos si el prefab coincide con uno en la lista
            if (pedido.orderPrefab == prefab)
            {
                Debug.Log("Prefab encontrado y coincide con el pedido: " + pedido.orderPrefab.name);
                return pedido;
            }
        }

        Debug.Log("No se encontró un pedido que coincida con ese prefab.");
        return null;
    }

    // Método para actualizar la lista de prefabs requeridos basado en las órdenes activas
    public void ActualizarItemsRequeridos()
    {
        itemsRequeridos.Clear();
        foreach (OrderPrefabData pedido in orderManager.GetActiveOrders())
        {
            itemsRequeridos.Add(pedido.orderPrefab);
        }
        Debug.Log("Items requeridos actualizados: " + string.Join(", ", itemsRequeridos));
    }
}
