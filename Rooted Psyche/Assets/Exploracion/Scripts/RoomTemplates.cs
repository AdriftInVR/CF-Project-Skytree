using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;

    public List<GameObject> rooms;

    public GameObject boss;
    public GameObject shop;
    public GameObject enemies;

    private void Start()
    {
        Invoke("Generate_Lv", 5.0f);
    }

    void Generate_Lv()
    {
        Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        //rooms.RemoveAt(rooms.Count - 1);
        Instantiate(shop, rooms[rooms.Count - 2].transform.position, Quaternion.identity);
        //rooms.RemoveAt(rooms.Count - 1);
        for (int i = 0; i < rooms.Count-1; i++)
        {
            Instantiate(enemies, rooms[i].transform.position, Quaternion.identity);
        }
    }

}
