using System;
using System.Collections.Generic;
using Assets.script.Mir.map;
using script.mir.objects;
using ServerPackets;
using UnityEngine;

public class HairObjectBuilder : MirObjectBuilder<ObjectPlayer>
{
    private static readonly string HAIR_RES_DIR = "mir/Data/CHair/";

    private static readonly string PLAYER_POFFSET_INFO_PATH = MapConfigs.Data_Dir + "CHair/cHair.info";

    public static List<Vector2Int> hairOffsets = readOffsets(PLAYER_POFFSET_INFO_PATH);

    public override GameObject gameObject(ObjectPlayer objectPlayer, Transform parent)
    {
        var npcPrefab = getPrefab("prefabs/npc");

        var anim = npcPrefab.GetComponent<Animator>();
        var npcResIndex = objectPlayer.Hair.ToString("00");
        var runtimeAnimatorControllerPath = HAIR_RES_DIR + npcResIndex + "/anim/" + npcResIndex;
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(runtimeAnimatorControllerPath);
        var mirGameObject = UnityEngine.Object.Instantiate(npcPrefab, parent);
        var offset = hairOffsets[objectPlayer.Hair];
        //  mirGameObject.transform.localPosition = new Vector3(0, 0, 0);
        mirGameObject.transform.position = calcPosition(objectPlayer.Location, offset);
        mirGameObject.name = "hair";
        mirGameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)objectPlayer.Location.y + 1000 + 1;
        return mirGameObject;
    }
}
