using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public PieceManager mPieceManager;

    public void drawBoard()
    {
        //create the board
        mBoard.Create();
        //create the pieces (no class set yet)
        mPieceManager.SetupBoard(mBoard);
    }
    public void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift)==true )
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) == true)
            {
                mPieceManager.playerExp = new int[]{99,99};
            }
        }        
    }
}
