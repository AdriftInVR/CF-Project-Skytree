using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    public string playerTag = "Player";        // Tag del jugador
    public float followRange = 20.0f;          // Distancia a la que el agente comenzará a seguir al jugador
    public GameObject surpriseIcon;            // Ícono de sorpresa como GameObject
    public float iconDisplayTime = 2.0f;       // Duración que el ícono se muestra

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private bool isIconVisible = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Buscar al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta " + playerTag);
        }

        // Desactivar el ícono de sorpresa al inicio
        if (surpriseIcon != null)
        {
            surpriseIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calcula la distancia entre el agente y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Si el jugador está dentro del rango, establecer el destino hacia el jugador
            if (distanceToPlayer <= followRange)
            {
                navMeshAgent.SetDestination(playerTransform.position);

                // Mostrar el ícono de sorpresa si aún no está visible
                if (!isIconVisible)
                {
                    StartCoroutine(ShowSurpriseIcon());
                }
            }
            else
            {
                // Detener el agente si el jugador está fuera del rango
                navMeshAgent.ResetPath();
            }
        }
    }

    IEnumerator ShowSurpriseIcon()
    {
        if (surpriseIcon != null)
        {
            surpriseIcon.SetActive(true);    // Activar el ícono
            isIconVisible = true;
            yield return new WaitForSeconds(iconDisplayTime); // Esperar el tiempo especificado
            surpriseIcon.SetActive(false);   // Desactivar el ícono
            isIconVisible = false;
        }
    }
}
