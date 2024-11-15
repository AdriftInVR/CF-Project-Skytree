using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class boton_M : MonoBehaviour
{
public GameObject hostButton; // Referencia al botón dentro del Canvas

    void Start()
    {
        // Verificar si el jugador es el host
        if (!PhotonNetwork.IsMasterClient)
        {
            // Desactivar el botón si no es el host
            hostButton.SetActive(false);
        }
    }


}
