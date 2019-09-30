

// BattleScript
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{
	public Wheel wheel;
    public List<TextMeshProUGUI> AttackerPreBattle = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> DefenderPreBattle = new List<TextMeshProUGUI>();

    private bool criticalHit;
	private bool lethality;
	private bool miracle;
	private bool greatShield;

    //Set to false on calculations, intended to show up on Prebattle forecast only when these skills are active/set to true
    private bool PreBattleTrample;
    private bool PreBattleUnderdog;
    private bool PreBattleArcherSkills;
    private bool PreBattleMercenarySkills;

    private int AstraDmg;
	private bool NihilNegationAtk;
	private bool NihilNegationDef;


	private void NihilChecks(BasePiece attacker, BasePiece defender)
	{
		if (attacker.Skills[1] == "Nihil" && attacker.promoted)
		{
			NihilNegationAtk = true;
		}
		else
		{
			NihilNegationAtk = false;
		}
		if (defender.Skills[1] == "Nihil" && defender.promoted)
		{
			NihilNegationDef = true;
		}
		else
		{
			NihilNegationDef = false;
		}
	}

	private void D4Skills(BasePiece attacker, BasePiece defender)
	{
		int num = Random.Range(1, 5);
		if (attacker.Skills[1] == "Astra" && attacker.promoted)
		{
			Debug.Log((object)"astra triggered");
			wheel.Astra = AstraDmg = num;
		}
		else
		{
            wheel.Astra = AstraDmg = 0;
		}
		if (defender.Skills[1] == "Great Shield" && defender.promoted && num == 4)
		{
			wheel.GreatShield = greatShield = true;
			Debug.Log((object)"Great Shield triggered");
		}
		else
		{
            wheel.GreatShield = greatShield = false;
		}
	}

	private void D10Skills(BasePiece attacker, BasePiece defender)
	{
        int num = Random.Range(1, 11);
		if (attacker.Skills[1] == "Lethality" && attacker.promoted && num == 10)
		{
			Debug.Log((object)"Lethality triggered");
            wheel.lethality = lethality = true;
		}
		else
		{
            wheel.lethality = lethality = false;
		}
        if (defender.Type[1] == "Cleric" && num == 10 && !NihilNegationAtk && defender.Stats[0] > 1)
		{
			Debug.Log((object)"Miracle triggered");
            wheel.miracle = miracle = true;
		}
		else
		{
			wheel.miracle = miracle = false;
		}
	}

	private int AvoidCalculator(BasePiece attacker, BasePiece defender, int Avo)
	{
        PreBattleUnderdog = PreBattleArcherSkills = PreBattleMercenarySkills = false;
        Avo += defender.danced;
        if (attacker.Type[1] == "Mage" && attacker.focusRelief && !NihilNegationDef)
		{
			Avo -= 3;
		}
		if (attacker.Skills[1] == "Underdog" && attacker.promoted && defender.Type[0] != "Pawn" && !NihilNegationDef)
		{
            PreBattleUnderdog = true;
			Avo -= 2;
		}
		if (attacker.Type[0] != "Pawn" && defender.Skills[1] == "Underdog" && defender.promoted && !NihilNegationAtk)
		{
			Avo += 2;
		}
		if (attacker.Skills[0] == "Patience" && attacker.pieceState == PieceState.Enemy && !NihilNegationDef)
		{
            PreBattleMercenarySkills = true;
			Avo -= 3;
		}
		if (attacker.Skills[0] == "Prescience" && attacker.pieceState == PieceState.Waiting && !NihilNegationDef)
		{
            PreBattleArcherSkills = true;
			Avo -= 3;
		}
		if (defender.Skills[0] == "Patience" && defender.pieceState == PieceState.Enemy && !NihilNegationAtk)
		{
			Avo += 3;
		}
		if (defender.Skills[0] == "Prescience" && defender.pieceState == PieceState.Waiting && !NihilNegationAtk)
		{
			Avo += 3;
		}
		if (defender.promoted && defender.Skills[1] == "Heavy Blade" && !NihilNegationAtk)
		{
			Avo--;
		}
		if (defender.promoted && defender.Skills[1] == "Dancing Blade" && !NihilNegationAtk)
		{
			Avo += 3;
		}
        if (defender.charmed)
        {
            Avo++;
        }
		if (defender.rallied)
		{
			Avo++;
		}
		return Avo;
	}

	private int HitCalculator()
	{
        int num = Random.Range(1, 13);
		wheel.CriticalHit = criticalHit = ((num == 12) ? true : false);
		wheel.animationNum = num;
		return num;
	}

	private int DamageCalculator(BasePiece attacker, BasePiece defender, bool battleCalc=true)
	{
        PreBattleTrample = false;
		int initialDamage;
        if (battleCalc) //will calculate damage normallly, prebattles require this to be false for bonus damage calculation
        {
            if (!attacker.blissed)
            {
                //calculates random damage
                initialDamage = Random.Range(1, 7);
                wheel.atk = initialDamage;
                if (attacker.Type[1] == "Beaststone")
                {
                    initialDamage++;
                }
                if (initialDamage < attacker.Stats[1])
                {
                    initialDamage = attacker.Stats[1];
                }
                if (initialDamage > attacker.Stats[2])
                {
                    initialDamage = attacker.Stats[2];
                }
            }
            else
            {
                initialDamage = attacker.Stats[2];
                wheel.atk = 6;
            }
        }
        else initialDamage = 0;
		int finalDamage = initialDamage;
		if (attacker.Type[1] == "Wyvern Rider" && defender.Type[0] != "Knight" && !NihilNegationDef)
		{
            PreBattleTrample = true;
			finalDamage += 2;
		}
		if (attacker.pieceState == PieceState.Enemy && attacker.Skills[1] == "Strong Riposte" && attacker.promoted && !NihilNegationDef)
		{
			finalDamage += 3;
		}
		if (attacker.pieceState == PieceState.Waiting && attacker.Skills[1] == "Quickdraw" && attacker.promoted && !NihilNegationDef)
		{
			finalDamage += 3;
		}
		if (criticalHit)
		{
			finalDamage *= 2;
		}
		if (attacker.promoted && attacker.Skills[1] == "Heavy Blade" && !NihilNegationDef)
		{
			finalDamage += 3;
		}
		if (defender.promoted && defender.Skills[1] == "Dancing Blade" && !NihilNegationAtk)
		{
			finalDamage++;
		}
		if (AstraDmg > 0 && !NihilNegationDef)
		{
			finalDamage += AstraDmg;
		}
		if (defender.Skills[0] == "Defense+2" && !NihilNegationAtk)
		{
			finalDamage -= 2;
		}
		if (attacker.rallied)
		{
			finalDamage++;
		}
		if (defender.rallied)
		{
			finalDamage--;
		}
        if (defender.danced == 2)
        {
            finalDamage--;
        }
        finalDamage += attacker.danced;
		if (finalDamage < 0 && battleCalc)
		{
			finalDamage = 0;
		}
		if (defender.Skills[1] == "Dragonskin" && defender.promoted && !NihilNegationAtk)
		{
			float dragonskinDamage = finalDamage;
			finalDamage = Mathf.CeilToInt(dragonskinDamage / 2f);
		}
		if (lethality && !NihilNegationDef)
		{
			finalDamage = 99;
		}
		if (greatShield && !NihilNegationAtk)
		{
			finalDamage = 0;
		}
		if (attacker.Skills[1] == "Sol" && attacker.promoted && !NihilNegationDef)
		{
			if (finalDamage > defender.Stats[0])
			{
				attacker.Heal(defender.Stats[0]);
			}
			else
			{
				attacker.Heal(finalDamage);
			}
		}
		return finalDamage;
	}

	public void Battle(BasePiece attacker, BasePiece defender)
	{
		NihilChecks(attacker, defender);
		D10Skills(attacker, defender);
		D4Skills(attacker, defender);
		wheel.Colors(attacker.spriteColor);
        wheel.attackerColor = attacker.spriteColor;
        wheel.defenderColor = defender.spriteColor;
		int avoid = AvoidCalculator(attacker, defender, defender.Stats[3]);
		Debug.Log((object)("Final Avo was: " + avoid));
		SetWheelTextValues(attacker, avoid);
		wheel.transform.SetAsLastSibling();
		int Hitrate = HitCalculator();
		Debug.Log((object)("Hitrate was: " + Hitrate));
		if (Hitrate > avoid)
		{
			attacker.atkConfirm = true;
			wheel.animationsCalled = 2;
			int dmg = DamageCalculator(attacker, defender);
			Debug.Log((object)("Final Dmg: " + dmg));
			defender.Stats[0] -= dmg;
		}
		else
		{
			attacker.atkConfirm = false;
			wheel.animationsCalled = 1;
			Debug.Log((object)"Missed Attack");
		}
		wheel.SpinBegin(attacker);
		if (defender.Stats[0] <= 0)
		{
			if (!miracle)
			{
				defender.Stats[0] = 0;
				if (attacker.Skills[1] == "Lifetaker" && attacker.promoted)
				{
					attacker.Stats[0] = attacker.Stats[4];
				}
				attacker.killConfirm = true;
				Debug.Log((object)"Defender died");
			}
			else
			{
				defender.Stats[0] = 1;
			}
		}
		else
		{
			attacker.killConfirm = false;
			Debug.Log((object)"Defender survived");
		}
		if (attacker.Skills[1] == "Grisly Wound" && attacker.promoted)
		{
			defender.Stats[0] -= 2;
			if (defender.Stats[0] < 1)
			{
				defender.Stats[0] = 1;
			}
		}
	}

	public void Entrap(BasePiece attacker, BasePiece defender)
	{
		wheel.animationsCalled = 1;
		D10Skills(attacker, defender);
		D4Skills(attacker, defender);
		wheel.Colors(attacker.spriteColor);
		int avo = AvoidCalculator(attacker, defender, defender.Stats[3]);
		SetWheelTextValues(attacker, avo);
		wheel.transform.SetAsLastSibling();
		int hit = HitCalculator();
		if (hit > avo)
		{
			attacker.atkConfirm = true;
		}
		else
		{
			attacker.atkConfirm = false;
		}
		wheel.SpinBegin(attacker);
	}

	private void SetWheelTextValues(BasePiece attacker, int avoid)
	{
		int[] array = new int[6]
		{
			1,
			2,
			3,
			4,
			5,
			6
		}; //Initial 1-6 values for attack wheel
		for (int i = 0; i < 11; i++)
		{
            //Will change 1-12 values on hit wheel to miss or hit if that number would result in a hit/miss
			if (i + 1 <= avoid)
			{
				wheel.hitText[i] = "MISS";
			}
			else
			{
				wheel.hitText[i] = "HIT";
			}
		}
		if (attacker.Type[1] != "Beaststone")
		{
			for (int j = 0; j < 6; j++)
			{
				if (array[j] < attacker.Stats[1])
				{
					array[j] = attacker.Stats[1];
				}
				if (array[j] > attacker.Stats[2])
				{
					array[j] = attacker.Stats[2];
				}
				wheel.atkText[j] = array[j].ToString();
			}
		}
		else
		{
			wheel.atkText = new string[6]
			{
				"2",
				"3",
				"4",
				"5",
				"6",
				"7"
			};
		}
		wheel.SetTexts();
	}

    public void PreBattle(BasePiece attacker, BasePiece defender) //Responsible for PreBattle Forecast
    {
        PreBattleCalc(attacker, defender, AttackerPreBattle);
        PreBattleCalc(defender, attacker, DefenderPreBattle);
        if (attacker.CheckCounterAttack(attacker, defender)==false && defender.Skills[0] != "Vantage")
        {
            DefenderPreBattle[2].text = "Damage: ";
            DefenderPreBattle[3].gameObject.SetActive(false);
            DefenderPreBattle[4].text = "Hit Chance: ";
            DefenderPreBattle[5].gameObject.SetActive(false);
        }
    }

    public void PreBattleCalc(BasePiece attacker, BasePiece defender, List<TextMeshProUGUI> ForecastTexts)
    {
        greatShield = miracle = lethality = false;
        AstraDmg = 0;
        ForecastTexts[0].text = attacker.Type[2];
        ForecastTexts[1].text = ("HP: "  + attacker.Stats[0] + "/" + attacker.Stats[4]);
        ForecastTexts[2].text = ("Damage: " + attacker.Stats[1] + "-" + attacker.Stats[2]);
        int bonusDmg = DamageCalculator(attacker, defender, false);
        if (bonusDmg != 0)
        {
            ForecastTexts[3].gameObject.SetActive(true);            
            //Enables/Disables Buff and Nerf Arrows
            if (bonusDmg < 0)
            {
                ForecastTexts[3].text = bonusDmg.ToString();
                ForecastTexts[3].transform.GetChild(0).gameObject.SetActive(false);
                ForecastTexts[3].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                ForecastTexts[3].text = "+" + bonusDmg;
                ForecastTexts[3].transform.GetChild(0).gameObject.SetActive(true);
                ForecastTexts[3].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else ForecastTexts[3].gameObject.SetActive(false);
        ForecastTexts[4].text = ("Hit Chance: " + (12 - defender.Stats[3])+ "/12");
        int bonusAvo = AvoidCalculator(attacker, defender, 0) * -1;
        if (bonusAvo != 0)
        {
            ForecastTexts[5].gameObject.SetActive(true);
            //Enables/Disables Buff and Nerf Arrows
            if (bonusAvo < 0)
            {
                ForecastTexts[5].text = bonusAvo.ToString();
                ForecastTexts[5].transform.GetChild(0).gameObject.SetActive(false);
                ForecastTexts[5].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                ForecastTexts[5].text = "+" + bonusAvo;
                ForecastTexts[5].transform.GetChild(0).gameObject.SetActive(true);
                ForecastTexts[5].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else ForecastTexts[5].gameObject.SetActive(false);
        //If the base skill of the unit is a combat skill, it is shown on the forecast window
        //For Archer, Wyvern Riders, and Mercenaries script checks if those skills are active
        ForecastTexts[6].text = "";
        if (NihilNegationDef==false)
        {
            if((attacker.Type[1] == "Mercenary" && PreBattleMercenarySkills) ||
            (attacker.Type[1] == "Archer" && PreBattleArcherSkills) ||
            (attacker.Type[1] == "Wyvern Rider" && PreBattleTrample) || (attacker.CombatSkills[0] &&
            (attacker.Type[1] != "Mercenary" && attacker.Type[1] != "Archer" && attacker.Type[1] != "Wyvern Rider")))
            {
                ForecastTexts[6].text = attacker.Skills[0];
            }
        }
        //Same as above, but for promoted skills and Villagers instead of Wyvern Riders
        ForecastTexts[7].text = "";
        if (attacker.promoted && NihilNegationDef == false)
        {
            if ((attacker.Type[1] == "Mercenary" && PreBattleMercenarySkills) ||
            (attacker.Type[1] == "Archer" && PreBattleArcherSkills) ||
            (attacker.Type[0] == "Pawn" && PreBattleUnderdog) || (attacker.CombatSkills[1] &&
            (attacker.Type[1] != "Mercenary" && attacker.Type[1] != "Archer" && attacker.Type[0] != "Pawn")))
            {
                ForecastTexts[7].text = attacker.Skills[1];
            }
        }
    }

}
