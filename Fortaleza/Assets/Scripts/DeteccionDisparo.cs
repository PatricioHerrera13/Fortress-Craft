using System.Collections;
using UnityEngine;

public class DeteccionDisparo : MonoBehaviour
{
    public Collider zonaRecarga;
    public GameObject cañon1;  // Cañón derecho para player 1
    public GameObject cañon2;  // Cañón izquierdo para player 2
    public Collider p1;
    public Collider p2;
    public float VelLen = 2f;  // Velocidad lenta (movimiento hacia atrás)
    public float VelRap = 5f;  // Velocidad rápida (movimiento hacia adelante)
    public float TiempoCarga = 3f;  // Tiempo de carga antes de disparar
    public bool cambioDir = false;
    private Vector3 dirDerecha = Vector3.back;  // Movimiento hacia atrás para cañón derecho
    private Vector3 dirIzquierda = Vector3.back;  // Movimiento hacia atrás para cañón izquierdo
    private Vector3 posInicialCañon1;  // Posición inicial del cañón derecho
    private Vector3 posInicialCañon2;  // Posición inicial del cañón izquierdo

    void Start()
    {
        Debug.Log("Pos. Inicial Lista");
        posInicialCañon1 = cañon1.transform.position;
        posInicialCañon2 = cañon2.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Entrando en el trigger con: " + other.tag);

        if (other.CompareTag("Player"))
        {
            if (other == p1 && Input.GetKey(KeyCode.E))
            {
                Debug.Log("p1 interactua");
                StartCoroutine(MoverCañon(cañon1, dirDerecha, posInicialCañon1));
            }

            if (other == p2 && Input.GetKey(KeyCode.E))
            {
                Debug.Log("p2 interactua");
                StartCoroutine(MoverCañon(cañon2, dirIzquierda, posInicialCañon2));
            }
        }
    }

    IEnumerator MoverCañon(GameObject cañon, Vector3 direccion, Vector3 posInicial)
    {
        Debug.Log("Preparando Cañon: " + cañon.name);
        
        // Movimiento hacia atrás (lento)
        float tiempo = 0f;
        Vector3 objetivo = cañon.transform.position + direccion;

        while (tiempo < TiempoCarga)
        {
            Debug.Log("Retrocediendo " + cañon.name);
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, objetivo, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelLen;
            yield return null;
        }

        Debug.Log("Pausa...");
        // Pausa en la posición de disparo
        yield return new WaitForSeconds(TiempoCarga);

        // Movimiento hacia adelante (rápido)
        tiempo = 0f;
        while (tiempo < TiempoCarga)
        {
            Debug.Log("Disparo de " + cañon.name);
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, posInicial, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelRap;
            yield return null;
        }
    }
}
