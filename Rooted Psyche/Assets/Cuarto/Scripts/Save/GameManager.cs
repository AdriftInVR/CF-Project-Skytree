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
            File.Delete(path);  // Borrar archivo de datos guardados
            Debug.Log("Previous save file deleted.");
        }

        // 2. Restablecer las variables del juego, como el puntaje y posición del jugador
        playerDataManager.ResetPlayerData();

        // 3. Cargar la escena inicial de juego (puedes cambiar el nombre de la escena según necesites)
        SceneManager.LoadScene("CuartoSkyler");  // Cambia esto por el nombre de tu escena inicial
    }

}


