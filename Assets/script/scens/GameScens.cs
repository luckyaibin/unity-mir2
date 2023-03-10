using System;
using System.Collections;
using System.Collections.Generic;
using Assets.script.Mir.log;
using Assets.script.Mir.map;
using Assets.script.Mir.scens;
using Client.MirNetwork;
using script.mir.objects;
using ServerPackets;
using UnityEngine;

public class GameScens : MonoBehaviour, ProcessPacket
{
    private MapController mapController;

    private List<GameObject> monsteres = new List<GameObject>();


    private Dictionary<uint, GameObject> npcs = new Dictionary<uint, GameObject>();

    private Dictionary<uint, GameObject> monsters = new Dictionary<uint, GameObject>();

    private Dictionary<uint, GameObject> players = new Dictionary<uint, GameObject>();

    private NpcObjectBuilder npcObjectBuilder = new NpcObjectBuilder();

    private MonsterObjectBuilder monsterObjectBuilder = new MonsterObjectBuilder();

    private PlayerObjectBuilder playerObjectBuilder = new PlayerObjectBuilder();

    private void Awake()
    {
        MirNetwork.gameScens = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        mapController = gameObject.GetComponent<MapController>();


    }

    // Update is called once per frame
    void Update()
    {
        LogUtil.log("Game scene ","update...");
    }


    public void process(Packet p)
    {

        switch (p.Index)
        {
            case (short)ServerPacketIds.UserInformation:
                UserInformation((ServerPackets.UserInformation)p);
                break;
            case (short)ServerPacketIds.ObjectMonster:
                objectMonster((ServerPackets.ObjectMonster)p);
                break;
            case (short)ServerPacketIds.ObjectNpc:
                objectNpc((ServerPackets.ObjectNPC)p);
                break;
            case (short)ServerPacketIds.ObjectAttack:
                objectAttack((ObjectAttack)p);
                break;
            case (short)ServerPacketIds.ObjectRun:
                objectRun((ObjectRun)p);
                break;
            case (short)ServerPacketIds.ObjectWalk:
                objectWalk((ObjectWalk)p);
                break;

            case (short)ServerPacketIds.ObjectPlayer:
                objectPlayer((ObjectPlayer)p);
                break;
        }
    }


    private void objectPlayer(ObjectPlayer p)
    {
        p.Hair = 0;
        p.Armour = 0;
        p.Weapon = 0;
        if (!players.ContainsKey(p.ObjectID))
        {
            players[p.ObjectID] = playerObjectBuilder.gameObject(p);
        }

    }

    private void objectMonster(ObjectMonster p)
    {

        if (!monsters.ContainsKey(p.ObjectID))
        {
            monsters[p.ObjectID] = monsterObjectBuilder.gameObject(p);
        }
    }

    private void objectAttack(ServerPackets.ObjectAttack p)
    {
        if (monsters.ContainsKey(p.ObjectID))
        {
            var monsterController = monsters[p.ObjectID].GetComponent<MonsterController>();
            monsterController.objectAttack(p);
        }
    }


    private void objectRun(ObjectRun p)
    {
        GameObject gameObjectTmp;
        if (monsters.TryGetValue(p.ObjectID, out gameObjectTmp))
        {
            var controller = gameObjectTmp.GetComponent<MonsterController>();
            controller.objectRun(p);
        }
        else if (players.TryGetValue(p.ObjectID, out gameObjectTmp))
        {
            var controller = gameObjectTmp.GetComponent<PlayerController>();
            controller.objectRun(p, playerObjectBuilder);
        }



    }

    private void objectWalk(ObjectWalk p)
    {
        GameObject gameObjectTmp;
        if (monsters.TryGetValue(p.ObjectID, out gameObjectTmp))
        {
            var controller = gameObjectTmp.GetComponent<MonsterController>();
            controller.objectWalk(p, monsterObjectBuilder);
        }
        if (players.TryGetValue(p.ObjectID, out gameObjectTmp))
        {
            var controller = gameObjectTmp.GetComponent<PlayerController>();
            controller.objectWalk(p, playerObjectBuilder);
        }

    }




    private void objectNpc(ObjectNPC p)
    {
        if (!npcs.ContainsKey(p.ObjectID))
        {
            npcs[p.ObjectID] = npcObjectBuilder.gameObject(p);
        }


    }

    private void UserInformation(ServerPackets.UserInformation p)
    {
        mapController.setMapInfo(p.Location.x, p.Location.y, "0");
        // mapController.setMapInfo(345, 338, "0");
    }



    private void OnDestroy()
    {

        Destroy(gameObject.GetComponent<GameManager>());
    }




}
