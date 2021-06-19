using System.Collections;
using System.Collections.Generic;
using Assets.script.Mir.map;
using mir.objects.magic.builder;
using UnityEngine;
namespace mir.objects.magic
{
    public class ElectricShockBuilder : TwoSectionBuilder
    {
        private static readonly string ElectricShock_INFO_PATH = MapConfigs.MAP_Data + "magic/ElectricShock/ElectricShock.info";
        private static readonly string ElectricShock_Animator_PATH = "mir/data/magic/ElectricShock/anim/ElectricShock";
        public static Vector2 offset = readOffsets(ElectricShock_INFO_PATH)[0];



        public override string getAnimatorControllerPath()
        {
            return ElectricShock_Animator_PATH;
        }

        public override Vector2 getOffset()
        {
            return offset;
        }

        public override Spell GetSpell()
        {
            return Spell.ElectricShock;
        }
    }
}