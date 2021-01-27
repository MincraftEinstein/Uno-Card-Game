using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static int Turns; // 0 = Player, 1 = Enemy 1, 2 = Enemy 2, 3 = Enemy 3
    public int iS;
    public float rate = 0.5F;
    public float secRate = 0.5F;
    public bool IsReversed;
    public string TurnResult;
    public static bool isWildMenuShown;
    public GameObject PlayerArea;
    public GameObject EnemyArea1;
    public GameObject EnemyArea2;
    public GameObject EnemyArea3;
    public GameObject DiscardArea;
    public GameObject WildMenu;
    private GameObject Canvas;
    private List<GameObject> PlayableCards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DiscardArea = GameObject.Find("DiscardArea");
        Canvas = GameObject.Find("Canvas");
        OutputLog.WriteToOutput("Top card: " + DrawCards.CurrentPlayedCard);
    }

//------Player Code---------------------------------------------------------------------------------------------------------------------------

    public void PlayerManager()
    {
        if (Turns == 0)
        {
            if (TurnResult == "Skip")
            {
                TurnResult = "normal";
                IncrementTurns();
            }
            else if (TurnResult == "Draw2")
            {
                DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 2);
                TurnResult = "normal";
                IncrementTurns();
            }
            else if (TurnResult == "WildDraw4")
            {
                DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 4);
                TurnResult = "normal";
                IncrementTurns();
            }
            else
            {
                for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
                {
                    DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = true;
                }

                GameObject TopCard = DrawCards.CurrentPlayedCard;
                CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
                if (TopCardProperties.cardType == "Skip")
                {
                    TurnResult = "Skip";
                    return;
                }
                else if (TopCardProperties.cardType == "Draw2")
                {
                    TurnResult = "Draw2";
                    return;
                }
                else if (TopCardProperties.cardType == "Reverse")
                {
                    IsReversed = !IsReversed;
                    TurnResult = "Reverse";
                    return;
                }
                else if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
                {
                    isWildMenuShown = true;
                    Instantiate(WildMenu, Canvas.transform);
                    OutputLog.WriteToOutput("Wild Menu shown");
                    TurnResult = "normal";
                    return;
                }
                IncrementTurns();
            }
        }
        else
        {
            for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
            {
                DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = false;
            }
        }
    }

    //public void TestPlayerCard()
    //{

    //    OutputLog.WriteToOutput("Made it this far (TurnManager)");
    //    CardProperties TopCardProperties = DrawCards.CurrentPlayedCard.GetComponent<CardProperties>();
    //    if (Turns == 0)
    //    {
    //        for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
    //        {
    //            DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = true;
    //        }
    //        if (TopCardProperties.cardType == "Reverse")
    //        {
    //            TopCardProperties.cardType = "Reversed";
    //            IsReversed = !IsReversed;
    //        }
    //        else if (TopCardProperties.cardType == "Skip")
    //        {
    //            SkipPerson(TopCardProperties, DrawCards.PlayerCards);
    //        }
    //        else if (TopCardProperties.cardType == "Draw2")
    //        {
    //            DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 2);
    //            SkipPerson(TopCardProperties, DrawCards.PlayedCards);
    //        }
    //        IncrementTurns();
    //    }
    //    else if (Turns == 3)
    //    {
    //        if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
    //        {
    //            isWildMenuShown = true;
    //            Instantiate(WildMenu, Canvas.transform);
    //            OutputLog.WriteToOutput("Wild Menu shown");
    //            if (TopCardProperties.cardType == "WildDraw4")
    //            {
    //                DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 4);
    //            }
    //        }
    //    }
    //}

