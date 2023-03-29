using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    //FUNCTIONS MADE TO BE USED AS ANIMATOR EVENTS

    //AudioSources
    public AudioSource movementAudioSource;
    public AudioSource combatAudioSource;

    //Movement Clips
    public AudioClip step;

    //Combat Clips
    public AudioClip[] swordAttack;
    public AudioClip[] takeDamage;
    public AudioClip defend;
    public AudioClip death;

    public void Step()
    {
        movementAudioSource.volume = 0.7f * GameController.controller.fxvalue;
        movementAudioSource.clip = step;
        movementAudioSource.Play();
    }

    public void SwordAttack()
    {
        combatAudioSource.volume = 0.2f * GameController.controller.fxvalue;
        combatAudioSource.clip = swordAttack[Random.Range(0, swordAttack.Length)];
        combatAudioSource.Play();
    }

    public void TakeDamage()
    {
        combatAudioSource.volume = 0.5f * GameController.controller.fxvalue;
        combatAudioSource.clip = takeDamage[Random.Range(0, takeDamage.Length)];
        combatAudioSource.Play();
    }

    public void Defend()
    {
        combatAudioSource.volume = 0.1f * GameController.controller.fxvalue;
        combatAudioSource.clip = defend;
        combatAudioSource.Play();
    }

    public void Die()
    {
        combatAudioSource.volume = 0.5f * GameController.controller.fxvalue;
        combatAudioSource.clip = death;
        combatAudioSource.Play();
    }
}
