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
        DiscardArea = GameObject.Find("DiscardArea");
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
                for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
                {
                    DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = true;
                }

                GameObject TopCard = DrawCards.CurrentPlayedCard;
                CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
                if (HasPlayableCard(DrawCards.PlayerCards, TopCardProperties) == false)
                {
                    int DeckTopCardNum = DrawCards.RemainingCards.Count - 1;
                    GameObject DeckTopCard = DrawCards.RemainingCards[DeckTopCardNum];
                    DeckTopCard.GetComponent<CardProperties>().HasAuthority = true;
                }
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
                OutputLog.WriteToOutput(Card + " was added to PlayableCards");
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
        PlayCardEnd:
        PlayableCards.Clear();
        CleanupDiscardArea();
        StartCoroutine(IncrementTurns());
    }

    private void WhenCardPlayed(List<GameObject> Hand, int i, string Name)
    {
        StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
        if (Hand != DrawCards.EnemyCards2 || Hand != DrawCards.PlayerCards)
        {
            if (Hand != DrawCards.EnemyCards2 || Hand != DrawCards.PlayerCards)
            {
                PlayableCards[i].transform.Rotate(Vector3.forward * 90);
            }
        }
        DrawCards.PlayedCards.Add(PlayableCards[i]);
        Hand.Remove(PlayableCards[i]);
        DrawCards.CurrentPlayedCard = PlayableCards[i];
        OutputLog.WriteToOutput(Name + ": Played " + PlayableCards[i].name);
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
            int DeckTopCardNum = DrawCards.RemainingCards.Count - 1;
            GameObject DeckTopCard = DrawCards.RemainingCards[DeckTopCardNum];
            OutputLog.WriteToOutput("Top Deck Card:" + DeckTopCard);
            //DeckTopCard.transform.SetParent(area.transform, false);
            //DrawCards.RemainingCards.Remove(DeckTopCard);
            StartCoroutine(MoveCard(DeckTopCard, DrawCards.RemainingCards, Area));
            if (Hand != DrawCards.EnemyCards2 || Hand != DrawCards.PlayerCards)
            {
                DeckTopCard.transform.Rotate(Vector3.forward * 90);
            }
            Hand.Add(DeckTopCard);
            OutputLog.WriteToOutput(Name + ": Drew " + DeckTopCard);
        }
        OutputLog.WriteToOutput(Name + ": Drew " + NumberOfCards + " cards");
    }

    // This method activates the next player
    public IEnumerator IncrementTurns()
//    public void StartCoroutine(IncrementTurns)()
    {

        yield return new WaitForSeconds(Random.Range(2, 4));
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
            if (areaTo == EnemyArea2)
            {
                StartCoroutine(DrawCards.MoveTo(areaTo, card, true, rate));
            }
            else
            {
                StartCoroutine(DrawCards.MoveTo(areaTo, card, false, rate));
            }
            //StartCoroutine(DrawCards.MoveTo(areaTo, card, true, rate));
            iS++;
        }
        listFrom.Remove(card);
        yield return new WaitForSeconds(secRate);
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
