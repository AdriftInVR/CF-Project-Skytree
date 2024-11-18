using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject shop;
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
        Invoke("Generate_Lv", 5.0f);
    }

    void Generate_Lv()
    {
        GameObject bossInstance;
        GameObject shopInstance;
        GameObject roomInstance;
        bossInstance = Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        bossInstance.transform.parent = enemyParent.transform;
        shopInstance = Instantiate(shop, rooms[Random.Range(1, rooms.Count-2)].transform.position, Quaternion.identity);
        shopInstance.transform.parent = enemyParent.transform;
        for (int i = 0; i < rooms.Count-1; i++)
        {
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            roomInstance = Instantiate(randomEnemy, rooms[i].transform.position, Quaternion.identity);
            roomInstance.transform.parent = enemyParent.transform;
        }
    }

}
