using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Fade fade;
    private GameObject tela;
    private Image backgroundpretobranco;
    private Image backgroundcolorido;
    private Slider loadingslider;
    private Text loadingtext;

    void Start()
    {
        tela = transform.GetChild(0).gameObject;
        backgroundpretobranco = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        backgroundcolorido = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        loadingslider = transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        loadingtext= transform.GetChild(0).GetChild(3).GetComponent<Text>();
    }

    public void ChangeScene(int index)
    {
        if (index==0)
        {
            if (GameController.controller.uiController!=null)
            {
                GameController.controller.uiController.UseMouse(true);
            }
            //Destroy(GameController.controller.gameObject);
        }
        StartCoroutine(LoadAsync(index));
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        tela.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            backgroundpretobranco.GetComponent<Image>().color = new Color(1, 1, 1,(1- progress));
            backgroundcolorido.GetComponent<Image>().color = new Color(1, 1, 1, progress);
            loadingslider.value = progress;
            loadingtext.text = "Carregando... " + (progress * 100).ToString("f") + "%";
            yield return null;
        }
    }
}
