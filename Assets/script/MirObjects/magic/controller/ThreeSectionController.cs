using System;
using DG.Tweening;
using mir.objects.magic.builder;
using UnityEngine;
namespace mir.objects.magic.controller
{
    public class ThreeSectionController : TwoSectionController
    {




        public override void clipCallback(string input)
        {

            if (input.Equals(SpellBuilder.magic_spell))
            {
                var duration = Functions.MaxDistance(selfPostion, targetPosition) * 50;

                var tweener = gameObject.transform.DOMove(spellBuilder.calcPosition(targetPosition, spellBuilder.getOffset()), duration / 1000f);
                tweener.SetUpdate(false);
                tweener.SetEase(Ease.Linear);
                tweener.onComplete = moveEnd;
                var direction = Functions.Direction16(selfPostion, targetPosition);
                animator.SetInteger(SpellBuilder.spell, (int)spellBuilder.GetSpell());
                animator.SetInteger(SpellBuilder.spell_action, direction);
            }
            else if (input.Equals(SpellBuilder.magic_hit))
            {
                Destroy(gameObject);
            }


        }


        public void moveEnd()
        {
            animator.SetInteger(SpellBuilder.spell, (int)spellBuilder.GetSpell());
            animator.SetInteger(SpellBuilder.spell_action, (int)MirSpellAction.hit);
        }
    }
}