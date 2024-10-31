using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
    }
}
