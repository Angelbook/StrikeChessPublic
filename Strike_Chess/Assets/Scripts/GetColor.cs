using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetColor : MonoBehaviour
{
    // Use this for initialization
    public int playerToggle;
    private void OnEnable()
    {
        
        if (playerToggle == 1)
        {
            this.GetComponent<Image>().color = PieceManager.Player1Appearance;
        }
        else this.GetComponent<Image>().color = PieceManager.Player2Appearance;       
    }
}
