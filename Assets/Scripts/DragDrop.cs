using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    //public int DiscardCounter; // = 0; // This is used to the change the orientation of the cards.
    public GameObject DiscardPile;
    public GameObject WildMenu;
    public static bool isWildMenuShown;
    private GameObject Canvas;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    //private GridLayoutGroup glg;
    private GameObject dropZone;
    private Vector2 startPosition;

    private void Start()
    {
         Canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
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
        startPosition = transform.position;
        if (GetComponent<CardProperties>().HasAuthority == true && TurnManager.Turns == 0)
        {
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        isDragging = false;
        //If the card is over the DropZone, then put it there; if it's not over a DropZone, then return it to 'startPosition'.
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            GetComponent<CardProperties>().HasAuthority = false;
            //DrawCards.PlayedCards.Add(gameObject);
            DrawCards.CurrentPlayedCard = gameObject;
            /*if (TurnManager.IsReversed == false)
            {
                TurnManager.IncrementTurns(DrawCards.PlayerCards);
            }
            else
            {
                TurnManager.DecreaseTurns(DrawCards.PlayerCards);
            }*/

            OutputLog.WriteToOutput("Player: Played " + gameObject);
            if (gameObject.GetComponent<CardProperties>().cardType == "Wild")
            {
                isWildMenuShown = true;
                Instantiate(WildMenu, Canvas.transform);
                OutputLog.WriteToOutput("Wild Menu shown");
            }
            if (gameObject.GetComponent<CardProperties>().cardType == "WildDraw4")
            {
                isWildMenuShown = true;
                Instantiate(WildMenu, Canvas.transform);
                OutputLog.WriteToOutput("Wild Menu shown");
            }
            TurnManager.IncrementTurns(DrawCards.PlayerCards);
            OutputLog.WriteToOutput("Turns: " + TurnManager.Turns);

            //DiscardCounter++;
            //OutputLog.WriteToOutput("DiscardCounter: " + DiscardCounter);

            //if (DiscardCounter == 5)
            //{
            //    glg = DiscardPile.GetComponent<GridLayoutGroup>();
            //    //SetProperty(glg.spacing.x = 250);
            //    //glg.constraint = GridLayoutGroup.Constraint.s.spacing.x;  //**
            //    //glg.constraintCount = 1;
            //    //glg1.spacing.x = glg.spacing.x * -1;
            //    glg.spacing = new Vector2(50, 0);
            //    DiscardCounter = 0;
            //}
        }
        else
        {
            transform.position = startPosition;
        }
    }
}

