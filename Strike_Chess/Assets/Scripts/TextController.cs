using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour {
    public static List<string> Values = new List<string>(new string[14]);
    public static List<bool> BuffValues = new List<bool>(new bool[6]);
    //0 Bliss, 1 Dance, 2 Veteran, 3 Charm, 4 Special Dance, 5 Rally
    public static Color ColorOfPiece;
    public static bool update;
    public static bool promoted;
    public static bool transforming;
    public static bool TileShifter;
    public PieceManager manager;
    public List<TextMeshProUGUI> DisplayP1 = new List<TextMeshProUGUI>();
    public List<GameObject> BuffsDisplayP1 = new List<GameObject>();
    public List<TextMeshProUGUI> DisplayP2 = new List<TextMeshProUGUI>();
    public List<GameObject> BuffsDisplayP2 = new List<GameObject>();

    public void ResetValues()
    {
        for (int i = 0; i <= Values.Count; i++) {
            Values[i] = "";
             }
        update = true;
    }

    private void DisplayBuffs(List<GameObject> UIBuffs)
    {
        for(int i=0; i<BuffValues.Count; i++)
        {
            if (i <= 3)
            {
                UIBuffs[i].SetActive(BuffValues[i]);
            }
            else if (BuffValues[i]) {
                UIBuffs[i - 3].GetComponent<Image>().color = new Color32(255, 208, 0, 255);
            }
            else UIBuffs[i - 3].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    private void DisplayText(List<TextMeshProUGUI> UIdisplay)
    {
        UIdisplay[0].text = "HP: " + Values[0] + "/" + Values[9];
        UIdisplay[1].text = "Attack: " + Values[1] + "-" + Values[2];
        UIdisplay[2].text = "Avoid: " + Values[3];
        UIdisplay[3].text = "PieceType: " + Values[4];
        UIdisplay[4].text = "Class: " + Values[5];
        UIdisplay[5].text = Values[6];
        UIdisplay[6].text = Values[7];
        UIdisplay[7].text = Values[8];
        UIdisplay[11].text = "Name: " + Values[13];
        if (TileShifter || transforming) {
            UIdisplay[9].text = "/" + Values[10] + "-" + Values[11]; //secondary atk
            UIdisplay[10].text = "/" + Values[12];
        }
        else
        {
            UIdisplay[9].text = "";
            UIdisplay[10].text = "";
        }
        if (promoted == false)
        {
            UIdisplay[6].color = new Color32(144, 144, 89, 255);
        }
        else
        {
            UIdisplay[6].color = new Color32(255,255,0,255);
        }
        if (transforming)
        {
            UIdisplay[10].color = UIdisplay[9].color = UIdisplay[6].color;
        }
        if (TileShifter)
        {
            UIdisplay[10].color = UIdisplay[9].color = new Color32(0, 0, 0, 255);
        }
        UIdisplay[7].text = Values[8]; //Will display nothing if no extra skill
    }

    void Update()
    {
        if (update == true)
        {
            if (ColorOfPiece == Color.blue)
            {
                DisplayText(DisplayP1);
                DisplayBuffs(BuffsDisplayP1);
                DisplayP1[8].text = "Experience Points: " + manager.playerExp[0].ToString();
            }
            else
            {
                DisplayText(DisplayP2);
                DisplayBuffs(BuffsDisplayP2);
                DisplayP2[8].text = "Experience Points: " + manager.playerExp[1].ToString();
            }
            update = false;
        }
    }
}
