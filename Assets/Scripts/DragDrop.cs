using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    public GameObject DiscardPile;
    private bool isDragging;
    private bool isOverDropZone;
    private GameObject dropZone;
    private Vector2 startPosition;
    private GameObject turnManager;

    private void Start()
    {
        DiscardPile = GameObject.Find("DiscardArea");
        turnManager = GameObject.Find("TurnManager");
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
            DrawCards.CurrentPlayedCard = gameObject;
            DrawCards.PlayedCards.Add(gameObject);
            string CardName = gameObject.name;
            CardName = CardName.Replace("(Clone)", "");
            OutputLog.WriteToOutput("Player: Played " + CardName);
            TurnManager.CleanupDiscardArea();
            turnManager.GetComponent<TurnManager>().PlayerManager();

            OutputLog.WriteToOutput("Turns: " + TurnManager.Turns);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
