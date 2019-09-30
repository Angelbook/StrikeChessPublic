using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameScript : MonoBehaviour {
    public PieceManager manager;
    public List<TextMeshProUGUI> P1Names;
    public List<TextMeshProUGUI> P2Names;

    public void AddNames()
    {
        for (int i = 0; i < P1Names.Count; i++)
        {
            manager.NamesP1[i] = P1Names[i].text;
            manager.NamesP2[i] = P2Names[i].text;
        }
    }
}
