using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        mMovement = new Vector3Int(1, 1, 1); //horizontal, Vertical, Diagonal
        int[] newStats = { 6, 1, 6, 2, 6 };
        string[] newSkills = { "Charm", pieceClass};
        bool[] newCombatSkills = { false, true, false };
        ChangeStats(newStats, newSkills, newCombatSkills, this);
        base.Type[0] = base.Type[1] = "Lord";
        GetSprite("Lord");
        upgradeCost = -6;
    }

    private Rook mLeftRook = null;
    private Rook mRightRook = null;

    protected override void CheckPathing(int caseNum = 1)
    {
        base.CheckPathing();
        base.CheckPathing(4);
        if (mLeftRook)
        {
            mHighlightedCells.Add(mLeftRook.CastleTriggerCell);
        }
        if (mRightRook)
        {
            mHighlightedCells.Add(mRightRook.CastleTriggerCell);
        }
    }

    public void CastleChecks()
    {
        if (TriggerCastle(mLeftRook))
        {
            mLeftRook.Castle();
        }
        if (TriggerCastle(mRightRook))
        {
            mRightRook.Castle();
        }
    }

    private bool TriggerCastle(Rook rook)
    {
        if (rook == null)
        {
            return false;
        }
        if(rook.CastleTriggerCell != mCurrentCell)
        {
            return false;
        }
        return true;
    }

    private Rook GetRook(int direction, int count)
    {
        //Checks if the King has moved
        if (!mIsFirstMove)
        {
            return null;
        }
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Examines Each Cell before the rook on the left/right (depending on direction)
        for(int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);
            CellState state = mCurrentCell.mBoard.ValidateCell(offsetX, currentY, this);
            if (state!= CellState.Free)
            {
                return null;
            }
        }
        //Checks the Rook Cell
        Cell rookCell = mCurrentCell.mBoard.mAllCells[currentX + (count * direction), currentY];
        Rook rook = null;

        //Checks for a piece
        if (rookCell.mCurrentPiece != null)
        {
            //Cast the rook
            if (rookCell.mCurrentPiece is Rook)
            {
                rook = (Rook)rookCell.mCurrentPiece;
            }
            else
            {
                return null;
            }
        } 

        //Return if no rook
        if (rook == null)
        {
            return null;
        }

        if(rook.mColor != mColor || !rook.mIsFirstMove)
        {
            return null;
        }    
        return rook;
    }

    public override void Kill()
    {
        base.Kill();
        mPieceManager.AreLordsAlive = false;
    }

    public override void LordUpgrade()
    {
    }

    public override void NewTurnResets()
    {
        CheckPathing(4);
        base.NewTurnResets();
        mLeftRook = GetRook(-1, 4);
        mRightRook = GetRook(1, 3);        
    }
}
