using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Knight : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        base.Type[0] = "Knight";
        int[] newStats;
        string[] newSkills;
        bool[] newCombatSkills;
        GetSprite(pieceClass);
        if (pieceClass == "Cavalier")
        {
            switchForPostMoveSkills = 3;
            newStats = new int[] { 4, 2, 4, 1, 4 };
            boxSkill = "Shelter";
            newSkills = new string[] { "Canto", "Shelter" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Pegasus Knight")
        {           
            newStats = new int[] { 4, 2, 3, 4, 4 };
            newSkills = new string[] { "Relief", "Galeforce" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Wyvern Rider")
        {
            newStats = new int[] { 5, 1, 4, 0, 5 };
            newSkills = new string[] { "Trample", "Lunge" };
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Manakete")
        {
            BlackWhiteSwitch = true;
            newStats = new int[] { 5, 1, 2, 5, 5 };
            PrimaryStats = new List<int>() { 5, 1, 2, 5, 5 }; //White Tile Stats
            SecondaryStats = new List<int>() { 5, 1, 5, 2, 5 }; //BlackTileStats
            newCombatSkills = new bool[] { false, true, false };
            newSkills = new string[] { "Even Rhythm ", "Dragonskin" };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Bird Tribe")
        {
            transformingUnit = true;
            newStats = new int[] { 4, 0, 0, 1, 4 };
            PrimaryStats = new List<int>() { 4, 0, 0, 1, 4 };
            SecondaryStats =  new List<int>() { 4, 2, 4, 4, 4 };
            newCombatSkills = new bool[] { false, false, false };
            newSkills = new string[] { "Transform", "Vortex" };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
    }

    protected override bool MatchesState(int targetX, int targetY, CellState targetState, CellState targetState2 = CellState.None, bool addE = true)
    {
        //Checks to see if targeted Cell matches the state we're looking for 
        CellState targetCell = CellState.None;
        targetCell = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);
        if (targetCell == CellState.Enemy && addE && !mCurrentCell.mBoard.mAllCells[targetX, targetY].EnPass)
        {
            mEnemyCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
        }
        if ((targetCell == targetState) || (targetCell == targetState2))
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (!mCurrentCell.mBoard.mAllCells[targetX, targetY].EnPass)
            {
                return true;
            }
        }
        return false;
    }

    private void CreateKnightPath(int DirectionFlip)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //left
        MatchesState(currentX - 2, currentY + (1 * DirectionFlip), CellState.Free, CellState.Enemy);
        //Right
        MatchesState(currentX + 2, currentY + (1 * DirectionFlip), CellState.Free, CellState.Enemy);
        //upperleft
        MatchesState(currentX - 1, currentY + (2 * DirectionFlip), CellState.Free, CellState.Enemy);
        //Upper Right
        MatchesState(currentX + 1, currentY + (2 * DirectionFlip), CellState.Free, CellState.Enemy);
        if (Type[1] == "Cavalier")
        {
            bool canto = false;
            //1left/Right
            MatchesState(currentX + (1 * DirectionFlip), currentY, CellState.Free, CellState.Free, canto);
            //2left/Right
            MatchesState(currentX + (2 * DirectionFlip), currentY, CellState.Free, CellState.Free, canto);
            //1up/down
            MatchesState(currentX, currentY + (1 * DirectionFlip), CellState.Free, CellState.Free, canto);
            //2up/down
            MatchesState(currentX, currentY + (2 * DirectionFlip), CellState.Free, CellState.Free, canto);
            //DiagonalUp
            MatchesState(currentX + (1 * DirectionFlip), currentY + 1, CellState.Free, CellState.Free, canto);
            //DiagonalDown
            MatchesState(currentX + (1 * DirectionFlip), currentY - 1, CellState.Free, CellState.Free, canto);
        }
        if (Type[1] == "Bird Tribe" && promoted)
        {
            //1left/Right
            MatchesState(currentX + (1 * DirectionFlip), currentY, CellState.Enemy, CellState.Enemy);
            //2left/Right
            MatchesState(currentX + (2 * DirectionFlip), currentY, CellState.Enemy, CellState.Enemy);
            //1up/down
            MatchesState(currentX, currentY + (1 * DirectionFlip), CellState.Enemy, CellState.Enemy);
            //2up/down
            MatchesState(currentX, currentY + (2 * DirectionFlip), CellState.Enemy, CellState.Enemy);
            //DiagonalUp
            MatchesState(currentX + (1 * DirectionFlip), currentY + 1, CellState.Enemy, CellState.Enemy);
            //DiagonalDown
            MatchesState(currentX + (1 * DirectionFlip), currentY - 1, CellState.Enemy, CellState.Enemy);
        }
    }

    protected override void CheckPathing(int unused = 0)
    {
        CreateKnightPath(1); //Checks available spaces upwards
        CreateKnightPath(-1); //Checks available spaces downwards
        if(Type[1]=="Pegasus Knight")
        {
            //Horizontal
            CreateAllyPath(1, 0, FocusReliefRange.x);
            CreateAllyPath(-1, 0, FocusReliefRange.x);

            //Vertical
            CreateAllyPath(0, 1, FocusReliefRange.y);
            CreateAllyPath(0, -1, FocusReliefRange.y);

            //upperDiagonals
            CreateAllyPath(1, 1, FocusReliefRange.z);
            CreateAllyPath(-1, 1, FocusReliefRange.z);

            //LowerDiagonals
            CreateAllyPath(-1, -1, FocusReliefRange.z);
            CreateAllyPath(1, -1, FocusReliefRange.z);
        }
    }
    
    public override void PostMoveSkill()
    {
        transform.position = mCurrentCell.gameObject.transform.position;
        pieceState = PieceState.SelectedAdjacent;
        ClearTileOutline();
        CheckPathing(5);
        ShowPlacementOutlines();
    }

}
