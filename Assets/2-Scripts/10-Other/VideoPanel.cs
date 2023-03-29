using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoPanel : MonoBehaviour
{
    public TMP_Dropdown resolutiondrop;
    public TMP_Dropdown qualitydrop;
    public Toggle windowmodetoggle;

    void Start()
    {
        GameController.controller.resolutiondrop = resolutiondrop;
        GameController.controller.qualitydrop = qualitydrop;
        GameController.controller.windowmodetoggle = windowmodetoggle;
        GameController.controller.SetConfigInfo();
    }

    public void SetWindowMode()
    {
        GameController.controller.SetWindowMode();
    }

    public void SetResolution()
    {
        GameController.controller.SetResolution();
    }

    public void SetQuality()
    {
        GameController.controller.SetQuality();
    }
}
