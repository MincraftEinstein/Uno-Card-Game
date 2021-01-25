﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DrawCards : MonoBehaviour
{
    public GameObject Blue_0;
    public GameObject Blue_1;
    public GameObject Blue_2;
    public GameObject Blue_3;
    public GameObject Blue_4;
    public GameObject Blue_5;
    public GameObject Blue_6;
    public GameObject Blue_7;
    public GameObject Blue_8;
    public GameObject Blue_9;
    public GameObject Blue_D;
    public GameObject Blue_R;
    public GameObject Blue_S;
    //---------------------------------------
    public GameObject Green_0;
    public GameObject Green_1;
    public GameObject Green_2;
    public GameObject Green_3;
    public GameObject Green_4;
    public GameObject Green_5;
    public GameObject Green_6;
    public GameObject Green_7;
    public GameObject Green_8;
    public GameObject Green_9;
    public GameObject Green_D;
    public GameObject Green_R;
    public GameObject Green_S;
    //---------------------------------------
    public GameObject Red_0;
    public GameObject Red_1;
    public GameObject Red_2;
    public GameObject Red_3;
    public GameObject Red_4;
    public GameObject Red_5;
    public GameObject Red_6;
    public GameObject Red_7;
    public GameObject Red_8;
    public GameObject Red_9;
    public GameObject Red_D;
    public GameObject Red_R;
    public GameObject Red_S;
    //---------------------------------------
    public GameObject Yellow_0;
    public GameObject Yellow_1;
    public GameObject Yellow_2;
    public GameObject Yellow_3;
    public GameObject Yellow_4;
    public GameObject Yellow_5;
    public GameObject Yellow_6;
    public GameObject Yellow_7;
    public GameObject Yellow_8;
    public GameObject Yellow_9;
    public GameObject Yellow_D;
    public GameObject Yellow_R;
    public GameObject Yellow_S;
    //---------------------------------------
    public GameObject Wild;
    public GameObject WildDraw4;
    public static GameObject CurrentPlayedCard;
    public GameObject PlayerArea;
    public GameObject EnemyArea1;
    public GameObject EnemyArea2;
    public GameObject EnemyArea3;
    public GameObject DeckZone;
    public GameObject DiscardPile;
    public GameObject Button;
    public GameObject Card;

    public static List<GameObject> AllCards = new List<GameObject>();
    public static List<GameObject> RemainingCards = new List<GameObject>();
    public static List<GameObject> DiscardCards = new List<GameObject>();

    public static List<GameObject> EnemyCards1 = new List<GameObject>();
    public static List<GameObject> EnemyCards2 = new List<GameObject>();
    public static List<GameObject> EnemyCards3 = new List<GameObject>();
    public static List<GameObject> PlayerCards = new List<GameObject>();

    public float rate = 0.5F;
    public float secRate = 0.5F;
    public int iS;
    private bool WasClicked;

    // Start is called before the first frame update
    void Start()
    {
        CreateNewDeck();
        Shuffle();
    }

    public void CreateNewDeck()
    {
        AllCards.Add(Blue_0);
        AllCards.Add(Green_0);
        AllCards.Add(Red_0);
        AllCards.Add(Yellow_0);

        for (int i = 0; i < 2; i++)
        {
            AllCards.Add(Blue_1);
            AllCards.Add(Blue_2);
            AllCards.Add(Blue_3);
            AllCards.Add(Blue_4);
            AllCards.Add(Blue_5);
            AllCards.Add(Blue_6);
            AllCards.Add(Blue_7);
            AllCards.Add(Blue_8);
            AllCards.Add(Blue_9);
            AllCards.Add(Blue_D);
            AllCards.Add(Blue_R);
            AllCards.Add(Blue_S);
            //---------------------------------------
            AllCards.Add(Green_1);
            AllCards.Add(Green_2);
            AllCards.Add(Green_3);
            AllCards.Add(Green_4);
            AllCards.Add(Green_5);
            AllCards.Add(Green_6);
            AllCards.Add(Green_7);
            AllCards.Add(Green_8);
            AllCards.Add(Green_9);
            AllCards.Add(Green_D);
            AllCards.Add(Green_R);
            AllCards.Add(Green_S);
            //---------------------------------------
            AllCards.Add(Red_1);
            AllCards.Add(Red_2);
            AllCards.Add(Red_3);
            AllCards.Add(Red_4);
            AllCards.Add(Red_5);
            AllCards.Add(Red_6);
            AllCards.Add(Red_7);
            AllCards.Add(Red_8);
            AllCards.Add(Red_9);
            AllCards.Add(Red_D);
            AllCards.Add(Red_R);
            AllCards.Add(Red_S);
            //---------------------------------------
            AllCards.Add(Yellow_1);
            AllCards.Add(Yellow_2);
            AllCards.Add(Yellow_3);
            AllCards.Add(Yellow_4);
            AllCards.Add(Yellow_5);
            AllCards.Add(Yellow_6);
            AllCards.Add(Yellow_7);
            AllCards.Add(Yellow_8);
            AllCards.Add(Yellow_9);
            AllCards.Add(Yellow_D);
            AllCards.Add(Yellow_R);
            AllCards.Add(Yellow_S);
        }

        for (int i = 0; i < 4; i++)
        {
            AllCards.Add(Wild);
            AllCards.Add(WildDraw4);
        }
    }

    public void Shuffle()
    {
        List<GameObject> tmp = new List<GameObject>();

        int max = AllCards.Count;
        while (max > 0)
        {
            int offset = UnityEngine.Random.Range(0, max);
            tmp.Add(AllCards[offset]);
            AllCards.RemoveAt(offset);
            max -= 1;
        }
        AllCards = tmp;
    }

    public void onClick()
    {
        if (WasClicked == false)
        {
            WasClicked = true;

            Blue_S.GetComponent<CardProperties>().cardType = "DontSkip";
            Green_S.GetComponent<CardProperties>().cardType = "Skip";
            Red_S.GetComponent<CardProperties>().cardType = "Skip";
            Yellow_S.GetComponent<CardProperties>().cardType = "Skip";
            //string CardColor = "";
            // Destroy any remaining cards before dealing new ones.
            for (int i = 0; i < PlayerArea.transform.childCount; i++)
                Destroy(PlayerArea.transform.GetChild(i).gameObject);

            for (int i = 0; i < EnemyArea1.transform.childCount; i++)
                Destroy(EnemyArea1.transform.GetChild(i).gameObject);

            for (int i = 0; i < EnemyArea2.transform.childCount; i++)
                Destroy(EnemyArea2.transform.GetChild(i).gameObject);

            for (int i = 0; i < EnemyArea3.transform.childCount; i++)
                Destroy(EnemyArea3.transform.GetChild(i).gameObject);

            // Deal out the cards.
            //for (int i = 0; i < AllCards.Count; i++)
            //{
            //    if (i < 28)
            //    {
            //        //Deal the first card to the Player.
            //        GameObject playerCard = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        playerCard.transform.SetParent(PlayerArea.transform, false);
            //        playerCard.GetComponent<CardProperties>().HasAuthority = true;
            //        playerCard.AddComponent<TurnManager>();
            //        PlayerCards.Add(playerCard);
            //        //Debug.Log(playerCard.name);
            //        //Debug.Log(playerCard.GetComponent<CardProperties>().cardColor);
            //        //Debug.Log(playerCard.GetComponent<CardProperties>().cardType);
            //        //Debug.Log(playerCard.GetComponent<CardProperties>().cardScore);
            //        i++;
            //        //Deal the next card to the opponent to the Player's right.
            //        GameObject enemyCard1 = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        enemyCard1.transform.SetParent(EnemyArea1.transform, false);
            //        enemyCard1.transform.Rotate(Vector3.forward * 90);
            //        enemyCard1.GetComponent<CardProperties>().isSideways = true;
            //        enemyCard1.GetComponent<CardProperties>().HasAuthority = false;
            //        enemyCard1.AddComponent<TurnManager>();
            //        EnemyCards1.Add(enemyCard1);
            //        //Deal the next card to the opponent across the table.
            //        i++;
            //        GameObject enemyCard2 = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        enemyCard2.transform.SetParent(EnemyArea2.transform, false);
            //        enemyCard2.GetComponent<CardProperties>().HasAuthority = false;
            //        enemyCard2.AddComponent<TurnManager>();
            //        EnemyCards2.Add(enemyCard2);
            //        //Deal the next card to the opponent to the Player's left.
            //        i++;
            //        GameObject enemyCard3 = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        enemyCard3.transform.SetParent(EnemyArea3.transform, false);
            //        enemyCard3.transform.Rotate(Vector3.forward * 90);
            //        enemyCard3.GetComponent<CardProperties>().isSideways = true;
            //        enemyCard3.GetComponent<CardProperties>().HasAuthority = false;
            //        enemyCard3.AddComponent<TurnManager>();
            //        EnemyCards3.Add(enemyCard3);
            //    }
            //    else if (i == 28)
            //    {
            //        //Deal the starter card to the DiscardPile.
            //        CurrentPlayedCard = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        CurrentPlayedCard.transform.SetParent(DiscardPile.transform, false);
            //        //PlayedCards.Add(CurrentPlayedCard);
            //    }
            //    else
            //    {
            //        //Put the remainder of the deck in the DeckZone.
            //        GameObject Card = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            //        Card.transform.SetParent(DeckZone.transform, false);
            //        RemainingCards.Add(Card);
            //        if (TurnManager.Turns == 0)
            //        {
            //            Card.GetComponent<CardProperties>().HasAuthority = true;
            //        }
            //    }

            //}

            // Create the deck of cards.
            //RemainingCards = AllCards;
            for (int i = 0; i < AllCards.Count; i++)
            {
                //Put the remainder of the deck in the DeckZone.
                Card = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
                Card.transform.SetParent(DeckZone.transform, false);
                RemainingCards.Add(Card);

            }

            // Deal out the cards.
            StartCoroutine(DealCard());
        }
    }

    IEnumerator DealCard()
    {
        OutputLog.WriteToOutput("hello from discard if statment");
        Card = RemainingCards[RemainingCards.Count - 1];
        iS = 0;
        while (iS < 1)
        {
            OutputLog.WriteToOutput("hello from discard while loop");
            yield return new WaitForSeconds(secRate);
            StartCoroutine(MoveTo(DiscardPile, Card, false, rate));
            iS++;
        }
        RemainingCards.Remove(Card);
        CurrentPlayedCard = Card;

        for (int i = 0; i < 7; i++)
        {
            for (int iP = 0; iP < 4; iP++)  //  I want this to deal out to 4 players
            {
                if (iP == 0)
                {
                    Card = RemainingCards[RemainingCards.Count - 1];
                    iS = 0;
                    while (iS < 1)
                    {
                        yield return new WaitForSeconds(secRate);
                        StartCoroutine(MoveTo(PlayerArea, Card, false, rate));
                        iS++;
                    }
                    RemainingCards.Remove(Card);
                    
                    Card.AddComponent<TurnManager>();
                    PlayerCards.Add(Card);

                }
                else if (iP == 1)
                {
                    Card = RemainingCards[RemainingCards.Count - 1];
                    iS = 0;
                    while (iS < 1)
                    {
                        yield return new WaitForSeconds(secRate);
                        StartCoroutine(MoveTo(EnemyArea1, Card, true, rate));
                        iS++;
                    }
                    RemainingCards.Remove(Card);
                    Card.AddComponent<TurnManager>();
                    EnemyCards1.Add(Card);
                }
                else if (iP == 2)
                {
                    Card = RemainingCards[RemainingCards.Count - 1];
                    iS = 0;
                    while (iS < 1)
                    {
                        yield return new WaitForSeconds(secRate);
                        StartCoroutine(MoveTo(EnemyArea2, Card, false, rate));
                        iS++;
                    }
                    RemainingCards.Remove(Card);
                    Card.AddComponent<TurnManager>();
                    EnemyCards2.Add(Card);
                }
                else if (iP == 3)
                {
                    Card = RemainingCards[RemainingCards.Count - 1];
                    iS = 0;
                    while (iS < 1)
                    {
                        yield return new WaitForSeconds(secRate);
                        StartCoroutine(MoveTo(EnemyArea3, Card, true, rate));
                        iS++;
                    }
                    RemainingCards.Remove(Card);
                    Card.AddComponent<TurnManager>();
                    EnemyCards3.Add(Card);
                }
            }
        }

        // Gives the player authority over there cards
        for (int i = 0; i < PlayerCards.Count; i++)
        {
            PlayerCards[i].GetComponent<CardProperties>().HasAuthority = true;
        }

        // Make the button inactive.
        Button.SetActive(false);

        OutputLog.WriteToOutput("Delt cards");
    }

    public static IEnumerator MoveTo(GameObject CurrentArea, GameObject theGO, bool isSideways, float time)
    {
        print("MoveTo");
        Vector3 start = theGO.transform.position;
        Vector3 end = CurrentArea.transform.position;// ;
        float t = 0;
        Debug.Log(t);

        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / time;
            theGO.transform.position = Vector3.Lerp(start, end, t);
            Debug.Log("CurrentArea = " + CurrentArea + "theGO = " + theGO);
        }
        theGO.transform.position = end;
        theGO.transform.SetParent(CurrentArea.transform, false);
        if (isSideways)
        {
            theGO.transform.Rotate(Vector3.forward * 90);
            theGO.GetComponent<CardProperties>().isSideways = true;
        }
    }
}