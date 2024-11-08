using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunHandle : MonoBehaviour
{
    private GameObject[] players;
    public GameObject[] playerSprites;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(players);
        }
    }

    public void Run()
    {
        PanelHandler.ClosePanel();
        Flip(playerSprites);
        foreach(GameObject player in players)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.AddForce(player.transform.forward*-3f, ForceMode.Impulse);
        }
        StartCoroutine(BackToExplore());
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
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Exploracion");
    }
}
