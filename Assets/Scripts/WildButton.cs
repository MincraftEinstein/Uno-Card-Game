using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildButton : MonoBehaviour
{
    public GameObject WildMenu;

    public void OnClick()
    {
        GameObject TopCard = DrawCards.CurrentPlayedCard;
        CardProperties TopCardProperties = TopCard.GetComponent<CardProperties>();
        if (gameObject.name == "RedButton")
        {
            OutputLog.WriteToOutput("Player called red");
            if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
            {
                TopCardProperties.cardColor = "Red";
            }
        }
        else if (gameObject.name == "YellowButton")
        {
            OutputLog.WriteToOutput("Player called yellow");
            if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
            {
                TopCardProperties.cardColor = "Yellow";
            }
        }
        else if (gameObject.name == "GreenButton")
        {
            OutputLog.WriteToOutput("Player called green");
            if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
            {
                TopCardProperties.cardColor = "Green";
            }
        }
        else if (gameObject.name == "BlueButton")
        {
            OutputLog.WriteToOutput("Player called blue");
            if (TopCardProperties.cardType == "Wild" || TopCardProperties.cardType == "WildDraw4")
            {
                TopCardProperties.cardColor = "Blue";
            }
        }
        Destroy(WildMenu);
        DragDrop.isWildMenuShown = false;
        OutputLog.WriteToOutput("Wild Menu hidden");
    }
}
