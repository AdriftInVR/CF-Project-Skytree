using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using System;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private Text skipText; 
    private float timeToShowSkip = 5f;

    [SerializeField] private string videoName; // Nombre del video sin extensión (configurable en el Inspector)

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;

        // Configurar la ruta del video según la plataforma
        SetVideoPath();

        // Ocultar el texto de "saltar" al inicio
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false);
        }

        // Iniciar la corrutina para mostrar el texto de "saltar" después de un tiempo
        StartCoroutine(ShowSkipText());
    }

    private void SetVideoPath()
    {
        // Ruta del video específica para WebGL y otras plataformas
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");

#if UNITY_WEBGL
        videoPlayer.url = videoPath; // Para WebGL
#else
        videoPlayer.url = "file://" + videoPath; // Para otras plataformas
#endif

        Debug.Log($"Video configurado: {videoPlayer.url}");
    }

    private IEnumerator ShowSkipText()
    {
        yield return new WaitForSeconds(timeToShowSkip);

        if (skipText != null)
        {
            skipText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    private void SkipVideo()
    {
        videoPlayer.Stop();
        videoPlayer.time = videoPlayer.length;
        ChangeScene();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        ChangeScene();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Pantalla_Carga");
    }
}
