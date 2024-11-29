using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Follow_Player_M : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 8, -10);
    public Transform player;
    private Camera mainCamera;

    void Start()
    {
        // Obtén la cámara principal
        mainCamera = Camera.main;

        // Si es el jugador local, activa la cámara para él
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.IsLocal)
        {
            mainCamera.gameObject.SetActive(true);  // Asegúrate de que la cámara esté activada solo para el jugador local
        }
        else
        {
            mainCamera.gameObject.SetActive(false);  // Desactiva la cámara para otros jugadores
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Solo mover la cámara si el jugador local ha sido asignado
            Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, playerPos + offset, 0.5f);
        }
        else
        {
            // Esperar hasta que el jugador local esté disponible
            if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.IsLocal)
            {
                GameObject localPlayer = GameObject.FindWithTag("Player"); // Asegúrate de que el jugador tenga el tag "Player"
                if (localPlayer != null)
                {
                    player = localPlayer.transform;
                }
            }
        }
    }
}
