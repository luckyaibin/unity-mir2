using System.Collections;
using System.Collections.Generic;
using Assets.script.Mir.log;
using Assets.script.Mir.map;
using DG.Tweening;
using ServerPackets;
using UnityEngine;

public class MonsterController : MirBaseController
{

    public ObjectMonster objectMonster;





    protected override void onStart()
    {
        animator.SetInteger(Mir_Direction, (int)objectMonster.Direction);
        animator.SetInteger(Mir_Action, (int)MirAction.Standing);
        var stateName = MirAction.Standing.ToString() + "_" + objectMonster.Direction.ToString();
        animator.Play(stateName, 0);
        // dealEffect();
    }

    protected override Vector2 getObjectOffset()
    {
        return MonsterObjectBuilder.monsterOffsets[(int)objectMonster.Image];

    }

    protected override void drawObjectName(float x, float y)
    {
        objectNameStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(x + (48 - objectNameSize.x) / 2,
              y - (32 - objectNameSize.y) / 2,
              objectNameSize.x,
              objectNameSize.y), objectMonster.Name, objectNameStyle);
    }

    protected override void calcNameSize()
    {
        if (objectNameStyle != null)
        {
            return;
        }
        objectNameStyle = new GUIStyle()
        {
            fontSize = 12
        };
        objectNameSize = objectNameStyle.CalcSize(new GUIContent(objectMonster.Name));
    }


    public void objectAttack(ObjectAttack p)
    {
        animator.SetInteger(Mir_Direction, (int)p.Direction);
        animator.SetInteger(Mir_Action, (int)MirAction.Standing);
        var action = MirAction.Attack1;
        switch (p.Type)
        {
            case 1:
                action = MirAction.Attack2;
                break;
            case 2:
                action = MirAction.Attack3;
                break;
            case 3:
                action = MirAction.Attack4;
                break;
        }

        //animator.SetInteger(Mir_Direction, (int)objectMonster.Direction);
        //animator.SetInteger(Mir_Action, (int)action);

        //LogUtil.log("MonsterController", "" + action + " " + objectMonster.Name);
        var stateName = action.ToString() + "_" + p.Direction.ToString();
        animator.Play(stateName, 0);


        // dealEffect();
    }


    public void objectRun(ObjectRun objectRun)
    {


    }


    public override void clipCallback(string input)
    {
        //animator.SetInteger(Mir_Direction, (int)objectMonster.Direction);
        //animator.SetInteger(Mir_Action, (int)MirAction.Standing);
        //var stateName = MirAction.Standing.ToString() + "_" + objectMonster.Direction.ToString();
        //animator.Play(stateName, 0);
    }

    public void objectWalk(ObjectWalk objectWalk, MonsterObjectBuilder monsterObjectBuilder)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)objectWalk.Location.y + 1000;
        animator.SetInteger(Mir_Direction, (int)objectWalk.Direction);
        animator.SetInteger(Mir_Action, (int)MirAction.Standing);
        var targetPosition = monsterObjectBuilder.calcPosition(objectWalk.Location, getObjectOffset());
        this.gameObject.transform.DOMove(targetPosition, 0.7f, true);
        var stateName = MirAction.Walking.ToString() + "_" + objectWalk.Direction.ToString();
        animator.Play(stateName, 0);
    }


    private GameObject effect;



    private void dealEffect()
    {
        if (objectMonster.Image == Monster.Scarecrow && effect == null)
        {

            var go = Resources.Load<GameObject>("prefabs/npc");

            effect = Instantiate(go, this.gameObject.transform);

            effect.transform.localPosition = new Vector3(0, 0, 0);
            var animator = effect.GetComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("mir/Data/Monster/005/anim/005_effect");
            effect.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("materials/blend_add");
            animator.SetInteger(Mir_Direction, 1);
            animator.SetInteger(Mir_Action, 1);
        }

    }
}
