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

    public GameObject bossPrefab;
    public GameObject[] enemies;
    public GameObject enemyParent;

    public float spawnDelay = 0.1f;

    private bool enemiesSpawned = false;

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
        // Espera hasta que todas las habitaciones estén generadas
        StartCoroutine(WaitForRoomGeneration());
    }

    IEnumerator WaitForRoomGeneration()
    {
        yield return new WaitForSeconds(spawnDelay + 2f); // Asegúrate de que todas las habitaciones estén listas
        if (PhotonNetwork.IsMasterClient && !enemiesSpawned)
        {
            SpawnEnemiesAndBoss();
        }
    }

private void SpawnEnemiesAndBoss()
{
    enemiesSpawned = true;

    // Generar el jefe en la última habitación
    GameObject bossRoom = rooms[rooms.Count - 1];
    GameObject bossInstance = PhotonNetwork.Instantiate(bossPrefab.name, bossRoom.transform.position, Quaternion.identity);
    bossInstance.transform.parent = enemyParent.transform;

    // Generar enemigos en todas las habitaciones excepto la del jefe
    for (int i = 0; i < rooms.Count - 1; i++)
    {
        GameObject room = rooms[i];
        SpawnEnemyInRoom(room);
    }

    // Reconstruir el NavMesh
    RebuildNavMesh();
}

private void SpawnEnemyInRoom(GameObject room)
{
    // Generar un único enemigo por cuarto
    Vector3 spawnPosition = room.transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
    GameObject randomEnemy = PhotonNetwork.Instantiate(enemies[Random.Range(0, enemies.Length)].name, spawnPosition, Quaternion.identity);
    randomEnemy.transform.parent = enemyParent.transform;
}



    private void RebuildNavMesh()
    {
        // Reconstruir el NavMesh para que incluya todos los elementos nuevos
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        foreach (var surface in navMeshSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
}
