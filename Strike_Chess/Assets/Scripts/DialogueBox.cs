using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueBox : MonoBehaviour {

    public GameObject Box; //For normal messages/skills
    public GameObject BattleStuffs; //For Battles
    public GameObject AttackerCard;
    public GameObject DefenderCard;
    public PieceManager manager;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI PreBattleText;

    public void PreBattleForecast(Color32 attackerColor, Color32 defenderColor)
    {
        PreBattleText.text = "Strike the enemy?";
        BattleStuffs.SetActive(true);
        BattleStuffs.transform.SetAsLastSibling();

        //sets colors 
        AttackerCard.GetComponent<Image>().color = BattleStuffs.GetComponent<Image>().color = attackerColor;
        DefenderCard.GetComponent<Image>().color = defenderColor;
    }

    public void BattleDialogues(string skill, Color32 boxColor, string upgradeSkill = "")
    {
        Box.SetActive(true);
        Box.transform.SetAsLastSibling();
        Box.GetComponent<Image>().color = boxColor;
        if (skill == "Lunge")
        {
            Text.text = "Strike with a lunge?";
        }
        else if (skill == "Shove")
        {
            Text.text = "Shove an adjacent unit?";
        }
        else if (skill == "Shelter")
        {
            Text.text = "Shelter an adjacent ally?";
        }
        else if (skill == "Heal")
        {
            Text.text = skill + " an ally?";
        }
        else if (skill=="Bliss" || skill == "Dance")
        {
            Text.text = skill + " an adjacent ally?";
        }
        else if (skill == "Warp")
        {
            Text.text = skill + " a distant unit?";
        }
        else if (skill == "Entrap")
        {
            Text.text = "Entrap this enemy?";
        }
        else if (skill == "Place")
        {
            Text.text = "Select a tile to place the unit";
        }
        else if (skill == "Upgrade")
        {
            Text.text = "Upgrade this piece to learn the skill '" + upgradeSkill + "'";
        }
    }
}
