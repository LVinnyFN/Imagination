using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{

    //AudioSource
    public AudioSource combatAudioSource;

    //AudioClips
    public AudioClip idle;
    public AudioClip attack;
    public AudioClip spotPlayer;
    public AudioClip takeDamage;
    public AudioClip die;

    private void Start()
    {
        combatAudioSource = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        combatAudioSource.volume = 0.2f * GameController.controller.fxvalue;
        combatAudioSource.clip = attack;
        combatAudioSource.Play();
    }

    public void SpotPlayer()
    {
        combatAudioSource.volume = 0.3f * GameController.controller.fxvalue;
        combatAudioSource.clip = spotPlayer;
        combatAudioSource.Play();
    }

    public void TakeHit()
    {
        combatAudioSource.volume = 0.2f * GameController.controller.fxvalue;
        combatAudioSource.clip = takeDamage;
        combatAudioSource.Play();
    }

    public void Die()
    {
        combatAudioSource.volume = 0.2f * GameController.controller.fxvalue;
        combatAudioSource.clip = die;
        combatAudioSource.Play();
    }
}
