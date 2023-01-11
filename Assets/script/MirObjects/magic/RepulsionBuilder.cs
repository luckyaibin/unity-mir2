using System.Collections;
using System.Collections.Generic;
using Assets.script.Mir.map;
using mir.objects.magic.builder;
using UnityEngine;
namespace mir.objects.magic
{
    public class RepulsionBuilder : OneSectionBuilder
    {
        private static readonly string ThunderBolt_INFO_PATH = MapConfigs.Data_Dir + "magic/Repulsion/Repulsion.info";
        private static readonly string ThunderBolt_Animator_PATH = "mir/data/magic/Repulsion/anim/Repulsion";
        public static Vector2 offset = readOffsets(ThunderBolt_INFO_PATH)[0];

        public override string getAnimatorControllerPath()
        {
            return ThunderBolt_Animator_PATH;
        }

        public override Vector2 getOffset()
        {
            return offset;
        }

        public override Spell GetSpell()
        {
            return Spell.Repulsion;
        }
    }
}