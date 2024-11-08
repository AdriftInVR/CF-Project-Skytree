using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunHandle : MonoBehaviour
{
    public GameObject[] players;
    public GameObject[] playerSprites;
    public static bool running;
    public static bool canRun = true;

    public void RunSetup()
    {
        running = true;
        PanelHandler.ClosePanel();
        Flip(playerSprites);
        PlayerController.locked = true;
        StartCoroutine(BackToExplore());
        StartCoroutine(RunAway());
    }

    void Flip(GameObject[] sprites)
    {
        foreach(GameObject sprite in sprites)
        {
            Vector3 selfScale = sprite.transform.localScale;
            selfScale.x *= -1;
            sprite.transform.localScale = selfScale;
        }
    }

    IEnumerator BackToExplore()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Exploracion");
    }

    IEnumerator RunAway()
    {
        while(true)
        {
            foreach(GameObject player in players)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.AddForce(player.transform.right*-1f, ForceMode.Impulse);
                if(player.transform.position.x < -30f)
                {
                    running = false;
                    break;
                }
            }
            yield return null;
        }
    }
}
