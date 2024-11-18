using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomtoList_M : MonoBehaviour
{
    private RoomTemplates_M templates;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates_M>();
        templates.rooms.Add(this.gameObject);
        
    }


}
