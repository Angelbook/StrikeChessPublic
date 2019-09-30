using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Queen : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        mMovement = new Vector3Int(7, 7, 7); //horizontal, Vertical, Diagonal
        base.Type[0] = "Queen";
        int[] newStats;
        string[] newSkills;
        bool[] newCombatSkills;
        GetSprite(pieceClass);
        if (pieceClass == "Cleric")
        {
            switchForPostMoveSkills = 3;
            postMoveAction = true;
            boxSkill = "Heal";
            newStats = new int[] { 4, 0, 0, 2, 4 };
            newSkills = new string[] { "Miracle", "Renewal", "Heal" };
            newCombatSkills = new bool[] { true, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Troubadour")
        {
            switchForPostMoveSkills = 4;
            postMoveAction = true;
            boxSkill = "Warp";
            newStats = new int[] { 3, 0, 0, 2, 3 };
            newSkills = new string[] { "Warp", "Entrap" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Dancer")
        {
            switchForPostMoveSkills = 2;
            postMoveAction = true;
            boxSkill = "Dance";
            newStats = new int[] { 2, 0, 0, 5, 2 };
            newSkills = new string[] { "Dance", "Special Dance" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Heron")
        {
            switchForPostMoveSkills = 2;
            transformingUnit = true;
            boxSkill = "Bliss";
            newStats = new int[] { 1, 0, 0, 1, 1 };
            PrimaryStats = new List<int>() { 1, 0, 0, 1, 1 };
            SecondaryStats = new List<int>() { 1, 0, 0, 4, 1 };
            newSkills = new string[] { "Transform", "Bliss" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Tactician")
        {
            newStats = new int[] { 4, 0, 0, 2, 4 };
            newSkills = new string[] { "Veteran", "Rally Spectrum" };
            newCombatSkills = new bool[] { false, false, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
    }

    public override void PostMoveSkill()
    {
        BasePiece targetPiece = mPostMoveCell.mCurrentPiece;
        if (Type[1] == "Cleric")
        {

            int distanceX = Mathf.Abs(targetPiece.mCurrentCell.mBoardPosition.x - mCurrentCell.mBoardPosition.x);
            int distanceY = Mathf.Abs(targetPiece.mCurrentCell.mBoardPosition.y - mCurrentCell.mBoardPosition.y);
            if ((distanceX+distanceY)>1)
            {
                targetPiece.Heal(Mathf.CeilToInt(targetPiece.Stats[4] / 2f));
            }
            else targetPiece.Heal(targetPiece.Stats[4]);
            targetPiece.animFX.enabled = true;
            targetPiece.PostMoveAnimation("Heal");
            transform.position = mCurrentCell.gameObject.transform.position;
            targetPiece.SendInfo();
            mPostMoveCell = null;
            mPieceManager.SwitchSides(mColor);
            mPieceManager.ResetBonuses();
            mPieceManager.clickedPiece = null;
            clickedUnit = false;
        }
        if(Type[1]== "Dancer")
        {
            if (promoted)
            {
                targetPiece.danced = 2;
            }
            else targetPiece.danced = 1;
            targetPiece.PostMoveAnimation("Dance");
            pieceState = PieceState.Friendly;
            targetPiece.danceBlissCount = 2;
            targetPiece.SendInfo();
            mPieceManager.CrossArrowAnimations();
            mPostMoveCell = null;
            targetPiece.pieceState = PieceState.Waiting;
            transform.position = mCurrentCell.gameObject.transform.position;
            mPieceManager.clickedPiece = null;
            clickedUnit = false;
            targetPiece = null;
        }
        if (Type[1] == "Troubadour")
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            pieceState = PieceState.SelectedAdjacent;
            if (targetPiece.pieceState == PieceState.Friendly)
            {
                targetPiece.anim.enabled = true;
                targetPiece.PostMoveAnimation("Warp");
                pieceState = PieceState.SelectedAdjacent;
                CheckPathing(5);
                ShowPlacementOutlines();
            }
            else StartCoroutine(Entrap());
        }
        if (Type[1] == "Heron")
        {
            targetPiece.animFX.enabled = true;
            targetPiece.PostMoveAnimation("Bliss");
            targetPiece.Heal(targetPiece.Stats[4]);
            targetPiece.blissed = true;
            targetPiece.SendInfo();
            pieceState = PieceState.Friendly;
            targetPiece.danceBlissCount = 2;
            mPieceManager.CrossArrowAnimations();
            mPostMoveCell = null;
            targetPiece.pieceState = PieceState.Waiting;
            transform.position = mCurrentCell.gameObject.transform.position;
            clickedUnit = false;
            mPieceManager.clickedPiece = null;
            targetPiece = null;
        }
        ClearTileOutline();
    }

    IEnumerator Entrap()
    {
        BasePiece targetPiece = mPostMoveCell.mCurrentPiece;
        mPieceManager.PromptBox(spriteColor, "Entrap");
        mPieceManager.blocker.SetActive(true);
        while (mPieceManager.boxResponse < 0) //If yes when initiating battle prompt
        {
            yield return null;
        }
        if (mPieceManager.boxResponse == 1)
        {
            mPieceManager.StartEntrapPhase(this, targetPiece);
            yield return new WaitForSeconds(3.0f);
            transform.position = mCurrentCell.gameObject.transform.position;
            if (atkConfirm)
            {
                targetPiece.PostMoveAnimation("Warp");
                pieceState = PieceState.SelectedAdjacent;
                CheckPathing(5);
                ShowPlacementOutlines();
            }
            else
            {
                ClearTileOutline();
                ClearPlacementOutlines();
                mPostMoveCell = null;
                mPostAdjacentCell = null;
                mPieceManager.clickedPiece = null;
                mPieceManager.SwitchSides(mColor);
                mPieceManager.ResetBonuses();
                clickedUnit = false;
            }
            mPieceManager.blocker.SetActive(false);
        }
        else if(mPieceManager.boxResponse == 0)
        {
            //Move object on board
            mPieceManager.blocker.SetActive(false);
            transform.position = mCurrentCell.transform.position;
            mPostMoveCell = null;
            pieceState = PieceState.Selected;
            yield break;
        }
    }
}
