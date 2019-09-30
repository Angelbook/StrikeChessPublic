using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour {

    public PieceManager Manager;
    // Use this for initialization
    public void Upgrade()
    {
        if (enabled && Manager.clickedPiece!=null)
        {
            Manager.clickedPiece.Upgrade();            
        }
    }
}
