using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite deselectedSprite;
    private string folder = @".\Saves\";

    public void Selectthis()
    {
        if (transform.GetComponent<Image>().sprite == deselectedSprite)
        {
            transform.GetComponent<Image>().sprite = selectedSprite;

            //Abre o arquivo de save referente ao item selecionado
            SaveController.DesCripto(transform.GetSiblingIndex() + 1);
            StreamReader enterfile = new StreamReader(folder + "descreep" + (transform.GetSiblingIndex() + 1) + ".xml");
            XmlSerializer xmlobject = new XmlSerializer(typeof(SaveGame));
            SaveGame carregado = (SaveGame)xmlobject.Deserialize(enterfile.BaseStream);

            //Carrega as informações do save
            GameController.controller.playerData = carregado.playerdata;
            GameController.controller.savenumber = carregado.playerdata.savenumber;
            enterfile.Close();
            File.Delete(folder + "descreep" + (transform.GetSiblingIndex() + 1) + ".xml");
        }        
    }
}
