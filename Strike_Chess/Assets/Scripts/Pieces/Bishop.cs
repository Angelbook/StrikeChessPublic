using UnityEngine;
using UnityEngine.UI;

public class Bishop : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, pieceClass);
        mMovement = new Vector3Int(0, 0, 7); //horizontal, Vertical, Diagonal
        base.Type[0] = "Bishop";
        int[] newStats;
        string[] newSkills;
        bool[] newCombatSkills;
        GetSprite(pieceClass);
        if (pieceClass == "Myrmidon")
        {
            newStats = new int[] { 3, 1, 4, 5, 3 };
            newSkills = new string[] { "Vantage", "Astra" };
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Mage")
        {
            newStats = new int[] { 3, 2, 4, 3, 3 };
            newSkills = new string[] { "Focus", "Nihil" };
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Thief")
        {
            newStats = new int[] { 3, 1, 2, 6, 3 };
            newSkills = new string[] { "Pass", "Lethality" };
            newCombatSkills = new bool[] { false, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Dark Mage")
        {
            newStats = new int[] { 5, 1, 5, 0, 5 };
            newSkills = new string[] { "Void Curse", "Lifetaker" };
            newCombatSkills = new bool[] { false, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
        else if (pieceClass == "Mercenary")
        {
            newStats = new int[] { 4, 2, 4, 2, 4 };
            newSkills = new string[] { "Patience", "Strong Riposte" };
            newCombatSkills = new bool[] { true, true, false };
            ChangeStats(newStats, newSkills, newCombatSkills, this);
        }
    }
}
