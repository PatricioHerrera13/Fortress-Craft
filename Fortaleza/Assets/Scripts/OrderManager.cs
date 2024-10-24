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
    
    public List<OrderPrefabData> orderPrefabList; // Lista que vincula sprites de pedidos con prefabs

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
            // Si hay más pedidos de los permitidos, remueve el más antiguo
            RemoveOldestOrder();
        }

        // Selecciona una nueva orden aleatoria
        Sprite newOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];

        // Encuentra el primer slot disponible
        for (int i = 0; i < orderSlots.Count; i++)
        {
            if (!activeOrders.Contains(orderSlots[i]))
            {
                orderSlots[i].sprite = newOrder;
                orderSlots[i].gameObject.SetActive(true);
                activeOrders.Add(orderSlots[i]);
                AdjustOrderPositions();

                // Notificar al portal de entregas que actualice los items requeridos
                FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
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

    public bool EliminarPedido(Sprite pedido)
    {
        foreach (Image orderSlot in activeOrders)
        {
            if (orderSlot.sprite == pedido)
            {
                orderSlot.gameObject.SetActive(false);
                activeOrders.Remove(orderSlot);
                AdjustOrderPositions();

                // Actualizar los items requeridos en el portal de entregas
                FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                return true;
            }
        }
        return false;
    }

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
                    }
                }
            }
        }
        return activeOrdersList;
    }

    void Teleport()
    {
        SceneManager.LoadScene("MENU");
    }
}

// Clase auxiliar para vincular sprites de pedidos con prefabs
[System.Serializable]
public class OrderPrefabData
{
    public Sprite orderSprite;  // Imagen que representa el pedido
    public GameObject orderPrefab;  // Prefab correspondiente al pedido
}
