using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallCast_M : MonoBehaviour
{
    public LayerMask wallLayer;
    private List<GameObject> wallsBlockingView = new List<GameObject>();
    private Camera playerCamera;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine) // Solo configurar para el jugador local
        {
            playerCamera = Camera.main; // Asocia la cámara local del jugador
            if (playerCamera == null)
            {
                Debug.LogError("No Main Camera found!");
                return;
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine || playerCamera == null) return;

        // Vuelve a activar el Renderer de todas las paredes desactivadas
        foreach (var wall in wallsBlockingView)
        {
            SetWallVisibility(wall, true);
        }
        wallsBlockingView.Clear();

        // Raycast desde la cámara del jugador hacia su posición
        Vector3 direction = transform.position - playerCamera.transform.position;
        Ray ray = new Ray(playerCamera.transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, wallLayer);

        foreach (RaycastHit hit in hits)
        {
            GameObject wall = hit.collider.gameObject;
            SetWallVisibility(wall, false);
            wallsBlockingView.Add(wall);
        }
    }

    void SetWallVisibility(GameObject wall, bool visible)
    {
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        if (wallRenderer != null)
        {
            wallRenderer.enabled = visible;
        }
    }
}
