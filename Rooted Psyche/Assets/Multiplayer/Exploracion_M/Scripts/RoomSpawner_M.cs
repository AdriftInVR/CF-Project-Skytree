using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.AI.Navigation;

public class RoomSpawner_M : MonoBehaviourPun
{
    public int openSide;
    public bool spawned = false;
    private RoomTemplates_M templates;

    private NavMeshSurface navMeshSurface; // Referencia al NavMeshSurface

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates_M>();

        if (PhotonNetwork.IsMasterClient)
        {
            // Solo el host genera las habitaciones
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(templates.spawnDelay);
        Spawn();
    }

    void Spawn()
    {
        if (!spawned)
        {
            int rand;
            GameObject room = null;

            switch (openSide)
            {
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    room = PhotonNetwork.Instantiate(templates.bottomRooms[rand].name, transform.position, Quaternion.identity);
                    break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    room = PhotonNetwork.Instantiate(templates.topRooms[rand].name, transform.position, Quaternion.identity);
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    room = PhotonNetwork.Instantiate(templates.leftRooms[rand].name, transform.position, Quaternion.identity);
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    room = PhotonNetwork.Instantiate(templates.rightRooms[rand].name, transform.position, Quaternion.identity);
                    break;
            }

            if (room != null)
            {
                templates.rooms.Add(room);
                navMeshSurface = room.GetComponent<NavMeshSurface>();

                // Si la sala tiene NavMeshSurface, construimos el NavMesh
                if (navMeshSurface != null)
                {
                    navMeshSurface.BuildNavMesh();
                }
            }

            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint") && !spawned)
        {
            if (!other.GetComponent<RoomSpawner_M>().spawned)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Instantiate(templates.closedRoom.name, transform.position, Quaternion.identity);
                }
            }

            spawned = true;
        }
    }
}
