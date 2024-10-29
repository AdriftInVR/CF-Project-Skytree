using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelSelection : MonoBehaviour
{
    private PlayerInput myInput;
    private bool rotationComplete = true;
    //public Transform targetRot;
    public static bool lockedRotation = false;
    private int cubes;
    private List<Transform> cubeTrans = new List<Transform>();
    public static int lastCube;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform){
            if(child.gameObject.activeSelf)
            {
                cubeTrans.Add(child);
                cubes++; 
            }
        }
        for (int i = 0; i<cubes; i++)
        {
            StartCoroutine(SpawnWheel(cubeTrans[(i + lastCube) % cubes], i));
        }
        myInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockedRotation && !CombatManager.menuOpen)
        {
            if (Mathf.Abs(CombatManager.direction.x)>0 && rotationComplete) {
                StartCoroutine(RotateWheel((int)Mathf.Sign(CombatManager.direction.x)));
            }
        }
    }

    IEnumerator RotateWheel(int direction)
    {
        rotationComplete = false;
        Transform targetRot = Instantiate(gameObject, transform.position, transform.rotation).transform;
        targetRot.SetParent(transform.parent);
        targetRot.gameObject.SetActive(false);
        targetRot.Rotate(Vector3.up,direction*(360f/cubes),Space.Self);
        float targetEuler = targetRot.eulerAngles.y;
        while (!rotationComplete){
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot.rotation, 0.11f);
            if ((Mathf.Abs(targetEuler - transform.eulerAngles.y)<1f&&Mathf.Abs(targetEuler - transform.eulerAngles.y)>-1f))
            {
                transform.rotation = targetRot.rotation;
                break;
            }
            yield return null;
        }
        rotationComplete = true;
        Destroy(targetRot.gameObject);
    }

    IEnumerator SpawnWheel(Transform cube, int i)
    {
        lockedRotation = true;
        bool spawned = false;
        bool lerping = false;
        Transform targetRot = Instantiate(cube.gameObject, cube.position, cube.rotation).transform;
        targetRot.SetParent(cube.parent);
        targetRot.gameObject.SetActive(false);
        targetRot.Rotate(Vector3.up,-i*(360f/cubes),Space.Self);
        float targetEuler = targetRot.eulerAngles.y;
        while (!spawned && i!=0){
            if(360-cube.localEulerAngles.y >= 180/cubes*i && !lerping && cube.localEulerAngles.y!=0)
            {
                lerping = true;
            }
            if(!lerping)
            {
                cube.Rotate(Vector3.up,-i,Space.Self);
            }
            else
            {
                cube.rotation = Quaternion.Lerp(cube.rotation, targetRot.rotation, 0.05f);
            }
            if (Mathf.Abs(targetEuler - cube.eulerAngles.y)<1f&&Mathf.Abs(targetEuler - cube.eulerAngles.y)>-1f)
            {
                cube.rotation = targetRot.rotation;
                break;
            }
            yield return null;
        }
        lockedRotation = false;
        Destroy(targetRot.gameObject);
        spawned = true;
        if(i == cubes-1)
        {
            PlayerController.locked = false;
        }
        yield return null;
    }
}
