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
    [PunRPC]
    public void M_ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);

    }
    public void M_ChangeScene_RPC(string sceneName)
    {
        photonView.RPC("M_ChangeScene", RpcTarget.All, sceneName);
    } 

}
