using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ExitLobby : MonoBehaviourPunCallbacks
{
    // Llamada para salir del cuarto
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // Este método se ejecutará una vez que el jugador haya dejado el cuarto
    public override void OnLeftRoom()
    {
        // Cambia a la escena de menú o la que desees
        SceneManager.LoadScene("MenuPrincipal");
        GameObject objToRemove = GameObject.Find("EXP");
        if (objToRemove != null) {
            Destroy(objToRemove);
        }
    }
}
