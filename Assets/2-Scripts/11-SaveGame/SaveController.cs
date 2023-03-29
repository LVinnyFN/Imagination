using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

public class SaveController : MonoBehaviour
{
    [SerializeField] private PlayerInfo currentplayer;
    [SerializeField] private Player controller;
    [SerializeField] Fade fade;
    private DateTime initialtime;
    private SaveGame newgame;
    private static string folder = @".\Saves\";
    private static int[] keys = { 3, 1, 7, 2 };


    void Start()
    {
        controller = GameObject.FindWithTag("Player").GetComponent<Player>();
        initialtime = DateTime.Now;
    }

    void Update()
    {
        CheckHotkeys();
    }

    public void CheckHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (GameController.controller.savenumber!=0)
            {
                Debug.Log("Player Loaded.");
                LoadGame(GameController.controller.savenumber);
            }
            else
            {
                Debug.Log("É necessário salvar o jogo antes de dar fast-load");
            }
        }
    }

    public void SaveGame()
    {

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        DirectoryInfo dir = new DirectoryInfo(folder); // info sobre o diretorio dos save games
        if (GameController.controller.savenumber==0) // Se é um novo jogo
        {
            GameController.controller.savenumber = dir.GetFiles().Length + 1; // salva um novo jogo (+1 comparado com a quantidade)
        }
        StreamWriter exitfile = new StreamWriter(folder+"save"+ GameController.controller.savenumber + ".xml");
        XmlSerializer xmlobject = new XmlSerializer(typeof(SaveGame));
        controller.Sincronize();
        currentplayer = controller.GetComponent<Player>().player1;
        newgame = new SaveGame();
        newgame.playerdata = currentplayer;
        newgame.initialtime = initialtime;
        xmlobject.Serialize(exitfile.BaseStream, newgame);
        exitfile.Close();
        Cripto();
        File.Delete(folder + "save" + GameController.controller.savenumber + ".xml");
        Debug.Log("Player Saved.");
    }

    public void LoadGame(int savenum)
    {
        if (savenum==0)
        {
            fade.StartFading(3);
        }
        else
        {
            DesCripto(GameController.controller.savenumber);
            StreamReader enterfile = new StreamReader(folder + "descreep" + savenum + ".xml");
            XmlSerializer xmlobject = new XmlSerializer(typeof(SaveGame));
            SaveGame carregado = (SaveGame)xmlobject.Deserialize(enterfile.BaseStream);
            GameController.controller.playerData = carregado.playerdata;
            controller.Load();
            enterfile.Close();
            File.Delete(folder + "descreep" + savenum + ".xml");
        }
    }
    public void ReloadGame() // usa quando estiver morto pra dar load novamente
    {

        if (GameController.controller.savenumber==0)
        {
            fade.StartFading(3);
        }
        else
        {
            DesCripto(GameController.controller.savenumber);
            StreamReader enterfile = new StreamReader(folder + "descreep" + GameController.controller.savenumber + ".xml");
            XmlSerializer xmlobject = new XmlSerializer(typeof(SaveGame));
            SaveGame carregado = (SaveGame)xmlobject.Deserialize(enterfile.BaseStream);
            GameController.controller.playerData = carregado.playerdata;
            controller.Load();
            enterfile.Close();

            //Revivendo
            controller.animator.SetBool("isDead", false);
            controller.isDead = false;
            controller.GetComponent<SkillsManager>().UseInvulnerability();
            controller.RefreshStatsUI();
            controller.RefreshSkillsUI();
            controller.ActivePlusSignals();
            GameController.controller.uiController.RefreshInventory();
            GameController.controller.uiController.RefreshUI();
            GameController.controller.uiController.UseMouse(false);

            File.Delete(folder + "descreep" + GameController.controller.savenumber + ".xml");
        }
    }

    public static void Cripto()
    {
        StreamReader enterfile = new StreamReader(folder + "save" + GameController.controller.savenumber + ".xml");
        StreamWriter exitfile = new StreamWriter(folder + "creep" + GameController.controller.savenumber + ".on");
        string text = enterfile.ReadToEnd();
        for (int i = 0; i < text.Length; i++)
        {
            int key = keys[i % keys.Length];
            char aux = text[i];
            char criptChar = CriptoChar(aux, key);
            exitfile.Write(criptChar);
        }
        enterfile.Close();
        exitfile.Close();
    }

    private static char CriptoChar(char original, int k)
    {
        return (char)(original + k);
    }

    public static void DesCripto(int value)
    {
        StreamReader enterfile = new StreamReader(folder + "creep" + value + ".on");
        StreamWriter exitfile = new StreamWriter(folder + "descreep" + value + ".xml");
        string text = enterfile.ReadToEnd();
        for (int i = 0; i < text.Length; i++)
        {
            int key = keys[i % keys.Length];
            char aux = text[i];
            char desCriptChar = DesCriptoChar(aux, key);
            exitfile.Write(desCriptChar);
        }
        enterfile.Close();
        exitfile.Close();
    }

    private static char DesCriptoChar(char original, int k)
    {
        return (char)(original - k);
    }


}
