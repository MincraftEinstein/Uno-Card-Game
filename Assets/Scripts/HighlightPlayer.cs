using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightPlayer : MonoBehaviour
{
    public Image playerImage;
    public GameObject playerArea;

    public Image enemy1Image;
    public GameObject enemyArea1;

    public Image enemy2Image;
    public GameObject enemyArea2;

    public Image enemy3Image;
    public GameObject enemyArea3;

    public GameObject playerArea2;

    // Update is called once per frame
    void Update()
    {
        SetSelection();
        ArrangeCards();
    }

    public void SetSelection()
    {
        //Debug.Log("Called SetSelection");
        if (TurnManager.Turns == 0)
        {
            playerImage.enabled = true;
            playerImage.rectTransform.sizeDelta = new Vector2(GetPlayerHandSize(playerArea), playerImage.rectTransform.sizeDelta.y);
            enemy1Image.enabled = false;
            enemy2Image.enabled = false;
            enemy3Image.enabled = false;
        }
        else if (TurnManager.Turns == 1)
        {
            playerImage.enabled = false;
            enemy1Image.enabled = true;
            enemy1Image.rectTransform.sizeDelta = new Vector2(enemy1Image.rectTransform.sizeDelta.x, GetPlayerHandSize(enemyArea1));
            enemy2Image.enabled = false;
            enemy3Image.enabled = false;
        }
        else if (TurnManager.Turns == 2)
        {
            playerImage.enabled = false;
            enemy1Image.enabled = false;
            enemy2Image.enabled = true;
            enemy2Image.rectTransform.sizeDelta = new Vector2(GetPlayerHandSize(enemyArea2), enemy2Image.rectTransform.sizeDelta.y);
            enemy3Image.enabled = false;
        }
        else if (TurnManager.Turns == 3)
        {
            playerImage.enabled = false;
            enemy1Image.enabled = false;
            enemy2Image.enabled = false;
            enemy3Image.enabled = true;
            enemy3Image.rectTransform.sizeDelta = new Vector2(enemy3Image.rectTransform.sizeDelta.x, GetPlayerHandSize(enemyArea3));
        }
    }

    float GetPlayerHandSize(GameObject Area)
    {
        Transform[] childern;
        GridLayoutGroup grid = Area.GetComponent<GridLayoutGroup>();
        childern = grid.GetComponentsInChildren<Transform>();
        float spacing;
        if (Area == playerArea || Area == enemyArea2)
        {
            spacing = grid.cellSize.x + grid.spacing.x;
        }
        else
        {
            spacing = grid.cellSize.y + grid.spacing.y;
        }
        float size = childern.Length * spacing;
        if (Area == playerArea)
        {
            return size - 26;
        }
        else
        {
            return size + 34;
        }
    }

    void ArrangeCards()
    {
        List<CardProperties> children = new List<CardProperties>();
        List<CardProperties> children2 = new List<CardProperties>();

        CardProperties[] cards = playerArea.GetComponentsInChildren<CardProperties>();
        for (int i = 0; i < cards.Length; i++)
        {
            children.Add(cards[i]);
        }

        CardProperties[] cards2 = playerArea2.GetComponentsInChildren<CardProperties>();
        for (int i = 0; i < cards2.Length; i++)
        {
            children2.Add(cards2[i]);
        }

        if ((children.Count - 1) > 6)
        {
            Debug.Log("Player had more than 7 cards");
            Debug.Log("Adding a new row");
            playerArea2.SetActive(true);

            for (int i = 0; i < children.Count; i++)
            {
                if (children.IndexOf(children[i]) > 6)
                {
                    children[i].transform.SetParent(playerArea2.transform, false);
                    children.Remove(children[i]);
                }
            }
        }
        else if ((children.Count - 1) < 6)
        {
            //Debug.Log("Player had less than 7 cards");
            //Debug.Log("Removing a row");

            if (children2.Count > 0)
            {
                children2[0].transform.SetParent(playerArea.transform, false);
                children2.Remove(children2[0]); 
            }
            //if (!children.Contains(children2[0]))
            //{
            //    children.Add(children2[0]);
            //}
            if (children2.Count <= 0)
            {
            //    for (int i = 0; i < children2.Count - 1; i++)
            //    {
            //        children[i].transform.SetParent(playerArea.transform, false);
            //    }
                playerArea2.SetActive(false);
            }
        }
    }
}
