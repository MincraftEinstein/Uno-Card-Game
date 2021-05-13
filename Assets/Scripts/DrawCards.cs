using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject PlayerArea;
    public GameObject EnemyArea1;
    public GameObject EnemyArea2;
    public GameObject EnemyArea3;
    public GameObject DeckZone;
    public GameObject DiscardPile;
    public GameObject Button;
    public GameObject Card;
    public static GameObject CurrentPlayedCard;

    public static List<GameObject> AllCards = new List<GameObject>();
    public static List<GameObject> RemainingCards = new List<GameObject>();

    public static List<GameObject> EnemyCards1 = new List<GameObject>();
    public static List<GameObject> EnemyCards2 = new List<GameObject>();
    public static List<GameObject> EnemyCards3 = new List<GameObject>();
    public static List<GameObject> PlayerCards = new List<GameObject>();
    public static List<GameObject> PlayedCards = new List<GameObject>();

    public float rate = 0.5F;
    public float secRate = 0.5F;
    public int iS;
    private GameObject turnManagerGO;
    public static GameObject dragObject;

    // Start is called before the first frame update
    public void Start()
    {
        //PlayerArea = GameObject.Find("PlayerArea");
        //EnemyArea1 = GameObject.Find("EnemyArea1");
        //EnemyArea2 = GameObject.Find("EnemyArea2");
        //EnemyArea3 = GameObject.Find("EnemyArea3");
        //DeckZone = GameObject.Find("DeckArea");
        //DiscardPile = GameObject.Find("DiscardArea");
        turnManagerGO = GameObject.Find("GameManager");
        dragObject = GameObject.Find("DragObject");
        CreateNewDeck(AllCards);
        Shuffle();
        onClick(true);
    }

    public void CreateNewDeck(List<GameObject> list)
    {
        list.Clear();
        list.Add(Blue_0);
        list.Add(Green_0);
        list.Add(Red_0);
        list.Add(Yellow_0);

        for (int i = 0; i < 2; i++)
        {
            list.Add(Blue_1);
            list.Add(Blue_2);
            list.Add(Blue_3);
            list.Add(Blue_4);
            list.Add(Blue_5);
            list.Add(Blue_6);
            list.Add(Blue_7);
            list.Add(Blue_8);
            list.Add(Blue_9);
            list.Add(Blue_D);
            list.Add(Blue_R);
            list.Add(Blue_S);
            //---------------------------------------
            list.Add(Green_1);
            list.Add(Green_2);
            list.Add(Green_3);
            list.Add(Green_4);
            list.Add(Green_5);
            list.Add(Green_6);
            list.Add(Green_7);
            list.Add(Green_8);
            list.Add(Green_9);
            list.Add(Green_D);
            list.Add(Green_R);
            list.Add(Green_S);
            //---------------------------------------
            list.Add(Red_1);
            list.Add(Red_2);
            list.Add(Red_3);
            list.Add(Red_4);
            list.Add(Red_5);
            list.Add(Red_6);
            list.Add(Red_7);
            list.Add(Red_8);
            list.Add(Red_9);
            list.Add(Red_D);
            list.Add(Red_R);
            list.Add(Red_S);
            //---------------------------------------
            list.Add(Yellow_1);
            list.Add(Yellow_2);
            list.Add(Yellow_3);
            list.Add(Yellow_4);
            list.Add(Yellow_5);
            list.Add(Yellow_6);
            list.Add(Yellow_7);
            list.Add(Yellow_8);
            list.Add(Yellow_9);
            list.Add(Yellow_D);
            list.Add(Yellow_R);
            list.Add(Yellow_S);
        }

        for (int i = 0; i < 4; i++)
        {
            list.Add(Wild);
            list.Add(WildDraw4);
        }

        //for (int i = 0; i < 1; i++)
        //{
        //    AllCards.Add(Red_S);
        //    AllCards.Add(Yellow_S);
        //    AllCards.Add(Green_S);
        //    AllCards.Add(Blue_S);
        //}
    }

    public void Shuffle()
    {
        List<GameObject> tmp = new List<GameObject>();

    reshuffle:
        int max = AllCards.Count;
        while (max > 0)
        {
            int offset = Random.Range(0, max);
            tmp.Add(AllCards[offset]);
            AllCards.RemoveAt(offset);
            max -= 1;
        }
        AllCards = tmp;

        // This is to make sure the discard is not a Wild card
        Card = AllCards[AllCards.Count - 29];
        if (Card.GetComponent<CardProperties>().cardType == "Wild" || Card.GetComponent<CardProperties>().cardType == "WildDraw4")
        {
            Debug.Log("Discard was WILD!!!");
            //max = AllCards.Count;
            //while (max > 0)
            //{
            //    int offset = Random.Range(0, max);
            //    tmp.Add(AllCards[offset]);
            //    AllCards.RemoveAt(offset);
            //    max -= 1;
            //}
            //AllCards = tmp;
            goto reshuffle;
        }
    }

    public void onClick(bool isFromThis)
    {
        // Destroy any remaining cards before dealing new ones.
        //for (int i = 0; i < PlayerArea.transform.childCount; i++)
        //    Destroy(PlayerArea.transform.GetChild(i).gameObject);

        //for (int i = 0; i < EnemyArea1.transform.childCount; i++)
        //    Destroy(EnemyArea1.transform.GetChild(i).gameObject);

        //for (int i = 0; i < EnemyArea2.transform.childCount; i++)
        //    Destroy(EnemyArea2.transform.GetChild(i).gameObject);

        //for (int i = 0; i < EnemyArea3.transform.childCount; i++)
        //    Destroy(EnemyArea3.transform.GetChild(i).gameObject);

        // Create the deck of cards.
        for (int i = 0; i < AllCards.Count; i++)
        {
            //Put the remainder of the deck in the DeckZone.
            Card = Instantiate(AllCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            Card.transform.SetParent(DeckZone.transform, false);
            Card.GetComponent<CardFlipper>().Flip();
            RemainingCards.Add(Card);
        }

        // Deal out the cards.
        if (isFromThis == true)
        {
            PlayerCards.Clear();
            EnemyCards1.Clear();
            EnemyCards2.Clear();
            EnemyCards3.Clear();
            StartCoroutine(DealCards());
        }
    }

    IEnumerator DealCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int iP = 0; iP < 4; iP++)  // I want this to deal out to 4 players
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
                    PlayerCards.Add(Card);
                    Debug.Log("Player received: " + Card.name);
                    Card.GetComponent<CardFlipper>().Flip();

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
                    EnemyCards1.Add(Card);
                    Debug.Log("Enemy1 received: " + Card.name);
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
                    EnemyCards2.Add(Card);
                    Debug.Log("Enemy2 received: " + Card.name);
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
                    EnemyCards3.Add(Card);
                    Debug.Log("Enemy3 received: " + Card.name);
                }
            }
        }

        Card = RemainingCards[RemainingCards.Count - 1];
        iS = 0;
        while (iS < 1)
        {
            yield return new WaitForSeconds(secRate);
            StartCoroutine(MoveTo(DiscardPile, Card, false, rate));
            iS++;
        }
        RemainingCards.Remove(Card);
        PlayedCards.Add(Card);
        CurrentPlayedCard = Card;
        Card.GetComponent<CardFlipper>().Flip();
        Debug.Log("DiscardPile received: " + Card.name);
        yield return new WaitForSeconds(secRate);

        // Gives the player authority over their cards
        for (int i = 0; i < PlayerCards.Count; i++)
        {
            PlayerCards[i].GetComponent<CardProperties>().HasAuthority = true;
        }

        //if (CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Zero"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "One" 
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Two" 
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Three" 
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Four"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Five"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Six"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Seven"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Eight"
        //    || CurrentPlayedCard.GetComponent<CardProperties>().cardType != "Nine")
        //{
        if (CurrentPlayedCard.GetComponent<CardProperties>().cardType == "Reverse")
        {
            turnManagerGO.GetComponent<TurnManager>().IsReversed = !turnManagerGO.GetComponent<TurnManager>().IsReversed;
        }

        if (CurrentPlayedCard.GetComponent<CardProperties>().cardType == "Skip"
            || CurrentPlayedCard.GetComponent<CardProperties>().cardType == "Draw2"
            || CurrentPlayedCard.GetComponent<CardProperties>().cardType == "Wild"
            || CurrentPlayedCard.GetComponent<CardProperties>().cardType == "WildDraw4")
        {
            turnManagerGO.GetComponent<TurnManager>().TurnResult = CurrentPlayedCard.GetComponent<CardProperties>().cardType;
        }

        turnManagerGO.GetComponent<HighlightPlayer>().enabled = true;
        //turnManagerGO.GetComponent<HighlightPlayer>().SetSelection();
        turnManagerGO.GetComponent<TurnManager>().CardColorImage.enabled = true;

        turnManagerGO.GetComponent<TurnManager>().PlayerManager();

        // Make the button inactive.
        Button.SetActive(false);

        Debug.Log("Dealt cards");
    }

    public static IEnumerator MoveTo(GameObject CurrentArea, GameObject theGO, bool isSideways, float time)
    {
        Debug.Log("MoveTo");
        Vector3 start = theGO.transform.position;
        Vector3 end = CurrentArea.transform.position;
        float t = 0;
        theGO.transform.SetParent(dragObject.transform, true);

        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / time;
            theGO.transform.position = Vector3.Lerp(start, end, t);
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