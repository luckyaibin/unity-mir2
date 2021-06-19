﻿using System;

using System.Collections.Generic;
using Client.MirGraphics;
using Client.MirObjects;


public class FrostCrunchBuilder : ThreeSectionSpellBuilder
{

    public override Spell getSpell()
    {
        return Spell.FrostCrunch;
    }



    public override Frame magicHitFrame()
    {
        return new Frame(570, 8, 0, 60);

    }

    public override List<Tuple<MirSpellAction, Frame>> magicMoveFrame()
    {
        var frames = new List<Tuple<MirSpellAction, Frame>>();
        var start = 410;
        var count = 4;
        var skip = 6;
        for (int i = (int)MirSpellAction.Up; i < (int)MirSpellAction.UpLeft2; i++)
        {
            var tmpStart = start + (count + skip) * i;
            frames.Add(Tuple.Create((MirSpellAction)i, new Frame(tmpStart, count, 0, 30)));
        }
        return frames;
    }

    public override Frame magicSpellFrame()
    {
        return new Frame(400, 10, 0, spellFrameTime / 10);
    }
    public override MLibrary getMagicLib()
    {
        return Libraries.Magic2;
    }
}
