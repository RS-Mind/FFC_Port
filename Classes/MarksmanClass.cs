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
            ClassesRegistry.Register(CardHolder.cards["AP Rounds"],  CardType.Card, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["Barrett .50 Cal"], CardType.Gate, CardHolder.cards["Marksman"]);
            ClassesRegistry.Register(CardHolder.cards["Extended Rifle Mag"], CardType.Card, CardHolder.cards["Barrett .50 Cal"]);
            yield return null;
        }
    }
}