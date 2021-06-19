using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace mir.objects.magic.builder
{
    public abstract class SpellBuilder : BaseBuilder
    {

        public static readonly string magic_spell = "magic_spell";
        public static readonly string magic_move = "magic_move";
        public static readonly string magic_hit = "magic_hit";
        public static readonly string spell = "Spell";
        public static readonly string spell_action = "SpellAction";





        public abstract string getAnimatorControllerPath();
        public abstract Vector2 getOffset();

        public abstract Spell GetSpell();
    }
}