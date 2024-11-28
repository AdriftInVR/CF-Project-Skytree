using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    private static SceneMusicPlayer instance;

    void Awake()
    {
        // Check if an instance of this script already exists
        if (instance != null)
        {
            // If it exists, destroy the new one to avoid duplicates
            Destroy(gameObject);
            return;
        }

        // Set this as the instance and prevent destruction on scene load
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Start playing music
        AudioSource audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true; // Loop the music
            audioSource.Play();
        }
    }
}
