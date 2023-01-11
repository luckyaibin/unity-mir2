using System.Collections;
using System.Collections.Generic;
using Assets.script.Mir.map;
using mir.objects.magic.builder;
using UnityEngine;
namespace mir.objects.magic
{
    public class GreatFireBallBuilder : ThreeSectionBuilder
    {

        private static readonly string FireBall_INFO_PATH = MapConfigs.Data_Dir + "magic/GreatFireBall/GreatFireBall.info";
        private static readonly string FireBall_Animator_PATH = "mir/data/magic/GreatFireBall/anim/GreatFireBall";
        public static Vector2 offset = readOffsets(FireBall_INFO_PATH)[0];



        public override string getAnimatorControllerPath()
        {
            return FireBall_Animator_PATH;
        }

        public override Vector2 getOffset()
        {
            return offset;
        }

        public override Spell GetSpell()
        {
            return Spell.GreatFireBall;
        }
    }
}