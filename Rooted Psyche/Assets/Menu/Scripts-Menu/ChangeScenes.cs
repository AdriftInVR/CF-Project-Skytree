using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ChangeScenes : MonoBehaviourPunCallbacks
{
    void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true; // Sincroniza escenas automáticamente
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void M_ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

}
