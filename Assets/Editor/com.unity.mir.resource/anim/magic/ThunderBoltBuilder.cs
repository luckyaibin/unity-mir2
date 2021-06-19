using Client.MirObjects;
using Client.MirGraphics;

public class ThunderBoltBuilder : TwoSectionSpellBuilder
{


    public override Spell getSpell()
    {
        return Spell.ThunderBolt;
    }

    public override Frame magicHitFrame()
    {
        return new Frame(10, 5, 0, 80);
    }

    public override Frame magicSpellFrame()
    {
        return new Frame(20, 3, 0, 100);
    }

    public override MLibrary getMagicLib()
    {
        return Libraries.Magic2;
    }


}
