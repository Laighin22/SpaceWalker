using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public enum MenuPage
{
    Home,
    Play,
    Levels,
    Wipe
}

public class Level
{
    public int id;
    public string name;
    public int maxProgress;

    public Level(int id, string name, int maxProgress)
    {
        this.id = id;
        this.name = name;
        this.maxProgress = maxProgress;
    }
}

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI balance;

    public MenuPage page;
    public AudioSource musicPlayer;
    public AudioClip mainMenuMusic;

    //GUI Elements for Main Menu
    public Button playButton;
    public Button quitButton;
    public GameObject playerAvatar;
    public GameObject playerShip;
    public GameObject podium;

    //GUI Elements for Play Menu
    public Button levelsButton;
    public Button WipeProgressButton;
    public Button storeButton;

    //GUI Elements for the level selector
    public TextMeshProUGUI levelID;
    public Button NextButton;
    public Button PreviousButton;
    public TextMeshProUGUI levelDesc;
    public Button BackButton;
    public Button StartButton;
    public Image levelIDBanner;

    //GUI Elements for the wipe progress button
    public Button yesButton;
    public Button noButton;

    public List<Level> levels = new List<Level>();
    public int levelSelectorIndex;

    //Info variables
    public PlayerInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {

        Home();

        levels.Add(new Level(0, "Level_0", 10));
        levels.Add(new Level(1, "Level_1", 10));
        levels.Add(new Level(2, "Level_2", 11));
        levels.Add(new Level(3, "Level_3", 28));
        levels.Add(new Level(4, "Final_Level", 0));

        levelSelectorIndex = 0;

        mainMenuMusic = Resources.Load<AudioClip>("Music/Eon - Ambient Sci Fi Music - (royalty free)");
        musicPlayer.clip = mainMenuMusic;
        musicPlayer.Play();
        musicPlayer.loop = true;
    }

    public void LoadPlayMenu()
    {
        page = MenuPage.Play;

        if (PlayerPrefs.GetFloat("playedCutScene", 0) != 1)
        {
            PlayerPrefs.SetFloat("playedCutScene", 1);
            SceneManager.LoadScene("IntroCutScene");
        }

        //Hide Level Selector GUI
        NextButton.interactable = false;
        NextButton.gameObject.SetActive(false);

        PreviousButton.interactable = false;
        PreviousButton.gameObject.SetActive(false);

        levelID.enabled = false;
        levelDesc.enabled = false;
        levelIDBanner.enabled = false;

        StartButton.gameObject.SetActive(false);
        StartButton.interactable = false;

        //Hide Home GUI
        quitButton.interactable = false;
        quitButton.gameObject.SetActive(false);
        playButton.interactable = false;
        playButton.gameObject.SetActive(false);

        //Hide Wiping Progress GUI
        yesButton.interactable = false;
        yesButton.gameObject.SetActive(false);
        noButton.interactable = false;
        noButton.gameObject.SetActive(false);


        //Show Play GUI
        levelsButton.interactable = true;
        levelsButton.gameObject.SetActive(true);
        WipeProgressButton.interactable = true;
        WipeProgressButton.gameObject.SetActive(true);
        storeButton.interactable = true;
        storeButton.gameObject.SetActive(true);
        BackButton.interactable = true;
        BackButton.gameObject.SetActive(true);

        playerAvatar.SetActive(true);
        LoadShip();
        podium.SetActive(true);

        balance.text = "Balance: " + playerInfo.GetCoins();
    }

    public void LoadLevelSelector()
    {
        page = MenuPage.Levels;

        //Hide main menu GUI
        levelsButton.interactable = false;
        levelsButton.gameObject.SetActive(false);

        quitButton.interactable = false;
        quitButton.gameObject.SetActive(false);

        playerAvatar.SetActive(false);
        playerShip.SetActive(false);
        podium.SetActive(false);

        //Hide Play GUI
        levelsButton.interactable = false;
        levelsButton.gameObject.SetActive(false);
        WipeProgressButton.interactable= false;
        WipeProgressButton.gameObject.SetActive(false);
        storeButton.interactable = false;
        storeButton.gameObject.SetActive(false);

        //Show Level Selector GUI
        NextButton.interactable = true;
        NextButton.gameObject.SetActive(true);

        PreviousButton.interactable = true;
        PreviousButton.gameObject.SetActive(true);

        levelID.enabled = true;
        levelDesc.enabled = true;
        levelIDBanner.enabled=true;

        StartButton.gameObject.SetActive(true);
        StartButton.interactable = true;

        BackButton.gameObject.SetActive(true);
        BackButton.interactable = true;

        balance.text = "Balance: " + playerInfo.GetCoins();

        //StartCoroutine(DisplayLevelID("Level " + levels[levelSelectorIndex].id.ToString()));
        levelID.text = "Level " + levels[levelSelectorIndex].id.ToString();
        levelDesc.text = GetLevelProgress();

        if (levels[levelSelectorIndex].name == "Final_Level")
        {
            levelID.text = "Final Level";
            if (!playerInfo.HasFullShip())
            {
                StartButton.interactable = false;
                levelDesc.text = "WARNING: Star Sparrow ship is incomplete.\nCannot perform take off.";
            }
        }
    }

    public void Home()
    {
        page = MenuPage.Home;

        //Hide Level Selector GUI
        NextButton.interactable = false;
        NextButton.gameObject.SetActive(false);

        PreviousButton.interactable = false;
        PreviousButton.gameObject.SetActive(false);

        levelID.enabled = false;
        levelDesc.enabled = false;
        levelIDBanner.enabled = false;

        StartButton.gameObject.SetActive(false);
        StartButton.interactable = false;

        BackButton.gameObject.SetActive(false);
        BackButton.interactable = false;

        //Hide Play GUI
        levelsButton.interactable = false;
        levelsButton.gameObject.SetActive(false);
        WipeProgressButton.interactable = false;
        WipeProgressButton.gameObject.SetActive(false);
        storeButton.interactable = false;
        storeButton.gameObject.SetActive(false);

        //Hide WipeProgress GUI
        yesButton.interactable = false;
        yesButton.gameObject.SetActive(false);
        noButton.interactable = false;
        noButton.gameObject.SetActive(false);
        levelDesc.enabled = false;

        //Show main menu GUI
        playButton.interactable = true;
        playButton.gameObject.SetActive(true);

        quitButton.interactable = true;
        quitButton.gameObject.SetActive(true);

        playerAvatar.SetActive(true);
        LoadShip();
        podium.SetActive(true);

        balance.text = "Balance: " + playerInfo.GetCoins();
    }

    public void NextLevel()
    {
        levelSelectorIndex++;
        if (levelSelectorIndex==levels.Count)
        {
            levelSelectorIndex = 0;
        }
        LoadLevelSelector();
    }

    public void PreviousLevel()
    {
        levelSelectorIndex--;
        if (levelSelectorIndex < 0)
        {
            levelSelectorIndex = levels.Count - 1;
        }
        LoadLevelSelector();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        string level = levels[levelSelectorIndex].name;
        if (SceneManager.GetSceneByName(level) != null)
        {
            SceneManager.LoadScene(level);
        }
        else Debug.Log("Level does not exist.");
    }

    void LoadShip()
    {
        playerShip.SetActive(true);
        foreach (Transform child in playerShip.transform)
        {
            child.gameObject.SetActive(false);
        }
        if (playerInfo!=null)
        {
            if (playerInfo.HasExtraCoins())
            {
                playerShip.transform.Find("StarSparrow_Core").gameObject.SetActive(true);
            }
            if (playerInfo.HasJumpBoost())
            {
                playerShip.transform.Find("StarSparrow_Wings").gameObject.SetActive(true);
            }
            if (playerInfo.HasSpeedBoost())
            {
                playerShip.transform.Find("StarSparrow_Thruster").gameObject.SetActive(true);
            }
        }
    }

    public void Back()
    {
        switch (page)
        {
            case MenuPage.Play:
                Home();
                break;
            case MenuPage.Levels:
                LoadPlayMenu();
                break;
            case MenuPage.Wipe:
                LoadPlayMenu();
                break;
        }
    }

    public void LoadStore()
    {
        SceneManager.LoadScene("Store_Mk3");
    }

    private string GetLevelProgress()
    {
        string levelKey = levels[levelSelectorIndex].name;
        if (levelKey=="Final_Level")
        {
            if (PlayerPrefs.GetFloat("finalLevelCompleted", 0) != 1)
            {
                return "Status: Incomplete";
            }
            else return "Status: Complete";
        }
        float percent = ((float)PlayerPrefs.GetInt(levelKey, 0) / levels[levelSelectorIndex].maxProgress)*100;
        return "Progress: " + System.Math.Round(percent, 2) + "%";
    }

    public void DoubleCheck()
    {
        page = MenuPage.Wipe;

        BackButton.interactable = false;
        BackButton.gameObject.SetActive(false);

        levelsButton.interactable = false;
        levelsButton.gameObject.SetActive(false);

        storeButton.interactable = false;
        storeButton.gameObject.SetActive(false);

        WipeProgressButton.interactable = false;
        WipeProgressButton.gameObject.SetActive(false);

        yesButton.interactable = true;
        yesButton.gameObject.SetActive(true);

        noButton.interactable = true;
        noButton.gameObject.SetActive(true);

        levelDesc.enabled = true;
        levelDesc.text = "Are you sure you want to wipe your progress? Any saved data will be lost and this can't be undone.";

        balance.text = "Balance: " + playerInfo.GetCoins();
    }

    public void DeleteProgress()
    {
        PlayerPrefs.DeleteAll();
        playerInfo.Reset();
        Home();
    }
}
