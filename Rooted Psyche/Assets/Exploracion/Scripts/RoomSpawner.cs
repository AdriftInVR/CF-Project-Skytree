using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openSide;
    // 1 --> Bottom Door
    // 2 --> Top Door
    // 3 --> Left Door
    // 4 --> Right Door
    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    private GameObject room;
    private Vector3 position; 

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        StartCoroutine(SpawnWithDelay());
        position = transform.position;
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(templates.spawnDelay);  // Espera el tiempo definido
        Spawn();
    }

    void Spawn()
    {
        if (!spawned)
        {
            switch (openSide)
            {
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, transform.parent.parent);
                    break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    room = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, transform.parent.parent);
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    room = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, transform.parent.parent);
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    room = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, transform.parent.parent);
                    break;
                default:
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpawnPoint")
        {
            if (!spawned && !other.GetComponent<RoomSpawner>().spawned)
            {
                Instantiate(templates.closedRoom, position, Quaternion.identity);
                StartCoroutine(SelfDestruct());
            }
            spawned = true;
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}