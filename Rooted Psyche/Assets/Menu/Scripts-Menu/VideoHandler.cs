using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private TMPro.TextMeshProUGUI skipText; 
    private float timeToShowSkip = 5f;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;
        
        // Hide skip text at start
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false);
        }

        // Start coroutine to show skip text after delay
        StartCoroutine(ShowSkipText());
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

