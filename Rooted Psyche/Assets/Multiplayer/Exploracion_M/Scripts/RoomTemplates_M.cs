using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Importamos Photon

public class RoomTemplates_M : MonoBehaviour
{
    public static RoomTemplates_M instance;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms;
    public GameObject boss;
    public GameObject[] enemies;
    public GameObject enemyParent;
    public float spawnDelay = 0.1f;

    private void Awake()
    {
        if (RoomTemplates_M.instance == null)
        {
            RoomTemplates_M.instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Invoke("Generate_Lv", 4.0f);
    }

    void Generate_Lv()
    {
        GameObject bossInstance;
        GameObject roomInstance;
        
        // Instanciamos al jefe usando Photon
        bossInstance = PhotonNetwork.Instantiate(boss.name, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        bossInstance.transform.parent = enemyParent.transform;
        
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            // Instanciamos enemigos usando Photon
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            roomInstance = PhotonNetwork.Instantiate(randomEnemy.name, rooms[i].transform.position, Quaternion.identity);
            roomInstance.transform.parent = enemyParent.transform;
        }
    }
}
