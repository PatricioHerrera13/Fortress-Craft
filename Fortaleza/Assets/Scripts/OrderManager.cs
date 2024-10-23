using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<Sprite> possibleOrders; // Lista de imágenes posibles para los pedidos
    public List<Image> orderSlots; // Los slots donde se mostrarán los pedidos
    public RectTransform orderPanel; // El panel que contiene los pedidos
    public float orderInterval = 6f; // Tiempo en segundos entre la aparición de cada pedido
    private List<Image> activeOrders = new List<Image>(); // Lista de imágenes que ya están activas
    private const float orderSize = 30f; // Tamaño fijo para las imágenes de pedidos (50x50)
    private const float maxSpacing = 50f; // Espacio máximo entre los pedidos
    public Button back;

    private void Start()
    {
        Button btn = back.GetComponent<Button>();
        btn.onClick.AddListener(Teleport);
        // Desactivar todos los slots de pedidos al inicio
        foreach (Image slot in orderSlots)
        {
            slot.gameObject.SetActive(false);
        }

        StartCoroutine(GenerateOrders());
    }

    private IEnumerator GenerateOrders()
    {
        while (true)
        {
            yield return new WaitForSeconds(orderInterval);
            AddNewOrder();
        }
    }

    private void AddNewOrder()
    {
        if (activeOrders.Count >= orderSlots.Count)
        {
            // Si hay 5 pedidos ya, remueve el más antiguo (primero) para hacer espacio
            RemoveOldestOrder();
        }

        // Selecciona una nueva orden aleatoria
        Sprite newOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];

        // Encuentra el primer slot disponible (el primero que no esté activo)
        for (int i = 0; i < orderSlots.Count; i++)
        {
            if (!activeOrders.Contains(orderSlots[i]))
            {
                orderSlots[i].sprite = newOrder;
                orderSlots[i].gameObject.SetActive(true);
                activeOrders.Add(orderSlots[i]);
                AdjustOrderPositions();
                break;
            }
        }
    }

    private void RemoveOldestOrder()
    {
        // Remover el pedido más antiguo y desactivar el slot de imagen
        activeOrders[0].gameObject.SetActive(false);
        activeOrders.RemoveAt(0);
    }

    private void AdjustOrderPositions()
    {
        // Obtener el ancho disponible del panel donde están los pedidos
        float panelWidth = orderPanel.rect.width;

        // Asegurarse de que haya suficiente espacio para cada pedido manteniendo el tamaño 50x50
        float totalOrdersWidth = activeOrders.Count * orderSize;

        // Calcular el espacio entre cada pedido, pero no mayor de maxSpacing
        float spacing = Mathf.Min((panelWidth - totalOrdersWidth) / (activeOrders.Count + 1), maxSpacing);

        for (int i = 0; i < activeOrders.Count; i++)
        {
            RectTransform orderTransform = activeOrders[i].GetComponent<RectTransform>();

            // Asignar la nueva posición en función del índice, espacio disponible y espaciado
            float newXPosition = spacing * (i + 1) + orderSize * i;
            Vector3 newPosition = new Vector3(newXPosition, orderTransform.anchoredPosition.y, 0);
            orderTransform.anchoredPosition = newPosition;

            // Mantener el tamaño del pedido en 50x50
            orderTransform.sizeDelta = new Vector2(orderSize, orderSize);
        }
    }

    // Método para eliminar un pedido específico del panel
    public bool EliminarPedido(Sprite pedido)
    {
        // Buscar el pedido en la lista de pedidos activos
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.sprite == pedido)
            {
                // Desactivar el pedido encontrado y eliminarlo de la lista
                orderSlot.gameObject.SetActive(false);
                activeOrders.Remove(orderSlot);
                AdjustOrderPositions(); // Reajustar las posiciones de los pedidos restantes
                return true; // Pedido eliminado con éxito
            }
        }

        return false; // El pedido no se encontró
    }

    // Método para obtener la lista de pedidos activos como strings de nombres de ítems
    public List<OrderData> GetOrders()
    {
        List<OrderData> orders = new List<OrderData>();
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.gameObject.activeSelf)
            {
                orders.Add(new OrderData { itemName = orderSlot.sprite.name });
            }
        }
        return orders;
    }

    // Método para obtener el sprite de un pedido dado el nombre del ítem
    public Sprite GetOrderSprite(string itemName)
    {
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.sprite.name == itemName)
            {
                return orderSlot.sprite;
            }
        }
        return null;
    }

    void Teleport()
    {
        SceneManager.LoadScene("MENU");
    }
}

// Clase auxiliar para los datos de cada pedido
public class OrderData
{
    public string itemName;
}
