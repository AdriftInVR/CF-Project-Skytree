using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
    }
}