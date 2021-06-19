using System;
using Client.MirObjects;

public class RepulsionBuilder : OneSectionBuilder
{
    public override Spell getSpell()
    {
        return Spell.Repulsion;
    }

    public override Frame magicSpellFrame()
    {
        return new Frame(900, 6, 0, spellFrameTime / 6);
    }
}
