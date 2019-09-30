using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadTeamButton : MonoBehaviour
{

    public TMP_Dropdown ThisDropdown;
    public List<TMP_Dropdown> Dropdowns;
    private int[] values = new int[8];


    private void ChangeDropdownValues()
    {
        for (int i=0; i<Dropdowns.Count; i++)
        {            
            Dropdowns[i].value = values[i];
        }
        ThisDropdown.value = 0;
    }

    public void ValueSet()
    {
        string ValueText = ThisDropdown.options[ThisDropdown.value].text;
        if(ValueText== "Attacker Aggregation")
        {
            values = new int[] { 1, 2, 2, 2, 1, 0, 2, 1 };
        }
        else if (ValueText == "Deadly Defense")
        {
            values = new int[] { 0, 3, 3, 0, 1, 3, 3, 0 };
        }
        else if (ValueText == "Evasive Ensemble")
        {
            values = new int[] { 4, 1, 4, 4, 0, 4, 1, 4};
        }
        else if (ValueText == "Movement Mayhem")
        {
            values = new int[] { 2, 0, 1, 1, 0, 1, 0, 2 };
        }
        else if (ValueText == "Shifter Squad")
        {
            values = new int[] { 3, 3, 2, 3, 0, 4, 4, 4 };
        }
        ChangeDropdownValues();
    }
}
