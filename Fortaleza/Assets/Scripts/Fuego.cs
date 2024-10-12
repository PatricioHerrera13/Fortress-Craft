using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    public GameObject firePrefab; // El prefab del fuego
    public float minTime = 10f; // Tiempo mínimo de expansión
    public float maxTime = 30f; // Tiempo máximo de expansión
    public float spreadDistance = 1.5f; // Distancia a la que se generarán las copias
    public static int currentFireCount = 0; // Contador estático para instancias de fuego
    public static int maxFireCount = 10; // Número máximo de instancias permitidas

    private float nextSpreadTime;
    private float creationTime; // Hora de creación para cada fuego
    private bool hasSpread = false; // Estado para verificar si ya se ha expandido
    private Vector3 lastDirection = Vector3.zero; // Dirección de la última expansión
    private bool isRestarting = false; // Evitar múltiples reinicios simultáneos

    void Start()
    {
        // Hora de creación del fuego
        creationTime = Time.time;

        // Cada fuego tendrá un tiempo de expansión individual entre minTime y maxTime
        nextSpreadTime = creationTime + Random.Range(minTime, maxTime);
    }

    void Update()
    {
        // Comprobar si es el momento de expandirse y no se ha expandido aún
        if (!hasSpread && Time.time >= nextSpreadTime)
        {
            SpreadFire();
            hasSpread = true; // Marcar como expandido
        }

        // Si no quedan fuegos en la escena y no estamos reiniciando ya, empezar la corrutina para reiniciar
        if (!isRestarting && GameObject.FindGameObjectsWithTag("Fuego").Length == 0)
        {
            StartCoroutine(RestartFireAfterDelay(20f)); // Espera 40 segundos antes de reiniciar
        }
    }

    void SpreadFire()
    {
        // Solo crear una nueva instancia si no hemos alcanzado el máximo permitido
        if (currentFireCount < maxFireCount)
        {
            // Elegir una dirección aleatoria para expandirse, excepto la contraria a la última
            List<Vector3> possibleDirections = new List<Vector3>
            {
                new Vector3(spreadDistance, 0, 0),   // Derecha
                new Vector3(-spreadDistance, 0, 0),  // Izquierda
                new Vector3(0, 0, spreadDistance),   // Delante
                new Vector3(0, 0, -spreadDistance)   // Detrás
            };

            // Remover la dirección contraria
            if (lastDirection != Vector3.zero)
            {
                Vector3 oppositeDirection = -lastDirection;
                possibleDirections.Remove(oppositeDirection);
            }

            // Elegir una nueva dirección aleatoria de las posibles
            Vector3 chosenDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

            // Verificar si la nueva posición ya tiene un fuego
            Vector3 newPosition = new Vector3(transform.position.x + chosenDirection.x, transform.position.y, transform.position.z + chosenDirection.z);
            Collider[] hitColliders = Physics.OverlapSphere(newPosition, 0.1f); // Radio pequeño para detectar colisiones
            bool positionOccupied = false;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Fuego")) // Asegúrate de asignar la etiqueta "Fuego" a todos los fuegos
                {
                    positionOccupied = true;
                    break;
                }
            }

            // Solo instanciar el nuevo fuego si la posición no está ocupada
            if (!positionOccupied)
            {
                GameObject newFire = Instantiate(firePrefab, newPosition, Quaternion.identity);

                // Asegúrate de que el nuevo fuego tenga asignado el mismo prefab
                Fuego newFireScript = newFire.GetComponent<Fuego>();
                if (newFireScript != null)
                {
                    newFireScript.firePrefab = firePrefab; // Asignar el prefab
                    currentFireCount++; // Incrementar el contador de instancias
                    newFireScript.lastDirection = chosenDirection; // Registrar la dirección de expansión
                }
                else
                {
                    Debug.LogError("El prefab de fuego no tiene el script Fuego asignado.");
                }
            }
            else
            {
                Debug.Log("La posición ya está ocupada, no se generó un nuevo fuego.");
            }
        }
        else
        {
            Debug.Log("Se ha alcanzado el número máximo de instancias de fuego.");
        }
    }

    IEnumerator RestartFireAfterDelay(float delay)
    {
        isRestarting = true; // Evitar que se llame a la corrutina varias veces
        Debug.Log("Esperando " + delay + " segundos antes de reiniciar...");
        yield return new WaitForSeconds(delay); // Esperar 40 segundos

        // Verificar nuevamente si no hay fuegos antes de reiniciar
        if (GameObject.FindGameObjectsWithTag("Fuego").Length == 0)
        {
            RestartFire();
        }
        isRestarting = false;
    }

    void RestartFire()
    {
        Debug.Log("No quedan más fuegos en la escena. Reiniciando el proceso...");
        hasSpread = false; // Resetear el estado de expansión
        currentFireCount = 0; // Reiniciar el contador de fuegos

        // Reiniciar el tiempo de expansión para el próximo fuego
        nextSpreadTime = Time.time + Random.Range(minTime, maxTime);
    }
}
