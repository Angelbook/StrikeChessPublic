using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextHoverExplanations : MonoBehaviour, IPointerClickHandler
{
    public UIScript GameUI;
    public static GameObject HoverPanel;
    public static TextMeshProUGUI PanelText;
    private RectTransform rectTrans;
    private float height;

    private void Start()
    {
        HoverPanel = GameUI.HoverP;
        PanelText = GameUI.PText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HoverPanel.SetActive(true);
        TextChecker(this.GetComponent<TextMeshProUGUI>().text);
        rectTrans = HoverPanel.GetComponent<RectTransform>();
        HoverPanel.transform.SetAsLastSibling();
        HoverPanel.transform.position = (Vector3)eventData.position;
        height = rectTrans.rect.height;
        PositionSet(height);
    }

    private void TextChecker(string text)
    {
        if (text.Contains("HP:"))
        {
            PanelText.text = "The amount of damage a unit can take before being defeated.";
        }
        else if (text.Contains("Damage:") || text.Contains("Attack:") || text.Contains("ATK:"))
        {
            PanelText.text = "The amount of damage this unit can deal to an opponent." + "\n" +
            "(Randomly calculated between 1 to 6).";
        }
        else if (text.Contains("Avoid:") || text.Contains("AVO:"))
        {
            PanelText.text = "The chance of this unit evading an attack." + "\n" +
            "(Chance out of 12).";
        }
        else if (text.Contains("Hit Chance:"))
        {
            PanelText.text = "The chance of this unit landing an attack." + "\n" +
            "(Chance out of 12).";
        }
        else if (text.Contains("PieceType:"))
        {
            PanelText.text = "The original Chess piece this unit inherits its movement from.";
        }
        else if (text.Contains("Class:"))
        {
            PanelText.text = "The Strike Chess class this unit inherits skills and stats from.";
        }
        else if (text.Contains("Experience Points:"))
        {
            PanelText.text = "Currency this player can use to upgrade pieces or use arts.";
        }
        else if (text == "Skills:")
        {
            PanelText.text = "Abilities of this unit that effect combat, movement, allies, or enemies.";
        }
        else if (text == "Paragon")
        {
            PanelText.text = "Doubles the exp this unit earns.";
        }
        else if (text == "Promotion")
        {
            PanelText.text = "If this unit reaches the opposite end of the board, " +
            "they may change into an unused class." +
            "\n" + "(Upgrades must be bought again).";
        }
        else if (text == "Underdog")
        {
            PanelText.text = "EX Skill. Combat Skill. If facing an enemy that is not another villager, " +
            "increase this unit's Avoid stat by 2, decrease the enemy's by 2.";
        }
        else if (text == "Defense+2")
        {
            PanelText.text = "Combat Skill. When this unit is attacked, reduce damage received by 2.";
        }
        else if (text == "Great Shield")
        {
            PanelText.text = "EX Skill. Combat Skill. When this unit is attacked, they receive no damage." +
            "\n" + "(1 out of 4 chance to activate).";
        }
        else if (text == "Shove")
        {
            PanelText.text = "After moving, this unit can an adjacent ally or enemy 1 tile away.";
        }
        else if (text == "Sol")
        {
            PanelText.text = "EX Skill. Combat Skill. This unit recovers HP equivalent to its damage dealt.";
        }
        else if (text == "Prescience")
        {
            PanelText.text = "Combat Skill. When this unit attacks on their turn, increase their avoid by 3 " +
            "and decrease their enemy's by 3.";
        }
        else if (text == "Quickdraw")
        {
            PanelText.text = "EX Skill. Combat skill. When this unit attacks on their turn, increase their attack by 3.";
        }
        else if (text == "Transform")
        {
            PanelText.text = "Upgrade this unit to their 2nd set of attack and avoid stats " +
            "when they earn their EX Skill.";
        }
        else if (text == "Roar")
        {
            PanelText.text = "EX Skill. Combat Skill. After attacking, the enemy cannot move on their next turn.";
        }
        else if (text == "Odd Rhythm")
        {
            PanelText.text = "Switch this unit's attack and avoid stats based on the color of their current tile." + "\n" +
            "(2-7 Atk and 0 Avo on black, 0 Atk and 7 Avo on White).";
        }
        else if (text == "Grisly Wound")
        {
            PanelText.text = "EX Skill. After battle, this unit deals an extra 2 damage to their" +
            " opponent regardless of missing their initial attack or even being defeated." + "\n" +
            "(This cannot be used to defeat an enemy and instead leaves them at 1hp).";
        }
        else if (text == "Vantage")
        {
            PanelText.text = "Combat Skill. This unit is always able to counterattack regardless of attack range.";
        }
        else if (text == "Astra")
        {
            PanelText.text = "EX Skill. Combat skill. Extra damage will be added to your attack power." + "\n" +
            "(Extra damage randomly ranges from 1 to 4)";
        }
        else if (text == "Focus")
        {
            PanelText.text = "Combat Skill. If this unit has no allies in a " +
            "2 tile radius when attacking, decrease the enemy’s avoid by 3.";
        }
        else if (text == "Nihil")
        {
            PanelText.text = "EX Skill. When attacking, nullify the enemy’s combat skills.";
        }
        else if (text == "Pass")
        {
            PanelText.text = "This unit may move over ally and enemy units." + "\n" +
            "(Attack range is still impeded by enemies but not allies.)";
        }
        else if (text == "Lethality")
        {
            PanelText.text = "EX Skill. Combat Skill. After a landed hit, the enemy is instantly defeated." + "\n" +
            "(1 out of 10 chance to trigger, does not work against the Miracle skill).";
        }
        else if (text == "Void Curse")
        {
            PanelText.text = "This unit gives no exp upon defeat.";
        }
        else if (text == "Lifetaker")
        {
            PanelText.text = "EX Skill. This unit restores their HP to full after defeating an enemy.";
        }
        else if (text == "Patience")
        {
            PanelText.text = "Combat Skill. When this unit attacks on their opponent's turn," +
            "increase their avoid by 3 and decrease their enemy's by 3.";
        }
        else if (text == "Strong Riposte")
        {
            PanelText.text = "EX Skill. Combat Skill. When this unit attacks on their opponent's turn, " +
            "increase their attack by 3.";
        }
        else if (text == "Canto")
        {
            PanelText.text = "This unit may stop anywhere in it’s potential movement range.";
        }
        else if (text == "Shelter")
        {
            PanelText.text = "EX Skill. After moving, this unit may move an adjacent ally " +
            "to any other free adjacent position around this unit.";
        }
        else if (text == "Relief")
        {
            PanelText.text = "At the beginning of your turn, this unit will recover 2 HP" +
            "if there are no allies within a 2 tile radius.";
        }
        else if (text == "Galeforce")
        {
            PanelText.text = "EX Skill. Once per turn, this unit returns to their" +
            "original tile after defeating an enemy and may move again.";
        }
        else if (text == "Trample")
        {
            PanelText.text = "When attacking a non-Knight unit, add 2 to damage dealt.";
        }
        else if (text == "Lunge")
        {
            PanelText.text = "EX Skill. When initiating an attack, this unit may" +
            "switch positions with the enemy it attacks.";
        }
        else if (text == "Vortex")
        {
            PanelText.text = "EX Skill. This unit’s attack range extends through " +
            "the entire L-Shape of their movement range.";
        }
        else if (text == "Even Rhythm")
        {
            PanelText.text = "Add this unit's attack or avoid stats based on the color of their current tile." + "\n" +
            "(1-5 on black, 5 Avo on White).";
        }
        else if (text == "Dragonskin")
        {
            PanelText.text = "EX Skill. Combat Skill. Halve all damage this unit receives in battle." + "\n" +
            "(Rounded up)";
        }
        else if (text == "Heal")
        {
            PanelText.text = "After moving, restore an adjacent unit to their Max HP or " +
            "heal any distant allies in your movement range by ½ the ally’s max HP." + "\n" +
            "(rounded up).";
        }
        else if (text == "Miracle")
        {
            PanelText.text = "Combat Skill. If this unit receives fatal damage and " +
            "would otherwise be defeated, this unit will survive with 1HP." + "\n" +
            "(1 out of 10 chance of activating).";
        }
        else if (text == "Renewal")
        {
            PanelText.text = "EX Skill. At the beginning of your turn, this unit" +
            " will fully restore their HP.";
        }
        else if (text == "Warp")
        {
            PanelText.text = "After moving, bring any ally in your movement range to a space adjacent to this unit." + "\n" +
            "(Other units do not obstruct your range to rescue distant allies.)";
        }
        else if (text == "Entrap")
        {
            PanelText.text = "EX Skill. After moving, bring any enemy unit to a space adjacent to this unit. " +
            "A spin must be initiated as if attacking as this skill can miss." + "\n" +
            "(Other units do not obstruct your range to rescue distant allies.)";
        }
        else if (text == "Dance")
        {
            PanelText.text = "After moving, this unit passes its turn to an adjacent ally and " +
            "increases the ally's damage dealt and avoid by 1 until your next turn.";
        }
        else if (text == "Special Dance")
        {
            PanelText.text = "EX Skill. Increase Dance skill effect to damage dealt +2, " +
            "avoid +2, and damage received -1.";
        }
        else if (text == "Bliss")
        {
            PanelText.text = "EX Skill. After moving  this unit passes its turn to an adjacent ally. " +
            "This fully restores their HP and they will always " +
            "deal their maximum damage until your next turn.";
        }
        else if (text == "Veteran")
        {
            PanelText.text = "Increase the exp gain of any allies in a 3 tile radius by 1." + "\n" +
            "(Applies after Paragon).";
        }
        else if (text == "Rally Spectrum")
        {
            PanelText.text = "EX Skill. Increase the avoid, attack, and reduce damage received " +
            "of any allies in a 3 tile radius by 1.";
        }
        else if (text == "Charm")
        {
            PanelText.text = "All allies in the 8 surrounding tiles from the lord receive + 1 avoid.";
        }
        else if (text == "Dancing Blade")
        {
            PanelText.text = "EX Skill. Sacrifices defense for speed. Increase " +
            "avoid by 3 while also increasing any damage received by 1.";
        }
        else if (text == "Heavy Blade")
        {
            PanelText.text = "EX Skill. Sacrifices evasion for strength. Increase " +
            "damage dealt by 3 while also decreasing avoid by 1.";
        }
        else HoverPanel.SetActive(false);
    }

    private void PositionSet(float height)
    {
        if ((HoverPanel.transform.position.x - 200) < 0)
        {
            HoverPanel.transform.Translate(new Vector3Int(75, 0, 0));
        }
        if ((rectTrans.rect.yMin) > Screen.height)
        {
            HoverPanel.transform.Translate(new Vector3Int(0, (int)(height), 0));
        }
        if ((rectTrans.transform.position.y + 200) > Screen.height)
        {
            HoverPanel.transform.Translate(new Vector3Int(0, -75, 0));
        }
        if((HoverPanel.transform.position.x + 200) > Screen.width)
        {
            HoverPanel.transform.Translate(new Vector3Int(-50, 0, 0));
        }
    }
}