using System;
using System.Collections.Generic;

public class AnimType
{
    public const int NPC = 0;
    public const int MONSTER = 1;



    public static List<MirDirection> forkAnimMirDirection(int animType)
    {
        switch (animType)
        {
            case NPC:
                return forkAnimMirDirectionNPC();
            case MONSTER:
                return forkAnimMirDirectionMonster();
        }
        return new List<MirDirection>();
    }



    private static List<MirDirection> forkAnimMirDirectionMonster()
    {
        List<MirDirection> directions = new List<MirDirection>();
        for (var i = MirDirection.Up; i <= MirDirection.UpLeft; i++)
            directions.Add(i);
        return directions;
    }



    private static List<MirDirection> forkAnimMirDirectionNPC()
    {
        List<MirDirection> directions = new List<MirDirection>();
        for (var i = MirDirection.Up; i <= MirDirection.Right; i++)
            directions.Add(i);
        return directions;
    }
}






