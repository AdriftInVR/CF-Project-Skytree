using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public static RoomTemplates instance;
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
        if (RoomTemplates.instance == null)
        {
            RoomTemplates.instance = this;
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
        bossInstance = Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        bossInstance.transform.parent = enemyParent.transform;
        for (int i = 0; i < rooms.Count-1; i++)
        {
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            roomInstance = Instantiate(randomEnemy, rooms[i].transform.position, Quaternion.identity);
            roomInstance.transform.parent = enemyParent.transform;
        }
    }

}
