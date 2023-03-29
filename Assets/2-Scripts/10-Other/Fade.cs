using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    [SerializeField] private SaveController savecontroller;
    public Animator animator;
    public LoadingScreen loadingscreen;
    private int leveltochange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SimpleFade();
        }
    }

    public void SimpleFade()
    {
        animator.SetTrigger("SimpleFadeOut");
    }

    public void StartFading(int index)
    {
        leveltochange = index;
        animator.SetTrigger("FadeOut");
    }

    public void ReloadFade()
    {
        animator.SetTrigger("ReloadFadeOut");
    }

    public void OnFadeComplete()
    {
        loadingscreen.ChangeScene(leveltochange);
    }

    public void NewFadeComplete()
    {
        savecontroller.ReloadGame();
    }
}
