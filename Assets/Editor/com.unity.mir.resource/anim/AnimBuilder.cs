using System;
using System.Collections.Generic;
using System.IO;
using Client.MirGraphics;
using Client.MirObjects;
using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;



public static class AnimBuilder
{
    static AnimationClip BuildAnimationClip(string[] imagePaths, string animDir, string name)
    {

        AnimationClip clip = new AnimationClip();

        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[imagePaths.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float frameTime = 1 / 10f;
        for (int i = 0; i < imagePaths.Length; i++)
        {

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(imagePaths[i]));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        //动画帧率，30比较合适
        clip.frameRate = 30;

        var clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
        clipSetting.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, clipSetting);


        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, animDir + "/" + name + ".anim");
        AssetDatabase.SaveAssets();


        return clip;
    }



    static AnimatorController BuildAnimationControllerMonster(List<Tuple<MirAction, MirDirection, AnimationClip>> clips, string animDir, string name)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animDir + "/" + name + ".controller");
        AnimatorControllerLayer layer = animatorController.layers[0];
        animatorController.AddParameter("MirAction", AnimatorControllerParameterType.Int);
        animatorController.AddParameter("MirDirection", AnimatorControllerParameterType.Int);
        AnimatorStateMachine stateMachine = layer.stateMachine;
        var states = new Dictionary<string, AnimatorState>();
        var clipsTmp = new Dictionary<string, AnimationClip>();
        foreach (var tuple in clips)
        {
            var mirAction = tuple.Item1;
            var mirDirection = tuple.Item2;
            var newClip = tuple.Item3;

            AnimatorState state = stateMachine.AddState(newClip.name);
            state.motion = newClip;
            states[newClip.name] = state;
            clipsTmp[newClip.name] = newClip;
            //AnimatorStateTransition trans = stateMachine.AddAnyStateTransition(state);
            // AnimatorTransition transition = stateMachine.AddEntryTransition(state);
            //trans.hasExitTime = true;
            //trans.hasFixedDuration = true;
            //trans.duration = 0;
            //trans.offset = 0;
            //trans.exitTime = newClip.length;
            //trans.AddCondition(AnimatorConditionMode.Equals, (float)mirAction, "MirAction");
            //trans.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");
        }

        // Attack1 ---> Standing

        foreach (var tuple in clips)
        {
            var mirAction = tuple.Item1;
            var mirDirection = tuple.Item2;
            var newClip = tuple.Item3;
            if (!mirAction.ToString().StartsWith(MirAction.Standing.ToString()) &&
                !mirAction.ToString().StartsWith(MirAction.Die.ToString()) &&
                 !mirAction.ToString().StartsWith(MirAction.Dead.ToString()))
            {
                var state = states[mirAction.ToString() + "_" + mirDirection.ToString()];
                var animatorTransition = stateMachine.AddEntryTransition(state);
                animatorTransition.AddCondition(AnimatorConditionMode.Equals, (float)mirAction, "MirAction");
                animatorTransition.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");
                var stateStanding = states[MirAction.Standing.ToString() + "_" + mirDirection.ToString()];
                var trans = state.AddTransition(stateStanding);
                trans.hasExitTime = true;
                trans.hasFixedDuration = true;
                trans.duration = 0;
                trans.offset = 0;
                trans.exitTime = 1;

                trans.AddCondition(AnimatorConditionMode.Equals, (float)MirAction.Standing, "MirAction");
                trans.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");
            }
            else if (mirAction.ToString().StartsWith(MirAction.Die.ToString()))
            {
                var state = states[mirAction.ToString() + "_" + mirDirection.ToString()];
                var animatorTransition = stateMachine.AddEntryTransition(state);
                animatorTransition.AddCondition(AnimatorConditionMode.Equals, (float)mirAction, "MirAction");
                animatorTransition.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");
                var stateDead = states[MirAction.Dead.ToString() + "_" + mirDirection.ToString()];
                var trans = state.AddTransition(stateDead);
                trans.hasExitTime = true;
                trans.hasFixedDuration = true;
                trans.duration = 0;
                trans.offset = 0;
                trans.exitTime = 1;

                trans.AddCondition(AnimatorConditionMode.Equals, (float)MirAction.Standing, "MirAction");
                trans.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");

            }
        }




        AssetDatabase.SaveAssets();
        return animatorController;
    }



    static AnimatorController BuildAnimationController(List<Tuple<MirAction, MirDirection, AnimationClip>> clips, string animDir, string name)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animDir + "/" + name + ".controller");
        AnimatorControllerLayer layer = animatorController.layers[0];
        animatorController.AddParameter("MirAction", AnimatorControllerParameterType.Int);
        animatorController.AddParameter("MirDirection", AnimatorControllerParameterType.Int);
        AnimatorStateMachine stateMachine = layer.stateMachine;

        foreach (var tuple in clips)
        {
            var mirAction = tuple.Item1;
            var mirDirection = tuple.Item2;
            var newClip = tuple.Item3;
            AnimatorState state = stateMachine.AddState(newClip.name);
            state.motion = newClip;

            AnimatorStateTransition trans = stateMachine.AddAnyStateTransition(state);

            trans.hasExitTime = false;
            trans.hasFixedDuration = false;
            trans.duration = 0;
            trans.offset = 0;
            trans.canTransitionToSelf = false;
            trans.exitTime = 1;
            trans.AddCondition(AnimatorConditionMode.Equals, (float)mirAction, "MirAction");
            trans.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");


            var transEntry = stateMachine.AddEntryTransition(state);

            transEntry.AddCondition(AnimatorConditionMode.Equals, (float)mirAction, "MirAction");
            transEntry.AddCondition(AnimatorConditionMode.Equals, (float)mirDirection, "MirDirection");
        }
        AssetDatabase.SaveAssets();
        return animatorController;
    }

    public static void testAnim()
    {
        var tmpRoot = "/Users/yangcai/Documents/unity-workspace/MirMobile/Assets/Resources/mir/Data/Map/WemadeMir2/Objects";

        var imagePaths = new string[10];
        for (int i = 0; i < 10; i++)
        {
            imagePaths[i] = tmpRoot + "/" + (i + 2723) + ".png";
        }
        buildMapAnim(imagePaths);
    }

    public static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets\\"));
        else
            return path.Substring(path.IndexOf("Assets/"));
    }

    public static void buildMapAnim(string[] imagePaths)
    {
        if (checkPathNull(imagePaths)) return;
        string parentName = Directory.GetParent(imagePaths[0]).FullName;
        var animSaveDir = parentName + "/anim";
        if (!Directory.Exists(animSaveDir))
        {
            Directory.CreateDirectory(animSaveDir);
        }
        animSaveDir = DataPathToAssetPath(animSaveDir);
        string animName = Path.GetFileNameWithoutExtension(imagePaths[0]);
        var clips = BuildAnimationClip(imagePaths, animSaveDir, animName);
        List<AnimationClip> listClips = new List<AnimationClip>();
        listClips.Add(clips);
        //        BuildAnimationController(listClips, animSaveDir, animName);

    }

    private static bool checkPathNull(string[] imagePaths)
    {
        foreach (var path in imagePaths)
        {
            if (path == null) return true;
        }
        return false;
    }


    public static void buildMonsterAnim(MLibrary monsterLib, string resPath)
    {
        buildMirAnim(monsterLib, resPath, AnimType.forkAnimMirDirection(AnimType.MONSTER), AnimType.MONSTER);
    }

    public static void buildMonsteNpc(MLibrary monsterLib, string resPath)
    {
        buildMirAnim(monsterLib, resPath, AnimType.forkAnimMirDirection(AnimType.NPC), AnimType.NPC);
    }

    public static void buildMirAnim(MLibrary mLibrary, string resPath, List<MirDirection> mirDirections, int animType)
    {
        var frames = mLibrary.Frames;
        var clips = new List<Tuple<MirAction, MirDirection, AnimationClip>>();

        var animSaveDir = resPath + "/anim";
        if (!Directory.Exists(animSaveDir))
        {
            Directory.CreateDirectory(animSaveDir);
        }
        animSaveDir = DataPathToAssetPath(animSaveDir);
        var animName = Path.GetFileNameWithoutExtension(resPath);
        foreach (var frame in frames)
        {
            var tmp = createFrameClip(frame, resPath, animSaveDir, mirDirections);
            clips.AddRange(tmp);
        }
        if (animType == AnimType.MONSTER)
        {
            BuildAnimationControllerMonster(clips, animSaveDir, animName);
        }
        else
        {
            BuildAnimationController(clips, animSaveDir, animName);
        }

    }

    private static string getSavePath(string path)
    {
        return path + "/anim";
    }




    private static List<Tuple<MirAction, MirDirection, AnimationClip>> createFrameClip(KeyValuePair<MirAction, Frame> frame, string resPath, string animSaveDir, List<MirDirection> mirDirections)
    {
        var clips = new List<Tuple<MirAction, MirDirection, AnimationClip>>();
        var mirAction = frame.Key.ToString();
        var frameInfo = frame.Value;


        foreach (var tmp in mirDirections)
        {
            int offset = frameInfo.Count + frameInfo.Skip;
            int startIndex = offset * (int)tmp + frameInfo.Start;
            var imagePath = new string[frameInfo.Count];
            for (int j = 0; j < frameInfo.Count; j++)
            {
                imagePath[j] = resPath + "/" + (startIndex + j) + ".png";

            }
            var clip = createFrameClip(frameInfo, mirAction, tmp, imagePath, animSaveDir);
            clips.Add(Tuple.Create(frame.Key, tmp, clip));

            if (clip.name.Equals(MirAction.Die.ToString() + "_" + MirDirection.Up.ToString()) &&
                resPath.Contains("005"))
            {

                startIndex = 224;
                for (int j = 0; j < frameInfo.Count; j++)
                {
                    imagePath[j] = resPath + "/" + (startIndex + j) + ".png";
                }
                clip = createFrameClipEffect("Die_Effect", imagePath, animSaveDir);

            }

        }
        return clips;
    }


    private static AnimationClip createFrameClipEffect(string clipName, string[] imagePaths, string savePath)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = clipName;
        var clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
        clipSetting.loopTime = true;
        clipSetting.cycleOffset = 0;
        clipSetting.orientationOffsetY = 0;
        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[imagePaths.Length + 1];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        //动画间隔计算有问题
        float frameTime = 100 / 1000f;
        for (int i = 0; i < imagePaths.Length; i++)
        {

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(imagePaths[i]));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        keyFrames[keyFrames.Length - 1] = new ObjectReferenceKeyframe()
        {
            time = frameTime * (keyFrames.Length - 1),
            value = keyFrames[keyFrames.Length - 2].value
        };

        //动画帧率，30比较合适
        clip.frameRate = 30;

        AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, savePath + "/" + clip.name + ".anim");
        AssetDatabase.SaveAssets();
        return clip;
    }


    private static AnimationClip createFrameClip(Frame frameInfo, string mirAction, MirDirection mirDirection, string[] imagePaths, string savePath)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = mirAction + "_" + mirDirection;
        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[imagePaths.Length + 1];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        //动画间隔计算有问题
        float frameTime = frameInfo.Interval / 1000f;
        for (int i = 0; i < imagePaths.Length; i++)
        {

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(imagePaths[i]));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        keyFrames[keyFrames.Length - 1] = new ObjectReferenceKeyframe()
        {
            time = frameTime * (keyFrames.Length - 1),
            value = keyFrames[keyFrames.Length - 2].value
        };

        //动画帧率，30比较合适
        clip.frameRate = 30;

        //var animevent = new AnimationEvent();

        //animevent.time = keyFrames[keyFrames.Length - 1].time;
        //animevent.functionName = "clipCallback";
        //animevent.stringParameter = clip.name;
        //var animevents = new AnimationEvent[1];
        //animevents[0] = animevent;
        //AnimationUtility.SetAnimationEvents(clip, animevents);

        var clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
        clipSetting.loopTime = true;
        clipSetting.loopBlend = true;
        clipSetting.cycleOffset = 0;
        clipSetting.orientationOffsetY = 0;


        AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, savePath + "/" + clip.name + ".anim");
        AssetDatabase.SaveAssets();





        return clip;
    }



    public static void buildPlayerAnim(MLibrary monsterLib, string resPath)
    {
        var mirDirections = AnimType.forkAnimMirDirection(AnimType.MONSTER);
        var frames = playerActionFrameSet();
        var clips = new List<Tuple<MirAction, MirDirection, AnimationClip>>();

        var animSaveDir = resPath + "/anim";
        if (!Directory.Exists(animSaveDir))
        {
            Directory.CreateDirectory(animSaveDir);
        }
        animSaveDir = DataPathToAssetPath(animSaveDir);
        var animName = Path.GetFileNameWithoutExtension(resPath);
        foreach (var frame in frames)
        {
            var tmp = createFrameClip(frame, resPath, animSaveDir, mirDirections);
            clips.AddRange(tmp);
        }

        BuildAnimationController(clips, animSaveDir, animName);

    }



    private static FrameSet playerActionFrameSet()
    {
        FrameSet Player = new FrameSet(); ;
        Player.Add(MirAction.Standing, new Frame(0, 4, 0, 500, 0, 8, 0, 250));
        Player.Add(MirAction.Walking, new Frame(32, 6, 0, 100, 64, 6, 0, 100));
        Player.Add(MirAction.Running, new Frame(80, 6, 0, 100, 112, 6, 0, 100));
        Player.Add(MirAction.Stance, new Frame(128, 1, 0, 1000, 160, 1, 0, 1000));
        Player.Add(MirAction.Stance2, new Frame(300, 1, 5, 1000, 332, 1, 5, 1000));
        Player.Add(MirAction.Attack1, new Frame(136, 6, 0, 100, 168, 6, 0, 100));
        Player.Add(MirAction.Attack2, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
        Player.Add(MirAction.Attack3, new Frame(232, 8, 0, 100, 264, 8, 0, 100));
        Player.Add(MirAction.Attack4, new Frame(416, 6, 0, 100, 448, 6, 0, 100));
        Player.Add(MirAction.Spell, new Frame(296, 6, 0, 100, 328, 6, 0, 100));
        Player.Add(MirAction.Harvest, new Frame(344, 2, 0, 300, 376, 2, 0, 300));
        Player.Add(MirAction.Struck, new Frame(360, 3, 0, 100, 392, 3, 0, 100));
        Player.Add(MirAction.Die, new Frame(384, 4, 0, 100, 416, 4, 0, 100));
        Player.Add(MirAction.Dead, new Frame(387, 1, 3, 1000, 419, 1, 3, 1000));
        Player.Add(MirAction.Revive, new Frame(384, 4, 0, 100, 416, 4, 0, 100) { Reverse = true });
        Player.Add(MirAction.Mine, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
        Player.Add(MirAction.Lunge, new Frame(139, 1, 5, 1000, 300, 1, 5, 1000));
        return Player;
    }

}
