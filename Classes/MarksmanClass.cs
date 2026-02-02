using ClassesManagerReborn;
using System.Collections;

// Handles class setup for Astronomer

namespace Ported_FFC_Classic.Classes
{
    class AstronomerClass : ClassHandler
    {
        public override IEnumerator Init()
        {
            ClassesRegistry.Register(CardHolder.cards["Marksman"],         CardType.Entry);
            ClassesRegistry.Register(CardHolder.cards["Armor-Piercing Rounds"],  CardType.Card, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["Barret .50 Cal"], CardType.Gate, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["Sniper Rifle Extended Mag"], CardType.Card, CardHolder.cards["Barrett .50 Cal"]);
            yield return null;
        }
    }
}