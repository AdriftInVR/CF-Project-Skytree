using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class Conection_Server : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Si se desconecta o falla la conexión, carga la pantalla de inicio || no lo he probado
        Debug.LogWarning("Desconectado de Photon: " + cause);
        SceneManager.LoadScene("MenuPrincipal");
    }
}
