using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, 0);
    }
}
