using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

public static class SaveSystem
{
    // Rutas de guardado
    public static readonly string savePathSlot1 = Application.persistentDataPath + "/gameDataSlot1.save";


    // Clave de encritación AES (debe ser de 16,24 o 34 caracteres)
    private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("4PPC0D320164G012");
    private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("4PPC0D320164G012");

    // Guardar datos en un archivo
    public static IEnumerator SaveGame(GameData data, string path)
    {
        // Convertir los datos a formato JSON
        string json = JsonUtility.ToJson(data);
        // Encriptar los datos
        byte[] encryptedData = EncryptStringToBytes_Aes(json);
        // Guardar los datos en un archivo
        File.WriteAllBytes(path, encryptedData);
        Debug.Log("Game saved");
        yield return null;
    }

    // Cargar datos de un archivo
    public static IEnumerator LoadGame(GameData data, string path)
    {
        byte[] encryptedData;

        // Cargar los datos del archivo
        encryptedData = File.ReadAllBytes(path);

        if (encryptedData != null)
        {

            // Desencriptar los datos con AES
            string decryptedData = DecryptStringFromBytes_Aes(encryptedData);
            // Convertir los datos a formato JSON
            JsonUtility.FromJsonOverwrite(decryptedData, data);

            Debug.Log("Game loaded");   
        }
        yield return null;
    }

    // Encriptar una cadena de texto utilizando AES
    private static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;
        // Crear un objeto AES con la clave y el vector de inicialización
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;
            // Crear un objeto de cifrado para encriptar los datos
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Crear un flujo de memoria para almacenar los datos encriptados
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // Crear un flujo de cifrado para escribir los datos encriptados
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // Crear un escritor para escribir los datos encriptados
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Escribir los datos encriptados
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        // Devolver los datos encriptados
        return encrypted;
    }

    private static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;
        // Crear un objeto AES con la clave y el vector de inicialización
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;
            // Crear un objeto de cifrado para desencriptar los datos
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Crear un flujo de memoria para almacenar los datos desencriptados
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                // Crear un flujo de cifrado para leer los datos encriptados
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    // Crear un lector para leer los datos desencriptados
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Leer los datos desencriptados
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        // Devolver los datos desencriptados
        return plaintext;
    }
}
