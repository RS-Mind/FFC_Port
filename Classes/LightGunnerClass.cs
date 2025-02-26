using ClassesManagerReborn;
using System.Collections;

namespace Ported_FFC.Classes
{
    class LightGunnerClass : ClassHandler
    {
        public override IEnumerator Init()
        {
            ClassesRegistry.Register(CardHolder.cards["Light Gunner"], CardType.Entry);
            ClassesRegistry.Register(CardHolder.cards["Fast Mags"],  CardType.Card, CardHolder.cards["Light Gunner"]);
            ClassesRegistry.Register(CardHolder.cards["Battle Experience"], CardType.Card, CardHolder.cards["Light Gunner"]);
            ClassesRegistry.Register(CardHolder.cards["LMG"], CardType.Card, CardHolder.cards["Light Gunner"]);
            ClassesRegistry.Register(CardHolder.cards["DMR"], CardType.Card, CardHolder.cards["Light Gunner"]);
            ClassesRegistry.Register(CardHolder.cards["Assault Rifle"], CardType.Card, CardHolder.cards["Light Gunner"]);
            yield return null;
        }

        public override IEnumerator PostInit()
        {
            ClassesRegistry.Get(CardHolder.cards["LMG"]).Blacklist(CardHolder.cards["DMR"]).Blacklist(CardHolder.cards["Assault Rifle"]);
            ClassesRegistry.Get(CardHolder.cards["DMR"]).Blacklist(CardHolder.cards["LMG"]).Blacklist(CardHolder.cards["Assault Rifle"]);
            ClassesRegistry.Get(CardHolder.cards["Assault Rifle"]).Blacklist(CardHolder.cards["DMR"]).Blacklist(CardHolder.cards["LMG"]);
            yield break;
        }
    }
}