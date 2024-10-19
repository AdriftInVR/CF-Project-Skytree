using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class PlayerDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int score;

    // Clave de encriptación AES (debe ser de 16,24 o 32 caracteres)
    private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("4PPC0D320164G012");
    private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("4PPC0D320164G012");

    // Guardar datos
    public void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };

        // Convertir datos a JSON
        string json = JsonUtility.ToJson(playerData);

        // Encriptar el JSON
        byte[] encryptedData = EncryptStringToBytes_Aes(json);

        // Guardar el archivo encriptado
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllBytes(path, encryptedData);

        Debug.Log("Game saved (encrypted).");
    }

    // Cargar datos
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

            // Actualizar posición del jugador
            Vector3 loadedPosition = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = loadedPosition;

            Debug.Log("Game loaded (decrypted).");
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
        }
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
}

