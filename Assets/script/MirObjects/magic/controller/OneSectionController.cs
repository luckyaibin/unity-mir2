using System.Collections;
using System.Collections.Generic;
using mir.objects.magic.builder;
using UnityEngine;
namespace mir.objects.magic.controller
{
    public class OneSectionController : MonoBehaviour
    {
        public Vector2 targetPosition;

        public Vector2 selfPostion;

        protected Animator animator;

        public SpellBuilder spellBuilder;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            var clips = animator.runtimeAnimatorController.animationClips;

            foreach (var clip in clips)
            {
                if (clip.name.Equals(SpellBuilder.magic_spell) || clip.name.Equals(SpellBuilder.magic_hit))
                {
                    var animevent = new AnimationEvent
                    {
                        time = clip.length,
                        functionName = "clipCallback",
                        stringParameter = clip.name
                    };
                    clip.AddEvent(animevent);
                }
            }
        }



        public virtual void clipCallback(string input)
        {
            if (input.Equals(SpellBuilder.magic_spell))
            {
                Destroy(gameObject);
            }
        }
    }
}