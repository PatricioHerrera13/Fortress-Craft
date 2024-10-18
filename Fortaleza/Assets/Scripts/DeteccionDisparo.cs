using System.Collections;
using UnityEngine;

public class DeteccionDisparo : MonoBehaviour
{
    public Collider zonaRecarga;
    public GameObject cañon1;  // Cañón derecho para player 1
    public GameObject cañon2;  // Cañón izquierdo para player 2
    public Collider p1;
    public Collider p2;
    public float VelLen = 1f;  // Velocidad lenta (movimiento hacia atrás)
    public float VelRap = 3f;  // Velocidad rápida (movimiento hacia adelante)
    public float TiempoCarga = 2f;  // Tiempo de carga antes de disparar
    public bool cambioDir = false;
    private Vector3 dirDerecha = Vector3.left;  // Movimiento hacia atrás para cañón derecho
    private Vector3 dirIzquierda = Vector3.right;  // Movimiento hacia atrás para cañón izquierdo
    private Vector3 posInicialCañon1;  // Posición inicial del cañón derecho
    private Vector3 posInicialCañon2;  // Posición inicial del cañón izquierdo
    public GameObject proyectil;
    public Transform puntoDisparo1;  // Punto de salida del proyectil para cañón1
    public Transform puntoDisparo2;  // Punto de salida del proyectil para cañón2
    public float fuerzaDisparo = 20f;  // Fuerza con la que el proyectil será disparado

    private bool haDisparado = false;  // Flag para evitar múltiples disparos

    void Start()
    {
        Debug.Log("Pos. Inicial Lista");
        posInicialCañon1 = cañon1.transform.position;
        posInicialCañon2 = cañon2.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other == p1 && Input.GetKey(KeyCode.E) && !haDisparado)  // Verificamos que no haya disparado aún
            {
                Debug.Log("p1 interactua");
                StartCoroutine(MoverCañon(cañon1, dirDerecha, posInicialCañon1, puntoDisparo1, Vector3.right));
            }

            if (other == p2 && Input.GetKey(KeyCode.E) && !haDisparado)  // Verificamos que no haya disparado aún
            {
                Debug.Log("p2 interactua");
                StartCoroutine(MoverCañon(cañon2, dirIzquierda, posInicialCañon2, puntoDisparo2, Vector3.left));
            }
        }
    }

    IEnumerator MoverCañon(GameObject cañon, Vector3 direccion, Vector3 posInicial, Transform puntoDisparo, Vector3 direccionDisparo)
    {
        Debug.Log("Preparando Cañon: " + cañon.name);
        
        haDisparado = true;  // Evitar múltiples disparos

        // Movimiento hacia atrás (lento)
        float tiempo = 0f;
        Vector3 objetivo = cañon.transform.position + direccion;

        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, objetivo, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelLen;
            yield return null;
        }

        Debug.Log("Pausa...");
        // Pausa en la posición de disparo
        yield return new WaitForSeconds(TiempoCarga);

        // Instanciar el proyectil en la punta del cañón (solo 1 vez)
        DispararProyectil(puntoDisparo, direccionDisparo);

        // Movimiento hacia adelante (rápido)
        tiempo = 0f;
        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, posInicial, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelRap;
            yield return null;
        }

        // Permitir que el cañón vuelva a disparar
        haDisparado = false;
    }

    void DispararProyectil(Transform puntoDisparo, Vector3 direccionDisparo)
    {
        GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Aplicar fuerza en la dirección especificada (X positivo o X negativo)
            rb.AddForce(direccionDisparo * fuerzaDisparo, ForceMode.Impulse);
        }
    }
}
