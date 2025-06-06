using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Rooms_Multiplayer : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(input_Create.text, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default, null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("M_CuartoSkyler");
    }

        public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Maneja el caso en el que no se puede unir al cuarto
        Debug.LogWarning("No se encontró el cuarto: " + message);
    }

}
