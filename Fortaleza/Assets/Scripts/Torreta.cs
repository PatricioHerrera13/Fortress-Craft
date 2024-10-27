using UnityEngine;
using System.Collections;

public class Torreta : MonoBehaviour
{
    public GameObject prefabRequerido; // Prefab que activa la torreta
    public Sprite spriteEsperado; // Sprite del prefab que se debe verificar
    public Transform puntoDisparo; // Punto desde donde dispara la torreta
    public GameObject proyectilPrefab; // Prefab del proyectil (invisible)
    public float intervaloDisparo = 1f;
    public float tiempoEsperaParaActivar = 0.5f; // Tiempo para dar oportunidad al jugador de activar
    private bool playerEnZona = false;
    public Player player; // Referencia al player en la zona
    private bool disparando = false; // Estado de disparo

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(player);
        player = other.GetComponent<Player>();
        if (player != null && player.TienePrefab(prefabRequerido))
        {
            StartCoroutine(EsperarYVerificarSprite());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(player);
        Player salirPlayer = other.GetComponent<Player>();
        if (salirPlayer != null && salirPlayer == player)
        {
            playerEnZona = false;
            CancelInvoke("Disparar");
            disparando = false; // Resetea el estado de disparo al salir de la zona
            StopAllCoroutines(); // Detiene la espera si sale de la zona
        }
    }

    private void Update()
    {
        // Verifica si el jugador está en la zona, presiona la tecla C y aún no está disparando
        if (playerEnZona && Input.GetKeyDown(KeyCode.C) && !disparando)
        {
            disparando = true; // Evita que se active más de una vez
            InvokeRepeating("Disparar", 0f, intervaloDisparo);
        }
    }

    private IEnumerator EsperarYVerificarSprite()
    {
        yield return new WaitForSeconds(tiempoEsperaParaActivar); // Espera un tiempo antes de activar
        if (player != null && player.TienePrefab(prefabRequerido) && player.TienePrefabConSprite(spriteEsperado))
        {
            playerEnZona = true; // Activa solo si el sprite del prefab es correcto
        }
    }

    void Disparar()
    {
        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
        proyectil.GetComponent<Rigidbody>().velocity = transform.forward * 10f; // Velocidad en Z
        proyectil.SetActive(false); // Mantén el proyectil invisible
    }
}
