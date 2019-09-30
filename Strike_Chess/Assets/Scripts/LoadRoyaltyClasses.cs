using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LoadRoyaltyClasses : MonoBehaviour {
    public PieceManager mPieceManager;
    public List<TMP_Dropdown> pieceDropdowns = new List<TMP_Dropdown>();
    public int menuNum; //1 = P1 Select, 2 P2 Select, 3 Prebattle confirmation 
    // Use this for initialization

    public void ForwardPress()
    {
        for (int i = 0; i < pieceDropdowns.Count; i++)
        {
            int val = (pieceDropdowns[i].value);
            string text = pieceDropdowns[i].options[val].text;
            if (menuNum == 1) {
                if (mPieceManager.P1RoyaltyReserves.Contains(text))
                {
                    mPieceManager.P1RoyaltyReserves.Remove(text);
                }
                mPieceManager.P1RoyaltySetup.Add(text);
            } else if(menuNum==2)
            {
                if (mPieceManager.P2RoyaltyReserves.Contains(text))
                {
                    mPieceManager.P2RoyaltyReserves.Remove(text);
                }
                mPieceManager.P2RoyaltySetup.Add(text);
            }
        }
    }

    public void BackPress()
    {
        List<string> refresh = new List<string>(new string[]{
        "Armored Knight", "Armored Knight", "Fighter", "Fighter", "Archer", "Archer", "BeastStone", "BeastStone", "Beast Tribe", "Beast Tribe",
        "Myrmidon", "Myrmidon", "Mage", "Mage", "Thief", "Thief", "Mercenary", "Mercenary", "DarkMage", "DarkMage",
        "Cavalier", "Cavalier", "Pegasus Knight", "Pegasus Knight", "Wyvern Rider", "Wyvern Rider","Manakete", "Manakete", "Bird Tribe", "Bird Tribe",
        "Cleric", "Troubadour", "Dancer", "Tactician", "Heron"
        });
        if (menuNum == 3)
        {
            mPieceManager.P2RoyaltyReserves.Clear();
            mPieceManager.P2RoyaltySetup.Clear();
            refresh.ForEach(val => { mPieceManager.P2RoyaltyReserves.Add(val); });
        }
        else if (menuNum == 2)
        {
            mPieceManager.P1RoyaltyReserves.Clear();
            mPieceManager.P1RoyaltySetup.Clear();
            refresh.ForEach(val => { mPieceManager.P1RoyaltyReserves.Add(val); });
        }
        else if (menuNum == 1)
        {
            SceneManager.LoadScene(0);

        }
    }  
}
