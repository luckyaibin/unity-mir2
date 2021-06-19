using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MirBaseController : MonoBehaviour
{

    protected static readonly string Mir_Direction = "MirDirection";
    protected static readonly string Mir_Action = "MirAction";

    protected Animator animator;
    protected List<Tuple<MirAction, MirDirection>> actions = new List<Tuple<MirAction, MirDirection>>();



    protected Vector2 objectNameSize;

    protected GUIStyle objectNameStyle;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        var clips = animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            var actionDirection = clip.name.Split('_');
            var mirAction = (MirAction)Enum.Parse(typeof(MirAction), actionDirection[0]);
            var mirDirection = (MirDirection)Enum.Parse(typeof(MirDirection), actionDirection[1]);
            actions.Add(Tuple.Create(mirAction, mirDirection));

            var animevent = new AnimationEvent
            {
                time = clip.length,
                functionName = "clipCallback",
                stringParameter = clip.name
            };
            clip.AddEvent(animevent);
        }

    }

    private void Start()
    {
        onStart();
    }


    protected abstract void onStart();



    protected abstract Vector2 getObjectOffset();




    private void OnGUI()
    {
        calcNameSize();
        var offest = getObjectOffset();
        var x = this.gameObject.transform.position.x - offest.x;
        var y = this.gameObject.transform.position.y + offest.y;

        var objectPosition = Camera.main.WorldToScreenPoint(new Vector3(x, y, 0));
        //得到真实怪物头顶的2D坐标
        drawObjectName(objectPosition.x, Screen.height - objectPosition.y);
    }

    protected abstract void drawObjectName(float x, float y);


    protected abstract void calcNameSize();



    public virtual void clipCallback(string input)
    {
    }
}
