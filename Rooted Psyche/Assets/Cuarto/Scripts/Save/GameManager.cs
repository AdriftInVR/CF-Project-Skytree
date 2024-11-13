using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public PlayerDataManager playerDataManager;  // Referencia al PlayerDataManager

    public void NewGame()
    {
        // 1. Eliminar los datos guardados anteriores
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Previous save file deleted.");
        }

        GameObject objToRemove = GameObject.Find("EXP");
        if (objToRemove != null) {
                Destroy(objToRemove);
            }

        // 2. Restablecer los datos del jugador
        playerDataManager.ResetPlayerData();

        // 3. Cargar la escena inicial del juego
        SceneManager.LoadScene("CuartoSkyler");
    }

}


