using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    public string coinID;
    private string PlayerPrefsID;
    private string levelID;
    private ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        levelID = SceneManager.GetActiveScene().name;
        particleSystem = GetComponent<ParticleSystem>();
        PlayerPrefsID = levelID + coinID;
        if (PlayerPrefs.GetInt(PlayerPrefsID, 0) == 1)
        {
            gameObject.SetActive(false);
        } 
    }

    public void Save()
    {
        PlayerPrefs.SetInt(PlayerPrefsID, 1);
        //PlayerPrefs.SetFloat(levelID, )
        //gameObject.SetActive(false);
    }
}
