using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<Sprite> possibleOrders; // Lista de imágenes posibles para los pedidos
    public List<Image> orderSlots; // Los slots donde se mostrarán los pedidos
    public RectTransform orderPanel; // El panel que contiene los pedidos
    public float orderInterval = 6f; // Tiempo en segundos entre la aparición de cada pedido
    private List<Image> activeOrders = new List<Image>(); // Lista de imágenes que ya están activas
    public List<OrderPrefabData> orderPrefabList; // Lista que vincula sprites de pedidos con prefabs

    private void Start()
    {
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
            // Si hay más pedidos de los permitidos, remueve el más antiguo
            RemoveOldestOrder();
        }

        // Selecciona una nueva orden aleatoria de los prefabs
        OrderPrefabData newOrderData = orderPrefabList[Random.Range(0, orderPrefabList.Count)];
        Sprite newOrderSprite = newOrderData.orderSprite;

        // Encuentra el primer slot disponible
        for (int i = 0; i < orderSlots.Count; i++)
        {
            if (!activeOrders.Contains(orderSlots[i]))
            {
                orderSlots[i].sprite = newOrderSprite;
                orderSlots[i].gameObject.SetActive(true);
                activeOrders.Add(orderSlots[i]);
                AdjustOrderPositions();

                // Notificar al portal de entregas que actualice los items requeridos
                FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                Debug.Log("Nueva orden añadida: " + newOrderSprite.name); // Depuración
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
        float panelWidth = orderPanel.rect.width;
        float totalOrdersWidth = activeOrders.Count * 50f;
        float spacing = Mathf.Min((panelWidth - totalOrdersWidth) / (activeOrders.Count + 1), 50f);

        for (int i = 0; i < activeOrders.Count; i++)
        {
            RectTransform orderTransform = activeOrders[i].GetComponent<RectTransform>();
            float newXPosition = spacing * (i + 1) + 50f * i;
            Vector3 newPosition = new Vector3(newXPosition, orderTransform.anchoredPosition.y, 0);
            orderTransform.anchoredPosition = newPosition;
            orderTransform.sizeDelta = new Vector2(50f, 50f);
        }
    }

    // Método para eliminar un pedido basado en el sprite del pedido
    public bool EliminarPedido(Sprite pedidoSprite)
    {
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.sprite == pedidoSprite)
            {
                orderSlot.gameObject.SetActive(false);
                activeOrders.Remove(orderSlot);
                AdjustOrderPositions();
                FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                return true;
            }
        }
        return false;
    }

    // Obtener las órdenes activas (retorna los datos de los pedidos activos)
    public List<OrderPrefabData> GetActiveOrders()
    {
        List<OrderPrefabData> activeOrdersList = new List<OrderPrefabData>();
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.gameObject.activeSelf)
            {
                foreach (OrderPrefabData pedido in orderPrefabList)
                {
                    if (pedido.orderSprite == orderSlot.sprite)
                    {
                        activeOrdersList.Add(pedido);
                        Debug.Log("Orden activa encontrada: " + pedido.orderPrefab.name); // Depuración
                    }
                }
            }
        }

        Debug.Log("Órdenes activas devueltas: " + activeOrdersList.Count); // Depuración
        return activeOrdersList;
    }
}
