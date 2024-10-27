using System.Collections;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public Collider zonaRecarga;
    public Collider jugadorCollider;  // Collider del jugador
    public float tiempoCarga = 2f;  // Tiempo de carga antes de disparar
    public GameObject proyectil;  // Prefab del proyectil
    public Transform puntoDisparo;  // Punto de salida del proyectil
    public float fuerzaDisparo = 20f;  // Fuerza con la que el proyectil será disparado
    public Transform handPoint;  // Punto en la mano del jugador donde se comprueba el prefab
    public GameObject prefabRequerido;  // Prefab que debe estar en el HandPoint

    private bool haDisparado = false;  // Flag para evitar múltiples disparos
    private bool enZonaRecarga = false;  // Flag para saber si el jugador está en la zona

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        Debug.Log("Torreta lista para disparar.");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Se detecta cuando el jugador entra en la zona de recarga
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other == jugadorCollider)
        {
            enZonaRecarga = true;  // El jugador está en la zona de recarga
            Debug.Log("Jugador ha entrado en la zona de recarga.");
        }
    }

    // Se detecta cuando el jugador sale de la zona de recarga
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other == jugadorCollider)
        {
            enZonaRecarga = false;  // El jugador ha salido de la zona de recarga
            Debug.Log("Jugador ha salido de la zona de recarga.");
        }
    }

    void Update()
    {
        if (enZonaRecarga && Input.GetKey(KeyCode.E) && !haDisparado && VerificarPrefabEnManos())
        {
            Debug.Log("Jugador interactúa con la torreta y tiene el prefab requerido en las manos.");
            StartCoroutine(Disparar());
        }
    }

    // Verifica si el jugador tiene el prefab requerido en el HandPoint
    bool VerificarPrefabEnManos()
    {
        if (handPoint.childCount > 0)
        {
            GameObject objetoEnManos = handPoint.GetChild(0).gameObject;

            // Obtener el sprite del objeto en manos
            PrefabSprite spriteEnManos = objetoEnManos.GetComponent<PrefabSprite>();
            PrefabSprite spriteRequerido = prefabRequerido.GetComponent<PrefabSprite>();

            if (spriteEnManos != null && spriteRequerido != null)
            {
                // Comparar los sprites
                if (spriteEnManos.sprite == spriteRequerido.sprite)
                {
                    Debug.Log("El sprite en las manos coincide con el prefab requerido.");
                    return true;
                }
                else
                {
                    Debug.LogWarning("Los sprites NO coinciden. Sprite en manos: " + spriteEnManos.sprite.name + ", Sprite requerido: " + spriteRequerido.sprite.name);
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PrefabSprite en uno de los objetos.");
            }
        }
        else
        {
            Debug.LogWarning("No hay objetos en el HandPoint.");
        }
        return false;
    }

    IEnumerator Disparar()
    {
        haDisparado = true;  // Evitar múltiples disparos

        for (int i = 0; i < 3; i++)  // Número de disparos (cambiar si es necesario)
        {
            Debug.Log("Disparando proyectil...");
            GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);

            // Hacer el proyectil invisible
            SpriteRenderer renderer = proyectilInstanciado.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;  // Desactiva el SpriteRenderer
            }

            // Asegúrate de que el collider esté activo
            Collider collider = proyectilInstanciado.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;  // Asegúrate de que el collider esté activo
            }

            Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * fuerzaDisparo, ForceMode.Impulse);  // Ajustar la dirección según sea necesario
            }

            if (handPoint.childCount > 0)
            {
                Destroy(handPoint.GetChild(0).gameObject);  // Elimina el objeto en las manos
                Debug.Log("Prefab requerido eliminado de las manos del jugador.");
            }

            yield return new WaitForSeconds(tiempoCarga);  // Esperar el tiempo de carga antes del siguiente disparo
        }

        haDisparado = false;  // Permitir que la torreta vuelva a disparar
    }
}
