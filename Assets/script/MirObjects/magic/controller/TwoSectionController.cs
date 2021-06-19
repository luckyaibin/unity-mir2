using System.Collections;
using System.Collections.Generic;
using mir.objects.magic.builder;
using UnityEngine;

namespace mir.objects.magic.controller
{
    public class TwoSectionController : OneSectionController
    {
        public override void clipCallback(string input)
        {
            if (input.Equals(SpellBuilder.magic_spell))
            {
                gameObject.transform.position = spellBuilder.calcPosition(targetPosition, spellBuilder.getOffset());
                animator.SetInteger(SpellBuilder.spell, (int)spellBuilder.GetSpell());
                animator.SetInteger(SpellBuilder.spell_action, (int)MirSpellAction.hit);
            }
            else if (input.Equals(SpellBuilder.magic_hit))
            {
                Destroy(gameObject);
            }
        }
    }
}