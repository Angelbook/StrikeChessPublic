using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//0 + num*30, 30 + num*60 atk
public class Wheel : MonoBehaviour {

    public List<TextMeshProUGUI> HitMissNumbers = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> AtkNumbers = new List<TextMeshProUGUI>();
    public GameObject hitWheel;
    public GameObject spinHit;
    public GameObject atkWheel;
    public GameObject spinAtk;
    public GameObject arrow;
    public GameObject blocker;
    public TextMeshProUGUI BattleText;
    public GameObject DamageFX;
    public Animator animHit;
    public Animator animAtk;
    public Animator AnimFX;
    public Animator AnimText;
    public int animationsCalled;
    public int animationNum = 12;
    public int atk;
    public string hitNum;
    public string atkNum;
    public string[] hitText = new string[11];
    public string[] atkText = new string [6];
    public bool hitDone = false;
    public bool atkDone = false;
    public bool noDamage = false;
    public bool lethality;
    public bool miracle;
    public bool GreatShield;
    public bool CriticalHit;
    public int Astra;
    private BasePiece piece;
    public Color32 attackerColor;
    public Color32 defenderColor;

public void SpinBegin(BasePiece attacker)
    {
        hitNum = animationsCalled.ToString();
        atkNum = atk.ToString() + "atk";
        piece = attacker;
        StartCoroutine(FirstSpin());
    }

    public void SetTexts()
    {
        for(int i=0; i<HitMissNumbers.Count; i++)
        {
            HitMissNumbers[i].text = hitText[i];
        }
        for(int i=0; i<AtkNumbers.Count; i++)
        {
            AtkNumbers[i].text = atkText[i];
        }
    }

    IEnumerator FirstSpin()
    {
        blocker.SetActive(true);
        animHit.Play("DropHit");
        yield return new WaitForSeconds(1.0f);
        arrow.SetActive(true);
        animHit.Play("HitStop");
        yield return new WaitForSeconds(.95f);
        animHit.Play(animationNum.ToString());
        yield return new WaitForSeconds(1f);
        if (animationNum == 12)
        {
            animHit.PlayInFixedTime("CritWheel", 0 ,0f);
        }
        piece.hitDone = true;
        if (animationsCalled > 1)
        {
            StartCoroutine(SecondSpin());
        }
        else
        {
            ResetWheels();
        }
    }

    IEnumerator SecondSpin()
    {
        animAtk.Play("DropAtk");
        yield return new WaitForSeconds(1f);
        animAtk.Play("AtkStop");
        yield return new WaitForSeconds(1f);
        animAtk.Play(atkNum);
        yield return new WaitForSeconds(1f);
        //ifs below are for determining text for activated skills and crit animation
        if (lethality)
        {
            BattleText.text = "Lethality";
            BattleText.colorGradientPreset = new TMP_ColorGradient
            (new Color(0.6320754f, 0, 0, 1), new Color(0.490566f, 0, 0, 1), new Color(0.2641509f, 0, 0, 1), new Color(0, 0, 0, 1));
            AnimText.Play("TextFade");
            CriticalHit = true;
            yield return new WaitForSeconds(1f);
        }
        if (Astra>0)
        {
            BattleText.colorGradientPreset = new TMP_ColorGradient(Color.white, Color.white, Color.white, Color.white);
            BattleText.color = attackerColor;
            BattleText.text = "Astra+" + Astra;
            AnimText.Play("TextFade");
            yield return new WaitForSeconds(1f);
        }
        float wait = 0f;
        if (CriticalHit)
        {
            AnimFX.Play("DamageCrit");
            wait = .5f;

        }
        else AnimFX.Play("Damage");
        if (miracle)
        {
            yield return new WaitForSeconds(wait);
            BattleText.colorGradientPreset = new TMP_ColorGradient(Color.white, Color.white, Color.white, Color.white);
            BattleText.color = defenderColor;
            BattleText.text = "Miracle";
            AnimText.PlayInFixedTime("TextFade", 0, 0f);
        }
        if (GreatShield)
        {
            yield return new WaitForSeconds(wait);
            BattleText.colorGradientPreset = new TMP_ColorGradient(Color.white, Color.white, Color.white, Color.white);
            BattleText.color = defenderColor;
            BattleText.text = "Great Shield";
            AnimText.PlayInFixedTime("TextFade",0,0f);
        }
        yield return new WaitForSeconds(1.5f);
        piece.atkDone = true;
        ResetWheels();
    }

    public void Colors(Color32 newcolor)
    {
        hitWheel.GetComponent<Image>().color = newcolor;
        atkWheel.GetComponent<Image>().color = newcolor;
        arrow.GetComponent<Image>().color = newcolor;
        spinHit.GetComponent<Image>().color = newcolor;
        spinAtk.GetComponent<Image>().color = newcolor;
    }

    public void ResetWheels()
    {
        arrow.SetActive(false);
        animHit.Rebind();
        animAtk.Rebind();
        AnimText.Rebind();
        AnimFX.Rebind();
        blocker.SetActive(false);
        atkDone = hitDone = false;
    }
}
