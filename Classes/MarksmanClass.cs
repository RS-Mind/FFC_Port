using ClassesManagerReborn;
using System.Collections;

// Handles class setup for Astronomer

namespace Ported_FFC.Classes
{
    class AstronomerClass : ClassHandler
    {
        public override IEnumerator Init()
        {
            ClassesRegistry.Register(CardHolder.cards["Marksman"],         CardType.Entry);
            ClassesRegistry.Register(CardHolder.cards["Sabot Rounds"], CardType.Gate, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["AP Rounds"],  CardType.Gate, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["Barrett .50 Cal"], CardType.Gate, new CardInfo[] { CardHolder.cards["AP Rounds"], CardHolder.cards["Sabot Rounds"] });
            ClassesRegistry.Register(CardHolder.cards["Extended Rifle Mag"], CardType.Card, CardHolder.cards["Barrett .50 Cal"]);
            yield return null;
        }
    }
}