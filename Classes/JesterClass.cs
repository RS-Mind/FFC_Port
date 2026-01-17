using ClassesManagerReborn;
using System.Collections;

namespace Ported_FFC.Classes
{
    class JesterClass : ClassHandler
    {
        public override IEnumerator Init()
        {
            ClassesRegistry.Register(CardHolder.cards["Jester"], CardType.Entry);
            ClassesRegistry.Register(CardHolder.cards["Joke's On You!"], CardType.Card, CardHolder.cards["Jester"]);
            ClassesRegistry.Register(CardHolder.cards["King of Fools"], CardType.Card, CardHolder.cards["Jester"], 2);
            ClassesRegistry.Register(CardHolder.cards["Way of the Jester"], CardType.Card, CardHolder.cards["Jester"]);
            ClassesRegistry.Register(CardHolder.cards["Wildcard"], CardType.Card, CardHolder.cards["Jester"]);
            yield return null;
        }
    }
}