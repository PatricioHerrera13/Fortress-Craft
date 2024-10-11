using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider; // Collider del jugador
    public Collider jugadorCollider1; // Collider del jugador
    public string itemRequerido; // El tipo de ítem que se necesita para entregar el pedido

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

                // Verificar si el jugador sostiene un ítem y si ese ítem es el requerido
                if (playerPickUp != null && !string.IsNullOrEmpty(playerPickUp.GetPickedItemType()))
                {
                    string tipoItemJugador = playerPickUp.GetPickedItemType();
                    Debug.Log(tipoItemJugador);

                    if (tipoItemJugador == itemRequerido)
                    {
                        // El ítem es el correcto, eliminarlo del OrderManager
                        Sprite pedidoSprite = BuscarPedidoSprite(itemRequerido);

                        if (pedidoSprite != null && orderManager.EliminarPedido(pedidoSprite))
                        {
                            // Eliminar el ítem del "HandPoint" del jugador
                            Transform hand = other.transform.Find("Hand/HandPoint");
                            if (hand != null && hand.childCount > 0)
                            {
                                // Destruir el objeto que se sostiene
                                GameObject objetoSostenido = hand.GetChild(0).gameObject;
                                Destroy(objetoSostenido);
                                Debug.Log("El objeto ha sido eliminado del jugador.");
                            }

                            // Eliminar el tipo de ítem de las manos del jugador
                            playerPickUp.pickedItemType = null;

                            // Sumar un punto al jugador (por ahora con un Debug)
                            Debug.Log("¡Pedido entregado! Punto sumado.");
                        }
                    }
                    else
                    {
                        Debug.Log("El jugador no sostiene el ítem correcto.");
                    }
                }
                else
                {
                    Debug.Log("El jugador no sostiene ningún ítem.");
                }
            }
        }
    }

    // Método para buscar el Sprite correspondiente al nombre del pedido
    private Sprite BuscarPedidoSprite(string nombrePedido)
    {
        foreach (Sprite pedido in orderManager.possibleOrders)
        {
            if (pedido.name == nombrePedido)
            {
                return pedido;
            }
        }
        return null; // No se encontró un Sprite con ese nombre
    }
}
