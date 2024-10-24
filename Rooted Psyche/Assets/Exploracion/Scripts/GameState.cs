using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameState
{
    public List<Vector3> enemyPositions;
    public List<Vector3> roomPositions;
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    void Start()
    {
        if (gameState == null)
        {
            gameState = new GameState();
        }
    }

    public void SaveGameState(List<GameObject> enemies, List<GameObject> rooms)
    {
        gameState.enemyPositions = new List<Vector3>();
        gameState.roomPositions = new List<Vector3>();

        foreach (var enemy in enemies)
        {
            gameState.enemyPositions.Add(enemy.transform.position);
        }

        foreach (var room in rooms)
        {
            gameState.roomPositions.Add(room.transform.position);
        }
    }

    public void LoadGameState(List<GameObject> enemies, List<GameObject> rooms)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = gameState.enemyPositions[i];
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].transform.position = gameState.roomPositions[i];
        }
    }

    /*public void ChangeToCombatScene()
    {
        SaveGameState(FindObjectsOfType<Enemy>().ToList(), FindObjectsOfType<Room>().ToList());
        SceneManager.LoadScene("CombatScene");
    }

    public void ReturnToGenerationScene()
    {
        SceneManager.LoadScene("GenerationScene");
        LoadGameState(FindObjectsOfType<Enemy>().ToList(), FindObjectsOfType<Room>().ToList());
    }*/
}