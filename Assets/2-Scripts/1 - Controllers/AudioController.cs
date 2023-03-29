using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] myaudios;
    public static AudioController controller;
    public static AudioSource audiosource;

    private void Start()
    {
        controller = this;
        audiosource = GetComponent<AudioSource>();
    }

    public void playaudio(int index, float volume)
    {
        audiosource.clip = myaudios[index];
        audiosource.volume = volume * GameController.controller.fxvalue;
        audiosource.Play();
    }



}
