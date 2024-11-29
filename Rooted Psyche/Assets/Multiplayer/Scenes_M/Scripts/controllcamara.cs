using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllcamara : MonoBehaviour
{
    public Camera playerCamera; // Asigna aquí la cámara del jugador

    void Start()
    {
        // Verifica si este objeto pertenece al jugador local
        if (PhotonView.Get(this).IsMine)
        {
            // Si es el jugador local, activa su cámara
            playerCamera.enabled = true;
        }
        else
        {
            // Si no, desactiva la cámara
            playerCamera.enabled = false;
        }
    }
}
