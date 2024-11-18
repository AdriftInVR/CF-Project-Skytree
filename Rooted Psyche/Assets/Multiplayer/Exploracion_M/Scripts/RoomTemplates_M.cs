using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.AI.Navigation;

public class RoomTemplates_M : MonoBehaviour
{
    public static RoomTemplates_M instance;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms = new List<GameObject>();

    public GameObject boss;
    public GameObject shop;
    public GameObject[] enemies;
    public GameObject enemyParent;

     public float spawnDelay = 0.1f;

    private NavMeshSurface navMeshSurface; // Referencia al NavMeshSurface global

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

    public void GenerateLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var room in rooms)
            {
                GenerateEnemies(room);
            }

            // Reconstruimos el NavMesh después de que se generan todas las habitaciones
            RebuildNavMesh();
        }
    }

    private void GenerateEnemies(GameObject room)
    {
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
        PhotonNetwork.Instantiate(randomEnemy.name, room.transform.position, Quaternion.identity);
    }

    private void RebuildNavMesh()
    {
        // Encuentra todos los NavMeshSurface y reconstruye el NavMesh global
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();

        foreach (var surface in navMeshSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
}
