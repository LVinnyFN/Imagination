using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject namefeedback;
    [SerializeField] private InputField playerName;
    [SerializeField] private GameObject saveListGrid;
    [SerializeField] private GameObject saveSlotprefab;
    [SerializeField] private RawImage cameraon;
    [SerializeField] private Texture rendererboy;
    [SerializeField] private RectTransform contentrect;

    [SerializeField] private Fade fade;

    private List<int> saves = new List<int>();
    private string folder = @".\Saves\";

    private void Start()
    {
        if (Directory.Exists(folder))
        {
            DirectoryInfo dir = new DirectoryInfo(folder); // info sobre o diretorio dos save games

            //cria uma lista setando um número inteiro para cada savegame dentro da pasta de saves
            for (int i = 1; i < dir.GetFiles().Length + 1; i++)
            {
                saves.Add(i);
            }
        }

        if (saveListGrid!=null) // significa que você esta na cena de Load Game
        {
            //lê cada save
            foreach (int value in saves)
            {
                GameObject savegame = Instantiate(saveSlotprefab, saveListGrid.transform); //cria o prefab na UI
                contentrect.sizeDelta = new Vector2(contentrect.sizeDelta.x, contentrect.sizeDelta.y + 32f);

                //Carrega o arquivo da vez
                SaveController.DesCripto(value);
                StreamReader enterfile = new StreamReader(folder + "descreep" + value + ".xml");
                XmlSerializer xmlobject = new XmlSerializer(typeof(SaveGame));
                SaveGame carregado = (SaveGame)xmlobject.Deserialize(enterfile.BaseStream);

                //Copia as informações de interesse na UI

                //savegame.transform.GetChild(0).GetComponent<Image>().sprite = null; // Sprite (COLOCAR IMAGEM!!!)
                savegame.transform.GetChild(3).GetComponent<Text>().text = value.ToString();
                savegame.transform.GetChild(1).GetComponent<Text>().text = carregado.playerdata.playername; //Nome
                savegame.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = carregado.playerdata.level.ToString(); // Level
                GameController.controller.playerData = carregado.playerdata;
                enterfile.Close();
                File.Delete(folder + "descreep" + value + ".xml");
            }
        }
        
    }

    public void RefeshName()
    {
        GameController.controller.playername = playerName.text;
        namefeedback.SetActive(false);
    }

    public void LoadGame()
    {
        if (GameController.controller.savenumber!=0)
        {
            fade.StartFading(3);
        }
    }

    public void GoBackToMain()
    {
        fade.StartFading(0);
    }

    public void StartNewGame()
    {
        if (GameController.controller.playername!="")
        {
            GameController.controller.savenumber = 0;
            fade.StartFading(4);
        }
        else
        {
            namefeedback.SetActive(true);
        }
        
    }
}
