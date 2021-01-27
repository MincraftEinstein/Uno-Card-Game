using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartaCardGame
{
    public class WildButton : MonoBehaviour
    {
        public GameObject WildMenu;

        public void OnClick()
        {
            string ColorSelected = gameObject.name;
            ColorSelected = ColorSelected.Replace("Button", "");
            DrawCards.CurrentPlayedCard.GetComponent<CardProperties>().cardColor = ColorSelected;
            OutputLog.WriteToOutput("Player called " + ColorSelected);
            Destroy(WildMenu);
            TurnManager.isWildMenuShown = false;
            OutputLog.WriteToOutput("Wild Menu hidden");
        }
    }
}