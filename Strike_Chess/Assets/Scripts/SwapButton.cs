using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwapButton : MonoBehaviour
{

    public TMP_Dropdown LeftDropdown;
    public TMP_Dropdown RightDropdown;

    public void Swap()
    {
        int switchVal;
        switchVal = LeftDropdown.value;
        LeftDropdown.value = RightDropdown.value;
        RightDropdown.value = switchVal;
    }
}