//------AI Code---------------------------------------------------------------------------------------------------------------------------

    private bool HasPlayableCard(List<GameObject> Hand, CardProperties TopCardProperties)
    {
        for (int i = 0; i < Hand.Count; i++)
        {
            GameObject Card = Hand[i];
            CardProperties CardProperties = Card.GetComponent<CardProperties>();
            if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
            {
                //PlayableCards.Add(Card);
                return true;
            }
        }
        return false;
    }

    private void PlayCard(List<GameObject> Hand, CardProperties TopCardProperties)
    {
        //Sort the list based on cardScore
        //PlayableCards.Sort((x, y) => x.GetComponent<CardProperties>().cardScore.CompareTo(y.GetComponent<CardProperties>().cardScore));

        for (int i = 0; i > Hand.Count; i++)
        {
            GameObject Card = Hand[i];
            CardProperties CardProperties = Card.GetComponent<CardProperties>();
            if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
            {
                PlayableCards.Add(Card);
                OutputLog.WriteToOutput(Hand + " has add " + Card + "to playable cards");
            }
        }

        //Temporary
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            OutputLog.WriteToOutput("PlayableCards[" + i + "]: " + PlayableCards[i].GetComponent<CardProperties>().cardType);
        }

        //Look for a Draw2 card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Draw2")
            {
                StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
                TurnResult = "Draw2";
                return;
            }
        }

        //Look for a Skip card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Skip")
            {
                StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
                TurnResult = "Skip";
                return;
            }
        }
        //Look for a Reverse card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Reverse")
            {
                StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
                IsReversed = !IsReversed;
                TurnResult = "Reverse";
                return;
            }
        }
        //Look for a Number card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardScore <= 9)
            {
                StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
                TurnResult = "NumberCard";
                return;
            }
        }
        PlayableCards.Clear();
        IncrementTurns();
    }

    private void DeterminePlay(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    {   // This method determines what the current player will do
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        OutputLog.WriteToOutput("Top card: " + TopCard);
        if (TurnResult == "Skip")
        {
            TurnResult = "normal";
            IncrementTurns();
        }
        else if (TurnResult == "Draw2")
        {
            DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 2);
            TurnResult = "normal";
            IncrementTurns();
        }
        else if (TurnResult == "WildDraw4")
        {
            DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 4);
            TurnResult = "normal";
            IncrementTurns();
        }
        else
        {
            //HasMatch(enemyHand, enemyArea, enemyName);
            if (HasPlayableCard(enemyHand, TopCardProperties) == true)
            {
                PlayCard(enemyHand, TopCardProperties);
            }
            else
            {
                DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 1);
                PlayCard(enemyHand, TopCardProperties);
            }
        }
    }

    //private void SkipPerson(CardProperties Card, List<GameObject> Hand)
    //{
    //    if (Hand == DrawCards.PlayerCards)
    //    {
    //        OutputLog.WriteToOutput("Skipped Player");
    //        Card.cardType = "Skipped";
    //        Turns = 1;
    //    }
    //    if (Hand == DrawCards.EnemyCards1)
    //    {
    //        OutputLog.WriteToOutput("Skipped Enemy 1");
    //        Card.cardType = "Skipped";
    //        Turns = 2;

    //    }
    //    if (Hand == DrawCards.EnemyCards2)
    //    {
    //        OutputLog.WriteToOutput("Skipped Enemy 2");
    //        Card.cardType = "Skipped";
    //        Turns = 3;
    //    }
    //    if (Hand == DrawCards.EnemyCards3)
    //    {
    //        OutputLog.WriteToOutput("Skipped Enemy 3");
    //        Card.cardType = "Skipped";
    //        Turns = 0;
    //    }
    //}

    //private void HasMatch(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    //{
    //    CardProperties TopCardProperties = DrawCards.CurrentPlayedCard.GetComponent<CardProperties>();
    //    OutputLog.WriteToOutput("Top card: " + TopCardProperties);
    //    bool HasFoundMatch = false;
    //    for (int i = 0; i < enemyHand.Count; i++)
    //    {
    //        GameObject Card = enemyHand[i];
    //        CardProperties CardProperties = Card.GetComponent<CardProperties>();
    //        if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType)
    //        {
    //            HasFoundMatch = true;
    //            if (enemyHand != DrawCards.EnemyCards2) // Fixs sideways cards
    //            {
    //                Card.transform.Rotate(Vector3.forward * 90);
    //            }
    //            //enemyHand.Remove(Card);
    //            //Card.transform.SetParent(DiscardArea.transform, false);
    //            StartCoroutine(MoveCard(Card, enemyHand, DiscardArea));
    //            DrawCards.CurrentPlayedCard = Card;
    //            DrawCards.PlayedCards.Add(Card);
    //            TopCardProperties = Card.GetComponent<CardProperties>();
    //            OutputLog.WriteToOutput(enemyName + ": Played " + Card + " (Card " + i + ")");
    //            //CleanupDiscardArea();
    //            TestPlayerCard();
    //            if (TopCardProperties.cardType == "Reverse")
    //            {
    //                TopCardProperties.cardType = "Reversed";
    //                IsReversed = !IsReversed;
    //                HasMatch(enemyHand, enemyArea, enemyName);
    //                OutputLog.WriteToOutput("IsReversed: " + IsReversed);
    //            }
    //            IncrementTurns();
    //            //return;
    //            break;
    //        }
    //        OutputLog.WriteToOutput(enemyName + ": " + Card + " (Card " + i + ") did not match");
    //    }
    //    if (HasFoundMatch == false)
    //    {
    //        OutputLog.WriteToOutput("HasFoundMatch = " + HasFoundMatch);
    //        DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 1);
    //        IncrementTurns();  // Temp fix till can test card
    //    }
    //}

    public void DrawCardsFromDeck(List<GameObject> hand, GameObject area, string name, int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            int DeckTopCardNum = DrawCards.RemainingCards.Count - 1;
            GameObject DeckTopCard = DrawCards.RemainingCards[DeckTopCardNum];
            OutputLog.WriteToOutput("Top Deck Card:" + DeckTopCard);
            DeckTopCard.transform.SetParent(area.transform, false);
            DrawCards.RemainingCards.Remove(DeckTopCard);
            //StartCoroutine(MoveCard(DeckTopCard, DrawCards.RemainingCards, enemyArea));
            if (hand != DrawCards.EnemyCards2)
            {
                DeckTopCard.transform.Rotate(Vector3.forward * 90);
            }
            hand.Add(DeckTopCard);
            OutputLog.WriteToOutput(name + ": Drew " + DeckTopCard);
        }
        OutputLog.WriteToOutput(name + ": Drew " + numberOfCards + " cards");
    }

    public void IncrementTurns()
    {
        if (!IsReversed)
        {   // This method activates the next player
            Turns++;
            if (Turns > 3)
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

        if (isWildMenuShown == false)
        {
            if (Turns == 1)
            {
                DeterminePlay(DrawCards.EnemyCards1, EnemyArea1, "Enemy 1");
            }
            if (Turns == 2)
            {
                DeterminePlay(DrawCards.EnemyCards2, EnemyArea2, "Enemy 2");
            }
            if (Turns == 3)
            {
                DeterminePlay(DrawCards.EnemyCards3, EnemyArea3, "Enemy 3");
            }
        }
    }

    IEnumerator MoveCard(GameObject card, List<GameObject> listFrom, GameObject areaTo)
    {
        iS = 0;
        while (iS < 1)
        {
            yield return new WaitForSeconds(secRate);
            if (areaTo != EnemyArea2)
            {
                StartCoroutine(DrawCards.MoveTo(areaTo, card, true, rate));
            }
            else
            {
                StartCoroutine(DrawCards.MoveTo(areaTo, card, false, rate));
            }

            iS++;
        }
        listFrom.Remove(card);
    }

    public static void CleanupDiscardArea()
    {
        if (DrawCards.PlayedCards.Count == 6)
        {
            OutputLog.WriteToOutput("Removed " + DrawCards.PlayedCards[0]);
            Destroy(DrawCards.PlayedCards[0]);
            DrawCards.PlayedCards.RemoveAt(0);
        }
    }
}
