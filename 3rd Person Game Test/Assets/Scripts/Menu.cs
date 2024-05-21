using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    static int CurrentLevel = 0;
    public PlayerInfo playerInfo;
    public CharacterMovement characterMovement;
    public Transform coinParent;
    public AudioSource musicPlayer;
    public AudioClip music;
    public bool finalLevel;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name=="Level_3")
        {
            music = Resources.Load<AudioClip>("Music/One Cosmos  Royalty Free Sci-Fi Background Music (No Copyright)");
            musicPlayer.clip = music;
            musicPlayer.Play();
            musicPlayer.loop = true;
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        //Save their coin count
        string nextLevel = "";
        if (SceneManager.GetActiveScene().name != "Store_Mk3")
        {
            nextLevel = "Store_Mk3";
            if (!finalLevel)
            {
                playerInfo.SetCoins(playerInfo.GetCoins() + characterMovement.TempCoinCount);
                SaveCoins();
            }
        }
        else
        {
            nextLevel = "MainMenu";
        }
        SceneManager.LoadScene(nextLevel);
    }

    public string GetNextLevelName()
    {
        CurrentLevel++;
        string name = "Level_" + CurrentLevel;

        return name;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveCoins()
    {
        string levelKey = SceneManager.GetActiveScene().name;
        int progress = 0;
        foreach (Transform child in coinParent)
        {
            if (child.GetComponent<Coin>()!=null)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.GetComponent<Coin>().Save();
                    progress++;
                }
            }
        }
        if (progress <= coinParent.childCount)
        {
            PlayerPrefs.SetInt(levelKey, progress);
            PlayerPrefs.Save();
        }
    }
}
