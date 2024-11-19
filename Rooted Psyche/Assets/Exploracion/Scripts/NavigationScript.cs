using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    public float followRange = 20.0f;          // Distancia a la que el agente comenzará a seguir al jugador
    public GameObject surpriseIcon;            // Ícono de sorpresa como GameObject
    public float iconDisplayTime = 0.5f;       // Duración que el ícono se muestra
    private NavMeshPath path;
    private NavMeshAgent agent;
    private Transform player;
    private bool seen = false;
    private bool pathAvailable;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        // Buscar al jugador por su etiqueta
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player)
        {
            // Calcula la distancia entre el agente y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            pathAvailable = CalculateNewPath();
            // Si el jugador está dentro del rango, establecer el destino hacia el jugador
            if (distanceToPlayer <= followRange && pathAvailable)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);

                // Mostrar el ícono de sorpresa si aún no está visible
                if (!seen)
                {
                    StartCoroutine(ShowSurpriseIcon());
                }
            }
            else
            {
                // Detener el agente si el jugador está fuera del rango
                agent.isStopped = true;
                seen = false;
            }
        }
    }

    bool CalculateNewPath() {
        agent.CalculatePath(player.position, path);
        if (path.status != NavMeshPathStatus.PathComplete) {
            return false;
        }
        else {
            return true;
        }
    }

    IEnumerator ShowSurpriseIcon()
    {
        if (surpriseIcon)
        {
            surpriseIcon.SetActive(true);    // Activar el ícono
            yield return new WaitForSeconds(iconDisplayTime); // Esperar el tiempo especificado
            surpriseIcon.SetActive(false);   // Desactivar el ícono
        }
        seen = true;
    }
}
