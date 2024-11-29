using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Importamos Photon
using UnityEngine.AI;
using Unity.AI.Navigation;

public class RoomSpawner_M : MonoBehaviour
{
    public int openSide; // 1 --> Bottom Door, 2 --> Top Door, 3 --> Left Door, 4 --> Right Door
    private RoomTemplates_M templates;
    private int rand;
    public bool spawned = false;
    private GameObject room;
    private Vector3 position;
    [SerializeField] NavMeshSurface navMeshSurface;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates_M>();
        StartCoroutine(SpawnWithDelay());
        position = transform.position;
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(templates.spawnDelay); // Espera el tiempo definido
        Spawn();
    }

    void Spawn(){
    if (!spawned)
    {
        // Comprobar si hay otro spawnPoint en el mismo punto
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("SpawnPoint") && collider.gameObject != this.gameObject)
            {
                // Si hay otro SpawnPoint en la misma posición, generar una closedRoom
                GameObject closedRoomInstance = PhotonNetwork.Instantiate(templates.closedRoom.name, transform.position, Quaternion.identity);
                closedRoomInstance.transform.SetParent(transform.parent.parent); // Asignar el padre después de instanciar
                Destroy(gameObject);
                spawned = true;
                return;
            }
        }

        // Seleccionar el prefab de cuarto adecuado basado en la apertura
        switch (openSide)
        {
            case 1:
                rand = Random.Range(0, templates.bottomRooms.Length);
                room = PhotonNetwork.Instantiate(templates.bottomRooms[rand].name, transform.position, templates.bottomRooms[rand].transform.rotation);
                break;
            case 2:
                rand = Random.Range(0, templates.topRooms.Length);
                room = PhotonNetwork.Instantiate(templates.topRooms[rand].name, transform.position, templates.topRooms[rand].transform.rotation);
                break;
            case 3:
                rand = Random.Range(0, templates.leftRooms.Length);
                room = PhotonNetwork.Instantiate(templates.leftRooms[rand].name, transform.position, templates.leftRooms[rand].transform.rotation);
                break;
            case 4:
                rand = Random.Range(0, templates.rightRooms.Length);
                room = PhotonNetwork.Instantiate(templates.rightRooms[rand].name, transform.position, templates.rightRooms[rand].transform.rotation);
                break;
        }

        // Asignar el cuarto instanciado al objeto padre
        room.transform.SetParent(transform.parent.parent);

        // Construcción de la NavMesh para la sala generada
        navMeshSurface = room.GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }

        spawned = true;
    }
}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner_M>().spawned == false && spawned == false)
        {
            // Instanciamos la closedRoom en caso de colisión
            PhotonNetwork.Instantiate(templates.closedRoom.name, transform.position, Quaternion.identity, 0); // Usamos PhotonNetwork.Instantiate
            Destroy(gameObject);
        }
        spawned = true;
    }
}
