using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PortalPrincipal : MonoBehaviour
{
    [SerializeField] private Transform teleportposition;
    [SerializeField] private GameObject player;
    [SerializeField] private PostProcessVolume ppv;
    [SerializeField] private Fade fade;
    [SerializeField] private ZoneEnter zoneenter;
    private LensDistortion lens;
    private Bloom bloom;

    private void Start()
    {
        ppv.profile.TryGetSettings(out lens);
        ppv.profile.TryGetSettings(out bloom);
    }
    public void TeleportPlayer()
    {
        zoneenter.DeliverAll();
        StartCoroutine(Teleport());
        GameController.controller.uiController.UseMouse(false);
    }

    IEnumerator Teleport()
    {
        fade.SimpleFade();
        for (float i = 0; i < 1; i+=Time.deltaTime)
        {
            lens.intensity.value = -i*100;
            bloom.intensity.value = i*100;
            yield return new WaitForEndOfFrame();
        }
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = teleportposition.position;
        player.GetComponent<CharacterController>().enabled = true;
        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            lens.intensity.value = -i * 100;
            bloom.intensity.value = i*100;
            yield return new WaitForEndOfFrame();
        }
        lens.intensity.value = 0;
            bloom.intensity.value = 0.05f;
    }
}


