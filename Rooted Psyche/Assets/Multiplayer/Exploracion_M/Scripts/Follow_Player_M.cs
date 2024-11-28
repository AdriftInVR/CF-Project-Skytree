using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Follow_Player_M : MonoBehaviour
{
    private PhotonView photonView;
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0, 8, -10);

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            mainCamera = Camera.main; // Encuentra la cámara principal
            if (mainCamera != null)
            {
                mainCamera.transform.SetParent(transform); // Asocia la cámara al jugador
                mainCamera.transform.localPosition = offset; // Ajusta la posición
                mainCamera.transform.localRotation = Quaternion.identity; // Restablece la rotación
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine && mainCamera != null)
        {
            Vector3 targetPosition = transform.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, 0.5f);
        }
    }
}
