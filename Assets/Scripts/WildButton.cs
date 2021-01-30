using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildButton : MonoBehaviour
{
    public GameObject WildMenu;
    private GameObject turnManagerGO;

    private void Start()
    {
        turnManagerGO = GameObject.Find("TurnManager");
    }

    public void OnClick()
    {
        TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
        string ColorSelected = gameObject.name;
        ColorSelected = ColorSelected.Replace("Button", "");
        DrawCards.CurrentPlayedCard.GetComponent<CardProperties>().cardColor = ColorSelected;
        OutputLog.WriteToOutput("Player called " + ColorSelected);
        StartCoroutine(turnManager.IncrementTurns());
        Destroy(WildMenu);
        TurnManager.isWildMenuShown = false;
        OutputLog.WriteToOutput("Wild Menu hidden");
    }
}
