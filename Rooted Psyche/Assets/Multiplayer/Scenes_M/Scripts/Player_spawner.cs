using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Player_spawner : MonoBehaviour
{

    public float SpawnTime;
    float timer;
    bool HasPlayerSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
       // PhotonNetwork.Instantiate("Skyler_M", new Vector3(3.5f, 1, 5), Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= SpawnTime)
        {
            if (!HasPlayerSpawned)
            {
                PhotonNetwork.Instantiate("Skyler_M", new Vector3(3.5f, 1, 5), Quaternion.identity, 0);
                HasPlayerSpawned = true;
            }

            timer = 1;
        }
    }
}
