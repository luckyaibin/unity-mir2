using System;
using Client.MirObjects;

//诱惑之光
public class ElectricShockBuilder : TwoSectionSpellBuilder
{
    public override Spell getSpell()
    {
        return Spell.ElectricShock;
    }

    public override Frame magicHitFrame()
    {
        return new Frame(1570, 10, 0, 100);
    }

    public override Frame magicSpellFrame()
    {
        return new Frame(1560, 10, 0, spellFrameTime / 10);
    }
}
