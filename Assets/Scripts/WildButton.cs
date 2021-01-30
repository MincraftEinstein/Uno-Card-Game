using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildButton : MonoBehaviour
{
    public GameObject WildMenu;
    private GameObject turnManager;

    private void Start()
    {
        turnManager = GameObject.Find("TurnManager");
    }

    public void OnClick()
    {
        string ColorSelected = gameObject.name;
        ColorSelected = ColorSelected.Replace("Button", "");
        DrawCards.CurrentPlayedCard.GetComponent<CardProperties>().cardColor = ColorSelected;
        OutputLog.WriteToOutput("Player called " + ColorSelected);
        StartCoroutine(turnManager.GetComponent<TurnManager>().IncrementTurns());
        Destroy(WildMenu);
        TurnManager.isWildMenuShown = false;
        OutputLog.WriteToOutput("Wild Menu hidden");
    }
}
