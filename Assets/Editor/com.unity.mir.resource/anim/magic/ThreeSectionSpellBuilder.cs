using System;
using System.Collections.Generic;
using Client.MirGraphics;
using Client.MirObjects;
using UnityEngine;

public abstract class ThreeSectionSpellBuilder : TwoSectionSpellBuilder
{



    public override void build()
    {
        var lib = getMagicLib();
        var imageInfos = new List<MImage>();
        var spellFrame = magicSpellFrame();
        var tmp = getImageInfoByFrame(spellFrame, lib);
        imageInfos.AddRange(tmp);

        var moveFrames = magicMoveFrame();
        foreach (var frame in moveFrames)
        {
            tmp = getImageInfoByFrame(frame.Item2, lib);
            imageInfos.AddRange(tmp);
        }

        var hitFrame = magicHitFrame();
        tmp = getImageInfoByFrame(hitFrame, lib);
        imageInfos.AddRange(tmp);

        var imageInfoArray = imageInfos.ToArray();
        var offset = lib.alignmentOffset(imageInfoArray);


        saveImageByFrame(spellFrame, lib, offset, getSpellSaveDir());
        foreach (var frame in moveFrames)
        {
            saveImageByFrame(frame.Item2, lib, offset, getSpellSaveDir());
        }
        saveImageByFrame(hitFrame, lib, offset, getSpellSaveDir());
        var magicSpellClip = createAnimationClipByFrame(spellFrame, getSpellSaveDir(), "magic_spell");

        var magicMovesClip = new List<Tuple<MirSpellAction, AnimationClip>>();
        foreach (var frame in moveFrames)
        {
            var tmpClip = createAnimationClipByFrame(frame.Item2, getSpellSaveDir(), "magic_move_" + frame.Item1.ToString());
            magicMovesClip.Add(Tuple.Create(frame.Item1, tmpClip));
        }
        var magicHitClip = createAnimationClipByFrame(hitFrame, getSpellSaveDir(), "magic_hit");

        buildAnimationController(magicSpellClip, magicMovesClip, magicHitClip, getSpellSaveDir() + "/anim", getSpell().ToString());

        lib.close();
        saveOffsets(new List<Vector2Int>() {
            offset
        }, getSpellSaveDir() + "/" + getSpell().ToString() + ".info");
    }





    public abstract List<Tuple<MirSpellAction, Frame>> magicMoveFrame();
}
