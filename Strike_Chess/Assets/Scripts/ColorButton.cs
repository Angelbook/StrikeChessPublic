using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour {

    public GameObject ColorStopper1;
    public GameObject ColorStopper2;
    public void changeColor()
    {
        if (MainMenu.P1Selected == true)
        {
            PieceManager.Player1Appearance = this.GetComponent<Image>().color;
            ColorStopper1.transform.position = transform.GetComponent<RectTransform>().position;
        }
        else
        {
            PieceManager.Player2Appearance = this.GetComponent<Image>().color;;
            ColorStopper2.transform.position = transform.GetComponent<RectTransform>().position;
        }
    }
}
