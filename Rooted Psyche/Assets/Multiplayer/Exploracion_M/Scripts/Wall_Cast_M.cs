using System.Collections.Generic;
using UnityEngine;

public class Wall_Cast_M : MonoBehaviour
{
    public Transform player;
    public LayerMask wallLayer;
    private List<GameObject> wallsBlockingView = new List<GameObject>();

    void Update()
    {
        // Vuelve a activar el Renderer de todas las paredes que se desactivaron en el último frame
        foreach (var wall in wallsBlockingView)
        {
            SetWallVisibility(wall, true);
        }
        wallsBlockingView.Clear();

        // Raycast desde la cámara hacia el jugador
        Vector3 direction = player.position - Camera.main.transform.position;
        Ray ray = new Ray(Camera.main.transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, wallLayer);

        foreach (RaycastHit hit in hits)
        {
            GameObject wall = hit.collider.gameObject;
            SetWallVisibility(wall, false); // Desactiva el Renderer de la pared
            wallsBlockingView.Add(wall);
        }
    }

    void SetWallVisibility(GameObject wall, bool visible)
    {
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        if (wallRenderer != null)
        {
            wallRenderer.enabled = visible; // Activa o desactiva el Renderer
        }
    }
}
