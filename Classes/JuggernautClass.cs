using ClassesManagerReborn;
using System.Collections;

namespace Ported_FFC.Classes
{
    class JuggernautClass : ClassHandler
    {
        public override IEnumerator Init()
        {
            ClassesRegistry.Register(CardHolder.cards["Juggernaut"], CardType.Entry);
            ClassesRegistry.Register(CardHolder.cards["Conditioning"], CardType.Card, CardHolder.cards["Juggernaut"]);
            ClassesRegistry.Register(CardHolder.cards["Size Matters"], CardType.Card, CardHolder.cards["Juggernaut"]);
            ClassesRegistry.Register(CardHolder.cards["Armor Plating"], CardType.Card, CardHolder.cards["Juggernaut"]);
            ClassesRegistry.Register(CardHolder.cards["Steroids"], CardType.Card, CardHolder.cards["Juggernaut"], 6);
            yield return null;
        }
    }
}