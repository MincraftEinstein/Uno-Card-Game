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
    //private GameObject playerArea2;
    private Vector2 startPosition;

    void Start()
    {
        //playerArea2 = GameObject.Find("PlayerArea2");
        turnManagerGO = GameObject.Find("GameManager");
        dragObject = GameObject.Find("DragObject");
        Canvas = GameObject.Find("Canvas");
        isDraggable = false;
        //playerArea2.SetActive(false);
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
            //turnManagerGO.GetComponent<HighlightPlayer>().SetSelection();
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if (isDraggable == true) return;
        isDragging = false;
        if (isOverDropZone && dropZone == GameObject.Find("PlayerArea"))
        {
            transform.SetParent(dropZone.transform, false);
            if (startParent == GameObject.Find("DeckArea"))
            {
                TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
                DrawCards.RemainingCards.Remove(gameObject);
                turnManager.PlayerManager();
                gameObject.GetComponent<CardFlipper>().Flip();
                DrawCards.PlayerCards.Add(gameObject);
                turnManager.testDrawnCard(gameObject);
                for (int i = 0; i < DrawCards.RemainingCards.Count; i++)
                {
                    if (DrawCards.RemainingCards[i] != null)
                    {
                        DrawCards.RemainingCards[i].GetComponent<CardProperties>().HasAuthority = false;
                    }
                }
            }

            //Transform[] children;
            //children = ;
            //if (dropZone.GetComponentsInChildren<Transform>().Length >= 7)
            //{
            //    playerArea2.SetActive(true);
            //    transform.SetParent(GameObject.Find("PlayerArea2").transform, false);
            //}
            //else
            //{
            
            //}
        }

        // If the card is over the DropZone, then put it there; if it's not over a DropZone, then return it to 'startPosition'.
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
        TurnManager turnManager = turnManagerGO.GetComponent<TurnManager>();
        bool RoundOver = false;
        if (TopCardProperties.HasAuthority)
        {
            DrawCards.CurrentPlayedCard = gameObject;
            DrawCards.PlayerCards.Remove(gameObject);
            DrawCards.PlayedCards.Add(gameObject);
            Debug.Log("Player: Played " + gameObject.name);
            TopCardProperties.HasAuthority = false;
            if (DrawCards.PlayerCards.Count <= 0)
            {
                turnManager.EndGame(DrawCards.PlayerCards);
                RoundOver = true;
            }
            else if (TopCardProperties.cardType == "Skip" || TopCardProperties.cardType == "Draw2")
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
                //turnManager.WildMenu.SetActive(true);

                Debug.Log("Wild Menu shown");
                if (TopCardProperties.cardType == "WildDraw4")
                {
                    turnManager.TurnResult = "WildDraw4";
                }
                else
                {
                    turnManager.TurnResult = "normal";
                }
            }
            //if (TopCardProperties.cardType != "Wild" || TopCardProperties.cardType != "WildDraw4")
            if (RoundOver == false)
            {
                if (TurnManager.isWildMenuShown == false)
                {
                    StartCoroutine(turnManager.IncrementTurns());
                    //turnManagerGO.GetComponent<HighlightPlayer>().SetSelection();
                }
            }
        }
    }
}
