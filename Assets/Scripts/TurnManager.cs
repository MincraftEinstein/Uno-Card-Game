using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static int Turns; // 0 = Player, 1 = Enemy 1, 2 = Enemy 2, 3 = Enemy 3
    public static bool IsReversed;
    public float rate = 0.5F;
    public float secRate = 0.5F;
    public int iS;
    public GameObject EnemyArea1;
    public GameObject EnemyArea2;
    public GameObject EnemyArea3;
    public GameObject DiscardArea;

    // Start is called before the first frame update
    void Start()
    {
        DiscardArea = GameObject.Find("DiscardArea");
        OutputLog.WriteToOutput("Top card: " + DrawCards.CurrentPlayedCard);
    }

    // Update is called once per frame
    void Update()
    {
        if (DragDrop.isWildMenuShown == false)
        {
            if (Turns == 1)
            {
                isActionCard(DrawCards.EnemyCards1, EnemyArea1, "Enemy 1");
            }
            if (Turns == 2)
            {
                isActionCard(DrawCards.EnemyCards2, EnemyArea2, "Enemy 2");
            }
            if (Turns == 3)
            {
                isActionCard(DrawCards.EnemyCards3, EnemyArea3, "Enemy 3");
            }
        }
    }

    private void isActionCard(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    {
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        OutputLog.WriteToOutput("Top card: " + TopCard);
        if (TopCardProperties.cardType == "Skip")
        {
            SkipEnemy(TopCardProperties, enemyHand);
        }
        else if (TopCardProperties.cardType == "Draw2")
        {
            DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 2);
            SkipEnemy(TopCardProperties, enemyHand);
        }
        /*else if (TopCardProperties.cardType == "Wild")
        {
            //WildMenu.s
        }*/
        else if (TopCardProperties.cardType == "WildDraw4")
        {
            DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 4);
            SkipEnemy(TopCardProperties, enemyHand);
        }
        else
        {
            HasMatch(enemyHand, enemyArea, enemyName);
        }
    }

    private void SkipEnemy(CardProperties card, List<GameObject> Hand)
    {
        if (Hand == DrawCards.PlayerCards)
        {
            OutputLog.WriteToOutput("Skipped Player");
            card.cardType = "Skipped";
            Turns = 1;
        }
        if (Hand == DrawCards.EnemyCards1)
        {
            OutputLog.WriteToOutput("Skipped Enemy 1");
            card.cardType = "Skipped";
            Turns = 2;

        }
        if (Hand == DrawCards.EnemyCards2)
        {
            OutputLog.WriteToOutput("Skipped Enemy 2");
            card.cardType = "Skipped";
            Turns = 3;
        }
        if (Hand == DrawCards.EnemyCards3)
        {
            OutputLog.WriteToOutput("Skipped Enemy 3");
            card.cardType = "Skipped";
            Turns = 0;
        }
    }

    private void HasMatch(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    {
        CardProperties TopCardProperties = DrawCards.CurrentPlayedCard.GetComponent<CardProperties>();
        OutputLog.WriteToOutput("Top card: " + TopCardProperties);
        //bool HasDrawnCardToPlay = false;
        for (int i = 0; i < enemyHand.Count; i++)
        {
            GameObject Card = enemyHand[i];
            CardProperties CardProperties = Card.GetComponent<CardProperties>();
            if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType)
            {
                if (enemyHand != DrawCards.EnemyCards2) //Fixs sideways cards
                {
                    Card.transform.Rotate(Vector3.forward * 90);
                }
                //enemyHand.Remove(Card);
                //Card.transform.SetParent(DiscardArea.transform, false);
                StartCoroutine(MoveCard(Card, enemyHand, DiscardArea));
                DrawCards.CurrentPlayedCard = Card;
                TopCardProperties = Card.GetComponent<CardProperties>();
                OutputLog.WriteToOutput(enemyName + ": Played " + Card + " (Card " + i + ")");
                if (TopCardProperties.cardType == "Reverse")
                {
                    IsReversed = !IsReversed;
                    HasMatch(enemyHand, enemyArea, enemyName);
                    OutputLog.WriteToOutput("IsReversed: " + IsReversed);
                }
                IncrementTurns(enemyHand);
                return;
            }
            //else if (i == enemyHand.Count - 1 && HasDrawnCardToPlay == false) {
            //    HasDrawnCardToPlay = true;
            //    DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 1);
            //    OutputLog.WriteToOutput(enemyName + ": Does not have a match");
            //    HasMatch(enemyHand, enemyArea, enemyName);
            ////ReverseGame(enemyHand);
            //IncrementTurns(enemyHand);
            //    return;
            //}
            OutputLog.WriteToOutput(enemyName + ": " + Card + " (Card " + i + ") did not match");
        }
    }

    private void DrawCardsFromDeck(List<GameObject> enemyHand, GameObject enemyArea, string enemyName, int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            int DeckTopCardNum = DrawCards.RemainingCards.Count - 1;
            GameObject DeckTopCard = DrawCards.RemainingCards[DeckTopCardNum];
            OutputLog.WriteToOutput("Top Deck Card:" + DeckTopCard);
            if (enemyHand != DrawCards.EnemyCards2)
            {
                DeckTopCard.transform.Rotate(Vector3.forward * 90);
            }
            DeckTopCard.transform.SetParent(enemyArea.transform, false);
            DrawCards.RemainingCards.Remove(DeckTopCard);
            enemyHand.Add(DeckTopCard);
            OutputLog.WriteToOutput(enemyName + ": Drew " + DeckTopCard);
        }
        OutputLog.WriteToOutput(enemyName + ": Drew " + numberOfCards + " cards");
    }

    public static void IncrementTurns(List<GameObject> Hand)
    {
        //if (IsReversed == false)
        //{
        //    if (Hand != DrawCards.EnemyCards3)
        //    {
        //        Turns++;
        //    }
        //    else if (Hand == DrawCards.EnemyCards3)
        //    {
        //        Turns = 0;
        //    }
        //}
        //else
        //{
        //    if (Hand != DrawCards.PlayerCards)
        //    {
        //        Turns--;
        //    }
        //    else if (Hand == DrawCards.PlayerCards)
        //    {
        //        Turns = 3;
        //    }
        //}
        if (!IsReversed)
        {
            Turns++;
            if(Turns > 3)
            {
                Turns = 0;
            }
        }
        else
        {
            Turns--;
            if (Turns < 0)
            {
                Turns = 3;
            }
        }
        OutputLog.WriteToOutput("Turns: " + Turns);
    }

    IEnumerator MoveCard(GameObject card, List<GameObject> listFrom, GameObject areaTo)
    {
        iS = 0;
        while (iS < 1)
        {
            yield return new WaitForSeconds(secRate);
            StartCoroutine(DrawCards.MoveTo(areaTo, card, false, rate));

            iS++;
        }
        listFrom.Remove(card);
    }
}
