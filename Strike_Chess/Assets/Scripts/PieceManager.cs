using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool AreLordsAlive = true;
    public bool upgradeUsed;
    public static Color P1Color = Color.blue;
    public static Color P2Color = Color.red;
    public static Color32 Player1Appearance = new Color32(12, 67, 131, 255);
    public static Color32 Player2Appearance = new Color32(207, 42, 49, 255);
    public static int P1ColorToggle;
    public static int P2ColorToggle;
    public GameObject promotionMenu;
    public GameObject mUnitPrefab;
    public GameObject blocker;
    public Button upgradeButton;
    public int boxResponse = -1;
    public string promotionResponse;
    private int promotedPiecesRed = 0;
    private int promotedPiecesBlue = 0;
    private List<BasePiece> mBlueUnits = null;
    private List<BasePiece> mRedUnits = null;
    public List<String> P1RoyaltySetup = new List<String>();
    public List<String> P2RoyaltySetup = new List<String>();
    public BattleScript Bscript;
    public string[] NamesP1 = new string[16];
    public string[] NamesP2 = new string[16];
    private PromotionDropdown DropScript;
    private DialogueBox Box;
    public int[] playerExp = { 0, 1 };
    public BasePiece clickedPiece;

    private string[] mUnitOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "Q", "K", "B", "KN", "R"
    };
    public List<String> P1RoyaltyReserves = new List<string>(new string[]{
        "Armored Knight", "Armored Knight", "Fighter", "Fighter", "Archer", "Archer", "Beaststone", "Beaststone", "Beast Tribe", "Beast Tribe",
        "Myrmidon", "Myrmidon", "Mage", "Mage", "Thief", "Thief", "Mercenary", "Mercenary", "Dark Mage", "Dark Mage",
        "Cavalier", "Cavalier", "Pegasus Knight", "Pegasus Knight", "Wyvern Rider", "Wyvern Rider","Manakete", "Manakete", "Bird Tribe", "Bird Tribe",
        "Cleric", "Troubadour", "Dancer", "Tactician", "Heron"
        });
    public List<String> P2RoyaltyReserves = new List<string>(new string[]{
        "Armored Knight", "Armored Knight", "Fighter", "Fighter", "Archer", "Archer", "Beaststone", "Beaststone", "Beast Tribe", "Beast Tribe",
        "Myrmidon", "Myrmidon", "Mage", "Mage", "Thief", "Thief", "Mercenary", "Mercenary", "Dark Mage", "Dark Mage",
        "Cavalier", "Cavalier", "Pegasus Knight", "Pegasus Knight", "Wyvern Rider", "Wyvern Rider","Manakete", "Manakete", "Bird Tribe", "Bird Tribe",
        "Cleric", "Troubadour", "Dancer", "Tactician", "Heron"
        });

    private Dictionary<string, Type> mUnitLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };

    public void SetupBoard(Board board)
    {
        Bscript = GetComponent<BattleScript>();
        Box = GetComponent<DialogueBox>();
        DropScript = GetComponent<PromotionDropdown>();
        playerExp[0] = 0;
        playerExp[1] = 1;
        //createBlueP/Player1
        mBlueUnits = CreateUnits(P1Color, Player1Appearance, board, P1RoyaltySetup);
        //createRedUnits/Player2
        mRedUnits = CreateUnits(P2Color, Player2Appearance, board, P2RoyaltySetup);
        PlaceUnits(1, 0, mBlueUnits, board);
        PlaceUnits(6, 7, mRedUnits, board);

        //blueGoesFirst
        //blueGoesFirst
        SwitchSides(P2Color); //may have to fix this
        ResetBonuses();
        CrossArrowAnimations();
        blocker.gameObject.transform.SetAsLastSibling();
    }

    private List<BasePiece> CreateUnits(Color teamColor, Color32 spriteColor, Board board, List<string> unitTypes)
    {
        List<BasePiece> newUnits = new List<BasePiece>();
        string[] names;
        if (teamColor == Color.blue)
        {
            names = NamesP1;
        }
        else names = NamesP2;

        for (int order = 0; order < mUnitOrder.Length; order++)
        {
            //Applies Unit type (Knight, Bishop, Pawn) to create a game of single units.
            string key = mUnitOrder[order];
            Type pieceType = mUnitLibrary[key];

            //Stores new piece, adds sprite and script for the piece
            BasePiece newUnit = CreateUnit(pieceType);
            newUnits.Add(newUnit);
            string pieceClass = "Villager"; //comeback
            //setupnewUnit
            if (order > 7)
            {
                pieceClass = unitTypes[order - 8];
            }
            newUnit.Setup(teamColor, spriteColor, this, pieceClass);
            
            if (names[order].Length == 1)
            {
                newUnit.Type[2] = newUnit.Type[1];
            }
            else newUnit.Type[2] = names[order];
        }


        return newUnits; //returns list of new units
    }

    private BasePiece CreateUnit(Type unitType)
    {
        //CreateNewObject
        GameObject newUnitObject = Instantiate(mUnitPrefab);
        newUnitObject.transform.SetParent(transform);

        //setUnitScaleandPosition
        newUnitObject.transform.localScale = new Vector3(1, 1, 1);
        newUnitObject.transform.localRotation = Quaternion.identity;

        BasePiece createdPiece = (BasePiece)newUnitObject.AddComponent(unitType);
        return createdPiece;
    }

    private void PlaceUnits(int pawnRow, int royaltyRow, List<BasePiece> Units, Board board)
    {
        Pawn pawn;
        for (int i = 0; i < 8; i++)
        {
            //places pawns
            Units[i].Place(board.mAllCells[i, pawnRow]);
            if(Units[i] is Pawn)
            {
                pawn = (Pawn)Units[i];
                pawn.SetEnPassantSpaces();
            }
            //places royalty
            Units[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }

    }

    public void SetState(List<BasePiece> allUnits, PieceState newstate) //also resets Bliss, Dance
    {
        foreach (BasePiece piece in allUnits)
        {
            piece.pieceState = newstate;
        }
    }

    public void ClickedResets(Color color)
    {
        if (color == Color.blue)
        {
            foreach (BasePiece piece in mBlueUnits)
            {
                piece.clickedUnit = false;
            }
        }
        else
        {
            foreach (BasePiece piece in mRedUnits)
            {
                piece.clickedUnit = false;
            }
        }
    }

    //resetsAnimations
    public void CrossArrowAnimations()
    {
        
        foreach(BasePiece piece in mBlueUnits)
        {
            if (piece.blissed || piece.danced > 0 || piece.charmed || piece.rallied)
            {
                piece.animArrow.PlayInFixedTime("BuffArrowPiece", 0, 0f);
            }
            else piece.animArrow.Rebind();
            if (piece.roarCount > 0)
            {
                piece.animCross.PlayInFixedTime("RoarCross", 0, 0f);
            }
            else piece.animCross.Rebind();
            if (piece.promoted)
            {
                piece.anim.PlayInFixedTime("PieceFlash", 0, 0f);
            }
            else piece.anim.Rebind();
        }
        foreach (BasePiece piece in mRedUnits)
        {
            if (piece.blissed || piece.danced > 0 || piece.charmed || piece.rallied)
            {
                piece.animArrow.PlayInFixedTime("BuffArrowPiece", 0, 0f);
            }
            else piece.animArrow.Rebind();
            if (piece.roarCount > 0)
            {
                piece.animCross.PlayInFixedTime("RoarCross", 0, 0f);
            }
            else piece.animCross.Rebind();
            if (piece.promoted)
            {
                piece.anim.PlayInFixedTime("PieceFlash", 0, 0f);
            }
            else piece.anim.Rebind();
        }
        
    }

    public void ResetBonuses()
    {
        for (int i = 0; i < mBlueUnits.Count; i++)
        {
            mBlueUnits[i].VeteranBoost = false;
            mBlueUnits[i].charmed = false;
            mBlueUnits[i].rallied = false;
            mBlueUnits[i].focusRelief = true;
        }
        for (int i = 0; i < mBlueUnits.Count; i++)
        {
            mBlueUnits[i].NewTurnResets();
        }
        for (int i = 0; i < mRedUnits.Count; i++)
        {
            mRedUnits[i].VeteranBoost = false;
            mRedUnits[i].charmed = false;
            mRedUnits[i].rallied = false;
            mRedUnits[i].focusRelief = true;
        }
        for (int i = 0; i < mRedUnits.Count; i++)
        {
            mRedUnits[i].NewTurnResets();
        }
    }

    public void SwitchSides(Color color)
    {
        if (!AreLordsAlive) //Change this later so players choose who goes first
        {
            ResetUnits(); //resets pieces

            AreLordsAlive = true;

            color = P2Color;
        }

        if ((color == P1Color) == true)
        {
            SetState(mBlueUnits, PieceState.Enemy);
            SetState(mRedUnits, PieceState.Waiting);
            int i = mBlueUnits[0].transform.GetSiblingIndex();
            foreach (BasePiece piece in mRedUnits)
            {
                piece.transform.SetSiblingIndex(i);
                i++;
            }
            Debug.Log("Red's turn");
        }
        else
        {
            SetState(mRedUnits, PieceState.Enemy);
            SetState(mBlueUnits, PieceState.Waiting);
            int i = mRedUnits[0].transform.GetSiblingIndex();
            foreach (BasePiece piece in mBlueUnits)
            {
                piece.transform.SetSiblingIndex(i);
                i++;
            }
            Debug.Log("Blue's Turn");
        }
        upgradeButton.interactable = false;
        upgradeUsed = false;
    }

    public void PostMoveStates(Color color)
    {
        if ((color == P1Color) == true)
        {
            SetState(mBlueUnits, PieceState.Friendly);
        }
        else
        {
            SetState(mRedUnits, PieceState.Friendly);
        }
        CrossArrowAnimations();
    }

    public void ResetUnits()
    {
        //resets units on both sides
        foreach (BasePiece piece in mBlueUnits)
        {
            piece.Reset();
        }
        foreach (BasePiece piece in mRedUnits)
        {
            piece.Reset();
        }
    }

    public void PromotePiece(BasePiece pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        StartCoroutine(PromotedUnit(pawn, cell, teamColor, spriteColor));
    }

    IEnumerator PromotedUnit(BasePiece pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        string key = "P";
        // Remove Pawn
        blocker.SetActive(true);
        promotionMenu.SetActive(true);
        promotionMenu.transform.SetAsLastSibling();
        if (teamColor == Color.blue)
        {
            DropScript.PopulateDropdown(P1RoyaltyReserves, spriteColor);
        }
        else DropScript.PopulateDropdown(P2RoyaltyReserves, spriteColor);
        boxResponse = -1;
        while (boxResponse < 0)
        {
            yield return null;
        }
        promotionMenu.SetActive(false);
        if (promotionResponse != null)
        {
            if (promotionResponse == "Armored Knight" || promotionResponse == "Archer" || promotionResponse == "Fighter" || promotionResponse == "Beaststone" || promotionResponse == "Beast Tribe")
            {
                key = "R";
            }
            else if (promotionResponse == "Mage" || promotionResponse == "Myrmidon" || promotionResponse == "Dark Mage" || promotionResponse == "Thief" || promotionResponse == "Mercenary")
            {
                key = "B";
            }
            else if (promotionResponse == "Cavalier" || promotionResponse == "Pegasus Knight" || promotionResponse == "Wyvern Rider" || promotionResponse == "Bird Tribe" || promotionResponse == "Manakete")
            {
                key = "KN";
            }
            else if (promotionResponse == "Cleric" || promotionResponse == "Troubadour" || promotionResponse == "Dancer" || promotionResponse == "Heron" || promotionResponse == "Tactician")
            {
                key = "Q";
            }
            //remove from reserve list
            if (teamColor == Color.blue)
            {
                P1RoyaltyReserves.Remove(promotionResponse);
            }
            else P2RoyaltyReserves.Remove(promotionResponse);

            Type promoteType = mUnitLibrary[key];

            //Create
            pawn.Kill();
            BasePiece promotedUnit = CreateUnit(promoteType);
            promotedUnit.Setup(teamColor, spriteColor, this, promotionResponse);
            //Place
            promotedUnit.Place(cell);

            //Add to list of Units and reset states           
            if (teamColor == Color.blue)
            {
                mBlueUnits.Add(promotedUnit);
            }
            else mRedUnits.Add(promotedUnit);
            promotedUnit.NewTurnResets();
            CrossArrowAnimations();

            //Switches Turn Appropriately
            promotedUnit.pieceState = pawn.pieceState;
            blocker.SetActive(false);
        }
        foreach (BasePiece piece in mBlueUnits)
        {
           piece.anim.PlayInFixedTime("PieceFlash", 0, 0f);            
        }
        foreach (BasePiece piece in mRedUnits)
        {
          piece.anim.PlayInFixedTime("PieceFlash", 0, 0f);
        }
    }

    public void StartBattlePhase(BasePiece attacker, BasePiece defender)
    {
        Bscript.Battle(attacker, defender);
    }

    public void StartEntrapPhase(BasePiece attacker, BasePiece defender)
    {
        Bscript.Entrap(attacker, defender);
    }

    public void PreBattlePhase(BasePiece attacker, BasePiece defender)
    {
        boxResponse = -1;
        Box.PreBattleForecast(attacker.spriteColor, defender.spriteColor);
        Bscript.PreBattle(attacker, defender);
    }

    public void PromptBox(Color32 color, string skill = "Battle", string upgrade = "")
    {
        boxResponse = -1;
        Box.BattleDialogues(skill, color, upgrade);
    }

    public void DialogueYesNo(int option)
    {
        boxResponse = option;
    }

    public void ButtonExpCheck(BasePiece piece)
    {
        
        if (upgradeUsed == false && piece.promoted == false)
        {
            if (piece.mColor == Color.blue)
            {
                if ((playerExp[0]) >= (piece.upgradeCost * -1))
                {
                    upgradeButton.interactable = true;
                }
                else upgradeButton.interactable = false;
            }
            else if (piece.mColor == Color.red)
            {
                if ((playerExp[1]) >= (piece.upgradeCost * -1))
                {
                    upgradeButton.interactable = true;
                }
                else upgradeButton.interactable = false;
            }
        }
        else upgradeButton.interactable = false;
    }

}
