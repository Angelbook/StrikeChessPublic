using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PromotionDropdown : MonoBehaviour {
    public TMP_Dropdown dropdown;
    public PieceManager Manager;
    public static List<String> items;
    public void PopulateDropdown(List<String>PopulateList, Color32 newColor)
    {
        dropdown.ClearOptions();
        dropdown.GetComponent<Image>().color = newColor;
        dropdown.transform.GetChild(4).GetComponent<Image>().color = newColor;
        for (int i = 0; i<PopulateList.Count; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = PopulateList[i] });
        }
        //this swith from 1 to 0 is only to refresh the visual DdMenu
        dropdown.value = 1;
        dropdown.value = 0;
    }

    public void SendSelection()
    {
        Manager.boxResponse = 1;
        Manager.promotionResponse = dropdown.options[dropdown.value].text;
    }
}
