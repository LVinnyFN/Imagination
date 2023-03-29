using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{

    [SerializeField] private Slider mastervolume;
    [SerializeField] private Slider musicvolume;
    [SerializeField] private Slider fxvolume;

    private void Start()
    {
        GameController.controller.masterVolume = mastervolume;
        GameController.controller.musicVolume = musicvolume;
        GameController.controller.fxVolume = fxvolume;
        mastervolume.value = GameController.controller.mastervalue;
        musicvolume.value = GameController.controller.musicvalue;
        fxvolume.value = GameController.controller.fxvalue;
    }

}
