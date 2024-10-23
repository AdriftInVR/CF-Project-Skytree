using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int score;

     // Add UI components
    [Header("UI Elements")]
    public GameObject notificationPanel; // Assign in inspector
    public Text notificationText; // Assign in inspector
    public float notificationDuration = 3f; // How long the notification stays visible

    
    // Guardar una referencia al delegado del evento
    private UnityAction<Scene, LoadSceneMode> sceneLoadedAction;

    // Clave de encriptación AES (debe ser de 16,24 o 32 caracteres)
    private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("4PPC0D320164G012");
    private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("4PPC0D320164G012");

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Hide notification panel at start
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        } 
    }
    
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            // Leer y desencriptar los datos
            byte[] encryptedData = File.ReadAllBytes(path);
            string json = DecryptStringFromBytes_Aes(encryptedData);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            // Asignar la función a la referencia
            sceneLoadedAction = (Scene scene, LoadSceneMode mode) =>
            {
                StartCoroutine(SetPlayerPosition(loadedData));
            };

            // Suscribirse al evento
            SceneManager.sceneLoaded += sceneLoadedAction;

            // Cargar la escena guardada
            SceneManager.LoadScene(loadedData.sceneName);
        }
        else
        {
            // Show notification when no save file exists
            ShowNotification("¡No existe una partida guardada!");
            Debug.LogWarning("Save file not found in " + path);
        }
    }

    // New method to show notifications
    private void ShowNotification(string message)
    {
        if (notificationPanel != null && notificationText != null)
        {
            notificationText.text = message;
            notificationPanel.SetActive(true);
            StartCoroutine(HideNotificationAfterDelay());
        }
        else
        {
            Debug.LogWarning("Notification UI elements are not assigned in the inspector!");
        }
    }

    // Coroutine to hide the notification after a delay
    private IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDuration);
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }

    private IEnumerator SetPlayerPosition(PlayerData loadedData)
    {
        yield return null; // Esperar un frame para asegurarse de que la escena está cargada

        GameObject player = GameObject.FindWithTag("Skyler");
        if (player != null)
        {
            playerTransform = player.transform;
            playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);

            Debug.Log("Game loaded and player repositioned.");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }

        // Desuscribirse del evento después de reposicionar al jugador
        SceneManager.sceneLoaded -= sceneLoadedAction;
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
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Skyler");  // Buscar el jugador por su tag si aún no ha sido asignado
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found in the scene.");
                return;
            }
        }

        Debug.Log("Player data has been reset.");
    }

}

