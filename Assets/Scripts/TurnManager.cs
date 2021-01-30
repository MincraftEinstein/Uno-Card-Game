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
    public DragDrop dragDrop;
    
    private List<GameObject> PlayableCards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //DiscardArea = GameObject.Find("DiscardArea");
    }
    private void Update()
    {
        CleanupDiscardArea();
    }

    private void CleanupDiscardArea()
    {
        if (DrawCards.PlayedCards.Count == 5)
        {
            DrawCards.PlayedCards.RemoveAt(0);
            OutputLog.WriteToOutput("Removed " + DrawCards.PlayedCards[0] + "from game");
            //Destroy(DrawCards.PlayedCards[0]);
            if (DrawCards.PlayedCards[0] != null)
            {
                // Do something  
                Destroy(DrawCards.PlayedCards[0]);
            }
        }
    }

    //------Player Code---------------------------------------------------------------------------------------------------------------------------

    public void PlayerManager()
    {
        if (Turns == 0)
        {
            if (TurnResult == "Skip")
            {
                OutputLog.WriteToOutput("Hello from PlayerManager.Skip");
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else if (TurnResult == "Draw2")
            {
                OutputLog.WriteToOutput("Hello from PlayerManager.Draw2");
                StartCoroutine(DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 2));
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else if (TurnResult == "WildDraw4")
            {
                OutputLog.WriteToOutput("Hello from PlayerManager.WildDraw4");
                StartCoroutine(DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 4));
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else
            {
                GameObject TopCard = DrawCards.CurrentPlayedCard;
                CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
                //Test the each card to identify playable cards.
                for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
                {
                    GameObject Card = DrawCards.PlayerCards[i];
                    Card.GetComponent<CardProperties>().HasAuthority = true;
                    CardProperties CardProperties = Card.GetComponent<CardProperties>();
                    if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
                    {
                        Card.GetComponent<DragDrop>().canPlayCard = true;
                    }
                }

                //If the player doesn't have any playable cards, draw one from the deck
                if (HasPlayableCard(DrawCards.PlayerCards, TopCardProperties) == false)
                {
                    OutputLog.WriteToOutput("player didn't have a playable card");
                    int CardNum = DrawCards.RemainingCards.Count - 1;
                    GameObject Card = DrawCards.RemainingCards[CardNum];
                    Card.GetComponent<CardProperties>().HasAuthority = true;
                    //Test the new card to see if it can be played
                    CardProperties CardProperties = Card.GetComponent<CardProperties>();
                    if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
                    {
                        //Card is playable                     
                        Card.GetComponent<DragDrop>().canPlayCard = true;
                    }
                    else
                    {
                        //Card isn't playable, so move to the next player.
                        TurnResult = "normal";
                        StartCoroutine(IncrementTurns());
                    }
                }
            }
        }
        //else
        ////Not the players turn
        //{
        //    for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
        //    {
        //        DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = false;
        //    }
        //}
    }

//------AI Code---------------------------------------------------------------------------------------------------------------------------

    private bool HasPlayableCard(List<GameObject> Hand, CardProperties TopCardProperties)
    {
        for (int i = 0; i < Hand.Count; i++)
        {
            GameObject Card = Hand[i];
            CardProperties CardProperties = Card.GetComponent<CardProperties>();
            if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
            {
                return true;
            }
        }
        return false;
    }

    private void PlayCard(List<GameObject> Hand, CardProperties TopCardProperties, string Name)
    {
        //Sort the list based on cardScore
        //PlayableCards.Sort((x, y) => x.GetComponent<CardProperties>().cardScore.CompareTo(y.GetComponent<CardProperties>().cardScore));
        for (int i = 0; i < Hand.Count; i++)
        {
            GameObject Card = Hand[i];
            CardProperties CardProperties = Card.GetComponent<CardProperties>();
            if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
            {
                PlayableCards.Add(Card);
                OutputLog.WriteToOutput(Card.name + " was added to PlayableCards");
            }
        }

        //Look for a Draw2 card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Draw2")
            {
                WhenCardPlayed(Hand, i, Name);
                TurnResult = "Draw2";
                goto PlayCardEnd;
            }
        }

        //Look for a Skip card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Skip")
            {
                WhenCardPlayed(Hand, i, Name);
                TurnResult = "Skip";
                goto PlayCardEnd;
            }
        }
        //Look for a Reverse card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Reverse")
            {
                WhenCardPlayed(Hand, i, Name);
                IsReversed = !IsReversed;
                TurnResult = "Reverse";
                goto PlayCardEnd;
            }
        }
        //Look for a Number card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardScore <= 9)
            {
                WhenCardPlayed(Hand, i, Name);
                TurnResult = "NumberCard";   
                goto PlayCardEnd;
            }
        }
        //Look for a Wild card.
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "Wild")
            {
                CountCardColors(Hand, PlayableCards[i]);
                WhenCardPlayed(Hand, i, Name);
                TurnResult = "Wild";
                goto PlayCardEnd;
            }
        }
        //Look for a Wild Draw 4
        for (int i = 0; i < PlayableCards.Count; i++)
        {
            if (PlayableCards[i].GetComponent<CardProperties>().cardType == "WildDraw4")
            {
                CountCardColors(Hand, PlayableCards[i]);
                WhenCardPlayed(Hand, i, Name);
                TurnResult = "WildDraw4";
                goto PlayCardEnd;
            }
        }
        PlayCardEnd:
        PlayableCards.Clear();
        //CleanupDiscardArea();
        StartCoroutine(IncrementTurns());
    }

    private void WhenCardPlayed(List<GameObject> Hand, int i, string Name)
    {
        //if (Hand == DrawCards.EnemyCards2)
        //{
        //    PlayableCards[i].transform.Rotate(Vector3.forward * 90);
        //}
        StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
        DrawCards.PlayedCards.Add(PlayableCards[i]);
        Hand.Remove(PlayableCards[i]);
        DrawCards.CurrentPlayedCard = PlayableCards[i];
        PlayableCards[i].GetComponent<CardFlipper>().Flip();
        OutputLog.WriteToOutput(Name + ": Played " + PlayableCards[i].name);
    }

    void CountCardColors(List<GameObject> Hand, GameObject Card)
    {
        int redCards = 0;
        int yellowCards = 0;
        int greenCards = 0;
        int blueCards = 0;
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].GetComponent<CardProperties>().cardColor == "Red")
            {
                redCards++;
            }
            if (Hand[i].GetComponent<CardProperties>().cardColor == "Yellow")
            {
                yellowCards++;
            }
            if (Hand[i].GetComponent<CardProperties>().cardColor == "Green")
            {
                greenCards++;
            }
            if (Hand[i].GetComponent<CardProperties>().cardColor == "Blue")
            {
                blueCards++;
            }
        }
        OutputLog.WriteToOutput("redCards: " + redCards);
        OutputLog.WriteToOutput("yellowCards: " + yellowCards);
        OutputLog.WriteToOutput("greenCards: " + greenCards);
        OutputLog.WriteToOutput("blueCards: " + blueCards);
        if(FindLargestNum(redCards, yellowCards, greenCards, blueCards) == redCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Red";
            OutputLog.WriteToOutput("wild color set to red");
        }
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == yellowCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Yellow";
            OutputLog.WriteToOutput("wild color set to yellow");
        } 
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == greenCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Green";
            OutputLog.WriteToOutput("wild color set to green");
        }
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == blueCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Blue";
            OutputLog.WriteToOutput("wild color set to blue");
        }
    }

    public int FindLargestNum(int n1, int n2, int n3, int n4)
    {
        // variable declaration 
        //int n1 = 5, n2 = 10,
        //    n3 = 15, n4 = 20, max;
        int max;

        // Largest among n1 and n2 
        max = (n1 > n2 && n1 > n2 && n1 > n2) ?
                    n1 : (n2 > n3 && n2 > n4) ?
                               n2 : (n3 > n4) ? n3 : n4;

        // Print the largest number 
        OutputLog.WriteToOutput("Largest number among " +
                            n1 + ", " + n2 + ", " +
                                n3 + " and " + n4 +
                                     " is " + max);
        return max;
    }

    // This method determines what the current player will do
    private void DeterminePlay(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    {
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        OutputLog.WriteToOutput("Top card: " + TopCard);
        if (TurnResult == "Skip")
        {
            TurnResult = "normal";
            StartCoroutine(IncrementTurns());
        }
        else if (TurnResult == "Draw2")
        {
            StartCoroutine(DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 2));
            TurnResult = "normal";
            StartCoroutine(IncrementTurns());
        }
        else if (TurnResult == "WildDraw4")
        {
            StartCoroutine(DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 4));
            TurnResult = "normal";
            StartCoroutine(IncrementTurns());
        }
        else
        {
            if (HasPlayableCard(enemyHand, TopCardProperties) == true)
            {
                PlayCard(enemyHand, TopCardProperties, enemyName);
                OutputLog.WriteToOutput("HasPlayableCard = true");
            }
            else
            {
                StartCoroutine(DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 1));
                PlayCard(enemyHand, TopCardProperties, enemyName);
                OutputLog.WriteToOutput("HasPlayableCard = false");
            }
        }
    }

    public IEnumerator DrawCardsFromDeck(List<GameObject> Hand, GameObject Area, string Name, int NumberOfCards)
    {
        for (int i = 0; i < NumberOfCards; i++)
        {
            yield return new WaitForSeconds(secRate);
            int CardNum = DrawCards.RemainingCards.Count - 1;
            GameObject Card = DrawCards.RemainingCards[CardNum];
            OutputLog.WriteToOutput("Top Deck Card:" + Card.name);
            StartCoroutine(MoveCard(Card, DrawCards.RemainingCards, Area));
            if (Area == PlayerArea)
            {
                Card.GetComponent<CardFlipper>().Flip();
            }
            Hand.Add(Card);
            OutputLog.WriteToOutput(Name + ": Drew " + Card.name);
        }
        OutputLog.WriteToOutput(Name + ": Drew " + NumberOfCards + " cards");
    }

    // This method activates the next player
    public IEnumerator IncrementTurns()
    {
        OutputLog.WriteToOutput("Hello from IncrementTurns");
        yield return new WaitForSeconds(2);
        if (!IsReversed)
        {
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

        PlayerManager();

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
            if (areaTo == EnemyArea1 || areaTo == EnemyArea3)
            {
                //Cards sent to EnemyArea1 and EnemyArea3 should be rotated.
                StartCoroutine(DrawCards.MoveTo(areaTo, card, true, rate));
            }
            else if (areaTo == EnemyArea2 || areaTo == PlayerArea)
            {
                //Cards sent to EnemyArea2 and PlayerArea should not be rotated.
                StartCoroutine(DrawCards.MoveTo(areaTo, card, false, rate));
            }
            else
            {
                //This is for playing cards on the discard pile
                //If the card is rotated (isSideways) we need to rotate it again
                StartCoroutine(DrawCards.MoveTo(areaTo, card, card.GetComponent<CardProperties>().isSideways, rate));
            }
            iS++;
        }
        listFrom.Remove(card);
        yield return new WaitForSeconds(secRate);
    }
}
