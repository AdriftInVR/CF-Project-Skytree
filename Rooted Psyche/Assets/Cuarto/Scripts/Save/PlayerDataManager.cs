using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int score;

    // Clave de encriptación AES (debe ser de 16,24 o 32 caracteres)
    private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("4PPC0D320164G012");
    private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("4PPC0D320164G012");

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            // Leer archivo encriptado
            byte[] encryptedData = File.ReadAllBytes(path);

            // Desencriptar los datos
            string json = DecryptStringFromBytes_Aes(encryptedData);

            // Convertir de JSON a PlayerData
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            // Registrar el evento que se llama cuando la escena ha terminado de cargar
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                // Buscar el jugador una vez que la escena ha sido cargada
                StartCoroutine(SetPlayerPosition(loadedData));
            };

            // Cargar la escena guardada
            SceneManager.LoadScene(loadedData.sceneName);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
        }
    }

    private IEnumerator SetPlayerPosition(PlayerData loadedData)
    {
        // Esperar un frame para asegurarse de que todo ha sido inicializado
        yield return null;

        // Buscar el GameObject del jugador en la escena cargada
        GameObject player = GameObject.FindWithTag("Player"); // Asegúrate de que el Player tiene el tag "Player"
        if (player != null)
        {
            playerTransform = player.transform;

            // Actualizar posición del jugador
            Vector3 loadedPosition = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = loadedPosition;

            Debug.Log("Game loaded and player repositioned.");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }

        // Una vez que la escena y el jugador están listos, desuscribirse del evento
        SceneManager.sceneLoaded -= (Scene scene, LoadSceneMode mode) => StartCoroutine(SetPlayerPosition(loadedData));
    }


    public void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };
        playerData.sceneName = SceneManager.GetActiveScene().name;  // Guardar el nombre de la escena

        // Convertir datos a JSON
        string json = JsonUtility.ToJson(playerData);

        // Encriptar el JSON
        byte[] encryptedData = EncryptStringToBytes_Aes(json);

        // Guardar el archivo encriptado
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllBytes(path, encryptedData);

        Debug.Log("Game saved (encrypted).");
    }



    // Encriptar una cadena utilizando AES
    private static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

    // Desencriptar una cadena utilizando AES
    private static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    public void ResetPlayerData()
    {
        // Si el playerTransform no ha sido asignado, buscar el jugador por su tag
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found in the scene.");
                return;  // No seguir si no se encontró el jugador
            }
        }

        // Resetear los datos del jugador
        playerTransform.position = Vector3.zero;  // O la posición inicial deseada
        score = 0;  // Restablecer el puntaje
        Debug.Log("Player data has been reset.");
    }
}

