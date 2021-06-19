using System;
using System.Collections;
using System.Timers;
using Assets.script.Mir.log;
using Assets.script.Mir.map;
using mir.objects.magic;
using ServerPackets;
using UnityEngine;

public class TestPlayerObject : MonoBehaviour
{
    private ObjectPlayer objectPlayer;
    private PlayerObjectBuilder playerObjectBuilder;

    private PlayerController playerController;

    private MirAction mirAction = MirAction.Standing;

    private MirDirection mirDirection = MirDirection.Up;

    public GameObject testSpell;

    private void Awake()
    {
        objectPlayer = new ObjectPlayer()
        {
            Hair = 0,
            Armour = 0,
            Weapon = 0,
            Location = new Vector2(1, 1),
            Direction = MirDirection.Up,
            Name = "test"
        };
    }

    private void Start()
    {
        MapConfigs.init();
        playerObjectBuilder = new PlayerObjectBuilder();
        var tmp = playerObjectBuilder.gameObject(objectPlayer);
        playerController = tmp.GetComponent<PlayerController>();

        //var gridView = GetComponentInParent<MirDirectionGridView>();
        //gridView.testPlayerObject = this;
        // playerController.playAnim(mirAction, mirDirection);

        var tmpAnim = testSpell.GetComponent<Animator>();
        tmpAnim.SetInteger("Spell", (int)Spell.FireBall);
        tmpAnim.SetInteger("MirSpellDirection", 1);

        //doMagicTest();
        //StartCoroutine(test());
    }


    public void onDirectionClick(MirDirection direction)
    {
        this.mirDirection = direction;
        playerController.playAnim(mirAction, mirDirection);
    }


    public void onActionClick(MirAction mirAction)
    {
        this.mirAction = mirAction;
        playerController.playAnim(mirAction, mirDirection);
        if (mirAction == MirAction.Spell)
        {
            doMagicTest();
        }
    }


    private FireBallBuilder fireBallBuilder;
    private GreatFireBallBuilder greatFireBallBuilder;
    private FrostCrunchBuilder frostCrunchBuilder;
    private ThunderBoltBuilder thunderBoltBuilder;
    private RepulsionBuilder repulsionBuilder;
    private ElectricShockBuilder electricShockBuilder;




    private IEnumerator test()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            LogUtil.log("test", "-----doMagicTest------");
            doMagicTest();
        }


    }


    private void doMagicTest()
    {
        if (fireBallBuilder == null)
            fireBallBuilder = new FireBallBuilder();
        fireBallBuilder.gameObject(new Vector2(-9, 5), new Vector2(1, 1));
        if (greatFireBallBuilder == null)
            greatFireBallBuilder = new GreatFireBallBuilder();
        greatFireBallBuilder.gameObject(new Vector2(-10, 5), new Vector2(2, 2));
        if (frostCrunchBuilder == null)
            frostCrunchBuilder = new FrostCrunchBuilder();
        frostCrunchBuilder.gameObject(new Vector2(-11, 5), new Vector2(3, 3));
        if (thunderBoltBuilder == null)
            thunderBoltBuilder = new ThunderBoltBuilder();
        thunderBoltBuilder.gameObject(new Vector2(1, 1), new Vector2(-11, 5));
        if (repulsionBuilder == null)
            repulsionBuilder = new RepulsionBuilder();
        repulsionBuilder.gameObject(new Vector2(6, 6));

        if (electricShockBuilder == null)
            electricShockBuilder = new ElectricShockBuilder();
        electricShockBuilder.gameObject(new Vector2(1, 1), new Vector2(-11, 1));
    }
}



