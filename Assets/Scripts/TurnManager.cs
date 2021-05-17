using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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
    public Text Enemy1Score;
    public Text Enemy2Score;
    public Text Enemy3Score;
    public Text PlayerScore;
    public GameObject PlayerWonText;
    public GameObject PlayerLostText;
    public int WinnerScore;
    public int PlayerScoreInt = 0;
    public int Enemy1ScoreInt = 0;
    public int Enemy2ScoreInt = 0;
    public int Enemy3ScoreInt = 0;
    private bool PlayerWon;
    public bool GameOver;
    public Image CardColorImage;
    public Sprite RedCardColor;
    public Sprite YellowCardColor;
    public Sprite GreenCardColor;
    public Sprite BlueCardColor;
    public bool isPaused;
    public GameObject RoundFinishedMenu;
    public GameObject GameOverMenu;

    private List<GameObject> PlayableCards = new List<GameObject>();

    private void Start()
    {
        PlayerScoreInt = PlayerPrefs.GetInt("playerScore");
        Enemy1ScoreInt = PlayerPrefs.GetInt("enemy1Score");
        Enemy2ScoreInt = PlayerPrefs.GetInt("enemy2Score");
        Enemy3ScoreInt = PlayerPrefs.GetInt("enemy3Score");
    }

    private void Update()
    {
        //CleanupDiscardArea();
        //Color newcolor;
        if (GameOver == false)
        {
            if (CardColorImage.enabled)
            {
                CardProperties TopCard = DrawCards.CurrentPlayedCard.GetComponent<CardProperties>();
                if (TopCard.cardColor == "Red")
                {
                    //CardColorImage.color = new Color(184, 0, 0);
                    //Color rgb = new Color(184, 0, 0);
                    //float h = 0.0F, s = 100.0F, v = 72.2F;
                    //CardColorImage.color = Color.RGBToHSV(rgb, out h, out s, out v);
                    CardColorImage.sprite = RedCardColor;
                }
                else if (TopCard.cardColor == "Yellow")
                {
                    //CardColorImage.color = new Color(246, 224, 14);
                    CardColorImage.sprite = YellowCardColor;
                }
                else if (TopCard.cardColor == "Green")
                {
                    //CardColorImage.color = new Color(54, 76, 18);
                    CardColorImage.sprite = GreenCardColor;
                }
                else if (TopCard.cardColor == "Blue")
                {
                    //CardColorImage.color = new Color(255, 0, 76, 189);
                    CardColorImage.sprite = BlueCardColor;
                }
                else
                {
                    CardColorImage.sprite = null;
                    CardColorImage.color = Color.white;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuButtons buttons = GetComponent<MenuButtons>();
                if (isPaused)
                {
                    buttons.PlayButton();
                }
                else
                {
                    buttons.PauseButton();
                }
            }
        }
    }

    private void ReshuffleDeck()
    {
        if (DrawCards.RemainingCards.Count <= 71)
        {
            Debug.Log("Deck has 0 cards");
            // This is for testing
            for (int i = 0; i < DrawCards.RemainingCards.Count; i++)
            {
                GameObject card = DrawCards.RemainingCards[i];
                DrawCards.RemainingCards.Remove(card);
                Destroy(card);
            }
            Debug.Log("Removed all old cards");

            DrawCards drawCards = GetComponent<DrawCards>();
            List<GameObject> tempDeck = new List<GameObject>();
            drawCards.CreateNewDeck(tempDeck);
            List<GameObject> newDeck = new List<GameObject>(tempDeck);
            for (int i = 0; i < tempDeck.Count; i++)
            {
                for (int i1 = 0; i1 < DrawCards.PlayerCards.Count; i1++)
                {
                    CardProperties deckCard = tempDeck[i].GetComponent<CardProperties>();
                    CardProperties handCard = DrawCards.PlayerCards[i1].GetComponent<CardProperties>();
                    if (deckCard.cardColor == handCard.cardColor && deckCard.cardType == handCard.cardType)
                    {
                        newDeck.Remove(tempDeck[i]);
                    }
                }
                for (int i2 = 0; i2 < DrawCards.EnemyCards1.Count; i2++)
                {
                    CardProperties deckCard = tempDeck[i].GetComponent<CardProperties>();
                    CardProperties handCard = DrawCards.EnemyCards1[i2].GetComponent<CardProperties>();
                    if (deckCard.cardColor == handCard.cardColor && deckCard.cardType == handCard.cardType)
                    {
                        newDeck.Remove(tempDeck[i]);
                    }
                }
                for (int i3 = 0; i3 < DrawCards.EnemyCards2.Count; i3++)
                {
                    CardProperties deckCard = tempDeck[i].GetComponent<CardProperties>();
                    CardProperties handCard = DrawCards.EnemyCards2[i3].GetComponent<CardProperties>();
                    if (deckCard.cardColor == handCard.cardColor && deckCard.cardType == handCard.cardType)
                    {
                        newDeck.Remove(tempDeck[i]);
                    }
                }
                for (int i4 = 0; i4 < DrawCards.EnemyCards3.Count; i4++)
                {
                    CardProperties deckCard = tempDeck[i].GetComponent<CardProperties>();
                    CardProperties handCard = DrawCards.EnemyCards3[i4].GetComponent<CardProperties>();
                    if (deckCard.cardColor == handCard.cardColor && deckCard.cardType == handCard.cardType)
                    {
                        newDeck.Remove(tempDeck[i]);
                    }
                }
            }
            Debug.Log("Generated new deck");
            DrawCards.AllCards = new List<GameObject>(newDeck);
            drawCards.onClick(false);
        }
    }

    private void CleanupDiscardArea()
    {
        if (DrawCards.PlayedCards.Count >= 5)
        {
            GameObject bottomCard = DrawCards.PlayedCards[0];
            DrawCards.PlayerCards.Remove(bottomCard);
            DrawCards.EnemyCards1.Remove(bottomCard);
            DrawCards.EnemyCards2.Remove(bottomCard);
            DrawCards.EnemyCards3.Remove(bottomCard);
            DrawCards.RemainingCards.Remove(bottomCard);
            DrawCards.PlayedCards.Remove(bottomCard);
            if (bottomCard != null)
            {
                Destroy(bottomCard);
                Debug.Log("Removed " + bottomCard + " from game");
            }
        }
    }

    //------Player Code---------------------------------------------------------------------------------------------------------------------------

    public void PlayerManager()
    {
        if (Turns == 0)
        {
            //gameObject.GetComponent<HighlightPlayer>().SetSelection();
            if (TurnResult == "Skip")
            {
                Debug.Log("Hello from PlayerManager.Skip");
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else if (TurnResult == "Draw2")
            {
                Debug.Log("Hello from PlayerManager.Draw2");
                StartCoroutine(DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 2));
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else if (TurnResult == "WildDraw4")
            {
                Debug.Log("Hello from PlayerManager.WildDraw4");
                StartCoroutine(DrawCardsFromDeck(DrawCards.PlayerCards, PlayerArea, "Player", 4));
                TurnResult = "normal";
                StartCoroutine(IncrementTurns());
            }
            else
            {
                GameObject TopCard = DrawCards.CurrentPlayedCard;
                CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
                // Test the each card to identify playable cards.
                for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
                {
                    GameObject Card = DrawCards.PlayerCards[i];
                    CardProperties CardProperties = Card.GetComponent<CardProperties>();
                    CardProperties.HasAuthority = true;
                    if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
                    {
                        Card.GetComponent<DragDrop>().canPlayCard = true;
                    }
                }

                // If the player doesn't have any playable cards, draw one from the deck
                if (HasPlayableCard(DrawCards.PlayerCards, TopCardProperties) == false)
                {
                    Debug.Log("Player didn't have a playable card");
                    int CardNum = DrawCards.RemainingCards.Count - 1;
                    GameObject Card = DrawCards.RemainingCards[CardNum];
                    Card.GetComponent<CardProperties>().HasAuthority = true;
                }
            }
        }
        //else
        //// Not the players turn
        //{
        //    for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
        //    {
        //        DrawCards.PlayerCards[i].GetComponent<CardProperties>().HasAuthority = false;
        //    }
        //}
    }

    public void testDrawnCard(GameObject Card)
    {
        // Test the new card to see if it can be played
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        CardProperties CardProperties = Card.GetComponent<CardProperties>();
        //if (TopCardProperties.cardType == "WildDraw4" || TopCardProperties.cardType == "Draw2")
        //{
        //    yield return new WaitForSeconds(0.5F);
        //}
        if (CardProperties.cardColor == TopCardProperties.cardColor || CardProperties.cardType == TopCardProperties.cardType || CardProperties.cardType == "Wild" || CardProperties.cardType == "WildDraw4")
        {
            // Card is playable                     
            Card.GetComponent<DragDrop>().canPlayCard = true;
            Debug.Log("Can play card");
        }
        else
        {
            // Card isn't playable, so move to the next player.
            TurnResult = "normal";
            Debug.Log("Can't play card");
            StartCoroutine(IncrementTurns());
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
                Debug.Log(Card.name + " was added to PlayableCards");
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
        if (Hand.Count <= 0)
        {
            EndGame(Hand);
        }
        else
        {
            StartCoroutine(IncrementTurns());
        }
    }

    private void WhenCardPlayed(List<GameObject> Hand, int i, string Name)
    {
        StartCoroutine(MoveCard(PlayableCards[i], Hand, DiscardArea));
        DrawCards.PlayedCards.Add(PlayableCards[i]);
        Hand.Remove(PlayableCards[i]);
        DrawCards.CurrentPlayedCard = PlayableCards[i];
        PlayableCards[i].GetComponent<CardFlipper>().Flip();
        Debug.Log(Name + ": Played " + PlayableCards[i].name);
        //gameObject.GetComponent<HighlightPlayer>().SetSelection();
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
        Debug.Log("redCards: " + redCards);
        Debug.Log("yellowCards: " + yellowCards);
        Debug.Log("greenCards: " + greenCards);
        Debug.Log("blueCards: " + blueCards);
        if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == redCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Red";
            Debug.Log("wild color set to red");
        }
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == yellowCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Yellow";
            Debug.Log("wild color set to yellow");
        }
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == greenCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Green";
            Debug.Log("wild color set to green");
        }
        else if (FindLargestNum(redCards, yellowCards, greenCards, blueCards) == blueCards)
        {
            Card.GetComponent<CardProperties>().cardColor = "Blue";
            Debug.Log("wild color set to blue");
        }
    }

    public int FindLargestNum(int num1, int num2, int num3, int num4)
    {
        int max = Math.Max(Math.Max(Math.Max(num1, num2), num3), num4);

        //// Largest among n1 and n2 
        //max = (n1 > n2 && n1 > n2 && n1 > n2) ?
        //            n1 : (n2 > n3 && n2 > n4) ?
        //                       n2 : (n3 > n4) ? n3 : n4;

        // Print the largest number 
        Debug.Log("Largest number among " + num1 + ", " + num2 + ", " + num3 + " and " + num4 + " is " + max);
        return max;
    }

    public void EndGame(List<GameObject> Hand)
    {
        if (CalculateScore(Hand) >= 500)
        {
            if (WinnerScore == PlayerScoreInt)
            {
                PlayerWon = true;
                Debug.Log("Player Won");
            }
            else
            {
                PlayerWon = false;
                Debug.Log("Player Lost");
            }
            GameOver = true;
        }
        StartCoroutine(LoadMenu(Hand));
    }

    private int CalculateScore(List<GameObject> Hand)
    {
        int roundWinner = GetOutHand(Hand);
        for (int i = 0; i < DrawCards.PlayerCards.Count; i++)
        {
            roundWinner = roundWinner + DrawCards.PlayerCards[i].GetComponent<CardProperties>().cardScore;
            Debug.Log("for loop 1");
        }
        for (int i = 0; i < DrawCards.EnemyCards1.Count; i++)
        {
            roundWinner = roundWinner + DrawCards.EnemyCards1[i].GetComponent<CardProperties>().cardScore;
            Debug.Log("for loop 2");
        }
        for (int i = 0; i < DrawCards.EnemyCards2.Count; i++)
        {
            roundWinner = roundWinner + DrawCards.EnemyCards2[i].GetComponent<CardProperties>().cardScore;
            Debug.Log("for loop 3");
        }
        for (int i = 0; i < DrawCards.EnemyCards3.Count; i++)
        {
            roundWinner = roundWinner + DrawCards.EnemyCards3[i].GetComponent<CardProperties>().cardScore;
            Debug.Log("for loop 4");
        }

        if (Turns == 0)
        {
            PlayerScoreInt = roundWinner;
        }
        else if (Turns == 1)
        {
            Enemy1ScoreInt = roundWinner;
        }
        else if (Turns == 2)
        {
            Enemy2ScoreInt = roundWinner;
        }
        else if (Turns == 3)
        {
            Enemy3ScoreInt = roundWinner;
        }

        WinnerScore = roundWinner;

        Debug.Log("Player scored: " + PlayerScoreInt);
        Debug.Log("Enemy 1 scored: " + Enemy1ScoreInt);
        Debug.Log("Enemy 2 scored: " + Enemy2ScoreInt);
        Debug.Log("Enemy 3 scored: " + Enemy3ScoreInt);

        return FindLargestNum(PlayerScoreInt, Enemy1ScoreInt, Enemy2ScoreInt, Enemy3ScoreInt);
    }

    int GetOutHand(List<GameObject> Hand)
    {
        if (Hand == DrawCards.PlayerCards)
        {
            Debug.Log("Round winner is Player");
            return PlayerScoreInt;
        }
        else if (Hand == DrawCards.EnemyCards1)
        {
            Debug.Log("Round winner is Enemy 1");
            return Enemy1ScoreInt;
        }
        else if (Hand == DrawCards.EnemyCards2)
        {
            Debug.Log("Round winner is Enemy2");
            return Enemy2ScoreInt;
        }
        else
        {
            Debug.Log("Round winner is Enemy3");
            return Enemy3ScoreInt;
        }
    }

    private IEnumerator LoadMenu(List<GameObject> Hand)
    {
        if (Hand != DrawCards.PlayerCards)
        {
            yield return new WaitForSeconds(0.6F);
        }
        else
        {
            yield return new WaitForSeconds(0.1F);
        }
        //Instantiate(MenuBackground, new Vector3(0, 0, 0), MenuBackground.transform.rotation);
        //GameObject menuInstance = GameObject.Find(MenuBackground.name + "(Clone)");
        //menuInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        if (GameOver)
        {
            //Instantiate(GameOverText, new Vector3(0, 118, 0), GameOverText.transform.rotation);
            //GameObject gameOverTextInstance = GameObject.Find("GameOverText(Clone)");
            //gameOverTextInstance.transform.SetParent(menuInstance.transform, false);
            GameOverMenu.SetActive(true);

            if (PlayerWon)
            {
                //Instantiate(PlayerWonText, new Vector3(0, 100, 0), PlayerWonText.transform.rotation);
                //GameObject playerWonTextInstance = GameObject.Find("PlayerWonText(Clone)");
                //playerWonTextInstance.transform.SetParent(menuInstance.transform, false);
                PlayerWonText.SetActive(true);
                Debug.Log("Player Won");
            }
            else
            {
                //Instantiate(PlayerLostText, new Vector3(0, 100, 0), PlayerLostText.transform.rotation);
                //GameObject playerLostTextInstance = GameObject.Find("PlayerLostText(Clone)");
                //playerLostTextInstance.transform.SetParent(menuInstance.transform, false);
                PlayerLostText.SetActive(true);
                Debug.Log("Player Lost");
            }

            //Instantiate(MainMenuButton, new Vector3(-70, -100, 0), MainMenuButton.transform.rotation);
            //GameObject mainMenuButtonInstance = GameObject.Find("MainMenuButton(Clone)");
            //mainMenuButtonInstance.transform.SetParent(menuInstance.transform, false);

            //Instantiate(NewGameButton, new Vector3(70, -100, 0), NewGameButton.transform.rotation);
            //GameObject newGameButtonInstance = GameObject.Find("NewGameButton(Clone)");
            //newGameButtonInstance.transform.SetParent(menuInstance.transform, false);
        }
        else
        {
            //Instantiate(RoundFinishedText, new Vector3(0, 118, 0), RoundFinishedText.transform.rotation);
            //GameObject roundFinishedTextInstance = GameObject.Find("RoundFinishedText(Clone)");
            //roundFinishedTextInstance.transform.SetParent(menuInstance.transform, false);

            //Instantiate(NextRoundButton, new Vector3(0, -100, 0), NextRoundButton.transform.rotation);
            //GameObject nextRoundButtonInstance = GameObject.Find("NextRoundButton(Clone)");
            //nextRoundButtonInstance.transform.SetParent(menuInstance.transform, false);
            RoundFinishedMenu.SetActive(true);
        }
        GameObject scoreParent = GameObject.Find("Menus");

        Instantiate(PlayerScore, new Vector3(-1.5F, 25, 20), PlayerScore.transform.rotation);
        GameObject playerScoreInstance = GameObject.Find("PlayerScore(Clone)");
        playerScoreInstance.transform.SetParent(scoreParent.transform, false);
        playerScoreInstance.GetComponent<Text>().text = "Player Scored: " + PlayerScoreInt.ToString();
        Debug.Log("Loaded player score");

        Instantiate(Enemy1Score, new Vector3(-12, -12, 20), Enemy1Score.transform.rotation);
        GameObject enemy1ScoreInstance = GameObject.Find("Enemy1Score(Clone)");
        enemy1ScoreInstance.transform.SetParent(scoreParent.transform, false);
        enemy1ScoreInstance.GetComponent<Text>().text = "Enemy 1 Scored: " + Enemy1ScoreInt.ToString();
        Debug.Log("Loaded enemy 1 score");

        Instantiate(Enemy2Score, new Vector3(-12, -49, 20), Enemy2Score.transform.rotation);
        GameObject enemy2ScoreInstance = GameObject.Find("Enemy2Score(Clone)");
        enemy2ScoreInstance.transform.SetParent(scoreParent.transform, false);
        enemy2ScoreInstance.GetComponent<Text>().text = "Enemy 2 Scored: " + Enemy2ScoreInt.ToString();
        Debug.Log("Loaded enemy 2 score");

        Instantiate(Enemy3Score, new Vector3(-12, -86, 20), Enemy3Score.transform.rotation);
        GameObject enemy3ScoreInstance = GameObject.Find("Enemy3Score(Clone)");
        enemy3ScoreInstance.transform.SetParent(scoreParent.transform, false);
        enemy3ScoreInstance.GetComponent<Text>().text = "Enemy 3 Scored: " + Enemy3ScoreInt.ToString();
        Debug.Log("Loaded enemy 3 score");
    }

    // This method determines what the current player will do
    private IEnumerator DeterminePlay(List<GameObject> enemyHand, GameObject enemyArea, string enemyName)
    {
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        Debug.Log("Top card: " + TopCard);
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
            yield return new WaitForSeconds(3);
            TurnResult = "normal";
            StartCoroutine(IncrementTurns());
        }
        else
        {
            if (HasPlayableCard(enemyHand, TopCardProperties) == true)
            {
                PlayCard(enemyHand, TopCardProperties, enemyName);
                Debug.Log("HasPlayableCard = true");
            }
            else
            {
                Debug.Log("HasPlayableCard = false");
                yield return StartCoroutine(DrawCardsFromDeck(enemyHand, enemyArea, enemyName, 1));
                yield return new WaitForSeconds(2.5F);
                PlayCard(enemyHand, TopCardProperties, enemyName);
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
            Debug.Log("Top Deck Card:" + Card.name);
            StartCoroutine(MoveCard(Card, DrawCards.RemainingCards, Area));
            if (Area == PlayerArea)
            {
                Card.GetComponent<CardFlipper>().Flip();
            }
            Hand.Add(Card);
            Debug.Log(Name + ": Drew " + Card.name);
        }
        Debug.Log(Name + ": Drew " + NumberOfCards + " cards");
    }

    // This method activates the next player
    public IEnumerator IncrementTurns()
    {
        //if (TurnResult == "Skip")
        //{
        //    yield return new WaitForSeconds(0);
        //}
        //else if (TurnResult == "Draw2")
        //{
        //    yield return new WaitForSeconds(5);
        //}
        //if (TurnResult == "WildDraw4")
        //{
        //    yield return new WaitForSeconds(4);
        //}
        //else
        //{
        yield return new WaitForSeconds(2);
        //}
        CleanupDiscardArea();
        ReshuffleDeck();
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
        Debug.Log("Turns: " + Turns);

        PlayerManager();

        if (GameOver == false)
        {
            if (isWildMenuShown == false)
            {
                if (Turns == 1)
                {
                    StartCoroutine(DeterminePlay(DrawCards.EnemyCards1, EnemyArea1, "Enemy 1"));
                }
                if (Turns == 2)
                {
                    StartCoroutine(DeterminePlay(DrawCards.EnemyCards2, EnemyArea2, "Enemy 2"));
                }
                if (Turns == 3)
                {
                    StartCoroutine(DeterminePlay(DrawCards.EnemyCards3, EnemyArea3, "Enemy 3"));
                }
            }
        }
        //gameObject.GetComponent<HighlightPlayer>().SetSelection();
    }

    IEnumerator MoveCard(GameObject card, List<GameObject> listFrom, GameObject areaTo)
    {
        iS = 0;
        while (iS < 1)
        {
            yield return new WaitForSeconds(secRate);
            if (areaTo == EnemyArea1 || areaTo == EnemyArea3)
            {
                // Cards sent to EnemyArea1 and EnemyArea3 should be rotated.
                StartCoroutine(DrawCards.MoveTo(areaTo, card, true, rate));
            }
            else if (areaTo == EnemyArea2 || areaTo == PlayerArea)
            {
                // Cards sent to EnemyArea2 and PlayerArea should not be rotated.
                StartCoroutine(DrawCards.MoveTo(areaTo, card, false, rate));
            }
            else
            {
                // This is for playing cards on the discard pile
                // If the card is rotated (isSideways) we need to rotate it again
                StartCoroutine(DrawCards.MoveTo(areaTo, card, /*card.GetComponent<CardProperties>().isSideways*/ false, rate));
                if (listFrom == DrawCards.EnemyCards1)
                {
                    card.transform.Rotate(0, 0, 90);
                }
                else if (listFrom == DrawCards.EnemyCards2)
                {
                    card.transform.Rotate(0, 0, 180);
                }
                else if (listFrom == DrawCards.EnemyCards3)
                {
                    card.transform.Rotate(0, 0, -90);
                }
            }
            iS++;
        }
        listFrom.Remove(card);
        yield return new WaitForSeconds(secRate);
    }
}
