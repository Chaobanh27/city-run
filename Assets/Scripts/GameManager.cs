using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIMainController uiMainController;
    public Color platformColor;
    public PlayerController playerController;

    [Header("Score")]
    public int coins;
    public float distance;
    public float score;

    [Header("Skybox Material")]
    [SerializeField] private Material[] SkyboxMaterial;



    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        //LoadColor();

        SetupSkyBox(PlayerPrefs.GetInt("SkyboxSetting"));
    }

    private void Update()
    {
        if(playerController.transform.position.x > distance)
        {
            distance = playerController.transform.position.x;
        }
    }

    public void SwitchSkyBox(int index)
    {
        AudioManagerController.instance.PlaySFX(1);
        GameManager.instance.SetupSkyBox(index);
    }

    public void SetupSkyBox(int i)
    {
        if(i <= 1)
        {
            RenderSettings.skybox = SkyboxMaterial[i];
        }
        else
        {
            RenderSettings.skybox = SkyboxMaterial[Random.Range(0, SkyboxMaterial.Length)];
        }

        PlayerPrefs.SetInt("SkyboxSetting", i);
    }

    //Phương thức này lưu trữ màu sắc của người chơi
    public void SaveColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("PlayerColorR", r);
        PlayerPrefs.SetFloat("PlayerColorG", g);
        PlayerPrefs.SetFloat("PlayerColorB", b);
    }

    //Phương thức này tải màu sắc đã lưu và áp dụng màu sắc cho đối tượng người chơi
    public void LoadColor()
    {
        SpriteRenderer playerSR = playerController.GetComponent<SpriteRenderer>();

        Color color = new Color(PlayerPrefs.GetFloat("ColorR"),
                                PlayerPrefs.GetFloat("ColorR"),
                                PlayerPrefs.GetFloat("ColorR"),
                                PlayerPrefs.GetFloat("ColorA",1));

        //Cập nhật màu sắc của SpriteRenderer của player với màu mới
        playerSR.color = color;
    }

    public void RestartLevel()
    {
        SaveInfo();
        SceneManager.LoadScene(0);
    }

    public void UnlockPlayer()
    {
        playerController.playerUnlocked = true;
    }

    //phương thức này lưu trữ thông tin trong game của người chơi
    public void SaveInfo()
    {
        //Lấy số tiền đã lưu trữ từ PlayerPrefs với khóa "Coins"
        int savedCoins = PlayerPrefs.GetInt("Coins");

        //Cập nhật số tiền mới bằng cộng số tiền đã lưu trữ với số tiền mới (biến coins)
        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        //tính điểm dựa trên khoảng cách(distance) và số tiền(coins)
        score = distance * coins;

        //Lưu điểm cuối cùng vào PlayerPrefs với khóa "LastScore"
        PlayerPrefs.SetFloat("LatestScore", score);

        //So sánh điểm cao nhất đã lưu với điểm mới tính được
        if (PlayerPrefs.GetFloat("HighestScore") < score)
        {
            //Cập nhật điểm cao nhất trong PlayerPrefs
            PlayerPrefs.SetFloat("HighestScore", score);
        }
    }

    public void GameEnded()
    {
        SaveInfo();
        uiMainController.OpenEndGameUI();
    }
}
