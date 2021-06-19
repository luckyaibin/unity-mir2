using System;
using DG.Tweening;
using ServerPackets;
using UnityEngine;

public class PlayerController : MirBaseController
{


    private static HairObjectBuilder hairObjectBuilder = new HairObjectBuilder();
    private static WeaponObjectBuilder wenponObjectBuilder = new WeaponObjectBuilder();

    public ObjectPlayer objectPlayer;

    private Animator hairAnimator;

    private Animator weaponAnimator;

    protected override void onStart()
    {
        playAnim(animator, MirAction.Standing, objectPlayer.Direction);
        var hairGameObject = hairObjectBuilder.gameObject(objectPlayer, this.gameObject.transform);
        hairAnimator = hairGameObject.GetComponent<Animator>();
        playAnim(hairAnimator, MirAction.Standing, objectPlayer.Direction);


        var weaponGameObject = wenponObjectBuilder.gameObject(objectPlayer, this.gameObject.transform);
        weaponAnimator = weaponGameObject.GetComponent<Animator>();
        playAnim(weaponAnimator, MirAction.Standing, objectPlayer.Direction);

    }



    private void playAnim(Animator animator, MirAction mirAction, MirDirection mirDirection)
    {
        var stateName = mirAction.ToString() + "_" + objectPlayer.Direction.ToString();
        animator.SetInteger(Mir_Direction, (int)mirDirection);
        animator.SetInteger(Mir_Action, (int)mirAction);

        //animator.Play(stateName, 0);
    }

    public void playAnim(MirAction mirAction, MirDirection mirDirection)
    {
        playAnim(hairAnimator, mirAction, mirDirection);
        playAnim(weaponAnimator, mirAction, mirDirection);
        chageSortingOrder(mirDirection);
        playAnim(animator, mirAction, mirDirection);
    }


    private void chageSortingOrder(MirDirection mirDirection)
    {
        var index = animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        weaponAnimator.gameObject.GetComponent<SpriteRenderer>().sortingOrder = wenponObjectBuilder.calcSortingOrder(index, mirDirection);
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
        objectNameSize = objectNameStyle.CalcSize(new GUIContent(objectPlayer.Name));
    }



    protected override void drawObjectName(float x, float y)
    {
        objectNameStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(x + (48 - objectNameSize.x) / 2,
              y - (32 - objectNameSize.y) / 2,
              objectNameSize.x,
              objectNameSize.y), objectPlayer.Name, objectNameStyle);
    }



    protected override Vector2 getObjectOffset()
    {
        return PlayerObjectBuilder.playerOffsets[objectPlayer.Armour];
    }







    public void objectRun(ObjectRun objectRun, PlayerObjectBuilder playerObjectBuilder)
    {
        var targetPosition = playerObjectBuilder.calcPosition(objectRun.Location, getObjectOffset());
        this.gameObject.transform.DOMove(targetPosition, 0.6f)
        .SetUpdate(true)
        .SetEase(Ease.Linear);
        playAnim(MirAction.Running, objectRun.Direction);
    }

    public void objectWalk(ObjectWalk objectWalk, PlayerObjectBuilder playerObjectBuilder)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)objectWalk.Location.y + 1000;
        var targetPosition = playerObjectBuilder.calcPosition(objectWalk.Location, getObjectOffset());
        this.gameObject.transform.DOMove(targetPosition, 0.6f)
        .SetUpdate(true)
        .SetEase(Ease.Linear);
        playAnim(MirAction.Walking, objectWalk.Direction);
    }
}
