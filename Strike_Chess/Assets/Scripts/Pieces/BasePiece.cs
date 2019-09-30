using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Image iconSprite;
    public PieceState pieceState = PieceState.Waiting;
    public Color mColor = Color.clear;
    public Color32 spriteColor;
    public bool mIsFirstMove = true;
    public List<int> Stats = new List<int>() { 1, 1, 1, 1, 1 }; //CurrentHP, MinAtk, MaxAtk, Avoid, MaxHP
    public List<int> PrimaryStats = new List<int>(); // For Units who switch between two sets of stats
    public List<int> SecondaryStats = new List<int>();
    public List<string> Type = new List<string>() { "No Type", "No Class", ""}; //Piecetype, Class, Name
    public List<string> Skills = new List<string>() { "None", "None", "" };
    public List<bool> CombatSkills = new List<bool>() { false, false, false }; //Confirms if a skill is a combat skill or not
    public string boxSkill;
    public int upgradeCost = -4;
    public int turnCount = 0;
    public int roarCount = 0;
    public int danceBlissCount = 2; //if count is at 0 or less,  buff is Removed
    public int danced = 0; //0 for off, 1 for dance, 2 for special dance
    public int switchForPostMoveSkills; //1 for shove, 2 for heal/dance/bliss, 3 for entrap
    public bool clickedUnit = false;
    public bool transformingUnit = false;
    public bool BlackWhiteSwitch = false;
    public bool promoted = false;
    public bool Galeforce = false;
    private bool ReliefRenewalStopper;
    public bool postMoveAction = false;
    public bool blissed = false;
    public bool charmed = false;
    public bool rallied = false;
    public bool VeteranBoost = false;
    public bool killConfirm;
    public bool atkConfirm;
    public bool focusRelief;
    public bool foundAlly;
    public bool hitDone;
    public bool atkDone;
    private bool upgradingClick;
    private bool animationFix; //Late Update to fix animation positioning issues
    private bool dragFix; //Same but activates when being dragged
    public Animator anim;
    public Animator animArrow;
    public Animator animCross;
    public Animator animFX;


    //HP = Damage a unit can take before death
    //MinAtk = Minimum Damage a unit can deal
    //Max ATk = Max Damage a unit can deal
    //Avoid = Evasive ability of unit

    protected Cell mOriginalCell = null;
    public Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;
    protected Cell mPostMoveCell = null;
    protected Cell mPostAdjacentCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected Vector3Int FocusReliefRange = new Vector3Int(2, 2, 1);
    protected Vector3Int rallyVeteranRange = new Vector3Int(3, 3, 1);
    protected Vector3Int CharmRange = Vector3Int.one;
    protected Vector3Int postMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();
    public List<Cell> mEnemyCells = new List<Cell>();
    protected List<Cell> mAllyCells = new List<Cell>();
    protected List<Cell> mPlacementCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, string pieceClass)
    {
        mPieceManager = newPieceManager;
        animFX = GetComponent<Animator>();
        animFX.enabled = false;
        anim = transform.GetChild(0).GetComponent<Animator>();
        animArrow = transform.GetChild(3).GetComponent<Animator>();
        animCross = transform.GetChild(4).GetComponent<Animator>();        

        mColor = newTeamColor;
        Type[1] = pieceClass;
        if (mColor == Color.blue)
        {
            turnCount = 0;
        }
        else turnCount = 1;
        spriteColor = newSpriteColor;
        gameObject.transform.GetChild(1).GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    protected void ChangeStats(int[] newStats, string[] newSkills, bool[] newCombatSkills, BasePiece changedPiece)
    {
        for (int i = 0; i <= 4; i++)
        {
            changedPiece.Stats[i] = newStats[i];
        }
        for (int i = 0; i < newSkills.Length; i++)
        {
            changedPiece.Skills[i] = newSkills[i];
        }
        for (int i = 0; i < newSkills.Length; i++)
        {
            changedPiece.CombatSkills[i] = newCombatSkills[i];
        }
    }

    protected void GetSprite(string SpriteName)
    {
        SpriteName = "Pieces/" + SpriteName;
        if (Resources.Load<Sprite>(SpriteName) == true)
        {
            this.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(SpriteName);
        }
    }

    public virtual void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        //sets logic on grid position

        //Physically Moves the object
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();
        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        //Clears Current cell
        mCurrentCell.mCurrentPiece = null;
        //Removes Piece
        gameObject.SetActive(false);
    }

    public int BonusAtk()
    {
        int finalAtk = 0;
        if (danced > 0)
        {
            finalAtk += danced;
        }
        if (rallied)
        {
            finalAtk += 1;
        }
        return finalAtk;
    }

    public int BonusAvo()
    {
        int finalAvo = 0;
        if (charmed)
        {
            finalAvo += 1;
        }
        if (danced>0)
        {
            finalAvo += danced;
        }
        if (rallied)
        {
            finalAvo += 1;
        }
        return finalAvo;
    }

    public void SendInfo()
    {
        TextController.Values[9] = Stats[4].ToString(); //MaxHP
        TextController.Values[0] = Stats[0].ToString(); //HP
        TextController.Values[1] = Stats[1].ToString(); //Atkmin
        TextController.Values[2] = Stats[2].ToString(); //AtkMax
        TextController.Values[3] = Stats[3].ToString(); //Avoid
        if (transformingUnit || BlackWhiteSwitch)
        {
            TextController.Values[10] = SecondaryStats[1].ToString(); //AltAtkmin
            TextController.Values[11] = SecondaryStats[2].ToString(); //AltAtkMax
            TextController.Values[12] = SecondaryStats[3].ToString(); //AltAvoid
        }
        else
        {
            //Resets Text to nothing
            TextController.Values[10] = TextController.Values[11] = TextController.Values[12] = "";
        }
        TextController.Values[4] = Type[0]; //PieceType
        TextController.Values[5] = Type[1]; //Class
        TextController.Values[6] = Skills[0]; //Base Skill
        TextController.Values[7] = Skills[1]; //Promoted Skill
        TextController.Values[8] = Skills[2]; //2nd Base Skill
        TextController.Values[13] = Type[2]; //Name
        if (blissed)
        {
            TextController.BuffValues[0] = true;
        }
        else TextController.BuffValues[0] = false;
        if (danced>0)
        {
            TextController.BuffValues[1] = true;
            if (danced == 2)
            {
                TextController.BuffValues[4] = true;
            }
            else TextController.BuffValues[4] = false;
        }
        else TextController.BuffValues[1] = false;
        if (VeteranBoost)
        {
            TextController.BuffValues[2] = true;
            if (rallied)
            {
                TextController.BuffValues[5] = true;
            }
            else TextController.BuffValues[5] = false;
        }
        else TextController.BuffValues[2] = false;
        if (charmed)
        {
            TextController.BuffValues[3] = true;
        }
        else TextController.BuffValues[3] = false;
        HPupdate();
        TextController.TileShifter = BlackWhiteSwitch;
        TextController.transforming = transformingUnit;
        TextController.promoted = this.promoted;
        TextController.ColorOfPiece = mColor;
        TextController.update = true;
    }

    #region Movement

    //chooses an x and y Movement direction, then chooses how many spaces that piece can Move in said direction
    public virtual void CreateCellPath(int xDirection, int yDirection, int Movement)
    {
        //Checks current location on the board
        int currentXPosition = mCurrentCell.mBoardPosition.x;
        int currentYPosition = mCurrentCell.mBoardPosition.y;
        //Checks each cell for Movement

        bool PassEnemyStop = false;

        for (int i = 1; i <= Movement; i++)
        {
            currentXPosition += xDirection;
            currentYPosition += yDirection;

            //GetStateofTargetCell
            CellState cellstate = CellState.None;
            cellstate = mCurrentCell.mBoard.ValidateCell(currentXPosition, currentYPosition, this);

            //If an enemy is on the space, adds it to the list and spaces and stops
            if (cellstate == CellState.Enemy && Stats[1] > 0)
            {
                Debug.Log("En pass is " + mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass);
                if (PassEnemyStop == false) //Flag for Thieves, stops their enemy range from moving over enemy units
                {
                    if (!mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass ||
                    (mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass && Type[0] == "Pawn"))
                    {
                        mEnemyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                        PassEnemyStop = true;
                    }
                }
                if (Skills[0] != "Pass")
                {
                    if (mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass==false)
                    {
                        break;
                    }
                }
            }
            if (cellstate == CellState.OutOfBounds)
            {
                break;
            }

            //Adds EnPass Cells to mHighlighted Cells for non-pawn units
            if (cellstate == CellState.Enemy && mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
            }

            //If the cell is not free, break
            if (cellstate != CellState.Free && this.Skills[0] != "Pass"
            && mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass==false)
            {
                break;
            }   
            
            //adds to list of highlighted cells 
            if(cellstate != CellState.Enemy && cellstate != CellState.Friendly)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
            }            
        }
    }

    //1 or nothing FocusReliefChecks, 2 for Rally/Veteran, 3 for Charm
    protected virtual void CreateAllyPath(int xDirection, int yDirection, int Movement, int switchNum=1)
    {
        int currentXPosition = mCurrentCell.mBoardPosition.x;
        int currentYPosition = mCurrentCell.mBoardPosition.y;

        for (int i = 1; i <= Movement; i++)
        {
            currentXPosition += xDirection;
            currentYPosition += yDirection;

            //GetStateofTargetCell
            CellState cellstate = CellState.None;
            cellstate = mCurrentCell.mBoard.ValidateCell(currentXPosition, currentYPosition, this);
            if (cellstate == CellState.OutOfBounds)
            {
                break;
            }
            if (cellstate != CellState.OutOfBounds)
            {
                if (cellstate == CellState.Friendly)
                {
                    switch (switchNum)
                    {
                        case 1:
                            focusRelief = false;
                            break;
                        case 2:
                            mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].mCurrentPiece.VeteranBoost = true;
                            if (promoted)
                            {
                                mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].mCurrentPiece.rallied = true;
                            }
                            break;
                        case 3:
                            mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].mCurrentPiece.charmed = true;
                            break;
                    }
                }
                    if (Type[0] == "Lord" || Type[1] == "Tactician" || Type[1] == "Pegasus Knight" || Type[1]=="Mage")
                {
                    mAllyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                }
            }
        }
    }

    protected virtual void CreateAdjacentPaths(int xDirection, int yDirection)
    {
        int currentXPosition = mCurrentCell.mBoardPosition.x;
        int currentYPosition = mCurrentCell.mBoardPosition.y;
        currentXPosition += xDirection;
        currentYPosition += yDirection;
        CellState cellstate = CellState.None;
        cellstate = mCurrentCell.mBoard.ValidateCell(currentXPosition, currentYPosition, this);
        if (cellstate == CellState.Free && cellstate != CellState.OutOfBounds)
        {
            if (Type[1] != "Cavalier" || (Type[1] == "Cavalier" && promoted))
            {
                mPlacementCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
            }
        }
    }

    //For Healers, Shelter, and Shovers
    protected virtual void CreatePostMovePath(int xDirection, int yDirection, int Movement, int caseSwitch)
    {
        int currentXPosition = mCurrentCell.mBoardPosition.x;
        int currentYPosition = mCurrentCell.mBoardPosition.y;

        for (int i = 1; i <= Movement; i++)
        {
            currentXPosition += xDirection;
            currentYPosition += yDirection;
            CellState cellstate = mCurrentCell.mBoard.ValidateCell(currentXPosition, currentYPosition, this);
            if (cellstate != CellState.OutOfBounds)
            {
                if (cellstate != CellState.Free)
                {
                    switch (caseSwitch)
                    {
                        case 1: //Shove
                            CellState pushcellstate = mCurrentCell.mBoard.ValidateCell(currentXPosition + xDirection, currentYPosition + yDirection, this);
                            if (pushcellstate == CellState.Free)
                            {
                                mAllyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                            }
                            break;
                        case 2: //Dance, Bliss
                            if(cellstate == CellState.Friendly)
                            {
                                BasePiece dancedPiece = mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].mCurrentPiece;
                                dancedPiece.CheckPathing();
                                if ((dancedPiece.mHighlightedCells.Count > 0|| dancedPiece.mEnemyCells.Count>0) && dancedPiece.roarCount<=0)
                                {
                                    mAllyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                                } 
                            }
                            break;
                        case 3: //Heal, Shelter
                            if((cellstate == CellState.Friendly && Type[1]=="Cleric") || cellstate == CellState.Friendly && mPlacementCells.Count>0)
                            {
                                mAllyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                            }
                            break;
                        case 4: //Entrap, Warp
                            if (mPlacementCells.Count > 0)
                            {
                                if (cellstate == CellState.Friendly)
                                {
                                    mAllyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                                }
                                if (cellstate == CellState.Enemy && promoted && !mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition].EnPass)
                                {                                   
                                    mEnemyCells.Add(mCurrentCell.mBoard.mAllCells[currentXPosition, currentYPosition]);
                                    i = mMovement.x;
                                }
                            }
                            break;
                    }
                }
            }
        }
        
    }

    protected virtual void CheckPosts(int caseNum) //1 for shove, 2 dance, 3 heal/shelter, 4 troubadour, 5 adjacents
    {
        if (caseNum==5)
        {
            //horizontal
            CreateAdjacentPaths(1, 0);
            CreateAdjacentPaths(-1, 0);

            //Vertical
            CreateAdjacentPaths(0, 1);
            CreateAdjacentPaths(0, -1);
        }
        if (caseNum == 4 || Type[1]=="Cleric")
        {
            //horizontal
            CreatePostMovePath(1, 0, mMovement.x, caseNum);
            CreatePostMovePath(-1, 0, mMovement.x, caseNum);

            //Vertical
            CreatePostMovePath(0, 1, mMovement.y, caseNum);
            CreatePostMovePath(0, -1, mMovement.y, caseNum);

            //upperDiagonals
            CreatePostMovePath(1, 1, mMovement.z, caseNum);
            CreatePostMovePath(-1, 1, mMovement.z, caseNum);

            //LowerDiagonals
            CreatePostMovePath(-1, -1, mMovement.z, caseNum);
            CreatePostMovePath(1, -1, mMovement.z, caseNum);
        }
        else
        {
            //horizontal
            CreatePostMovePath(1, 0, postMovement.x, caseNum);
            CreatePostMovePath(-1, 0, postMovement.x, caseNum);

            //Vertical
            CreatePostMovePath(0, 1, postMovement.y, caseNum);
            CreatePostMovePath(0, -1, postMovement.y, caseNum);
        }
    }

    //Nothing for normal paths, 2 for Relief/Focus, 3 for Rally, 4 for Charm
    protected virtual void CheckPathing(int caseNum = 1)
    {
        switch (caseNum)
        {
            case 1:
                //Horizontal
                CreateCellPath(1, 0, mMovement.x);
                CreateCellPath(-1, 0, mMovement.x);

                //Vertical
                CreateCellPath(0, 1, mMovement.y);
                CreateCellPath(0, -1, mMovement.y);

                //upperDiagonals
                CreateCellPath(1, 1, mMovement.z);
                CreateCellPath(-1, 1, mMovement.z);

                //LowerDiagonals
                CreateCellPath(-1, -1, mMovement.z);
                CreateCellPath(1, -1, mMovement.z);
                break;
            case 2:
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
                break;
            case 3:
                //Horizontal
                CreateAllyPath(1, 0, rallyVeteranRange.x, 2);
                CreateAllyPath(-1, 0, rallyVeteranRange.x, 2);

                //Vertical
                CreateAllyPath(0, 1, rallyVeteranRange.y, 2);
                CreateAllyPath(0, -1, rallyVeteranRange.y, 2);

                //upperDiagonals
                CreateAllyPath(1, 1, rallyVeteranRange.z, 2);
                CreateAllyPath(-1, 1, rallyVeteranRange.z, 2);

                //LowerDiagonals
                CreateAllyPath(-1, -1, rallyVeteranRange.z, 2);
                CreateAllyPath(1, -1, rallyVeteranRange.z, 2);
                
                //L Shapes up
                CreateAllyPath(1, 2, 1, 2);
                CreateAllyPath(-1, 2, 1, 2);

                //L Shapes down
                CreateAllyPath(1, -2, 1, 2);
                CreateAllyPath(-1, -2, 1, 2);
               
                //L Shapes Right
                CreateAllyPath(2, 1, 1, 2);
                CreateAllyPath(2, -1, 1, 2);
                
                //L Shapes Left
                CreateAllyPath(-2, 1, 1, 2);
                CreateAllyPath(-2, -1, 1, 2);                
                break;
            case 4:
                //Horizontal
                CreateAllyPath(1, 0, CharmRange.x, 3);
                CreateAllyPath(-1, 0, CharmRange.x, 3);

                //Vertical
                CreateAllyPath(0, 1, CharmRange.y, 3);
                CreateAllyPath(0, -1, CharmRange.y, 3);

                //upperDiagonals
                CreateAllyPath(1, 1, CharmRange.z, 3);
                CreateAllyPath(-1, 1, CharmRange.z, 3);

                //LowerDiagonals
                CreateAllyPath(-1, -1, CharmRange.z, 3);
                CreateAllyPath(1, -1, CharmRange.z, 3);
                break;
        }
    }

    protected void ShowTileOutline()
    {
        foreach (Cell tile in mHighlightedCells)
            tile.mOutlineImage.enabled = true;
        foreach (Cell tile in mEnemyCells)
            tile.mEnemyImage.enabled = true;
        foreach (Cell tile in mAllyCells)
            tile.mAllyImage.enabled = true;
    }

    //Placements are separate as they are a 2-step process
    protected void ShowPlacementOutlines()
    {
        foreach (Cell tile in mPlacementCells)
            tile.mAllyImage.enabled = true;
    }

    protected void ClearPlacementOutlines()
    {
        foreach (Cell tile in mPlacementCells)
            tile.mAllyImage.enabled = false;
        mPlacementCells.Clear();
    }

    protected void ClearTileOutline()
    {
        foreach(Cell tile in mHighlightedCells)
            tile.mOutlineImage.enabled = false;
        mHighlightedCells.Clear();
        foreach(Cell tile in mEnemyCells)
            tile.mEnemyImage.enabled = false;
        mEnemyCells.Clear();
        foreach (Cell tile in mAllyCells)
            tile.mAllyImage.enabled = false;
        mAllyCells.Clear();
    }

    public void PostMoveAnimation(string animation)
    {
        StartCoroutine(PostMoveAnim(animation));
    }

    IEnumerator PostMoveAnim(string animation)
    {
        animationFix = true;
        animFX.enabled = true;
        animFX.PlayInFixedTime(animation, 0, 0f);
        yield return new WaitForSeconds(2f);
        if(animation!="Warp")
        animFX.Rebind();
    }

    IEnumerator MoveNumerator()
    {
        bool battled = false;
        if (MatchesState(mTargetCell.mBoardPosition.x, mTargetCell.mBoardPosition.y, CellState.Enemy) == true)
        {
            battled = true;
            BasePiece attacker = this;
            BasePiece defender = mTargetCell.mCurrentPiece;
            defender.SendInfo();
            bool lunge = false;
            bool counterAttack = CheckCounterAttack(attacker, defender);
            mPieceManager.PreBattlePhase(attacker, defender);
            mPieceManager.blocker.SetActive(true);
            while (mPieceManager.boxResponse < 0) //If yes when initiating battle prompt
            {
                yield return null;
            }
            if (mPieceManager.boxResponse == 1)
            {
                if (Skills[1] == "Lunge" && promoted)
                {
                    mPieceManager.PromptBox(spriteColor, "Lunge");
                    while (mPieceManager.boxResponse < 0) //If yes when initiating battle prompt
                    {
                        yield return null;
                    }
                    if (mPieceManager.boxResponse == 1)
                    {
                        lunge = true;
                    }
                }
                StartFight(attacker, defender);
                while (hitDone==false)
                {
                    yield return null;
                }
                if (atkConfirm == true)
                {
                    if(Type[1]=="Beast Tribe" && promoted)
                    {
                        defender.roarCount = 2;
                    }
                    while (atkDone == false)
                    {
                        yield return null;
                    }
                }
                defender.SendInfo();
                if (killConfirm == false)
                {
                    mTargetCell = mCurrentCell;
                    if (counterAttack || defender.Skills[0] == "Vantage")
                    {
                        StartFight(defender, attacker);
                        while (defender.hitDone == false)
                        {
                            yield return null;
                        }
                        if (defender.atkConfirm == true)
                        {
                            while (defender.atkDone == false)
                            {
                                yield return null;
                            }
                        }
                        if (attacker.Stats[0] < 1)
                        {
                            animFX.enabled = true;
                            animFX.Play("Defeated");
                            animationFix = true;
                            yield return new WaitForSeconds(1f);
                            animFX.enabled = false;
                            animationFix = false;
                            defender.ExpRewards(defender, attacker);
                            SendInfo();
                            mTargetCell = null;
                            ClearTileOutline();
                            mCurrentCell.RemovePiece();
                            mPieceManager.SwitchSides(mColor);
                            mPieceManager.ResetBonuses();
                            mPieceManager.CrossArrowAnimations();
                            Kill();
                            yield break;
                        }
                        SendInfo();
                    }
                    if (attacker.Stats[0] > 0 && lunge)
                    {
                        //clear current cell
                        mCurrentCell.mCurrentPiece = null;
                        defender.mCurrentCell.mCurrentPiece = null;

                        //Switch Original cell with Target Cell
                        Cell swapCell = defender.mCurrentCell;
                        defender.mCurrentCell = mTargetCell;
                        mTargetCell = swapCell;
                        defender.mCurrentCell.mCurrentPiece = defender;
                        mCurrentCell.mCurrentPiece = attacker;

                        //Move object on board
                        defender.transform.position = defender.mCurrentCell.transform.position;
                        defender.atkDone = defender.hitDone = false;
                    }
                }
                else
                {
                    ExpRewards(attacker, defender);
                    Debug.Log("Enemy defeated");
                    defender.animationFix = true;
                    defender.animFX.enabled = true;
                    defender.animFX.Play("Defeated");                    
                    yield return new WaitForSeconds(1.5f);
                    defender.animFX.enabled = false;
                    defender.animationFix = false;
                    defender.Kill();
                    if (Galeforce && promoted)
                    {
                        Galeforce = false;
                        transform.position = mCurrentCell.transform.position;
                        mTargetCell = null;
                        mPieceManager.PostMoveStates(mColor);
                        pieceState = PieceState.Waiting;
                        yield break;
                    }
                }
            }
            else
            {
                //Move object on board
                mPieceManager.blocker.SetActive(false);
                transform.position = mCurrentCell.transform.position;
                mTargetCell = null;
                yield break;
            }
        }
        //reset flags
        atkDone = hitDone = false;    

        //clear current cell
        mCurrentCell.mCurrentPiece = null;

        //Removes EnPass incase a miss occurs
        if (mTargetCell.EnPass == true)
        {
            mTargetCell.mCurrentPiece = null;
        }

        //Switch Original cell with Target Cell        
        mCurrentCell = mTargetCell;       
        mCurrentCell.mCurrentPiece = this;

        //Move object on board
        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
        ClearTileOutline();
        if (Type[1] == "Villager")
        {
            CheckForPromotion();
        }

        if (battled == false && postMoveAction)
        {
            CheckPosts(5);
            CheckPosts(switchForPostMoveSkills);
            if (mAllyCells.Count > 0)
            {
                mPieceManager.blocker.SetActive(true);
                mPieceManager.PromptBox(spriteColor, boxSkill);
                while (mPieceManager.boxResponse < 0)
                {
                    yield return null;
                }
                if (mPieceManager.boxResponse == 1)
                {
                    mPieceManager.blocker.SetActive(false);
                    mPieceManager.PostMoveStates(mColor);
                    pieceState = PieceState.Selected;
                    CheckPosts(switchForPostMoveSkills);
                    ShowTileOutline();
                    clickedUnit = true;
                    yield break;
                }
                mPieceManager.blocker.SetActive(false);
            }
        }
        //end turn
        if (Type[1] == "Lord")
        {
            King Lord = (King)this;
            Lord.CastleChecks();
        }
        if (Stats[0] > 0)
        {
            mPieceManager.SwitchSides(mColor);
            mPieceManager.ResetBonuses();
            mPieceManager.CrossArrowAnimations();
            SendInfo();
            yield break;
        }
        else yield break;
    }

    public bool CheckCounterAttack(BasePiece attacker, BasePiece defender)
    {
        bool counterable = false;
        defender.CheckPathing();
        if (defender.mEnemyCells.Contains(attacker.mCurrentCell))
        {
            counterable = true;
        }
        defender.ClearTileOutline();
        return counterable;
    }

    protected virtual void Move()
    {
        StartCoroutine(MoveNumerator());
        if(Type[0]!="Pawn")
        mIsFirstMove = false; //After making a call to Move, first Move value turns off.
    }

    protected virtual bool MatchesState(int targetX, int targetY, CellState targetState, CellState targetState2 = CellState.None, bool addE = true)
    {
        //Checks to see if targeted Cell matches the state we're looking for 
        CellState targetCell = CellState.None;
        targetCell = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);
        if (targetCell == targetState)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if(targetCell == CellState.Enemy)
            {
                if (!mCurrentCell.mBoard.mAllCells[targetX, targetY].EnPass ||
                (mCurrentCell.mBoard.mAllCells[targetX, targetY].EnPass && Type[0] == "Pawn"))
                {
                    mEnemyCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                }
                else return false;
            }
            return true;
        }
        return false;
    }

    public virtual void PostMoveSkill()
    {

    }

    public virtual void PostMoveSkillAdjacent()
    {
        BasePiece targetPiece = mPostMoveCell.mCurrentPiece;

        //updates for Moved piece
        targetPiece.mCurrentCell.mCurrentPiece = null;
        targetPiece.mCurrentCell = mPostAdjacentCell;
        targetPiece.mCurrentCell.mCurrentPiece = targetPiece;
        targetPiece.transform.position = targetPiece.mCurrentCell.gameObject.transform.position;

        if (Type[1] == "Troubadour")
        {
            targetPiece.PostMoveAnimation("Warp2");
        }
        transform.position = mCurrentCell.gameObject.transform.position;
        ClearPlacementOutlines();
        mPostMoveCell = null;
        mPostAdjacentCell = null;
        clickedUnit = false;
        mPieceManager.clickedPiece = null;
        mPieceManager.SwitchSides(mColor);
        mPieceManager.ResetBonuses();
        mPieceManager.CrossArrowAnimations();
        targetPiece.mIsFirstMove = false;

        if (targetPiece.Type[1] == "Villager")
        {
            targetPiece.CheckForPromotion();            
        }
    }

    #endregion

    #region Events

    public void CheckForPromotion()
    {
        //target position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        CellState cellstate = mCurrentCell.mBoard.ValidateCell(currentX, currentY + mMovement.y, this);

        if (cellstate == CellState.OutOfBounds)
        {
            mPieceManager.PromotePiece(this, mCurrentCell, mColor, spriteColor);
        }
    }

    public void ClickCheckPostAdjacent()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            //Rect clickRect = new Rect(Input.mousePosition, Vector2.one);
            foreach (Cell cell in mPlacementCells)
            {
                if (mousePosition.x > (cell.mRectTransform.position.x - (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.x < (cell.mRectTransform.position.x + (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.y > (cell.mRectTransform.position.y - (cell.mRectTransform.rect.height / 4)) &&
                     mousePosition.y < (cell.mRectTransform.position.y + (cell.mRectTransform.rect.height / 4)))
                {
                    mPostAdjacentCell = cell;
                }
            }
            if (mPostAdjacentCell != null)
            {
                PostMoveSkillAdjacent();
            }
        }
    }

    public void ClickCheckPostMove()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            foreach (Cell cell in mAllyCells)
            {
                if (mousePosition.x > (cell.mRectTransform.position.x - (cell.mRectTransform.rect.width / 4)) &&
                      mousePosition.x < (cell.mRectTransform.position.x + (cell.mRectTransform.rect.width / 4)) &&
                      mousePosition.y > (cell.mRectTransform.position.y - (cell.mRectTransform.rect.height / 4)) &&
                      mousePosition.y < (cell.mRectTransform.position.y + (cell.mRectTransform.rect.height / 4)))
                {
                    mPostMoveCell = cell;
                }
            }
            if (Type[1] == "Troubadour")
            {
                foreach (Cell cell in mEnemyCells)
                {
                    if (mousePosition.x > (cell.mRectTransform.position.x - (cell.mRectTransform.rect.width / 4)) &&
                          mousePosition.x < (cell.mRectTransform.position.x + (cell.mRectTransform.rect.width / 4)) &&
                          mousePosition.y > (cell.mRectTransform.position.y - (cell.mRectTransform.rect.height / 4)) &&
                          mousePosition.y < (cell.mRectTransform.position.y + (cell.mRectTransform.rect.height / 4)))
                    {
                        mPostMoveCell = cell;
                    }
                }
            }
            if (mPostMoveCell != null)
            {
                PostMoveSkill();
            }
        }
    }

    public void ClickCheck()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            foreach (Cell cell in mHighlightedCells)
            {
                if (mousePosition.x > (cell.mRectTransform.position.x - (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.x < (cell.mRectTransform.position.x + (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.y > (cell.mRectTransform.position.y - (cell.mRectTransform.rect.height / 4)) &&
                     mousePosition.y < (cell.mRectTransform.position.y + (cell.mRectTransform.rect.height / 4)))
                {
                    mTargetCell = cell;
                    clickedUnit = false;
                }
            }
            foreach (Cell cell in mEnemyCells)
            {
                if (mousePosition.x > (cell.mRectTransform.position.x - (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.x < (cell.mRectTransform.position.x + (cell.mRectTransform.rect.width / 4)) &&
                     mousePosition.y > (cell.mRectTransform.position.y - (cell.mRectTransform.rect.height / 4)) &&
                     mousePosition.y < (cell.mRectTransform.position.y + (cell.mRectTransform.rect.height / 4)))
                {
                    mTargetCell = cell;
                    clickedUnit = false;
                }
            }
            if (mousePosition.x<(mCurrentCell.mBoard.GetComponent<RectTransform>().rect.xMax) || mPieceManager.upgradeUsed==true)
            {
                clickedUnit = false;
                mPieceManager.clickedPiece = null;
            }
            ClearTileOutline();
            if (mTargetCell != null)
            {
                Move();
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        transform.position = mCurrentCell.transform.position;
        if (pieceState == PieceState.Waiting && roarCount<=0)
        {
            CheckPathing();
            ShowTileOutline();
            mPieceManager.ClickedResets(mColor);
            clickedUnit = true;
            mPieceManager.clickedPiece = this;
        }
        else if (pieceState == PieceState.Selected)
        {
            CheckPosts(switchForPostMoveSkills);
            ShowTileOutline();
            clickedUnit = true;
            mPieceManager.clickedPiece = this;
        }
        else if (pieceState == PieceState.SelectedAdjacent)
        {
            CheckPosts(5);
            ShowPlacementOutlines();
            clickedUnit = true;
            mPieceManager.clickedPiece = this;
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        SendInfo();
        if(pieceState == PieceState.Waiting)
        {
            mPieceManager.ButtonExpCheck(this);
        }
        clickedUnit = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (pieceState == PieceState.Waiting && mPieceManager.clickedPiece==null && roarCount <= 0)
        {
            CheckPathing();
            if (Type[0] == "Lord")
            {
                CheckPathing(4);
            }
            if (Type[1] == "Tactician")
            {
                CheckPathing(3);
            }
            if(Type[1] == "Mage" || Type[1]== "PegasusKnight")
            {
                CheckPathing(2);
            }
            ShowTileOutline();
        }
        if (pieceState == PieceState.Selected)
        {
            CheckPosts(switchForPostMoveSkills);
            ShowTileOutline();
        }
        if(pieceState == PieceState.SelectedAdjacent)
        {
            CheckPosts(5);
            ShowPlacementOutlines();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (clickedUnit == false)
        {
            ClearTileOutline();
            if (pieceState == PieceState.SelectedAdjacent)
            {
                ClearPlacementOutlines();
            }
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        bool breakflag = false;
        if (pieceState == PieceState.Waiting && roarCount <= 0)
        {
            dragFix = true;
            CheckPathing();
            if (Type[0] == "Lord")
            {
                CheckPathing(4);
            }
            if (Type[1] == "Tactician")
            {
                CheckPathing(3);
            }
            if (Type[1] == "Mage" || Type[1] == "PegasusKnight")
            {
                CheckPathing(2);
            }
            ShowTileOutline();
            transform.position += (Vector3)eventData.delta; //Moves object with pointer
            foreach (Cell cell in mHighlightedCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
                {
                    //If the mouse points to a legitimate highlighted cell, then the piece will transform it's position
                    mTargetCell = cell;
                    breakflag = true;
                }
                if (breakflag == false)
                {
                    mTargetCell = null;
                }
                else break;
                //if the mouse isn't in a highlighted cell, the Move is not valid
            }
            foreach (Cell cell in mEnemyCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
                {
                    //If the mouse points to a legitimate highlighted cell, then the piece will transform it's position
                    mTargetCell = cell;
                    breakflag = true;
                }
                if (breakflag == false)
                {
                    mTargetCell = null;
                }
                else break;
                //if the mouse isn't in a highlighted cell, the Move is not valid               
            }
        }
        if (pieceState == PieceState.Selected)
        {
            dragFix = true;
            CheckPosts(switchForPostMoveSkills);
            ShowTileOutline();
            transform.position += (Vector3)eventData.delta;
            foreach (Cell cell in mAllyCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
                {
                    //If the mouse points to a legitimate highlighted cell, then the piece will transform it's position
                    mPostMoveCell = cell;
                    breakflag = true;
                }
                if (breakflag == false)
                {
                    mPostMoveCell = null;
                }
                else break;
                //if the mouse isn't in a highlighted cell, the Move is not valid
            }
            foreach (Cell cell in mEnemyCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
                {
                    //If the mouse points to a legitimate highlighted cell, then the piece will transform it's position
                    mPostMoveCell = cell;
                    breakflag = true;
                }
                if (breakflag == false)
                {
                    mPostMoveCell = null;
                }
                else break;
            }
        }
        if (pieceState == PieceState.SelectedAdjacent)
        {
            dragFix = true;
            CheckPosts(5);
            ShowPlacementOutlines();
            transform.position += (Vector3)eventData.delta;
            foreach (Cell cell in mPlacementCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
                {
                    //If the mouse points to a legitimate highlighted cell, then the piece will transform it's position
                    mPostAdjacentCell = cell;
                    breakflag = true;
                }
                if (breakflag == false)
                {
                    mPostAdjacentCell = null;
                }
                else break;
                //if the mouse isn't in a highlighted cell, the Move is not valid
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        dragFix = false;
        ClearTileOutline();
        if(pieceState == PieceState.Selected)
        {
            if (!mPostMoveCell)
            {
                transform.position = mCurrentCell.gameObject.transform.position;
                return;
            }
            PostMoveSkill();
            return;
        }
        if (pieceState == PieceState.SelectedAdjacent)
        {
            if (!mPostAdjacentCell)
            {
                transform.position = mCurrentCell.gameObject.transform.position;
                return;
            }
            PostMoveSkillAdjacent();
            return;
        }
        if (pieceState == PieceState.Waiting)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(mPieceManager.upgradeButton.GetComponent<RectTransform>(), Input.mousePosition))
            {
                if (mPieceManager.upgradeButton.interactable && pieceState == PieceState.Waiting)
                {
                    Upgrade();
                }
            }
            if (!mTargetCell) //If there's no highlighted Cell, the position resets to current Cell.
            {
                transform.position = mCurrentCell.gameObject.transform.position;
                return;
            }
            Move();
        }
    }

    #endregion

    #region BattleMechanics

    //Runs for AOE checks at the start of a turn
    public virtual void NewTurnResets()
    {
        if (Type[1] == "Cleric" && promoted && !mPieceManager.upgradeUsed && turnCount % 2 == 0)
        {
            Heal(4);
        }
        danceBlissCount -= 1;
        if (danceBlissCount <= 0)
        {
            danced = 0;
            blissed = false;
        }
        if (Type[1] == "Tactician")
        {
            CheckPathing(3);
        }
        if (Type[1] == "Pegasus Knight")
        {
            Galeforce = true;
            turnCount += 1;
            CheckPathing(2);
            if (focusRelief && turnCount % 2 == 0 && !mPieceManager.upgradeUsed)
            {
                Heal(2);
            }
        }
        if (Type[1] == "Mage")
        {
            CheckPathing(2);
        }
        if (roarCount > 0)
        {
            roarCount -= 1;
        }
        if (BlackWhiteSwitch)
        {
            TileColorCheck();
        }        
    }

    void StartFight(BasePiece Self, BasePiece Enemy)
    {
        mPieceManager.StartBattlePhase(Self, Enemy);
    }

    public void Upgrade()
    {
        StartCoroutine(UpgradeNumerator(this));
    }

    IEnumerator UpgradeNumerator(BasePiece piece)
    {
        mPieceManager.PromptBox(spriteColor, "Upgrade", Skills[1]);
        while (mPieceManager.boxResponse < 0)
        {
            yield return null;
        }
        if (mPieceManager.boxResponse == 1)
        {
            transform.GetChild(0).SetSiblingIndex(1);
            piece.promoted = true;
            if (transformingUnit)
            {
                Stats = SecondaryStats;
            }
            if (Type[1] == "Heron" || Type[1] == "Cavalier")
            {
                postMoveAction = true;
            }
            if (Type[0] == "Lord")
            {
                LordUpgrade();
            }
            mPieceManager.upgradeUsed = true;
            ExpGain(upgradeCost);
            NewTurnResets();
            mPieceManager.CrossArrowAnimations();
        }
        mPieceManager.upgradeButton.interactable = false;
        SendInfo();
    }

    public void Heal(int healAmount)
    {
        Stats[0] += healAmount;
        if (Stats[0] > Stats[4])
        {
            Stats[0] = Stats[4];
        }
        HPupdate();
    }

    public void ExpGain(int expChange)
    {
        if (mColor == Color.blue)
        {
            mPieceManager.playerExp[0] += expChange;
        }
        else
        {
            mPieceManager.playerExp[1] += expChange;
        }
        SendInfo();
        TextController.ColorOfPiece = mColor;
        TextController.update = true;
    }

    private void ExpRewards(BasePiece attacker, BasePiece defender)
    {
        int expRewards = 1;
        if (defender.Type[1] != "Villager")
        {
            expRewards = 2;
        }
        if (attacker.Type[1] == "Villager")
        {
            expRewards *= 2;
        }
        if (attacker.VeteranBoost)
        {
            expRewards += 1;
        }
        if (defender.Skills[0] == "Void Curse")
        {
            expRewards = 0;
        }
        ExpGain(expRewards);
    }

    protected virtual void TileColorCheck()
    {
        if (mCurrentCell.tileColor == Color.white)
        {
            Stats = PrimaryStats;
        }
        else Stats = SecondaryStats;
    }

    public virtual void LordUpgrade()
    {

    }

    public void HPupdate()
    {
        //Adjusts the fill amount of the color and flash
        gameObject.transform.GetChild(1).GetComponent<Image>().fillAmount = gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = (((float)Stats[0] / (float)Stats[4]));
    }

    #endregion

    private void Update()
    {
        if (danceBlissCount <= 0)
        {
            danced = 0;
            blissed = false;
        }
        if (clickedUnit)
        {
            if(pieceState == PieceState.Waiting)
            {
                ClickCheck();
            }
            else if (pieceState == PieceState.Selected)
            {
               ClickCheckPostMove();
            }
            else if (pieceState == PieceState.SelectedAdjacent)
            {
               ClickCheckPostAdjacent();
            }
        }
    }

    private void LateUpdate()
    {
        if (animationFix)
        {
            if (dragFix)
            {
                transform.position = Input.mousePosition;
            }
            else transform.position = mCurrentCell.transform.position;
        }
    }
}
