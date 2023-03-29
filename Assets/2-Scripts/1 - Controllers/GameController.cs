using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameController : MonoBehaviour
{
    //GameConfig
    public Texture2D meucursor;
    public TMP_Dropdown resolutiondrop;
    public TMP_Dropdown qualitydrop;
    public Toggle windowmodetoggle;
    private List<string> resolutions = new List<string>();
    private List<string> quality = new List<string>();


    //Audio
    public AudioSource generalaudiosorce;
    public AudioClip[] myaudios;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider fxVolume;
    public float mastervalue;
    public float musicvalue;
    public float fxvalue;

    //Outros
    public float difficulty = 1;
    public static GameController controller;
    public UiController uiController;
    public GlobalStats globalStats;
    public GameObject player;
    public RespawnController respawnController;
    public bool tutorialdone = false;

    //Savedata
    public PlayerInfo playerData;
    public int savenumber = 0;
    public string playername;
    public int playersex = 1;

    private void Awake()
    {
        if (GameController.controller!=null)
        {
            Destroy(this.gameObject);
        }
        else
        {
        controller = this;
        }
    }

    public GameObject logopuc;

    void Start()
    {
        playerData = new PlayerInfo();
        Cursor.SetCursor(meucursor, new Vector2(5, 0), CursorMode.Auto);
        DontDestroyOnLoad(this);
        if (logopuc!=null)
        {
        logopuc.SetActive(true);
        }

    }

    private void Update()
    {
        if (masterVolume != null)
        {
            mastervalue = masterVolume.value;
            AudioListener.volume = mastervalue;
        }
        if (musicVolume != null)
        {
            musicvalue = musicVolume.value;
            transform.GetChild(0).GetComponent<AudioSource>().volume = musicvalue;
        }
        if (fxVolume != null)
        {
            fxvalue = fxVolume.value;
        }
    }

    public void SetConfigInfo()
    {
        Resolution[] arrResolution = Screen.resolutions;
        foreach (Resolution r in arrResolution)
        {
            resolutions.Add(string.Format("{0} X {1}", r.width, r.height));
        }
        resolutiondrop.AddOptions(resolutions);
        resolutiondrop.value = (resolutions.Count - 1);

        quality = QualitySettings.names.ToList<string>();
        qualitydrop.AddOptions(quality);
        qualitydrop.value = QualitySettings.GetQualityLevel();
    }

    public void SetWindowMode()
    {
        if (windowmodetoggle.isOn)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    public void SetResolution()
    {
        string[] res = resolutions[resolutiondrop.value].Split('X');
        int w = Convert.ToInt16(res[0].Trim());
        int h = Convert.ToInt16(res[1].Trim());

        Screen.SetResolution(w, h, Screen.fullScreen);
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(qualitydrop.value, true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}









