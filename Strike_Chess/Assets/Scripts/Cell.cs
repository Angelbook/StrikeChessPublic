using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Color tileColor = Color.clear;

    public Image mOutlineImage;

    public Image mAllyImage;

    public Image mEnemyImage;

    public bool EnPass = false;
    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    [HideInInspector]
    public BasePiece mCurrentPiece = null;

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }

    public void RemovePiece()
    {
        if (mCurrentPiece!=null)
        {
            mCurrentPiece.Kill();
        }
    }
}
