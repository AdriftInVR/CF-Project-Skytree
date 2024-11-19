using UnityEngine;
using Photon.Pun;

public class Player_Cam : MonoBehaviour
{
    private void Start()
    {
        // Encuentra la cámara de seguimiento
        Follow_Player_M cameraFollow = FindObjectOfType<Follow_Player_M>();

        // Registra este jugador en la cámara
        if (cameraFollow != null)
        {
            cameraFollow.AddPlayer(this.transform);
        }
    }

    private void OnDestroy()
    {
        // Elimina al jugador si abandona la escena o el juego
        Follow_Player_M cameraFollow = FindObjectOfType<Follow_Player_M>();
        if (cameraFollow != null)
        {
            cameraFollow.RemovePlayer(this.transform);
        }
    }
}
