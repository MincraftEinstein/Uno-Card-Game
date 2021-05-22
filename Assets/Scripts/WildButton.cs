using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildButton : MonoBehaviour
{
    private GameObject WildMenu;
    private GameObject turnManagerGO;
    TurnManager turnManager;

    private void Start()
    {
        WildMenu = gameObject.transform.parent.gameObject;
        turnManagerGO = GameObject.Find("GameManager");
        turnManager = turnManagerGO.GetComponent<TurnManager>();
    }

    public void OnClick()
    {
        string ColorSelected = gameObject.name;
        ColorSelected = ColorSelected.Replace("Button", "");
        DrawCards.CurrentPlayedCard.GetComponent<CardProperties>().cardColor = ColorSelected;
        Debug.Log("Player called " + ColorSelected);
        StartCoroutine(turnManager.IncrementTurns(true));
        //turnManagerGO.GetComponent<HighlightPlayer>().SetSelection();
        WildMenu.transform.position = new Vector3(-10000, 0, 0);
        TurnManager.isWildMenuShown = false;
        StartCoroutine(removeMenu());
        Debug.Log("Wild Menu hidden");
    }

    IEnumerator removeMenu()
    {
        yield return new WaitForSeconds(2.5F);
        Destroy(WildMenu);
        //turnManager.WildMenu.SetActive(false);
    }
}
