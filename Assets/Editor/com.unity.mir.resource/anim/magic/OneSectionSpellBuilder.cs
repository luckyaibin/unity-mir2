using System;
using System.Collections.Generic;
using Client.MirGraphics;
using Client.MirObjects;
using mir.objects.magic;
using mir.objects.magic.builder;
using UnityEngine;
//一个动画的技能
public abstract class OneSectionBuilder : MirSpellBuilder
{
    public override void build()
    {
        var lib = getMagicLib();
        var imageInfos = new List<MImage>();
        var spellFrame = magicSpellFrame();
        var tmp = getImageInfoByFrame(spellFrame, lib);
        imageInfos.AddRange(tmp);
        var imageInfoArray = imageInfos.ToArray();
        var offset = lib.alignmentOffset(imageInfoArray);
        saveImageByFrame(spellFrame, lib, offset, getSpellSaveDir());
        var magicSpellClip = createAnimationClipByFrame(spellFrame, getSpellSaveDir(), SpellBuilder.magic_spell);
        buildAnimationController(magicSpellClip, getSpellSaveDir() + "/anim", getSpell().ToString());

        lib.close();
        saveOffsets(new List<Vector2Int>() {
            offset
        }, getSpellSaveDir() + "/" + getSpell().ToString() + ".info");
    }



    public abstract Frame magicSpellFrame();


}
