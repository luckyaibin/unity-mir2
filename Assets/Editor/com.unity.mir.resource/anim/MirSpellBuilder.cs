using System;
using System.Collections.Generic;
using System.IO;
using Client.MirGraphics;
using Client.MirObjects;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public abstract class MirSpellBuilder
{

    public static string magic_dir = "/Users/yangcai/Documents/unity-workspace/MirMobile/Assets/Resources/mir/data/magic/";


    protected static int spellFrameTime = 6 * 100;



    public abstract void build();


    public List<MImage> getImageInfoByFrame(Frame frame, MLibrary library)
    {
        var imageInfos = new List<MImage>();
        for (var i = 0; i < frame.Count; i++)
        {
            var tmp = library.getMImageInfo(i + frame.Start);
            imageInfos.Add(tmp);
        }
        return imageInfos;
    }

    public void saveImageByFrame(Frame frame, MLibrary library, Vector2Int offset, string saveDir)
    {

        for (var i = 0; i < frame.Count; i++)
        {
            var imageIndex = i + frame.Start;
            var tmp = library.getMImageInfo(imageIndex);
            tmp = library.checkImageAlignmentOffset(offset, imageIndex, tmp);
            saveMImage(tmp, saveDir, imageIndex);
        }

    }


    public void saveBytes(string filePath, byte[] bytes)
    {
        var saveFile = File.Open(filePath, FileMode.OpenOrCreate);
        var binary = new BinaryWriter(saveFile);
        binary.Write(bytes);
        binary.Flush();
        binary.Close();
        saveFile.Close();
    }


    public void saveMImage(MImage mImage, string saveDir, int imageIndex)
    {
        if (mImage == null)
        {
            return;
        }

        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        var savePath = saveDir + "/" + imageIndex + ".png";
        if (File.Exists(savePath))
        {
            return;
        }
        if (mImage.Image == null)
        {
            return;
        }
        saveBytes(savePath, mImage.Image.EncodeToPNG());
    }


    public void saveOffsets(List<Vector2Int> alignOffsetes, string savePath)
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        var fileStream = File.Open(savePath, FileMode.OpenOrCreate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
        binaryWriter.Write(alignOffsetes.Count);
        foreach (var offset in alignOffsetes)
        {
            binaryWriter.Write(offset.x);
            binaryWriter.Write(offset.y);
        }
        binaryWriter.Flush();
        binaryWriter.Close();
        fileStream.Close();

    }

    public AnimationClip createAnimationClipByFrame(Frame frame, string saveDir, string clipName)
    {
        var images = new string[frame.Count];

        for (var i = 0; i < frame.Count; i++)
        {
            var imageIndex = frame.Start + i;
            images[i] = saveDir + "/" + imageIndex + ".png";
        }
        if (!Directory.Exists(saveDir + "/anim"))
        {
            Directory.CreateDirectory(saveDir + "/anim");
        }
        return createAnimationClip(frame, clipName, images, saveDir + "/anim");
    }


    public AnimationClip createAnimationClip(Frame frameInfo, string clipName, string[] imagePaths, string savePath)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = clipName;
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

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AnimBuilder.DataPathToAssetPath(imagePaths[i]));
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
        AssetDatabase.CreateAsset(clip, AnimBuilder.DataPathToAssetPath(savePath + "/" + clip.name + ".anim"));
        AssetDatabase.SaveAssets();
        return clip;
    }

    public void buildAnimationController(AnimationClip spell, List<Tuple<MirSpellAction, AnimationClip>> moves, AnimationClip hit, string saveDir, string name)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(AnimBuilder.DataPathToAssetPath(saveDir + "/" + name + ".controller"));
        AnimatorControllerLayer layer = animatorController.layers[0];
        animatorController.AddParameter("Spell", AnimatorControllerParameterType.Int);
        animatorController.AddParameter("SpellAction", AnimatorControllerParameterType.Int);
        AnimatorStateMachine stateMachine = layer.stateMachine;
        //magic spell 
        var spellState = stateMachine.AddState(spell.name);
        spellState.motion = spell;
        makeAnimatorStateTransition(stateMachine.AddAnyStateTransition(spellState), MirSpellAction.sepll);

        //magic hit
        if (hit != null)
        {
            var hitState = stateMachine.AddState(hit.name);
            hitState.motion = hit;
            makeAnimatorStateTransition(stateMachine.AddAnyStateTransition(hitState), MirSpellAction.hit);
        }
        //magic move
        if (moves != null)
            foreach (var tuple in moves)
            {
                var spellDirection = tuple.Item1;
                var newClip = tuple.Item2;
                var moveState = stateMachine.AddState(newClip.name);
                moveState.motion = newClip;
                makeAnimatorStateTransition(stateMachine.AddAnyStateTransition(moveState), spellDirection);
            }
        AssetDatabase.SaveAssets();
    }


    public void buildAnimationController(AnimationClip spell, AnimationClip hit, string saveDir, string name)
    {
        buildAnimationController(spell, null, hit, saveDir, name);
    }

    public void buildAnimationController(AnimationClip spell, string saveDir, string name)
    {
        buildAnimationController(spell, null, null, saveDir, name);
    }
    private void makeAnimatorStateTransition(AnimatorStateTransition trans, MirSpellAction spellDirection)
    {

        trans.hasExitTime = false;
        trans.hasFixedDuration = true;
        trans.duration = 0;
        trans.offset = 0;
        trans.canTransitionToSelf = false;
        trans.exitTime = 1;
        trans.AddCondition(AnimatorConditionMode.Equals, (float)getSpell(), "Spell");
        trans.AddCondition(AnimatorConditionMode.Equals, (float)spellDirection, "SpellAction");
        //return trans;

    }


    public abstract Spell getSpell();

    public string getSpellSaveDir()
    {
        return magic_dir + getSpell().ToString();
    }

    public virtual MLibrary getMagicLib()
    {
        return Libraries.Magic;
    }


}
