using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownTextManager : MonoBehaviour {

    public TMP_Dropdown Dropdown;
    public static List<string> Values = new List<string>(new string[6]); //HP, ATK, AVO, SKILLS
    public List<TextMeshProUGUI> Texts = new List<TextMeshProUGUI>();
    public bool Lord = false;

    public void ChangeTexts()
    {
        string selected = Dropdown.options[Dropdown.value].text;
        ChoiceChecks(selected, Lord);
        if (!Lord)
        {
            Texts[0].text = "HP: " + Values[0];
            Texts[1].text = "ATK: " + Values[1];
            Texts[2].text = "Avoid: " + Values[2];
            Texts[3].text = Values[3];
            Texts[4].text = Values[4];
            Texts[5].text = Values[5];
        }
        else Texts[0].text = Values[0];
    }

    private void OnEnable()
    {
        ChangeTexts();
    }

    private void ChoiceChecks(string choice, bool LordSkill)
    {
        if (LordSkill)
        {
            Values[0] = choice;
        }
        else
        {
            if (choice == "Armored Knight")
            {
                Values[0] = "4";
                Values[1] = "2-3";
                Values[2] = "0";
                Values[3] = "Defense+2";
                Values[4] = "Great Shield";
                Values[5] = "";
            }
            else if (choice == "Archer")
            {
                Values[0] = "4";
                Values[1] = "2-5";
                Values[2] = "1";
                Values[3] = "Prescience";
                Values[4] = "Quickdraw";
                Values[5] = "";
            }
            else if (choice == "Fighter")
            {
                Values[0] = "6";
                Values[1] = "1-6";
                Values[2] = "0";
                Values[3] = "Shove";
                Values[4] = "Sol";
                Values[5] = "";
            }
            else if (choice == "Beast Tribe")
            {
                Values[0] = "6";
                Values[1] = "0/3-6";
                Values[2] = "0/2";
                Values[3] = "Transform";
                Values[4] = "Roar";
                Values[5] = "";
            }
            else if (choice == "Beaststone")
            {
                Values[0] = "5";
                Values[1] = "0/2-7";
                Values[2] = "0/7";
                Values[3] = "Odd Rhythm";
                Values[4] = "Grisly Wound";
                Values[5] = "";
            }
            else if (choice == "Cavalier")
            {
                Values[0] = "4";
                Values[1] = "2-4";
                Values[2] = "1";
                Values[3] = "Canto";
                Values[4] = "Shelter";
                Values[5] = "";
            }
            else if (choice == "Pegasus Knight")
            {
                Values[0] = "4";
                Values[1] = "2-3";
                Values[2] = "4";
                Values[3] = "Relief";
                Values[4] = "Galeforce";
                Values[5] = "";
            }
            else if (choice == "Wyvern Rider")
            {
                Values[0] = "5";
                Values[1] = "1-4";
                Values[2] = "0";
                Values[3] = "Trample";
                Values[4] = "Lunge";
                Values[5] = "";
            }
            else if (choice == "Manakete")
            {
                Values[0] = "5";
                Values[1] = "1-2/1-5";
                Values[2] = "2/5";
                Values[3] = "Even Rhythm";
                Values[4] = "Dragonskin";
                Values[5] = "";
            }
            else if (choice == "Bird Tribe")
            {
                Values[0] = "4";
                Values[1] = "0/2-4";
                Values[2] = "1/4";
                Values[3] = "Transform";
                Values[4] = "Vortex";
                Values[5] = "";
            }
            else if (choice == "Myrmidon")
            {
                Values[0] = "3";
                Values[1] = "1-4";
                Values[2] = "5";
                Values[3] = "Vantage";
                Values[4] = "Astra";
                Values[5] = "";
            }
            else if (choice == "Mage")
            {
                Values[0] = "3";
                Values[1] = "2-4";
                Values[2] = "3";
                Values[3] = "Focus";
                Values[4] = "Nihil";
                Values[5] = "";
            }
            else if (choice == "Thief")
            {
                Values[0] = "3";
                Values[1] = "1-2";
                Values[2] = "6";
                Values[3] = "Pass";
                Values[4] = "Lethality";
                Values[5] = "";
            }
            else if (choice == "Dark Mage")
            {
                Values[0] = "5";
                Values[1] = "1-5";
                Values[2] = "0";
                Values[3] = "Void Curse";
                Values[4] = "Lifetaker";
                Values[5] = "";
            }
            else if (choice == "Mercenary")
            {
                Values[0] = "4";
                Values[1] = "2-4";
                Values[2] = "2";
                Values[3] = "Patience";
                Values[4] = "Strong Riposte";
                Values[5] = "";
            }
            else if (choice == "Cleric")
            {
                Values[0] = "4";
                Values[1] = "0";
                Values[2] = "2";
                Values[3] = "Heal";
                Values[4] = "Renewal";
                Values[5] = "Miracle";
            }
            else if (choice == "Troubadour")
            {
                Values[0] = "3";
                Values[1] = "0";
                Values[2] = "2";
                Values[3] = "Warp";
                Values[4] = "Entrap";
                Values[5] = "";
            }
            else if (choice == "Dancer")
            {
                Values[0] = "2";
                Values[1] = "0";
                Values[2] = "5";
                Values[3] = "Dance";
                Values[4] = "Special Dance";
                Values[5] = "";
            }
            else if (choice == "Heron")
            {
                Values[0] = "1";
                Values[1] = "0";
                Values[2] = "1/4";
                Values[3] = "Transform";
                Values[4] = "Bliss";
                Values[5] = "";
            }
            else if (choice == "Tactician")
            {
                Values[0] = "4";
                Values[1] = "0";
                Values[2] = "2";
                Values[3] = "Veteran";
                Values[4] = "Rally Spectrum";
                Values[5] = "";
            }
        }
    }
}
