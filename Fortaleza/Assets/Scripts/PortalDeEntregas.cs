using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider; // Collider del jugador
    public Collider jugadorCollider1; // Collider del jugador
    public List<string> itemsRequeridos; // Lista de tipos de ítems requeridos
    public float cantEntrega = 0; // Contador de entregas

    private void OnTriggerStay(Collider other)
    {
        // Verificar si el objeto que está dentro del trigger es el jugador
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            // Comprobar si el jugador presiona la tecla 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("El jugador presionó E.");
                PickUpItem playerPickUp = other.GetComponentInChildren<PickUpItem>();

                // Verificar si el jugador sostiene un ítem y si ese ítem es uno de los requeridos
                if (playerPickUp != null && !string.IsNullOrEmpty(playerPickUp.GetPickedItemType()))
                {
                    string tipoItemJugador = playerPickUp.GetPickedItemType();
                    Debug.Log(tipoItemJugador);

                    if (itemsRequeridos.Contains(tipoItemJugador))
                    {
                        // El ítem es el correcto, eliminarlo del OrderManager
                        Debug.Log("ITEM CORRECTO");
                        Sprite pedidoSprite = BuscarPedidoSprite(tipoItemJugador);

                        if (pedidoSprite != null && orderManager.EliminarPedido(pedidoSprite))
                        {
                            // Eliminar el ítem del "HandPoint" del jugador
                            EliminarItemDeLasManos(other);

                            // Sumar un punto a las entregas
                            cantEntrega += 1;
                            Debug.Log("¡Pedido entregado! Punto sumado.");
                        }
                    }
                    else
                    {
                        // Si el ítem es incorrecto, eliminarlo igualmente pero sin sumar nada
                        Debug.Log("Pedido Incorrecto, No se ha sumado nada");
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

    // Método para eliminar el ítem de las manos del jugador
    private void EliminarItemDeLasManos(Collider other)
    {
        Transform hand = other.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Destroy(hand.GetChild(0).gameObject);
            Debug.Log("El objeto ha sido eliminado del jugador.");
        }
    }

    // Método para buscar el Sprite correspondiente al nombre del pedido
    public Sprite BuscarPedidoSprite(string nombrePedido)
    {
        foreach (Sprite pedido in orderManager.possibleOrders)
        {
            if (pedido.name.Trim() == nombrePedido.Trim())
            {
                Debug.Log("Sprite encontrado: " + pedido.name);
                return pedido;
            }
        }

        Debug.Log("No se encontró un Sprite con ese nombre");
        return null; // No se encontró un Sprite con ese nombre
    }
}
