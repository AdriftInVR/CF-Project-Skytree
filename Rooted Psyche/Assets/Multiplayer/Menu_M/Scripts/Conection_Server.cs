using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class Conection_Server : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
// En caso de que se conecte al servidor se unira al lobby   
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor");
        PhotonNetwork.JoinLobby();
    }

// En caso de que se conecte al servidor se cargara el menu principal con conexion
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

// En caso de que se desconecte de Photon se regresara al menu principal sin conexion
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado de Photon: " + cause);
        SceneManager.LoadScene("MenuPrincipal");
    }
}
