using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// New

public class TileColors : MonoBehaviour
{
    public static Color TileColorBlack = new Color32(0, 0, 0, 100);
    public static Color TileColorWhite = new Color32(255, 255, 255, 100);
}

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds,
    Selected,
    SelectedEnemy
}

public enum PieceState
{
    None,
    Waiting,
    Friendly,
    Selected,
    SelectedAdjacent,
    Enemy,
    SelectedEnemy
}

public class Board : MonoBehaviour
{
    //creates object
    public GameObject mCellPrefab;
    [HideInInspector]
    //creates an 8 by 8 object of cells based on the Cell class.
    public Cell[,] mAllCells = new Cell[8, 8];

    public void Create()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)  
            {
                //creates an individual Tile/Cell
                GameObject newTile = Instantiate(mCellPrefab, transform);

                //Position and anchors for new Tiles
                RectTransform position = newTile.GetComponent<RectTransform>();
                position.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                //Setup
                //Gets components/info of new tile object
                mAllCells[x, y] = newTile.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
                //calls Setup function/method from earlier, uses the new anchored position to create a new rect/tile

            }
        }

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 0) //Sets Tiles to Black
                {
                    mAllCells[x, y].GetComponent<Image>().color = TileColors.TileColorBlack;
                    mAllCells[x, y].tileColor = Color.black;
                }
                else
                {
                    mAllCells[x, y].GetComponent<Image>().color = TileColors.TileColorWhite;
                    mAllCells[x, y].tileColor = Color.white;
                }
            }

        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {   
        if(targetX <0 || targetX > 7)
        {
            return CellState.OutOfBounds;
        }
        if (targetY<0 || targetY>7)
        {
            return CellState.OutOfBounds;
        }

        //get Cell
        Cell targetCell = mAllCells[targetX, targetY];

        if(targetCell.mCurrentPiece != null)
        {
            if(targetCell.mCurrentPiece.mColor == checkingPiece.mColor)
            {
                return CellState.Friendly;
            }
            if(targetCell.mCurrentPiece.mColor != checkingPiece.mColor)
            {
                return CellState.Enemy;
            }
        }
            return CellState.Free;
    }
}
