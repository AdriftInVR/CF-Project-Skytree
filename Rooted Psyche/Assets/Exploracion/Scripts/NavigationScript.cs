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
    private bool seen = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Buscar al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta " + playerTag);
        }
    }

    void Update()
    {
        if (playerTransform)
        {
            // Calcula la distancia entre el agente y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Si el jugador está dentro del rango, establecer el destino hacia el jugador
            if (distanceToPlayer <= followRange)
            {
                navMeshAgent.SetDestination(playerTransform.position);

                // Mostrar el ícono de sorpresa si aún no está visible
                if (!seen)
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
        else
        {
            seen = false; 
        }
    }

    IEnumerator ShowSurpriseIcon()
    {
        seen = true;
        if (surpriseIcon)
        {
            surpriseIcon.SetActive(true);    // Activar el ícono
            yield return new WaitForSeconds(iconDisplayTime); // Esperar el tiempo especificado
            surpriseIcon.SetActive(false);   // Desactivar el ícono
        }
    }
}
