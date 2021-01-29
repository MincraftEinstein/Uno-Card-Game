using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    private GameObject dropZone;
    private GameObject turnManagerGO;
    private GameObject dragObject;
    private GameObject Canvas;
    private Vector2 startPosition;
    private bool isDragging;
    public bool isDraggable;
    private bool isOverDropZone;
    private GameObject startParent;

    private void Start()
    {
        turnManagerGO = GameObject.Find("TurnManager");
        dragObject = GameObject.Find("DragObject");
        Canvas = GameObject.Find("Canvas");
        isDraggable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging == true)
        {
            transform.SetParent(dragObject.transform, true);
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    public void StartDrag()
    {
        if (isDraggable == true) return;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        if (GetComponent<CardProperties>().HasAuthority == true && TurnManager.Turns == 0)
        {
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if (isDraggable == true) return;
        isDragging = false;
        //If the card is over the DropZone, then put it there; if it's not over a DropZone, then return it to 'startPosition'.
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            //turnManagerGO.GetComponent<TurnManager>().PlayerManager();
            PlayerLogic(gameObject.GetComponent<CardProperties>());
            //OutputLog.WriteToOutput("Turns: " + TurnManager.Turns);
        }
        else
        {
            transform.SetParent(startParent.transform, true);
            transform.position = startPosition;
        }
    }

    public void PlayerLogic(CardProperties TopCardProperties)
    {
        if (GetComponent<CardProperties>().HasAuthority == true)
        {
            TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
            if (TopCardProperties.cardType == "Skip")
            {
                turnManager.TurnResult = "Skip";
            }
            else if (TopCardProperties.cardType == "Draw2")
            {
                turnManager.TurnResult = "Draw2";
            }
            else if (TopCardProperties.cardType == "Reverse")
            {
                turnManager.IsReversed = !turnManager.IsReversed;
                turnManager.TurnResult = "Reverse";
            }
            else if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
            {
                TurnManager.isWildMenuShown = true;
                Instantiate(turnManager.WildMenu, Canvas.transform);
                OutputLog.WriteToOutput("Wild Menu shown");
                turnManager.TurnResult = "normal";
            }
            DrawCards.CurrentPlayedCard = gameObject;
            DrawCards.PlayerCards.Remove(gameObject);
            DrawCards.PlayedCards.Add(gameObject);
            string CardName = gameObject.name;
            CardName = CardName.Replace("(Clone)", "");
            OutputLog.WriteToOutput("Player: Played " + CardName);
            GetComponent<CardProperties>().HasAuthority = false;
            if (TopCardProperties.cardType != "Wild" || TopCardProperties.cardType != "WildDraw4")
            {
                StartCoroutine(turnManager.IncrementTurns());
            }
        }
    }
}
