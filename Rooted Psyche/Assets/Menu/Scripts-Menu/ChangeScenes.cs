using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ChangeScenes : MonoBehaviourPunCallbacks
{
    public string nextSceneName;
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void M_ChangeScene(string sceneName)
    {
        // Solo el MasterClient (anfitrión) ejecuta el cambio de escena
        if (PhotonNetwork.IsMasterClient)
        {
        // Usar PhotonNetwork.LoadLevel para cargar la escena de forma sincronizada
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            Debug.LogWarning("Solo el anfitrión puede iniciar el cambio de escena.");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Verifica que el MasterClient ya esté en la nueva escena y actualiza al jugador que entra
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != nextSceneName)
        {
            PhotonNetwork.LoadLevel(nextSceneName);
        }
    }


    

}
