using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider; // Collider del jugador
    public string pedidoEnManos; // El pedido que el jugador está sosteniendo

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other == jugadorCollider)
        {
            // Verificar si el jugador sostiene un pedido
            if (!string.IsNullOrEmpty(pedidoEnManos))
            {
                // Buscar el Sprite correspondiente al pedido que el jugador tiene en manos
                Sprite pedidoSprite = BuscarPedidoSprite(pedidoEnManos);

                if (pedidoSprite != null && orderManager.EliminarPedido(pedidoSprite))
                {
                    // Eliminar el pedido de las manos del jugador
                    pedidoEnManos = null;

                    // Sumar un punto al jugador (por ahora con un Debug)
                    Debug.Log("¡Pedido entregado! Punto sumado.");
                }
                else
                {
                    Debug.Log("El pedido no está en la lista.");
                }
            }
            else
            {
                Debug.Log("El jugador no sostiene ningún pedido.");
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