using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class RoomtoList_M : MonoBehaviour
{
    private RoomTemplates_M templates;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates_M>();

        if (PhotonNetwork.IsMasterClient)
        {
            templates.rooms.Add(this.gameObject);
        }
    }
}
