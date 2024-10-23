using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadNotification : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;
    
    [Header("Settings")]
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float fadeOutTime = 1f;
    
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        // Get or add CanvasGroup component
        canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = notificationPanel.AddComponent<CanvasGroup>();
            
        // Hide notification at start
        notificationPanel.SetActive(false);
    }
    
    public void ShowSaveNotification()
    {
        // Stop any running notifications
        StopAllCoroutines();
        
        // Show and start the notification sequence
        notificationPanel.SetActive(true);
        notificationText.text = "No existe un archivo de guardado en la ruta especificada.";
        canvasGroup.alpha = 1f;
        
        StartCoroutine(HideNotificationAfterDelay());
    }
    
    private IEnumerator HideNotificationAfterDelay()
    {
        // Wait for display time
        yield return new WaitForSeconds(displayTime);
        
        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            yield return null;
        }
        
        // Hide the panel
        notificationPanel.SetActive(false);
    }
}
