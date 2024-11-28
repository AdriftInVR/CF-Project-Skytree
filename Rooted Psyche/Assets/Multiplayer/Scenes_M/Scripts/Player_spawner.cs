using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public float spawnTime; // Tiempo de espera para spawnear
    private float timer;
    private bool hasPlayerSpawned = false;

    public string timberPrefabPath;
    public string brierPrefabPath;


    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime && !hasPlayerSpawned)
        {
            SpawnPlayer();
            hasPlayerSpawned = true; // Asegúrate de que el jugador solo sea spawneado una vez
        }
    }

    private void SpawnPlayer()
    {
        string prefabPath;

        // Decide el prefab basado en el número de jugador en el cuarto
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            prefabPath = timberPrefabPath; // Primer jugador: Timber
        }
        else
        {
            prefabPath = brierPrefabPath; // Segundo jugador: brier
        }

        // Instancia el prefab correspondiente
        PhotonNetwork.Instantiate(prefabPath, new Vector3(3.5f, 1, 5), Quaternion.identity, 0);
    }
}
