using System.Collections.Generic;
using Client.MirGraphics;
using Client.MirObjects;
using UnityEngine;

public abstract class TwoSectionSpellBuilder : OneSectionBuilder
{
    public override void build()
    {
        var lib = getMagicLib();
        var imageInfos = new List<MImage>();
        var spellFrame = magicSpellFrame();
        var tmp = getImageInfoByFrame(spellFrame, lib);
        imageInfos.AddRange(tmp);



        var hitFrame = magicHitFrame();
        tmp = getImageInfoByFrame(hitFrame, lib);
        imageInfos.AddRange(tmp);

        var imageInfoArray = imageInfos.ToArray();
        var offset = lib.alignmentOffset(imageInfoArray);


        saveImageByFrame(spellFrame, lib, offset, getSpellSaveDir());

        saveImageByFrame(hitFrame, lib, offset, getSpellSaveDir());
        var magicSpellClip = createAnimationClipByFrame(spellFrame, getSpellSaveDir(), "magic_spell");

        var magicHitClip = createAnimationClipByFrame(hitFrame, getSpellSaveDir(), "magic_hit");

        buildAnimationController(magicSpellClip, magicHitClip, getSpellSaveDir() + "/anim", getSpell().ToString());

        lib.close();
        saveOffsets(new List<Vector2Int>() {
            offset
        }, getSpellSaveDir() + "/" + getSpell().ToString() + ".info");
    }



    public abstract Frame magicHitFrame();
}
