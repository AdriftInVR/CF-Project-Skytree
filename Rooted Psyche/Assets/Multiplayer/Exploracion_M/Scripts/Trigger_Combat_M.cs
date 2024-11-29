using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Trigger_Combat_M : MonoBehaviourPunCallbacks
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.name != "BrierTarget")
        {
            Debug.Log("Player has entered the combat zone");

            if (PhotonNetwork.IsConnected)
            {
                // En multijugador: usa Photon para sincronizar el cambio de escena
                //photonView.RPC("ChangeSceneForAll", RpcTarget.All);

                // Destruir el objeto en el MasterClient
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                else
                {
                    photonView.RPC("RequestDestroy", RpcTarget.MasterClient, photonView.ViewID);
                }
            }
            else
            {
                // En modo single player: carga la escena localmente
                SceneManager.LoadScene("Combate");

                // Destruir el objeto localmente
                Destroy(this.gameObject);
            }
        }
    }

    [PunRPC]
    void ChangeSceneForAll()
    {
        PhotonNetwork.LoadLevel("Combate");
    }

    [PunRPC]
    void RequestDestroy(int viewID)
    {
        // Verificar que este cliente sea el MasterClient antes de destruir el objeto
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView targetView = PhotonView.Find(viewID);
            if (targetView != null)
            {
                PhotonNetwork.Destroy(targetView.gameObject);
            }
        }
    }
}
