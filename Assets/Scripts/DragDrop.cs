using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    public bool isDraggable;
    public bool canPlayCard;
    private bool isDragging;
    private bool isOverDropZone;
    private GameObject dropZone;
    private GameObject turnManagerGO;
    private GameObject dragObject;
    private GameObject Canvas;
    private GameObject startParent;
    private Vector2 startPosition;

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
        if (isOverDropZone && dropZone == GameObject.Find("PlayerArea"))
        {
            if (startParent == GameObject.Find("DeckArea"))
            {
                TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
                DrawCards.RemainingCards.Remove(gameObject);
                turnManager.PlayerManager();
            }
            transform.SetParent(dropZone.transform, false);
            DrawCards.PlayerCards.Add(gameObject);
        }
        //If the card is over the DropZone, then put it there; if it's not over a DropZone, then return it to 'startPosition'.
        else if (isOverDropZone && canPlayCard)
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            PlayerLogic(gameObject.GetComponent<CardProperties>());
        }
        else
        {
            transform.SetParent(startParent.transform, true);
            transform.position = startPosition;
        }
    }

    public void PlayerLogic(CardProperties TopCardProperties)
    {
        //Test if (GetComponent<CardProperties>().HasAuthority == true)
        if (TopCardProperties.HasAuthority)
        {
            TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
            if (TopCardProperties.cardType == "Skip" || TopCardProperties.cardType == "Draw2")
            {
                turnManager.TurnResult = TopCardProperties.cardType;
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
            OutputLog.WriteToOutput("Player: Played " + gameObject.name);
            //Test GetComponent<CardProperties>().HasAuthority = false;
            TopCardProperties.HasAuthority = false;
            //if (TopCardProperties.cardType != "Wild" || TopCardProperties.cardType != "WildDraw4")
            if (TurnManager.isWildMenuShown == false)
            {
                StartCoroutine(turnManager.IncrementTurns());
            }
        }
    }
}
