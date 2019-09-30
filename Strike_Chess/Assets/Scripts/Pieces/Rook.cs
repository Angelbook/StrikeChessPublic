using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Rook : BasePiece
{
    [HideInInspector]
    public Cell CastleTriggerCell = null; //Causes Castle Event when landed on by a Lord
    private Cell CastleCell = null; //Cell Rook moves to on castle  

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        mMovement = new Vector3Int(7, 7, 0); //horizontal, Vertical, Diagonal
        base.Type[0] = "Rook";
        int[] newStats;
        string[] newSkills;
        bool[] newCombatSkills;
        GetSprite(pieceClass);
        if (pieceClass == "Armored Knight")
        {
            newStats = new int[] { 4, 2, 3, 0, 4 };
            newSkills = new string[] { "Defense+2", "Great Shield"};
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Archer")
        {
            newStats = new int[] { 4, 2, 5, 1, 4 };
            newSkills = new string[] { "Prescience", "Quickdraw" };
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Fighter")
        {
            switchForPostMoveSkills = 1;
            postMoveAction = true;
            boxSkill = "Shove";
            newStats = new int[] { 6, 1, 6, 0, 6 };
            newSkills = new string[] { "Shove", "Sol" };
            newCombatSkills = new bool[]{ false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Beast Tribe")
        {
            transformingUnit = true;
            newStats = new int[] { 6, 0, 0, 0, 6 };
            PrimaryStats = new List<int>() { 6, 0, 0, 0, 6 };
            SecondaryStats = new List<int>() { 6, 3, 6, 2, 6 };
            newSkills = new string[] { "Transform", "Roar" };
            newCombatSkills = new bool[] { false, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Beaststone")
        {
            BlackWhiteSwitch = true;
            newStats = new int[] { 5, 0, 0, 7, 5 };
            PrimaryStats = new List<int>() { 5, 0, 0, 7, 5 }; //White Tile Stats
            SecondaryStats = new List<int>() { 5, 2, 7, 0, 5 }; //Black Tile Stats
            newSkills = new string[] { "Odd Rhythm", "Grisly Wound" };
            newCombatSkills = new bool[] { false, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
    }

    public override void PostMoveSkill()
    {        
        BasePiece MovedPiece = mPostMoveCell.mCurrentPiece;
        int distanceX = (MovedPiece.mCurrentCell.mBoardPosition.x) - (mCurrentCell.mBoardPosition.x);
        int distanceY = (MovedPiece.mCurrentCell.mBoardPosition.y) - (mCurrentCell.mBoardPosition.y);
        Cell newCell = mCurrentCell.mBoard.mAllCells[(MovedPiece.mCurrentCell.mBoardPosition.x + distanceX), (MovedPiece.mCurrentCell.mBoardPosition.y + distanceY)];
        MovedPiece.mCurrentCell.mCurrentPiece = null;
        MovedPiece.Place(newCell);
        PostMoveAnimation("Shove");
        MovedPiece.transform.position = MovedPiece.mCurrentCell.gameObject.transform.position;
        transform.position = mCurrentCell.gameObject.transform.position;
        ClearTileOutline();
        mPieceManager.SwitchSides(mColor);
        mPieceManager.ResetBonuses();
        MovedPiece.mIsFirstMove = false;
        if (MovedPiece.Type[1] == "Villager")
        {
            MovedPiece.CheckForPromotion();
        }
    }

    public override void Place(Cell newCell)
    {
        base.Place(newCell);

        //Trigger Cell
        int triggerOffset = mCurrentCell.mBoardPosition.x < 4 ? 2 : -1; //determines if Rook is on the left or right
        CastleTriggerCell = SetCell(triggerOffset);

        //Castle Cell
        int castleOffset = mCurrentCell.mBoardPosition.x < 4 ? 3 : -2;
        CastleCell = SetCell(castleOffset);
    }

    public void Castle()
    {
        //Sets new Target Cell
        Debug.Log("Castle Cell at " + CastleCell.mBoardPosition);
        mTargetCell = CastleCell;
        postMoveAction = false;
        Move();
        if (Type[1] == "Fighter")
        {
            postMoveAction = true;
        }
    }

    private Cell SetCell(int offset)
    {
        //New position
        Vector2Int newPosition = mCurrentCell.mBoardPosition;
        newPosition.x += offset;

        //Returns position of new cell
        return mCurrentCell.mBoard.mAllCells[newPosition.x, newPosition.y];
    }
}
