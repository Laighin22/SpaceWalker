using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class finishLevel : MonoBehaviour
{
    public GameObject winTextObject;
    [SerializeField] private Transform player;
    public bool finalLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        winTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player finished");
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            GameStatus gameStatusScript = player.GetComponent<GameStatus>();
            if (gameStatusScript != null)
            {
                gameStatusScript.StopAllCoroutines();
            }

            if (finalLevel)
            {
                PlayerPrefs.SetFloat("finalLevelCompleted", 1);
                SceneManager.LoadScene("FinalCutScene");
            }
            else winTextObject.SetActive(true);

        }
    }
}
