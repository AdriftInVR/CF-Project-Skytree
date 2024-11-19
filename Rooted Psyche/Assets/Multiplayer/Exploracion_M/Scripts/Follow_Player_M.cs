using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Follow_Player_M : MonoBehaviour
{
    private List<Transform> players = new List<Transform>(); // Lista dinámica de jugadores
    public float smoothTime = 0.5f; // Tiempo de suavizado para el movimiento
    public Vector3 offset; // Desplazamiento de la cámara respecto al centro
    public float minZoom = 5f; // Zoom mínimo de la cámara
    public float maxZoom = 15f; // Zoom máximo de la cámara
    public float zoomLimiter = 50f; // Control de la sensibilidad del zoom

    private Vector3 velocity;

    void Start()
    {
        // Busca jugadores existentes al iniciar la cámara
        FindExistingPlayers();
    }

    void LateUpdate()
    {
        if (players.Count == 0)
            return;

        MoveCamera();
        AdjustZoom();
    }

    private void FindExistingPlayers()
    {
        // Busca todos los jugadores en la escena con el tag "Player"
        GameObject[] existingPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in existingPlayers)
        {
            players.Add(player.transform);
        }
    }

    public void AddPlayer(Transform playerTransform)
    {
        if (!players.Contains(playerTransform))
        {
            players.Add(playerTransform);
        }
    }

    public void RemovePlayer(Transform playerTransform)
    {
        if (players.Contains(playerTransform))
        {
            players.Remove(playerTransform);
        }
    }

    private void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void AdjustZoom()
    {
        float maxDistance = GetGreatestDistance();
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Clamp(maxDistance / zoomLimiter, minZoom, maxZoom), Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);
        foreach (Transform player in players)
        {
            bounds.Encapsulate(player.position);
        }
        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }

    private Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);
        foreach (Transform player in players)
        {
            bounds.Encapsulate(player.position);
        }
        return bounds.center;
    }
}
