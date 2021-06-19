using System;
using System.Collections.Generic;
using Assets.script.Mir.map;
using script.mir.objects;
using ServerPackets;
using UnityEngine;

public class WeaponObjectBuilder : MirObjectBuilder<ObjectPlayer>
{
    private static readonly string Weapon_RES_DIR = "mir/Data/CWeapon/";

    private static readonly string Weapon_POFFSET_INFO_PATH = MapConfigs.MAP_Data + "CWeapon/CWeapon.info";

    public static List<Vector2Int> weaponOffsets = readOffsets(Weapon_POFFSET_INFO_PATH);

    public override GameObject gameObject(ObjectPlayer objectPlayer, Transform parent)
    {
        var npcPrefab = getPrefab("prefabs/npc");

        var anim = npcPrefab.GetComponent<Animator>();
        var npcResIndex = objectPlayer.Weapon.ToString("00");
        var runtimeAnimatorControllerPath = Weapon_RES_DIR + npcResIndex + "/anim/" + npcResIndex;
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(runtimeAnimatorControllerPath);
        var mirGameObject = UnityEngine.Object.Instantiate(npcPrefab, parent);
        var offset = weaponOffsets[objectPlayer.Weapon];
        //  mirGameObject.transform.localPosition = new Vector3(0, 0, 0);
        mirGameObject.transform.position = calcPosition(objectPlayer.Location, offset);
        mirGameObject.name = "weapon";
        mirGameObject.GetComponent<SpriteRenderer>().sortingOrder = calcSortingOrder((int)objectPlayer.Location.y + 1000, objectPlayer.Direction);
        return mirGameObject;
    }


    public int calcSortingOrder(int sortingOrderParent, MirDirection direction)
    {
        if (direction == MirDirection.DownLeft ||
            direction == MirDirection.Left ||
            direction == MirDirection.UpLeft)
        {
            return sortingOrderParent - 1;
        }
        else if (direction == MirDirection.DownRight ||
          direction == MirDirection.Right ||
          direction == MirDirection.UpRight)
        {
            return sortingOrderParent + 1;
        }
        return sortingOrderParent;
    }
}
