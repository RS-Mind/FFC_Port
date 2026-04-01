using ClassesManagerReborn.Util;
using ModdingUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Utils;
using UnityEngine;

namespace Ported_FFC.Monobehaviors
{
    internal class WildCardMono : MonoBehaviour
    {
        public void Start()
        {
            PFFC.instance.ExecuteAfterFrames(5, () =>
            {
                Player player = GetComponentInParent<Player>();
                CardInfo[] availableCards = UnboundLib.Utils.CardManager.cards.Values.Where(c => c.cardInfo != CardHolder.cards["Wildcard"] && c.cardInfo != CardHolder.cards["Jester"]
                    && c.cardInfo.GetComponent<Gun>() != null && c.cardInfo.GetComponent<Gun>().reflects > 0
                    && c.enabled && ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, c.cardInfo)).Select(c => c.cardInfo).ToArray();
                availableCards.Shuffle();
                CardInfo card1 = availableCards[0];
                //CardInfo card2 = availableCards[0];
                List<CardInfo> playerCards = player.data.currentCards.ToList();
                playerCards.Add(card1);
                //do
                //{
                //    availableCards.Shuffle();
                //    card2 = availableCards[0];
                //} while (!ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card2, playerCards.ToArray()));
                //ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(player, new CardInfo[] { card1, card2 },false, null,null,null,true);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card1, false, "", 0, 0);
                Destroy(this);
            });
        }
    }
}
