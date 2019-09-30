using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private Cell mEnPassantFlag;
    private Cell mEnPassantTile;
    bool enPassActive = false;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        int[] newStats = { 2, 1, 4, 2, 2 };
        string[] newSkills = {"Paragon","Underdog","Promotion" };
        bool[] newCombatSkill = { false, true, false };     
        ChangeStats(newStats, newSkills, newCombatSkill, this);
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        base.Type[0] = "Pawn";
        upgradeCost = -2;
        mMovement = (mColor == PieceManager.P1Color ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1));
        GetSprite("Villager");
   }

    protected override void Move()
    {
        base.Move();
        if(mIsFirstMove && mCurrentCell.mBoardPosition == mEnPassantFlag.mBoardPosition)
        {
            mEnPassantTile.mCurrentPiece = this;
            enPassActive = true;
            mEnPassantTile.EnPass = true;
        }
        mIsFirstMove = false;
    }

    public override void NewTurnResets()
    {
        base.NewTurnResets();
        mEnPassantTile.EnPass = false;
        if (enPassActive)
        {   
            if(mEnPassantTile.mCurrentPiece == this)
            {
                mEnPassantTile.mCurrentPiece = null;
            }
            enPassActive = false;
        }
        if (Stats[0]>0)
        {
            mCurrentCell.mCurrentPiece = this; //Incase pawn is shoved to the EnPass tile after activating it
        }
    }

    protected override bool MatchesState(int targetX, int targetY, CellState targetState, CellState targetState2 = CellState.None, bool addE = true)
    {
        //Checks to see if targeted Cell matches the state we're looking for 
        CellState targetCell = CellState.None;
        targetCell = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);

        if ((targetCell == targetState))
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (addE)
            {
                mEnemyCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }
            return true;
        }
        return false;
    }

    protected override void CheckPathing(int unused = 0)
    {
        //target position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Checks diagonally left position for enemies
        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellState.Enemy);
        //Diagonally Right  
        MatchesState(currentX + mMovement.z, currentY + mMovement.z, CellState.Enemy);

        //checks forward
        bool noEnemy = false;
        if (MatchesState(currentX,currentY + mMovement.y, CellState.Free, CellState.Free, noEnemy))
        {
            //If this is the pawn's first Move, check the 2nd forward cell
            if (mIsFirstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellState.Free, CellState.Free, noEnemy );
            }
        }

        //Checks Topright
    }

    public void SetEnPassantSpaces()
    {
        mEnPassantFlag = mCurrentCell.mBoard.mAllCells[mCurrentCell.mBoardPosition.x, (mOriginalCell.mBoardPosition.y + (mMovement.y * 2))];
        mEnPassantTile = mCurrentCell.mBoard.mAllCells[mCurrentCell.mBoardPosition.x, (mOriginalCell.mBoardPosition.y + mMovement.y)];
    }
}
