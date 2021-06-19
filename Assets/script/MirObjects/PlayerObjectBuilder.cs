using System;
using System.Collections.Generic;
using Assets.script.Mir.map;
using script.mir.objects;
using ServerPackets;
using UnityEngine;

public class PlayerObjectBuilder : MirObjectBuilder<ObjectPlayer>
{

    private static readonly string PLAYER_RES_DIR = "mir/Data/CArmour/";

    private static readonly string PLAYER_POFFSET_INFO_PATH = MapConfigs.MAP_Data + "CArmour/cArmour.info";

    public static List<Vector2Int> playerOffsets = readOffsets(PLAYER_POFFSET_INFO_PATH);






    public override GameObject gameObject(ObjectPlayer objectPlayer)
    {
        var npcPrefab = getPrefab("prefabs/npc");

        var anim = npcPrefab.GetComponent<Animator>();
        var npcResIndex = objectPlayer.Armour.ToString("00");
        var runtimeAnimatorControllerPath = PLAYER_RES_DIR + npcResIndex + "/anim/" + npcResIndex;
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(runtimeAnimatorControllerPath);



        var mirGameObject = GameObject.Instantiate(npcPrefab, calcPosition(objectPlayer.Location, playerOffsets[objectPlayer.Armour]), Quaternion.identity);
        var animController = mirGameObject.AddComponent<PlayerController>();
        animController.objectPlayer = objectPlayer;
        mirGameObject.name = "objectPlayer(" + objectPlayer.Name + ")";
        mirGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "map_front";
        mirGameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)objectPlayer.Location.y + 1000;
        mirGameObject.layer = 10;
        return mirGameObject;
    }
}
