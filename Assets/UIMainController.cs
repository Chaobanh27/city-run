using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMainController : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;

    [Space]
    [SerializeField] private TextMeshProUGUI latestScoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    public static int Number = 0;

    [Header("Volume Slider")]
    [SerializeField] private VolumeSliderController[] sliders;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image inGameMuteIcon;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].SetupSlider();
        }

        SwitchMenuTo(mainMenu);

        latestScoreText.text = "Latest Score: " + PlayerPrefs.GetFloat("LatestScore").ToString("#,#"); 
        highestScoreText.text = "Highest Score: " + PlayerPrefs.GetFloat("HighestScore").ToString("#,#");
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
        AudioManagerController.instance.PlaySFX(1);
    }
    public void SwitchSkyBoxTo(int index)
    {
        AudioManagerController.instance.PlaySFX(1);
        GameManager.instance.SwitchSkyBox(index);
    }
    public void StartGameBTN()
    {
        muteIcon = inGameMuteIcon;
        if (gameMuted) 
        {
            muteIcon.color = new Color(1, 1, 1, .5f);
        }
        GameManager.instance.UnlockPlayer();
    }
    public void PausedGameBTN()
    {
        if(gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused=true;
        }
    }

    public void RestartGameBTN()
    {
        GameManager.instance.RestartLevel();
    }

    public void OpenEndGameUI()
    {
        SwitchMenuTo(endGame);
    }

    public void MuteBTN()
    {
        gameMuted = !gameMuted;
        if(gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, .5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }

}
