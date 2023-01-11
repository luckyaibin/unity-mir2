using Client.MirGraphics;
using Client.MirObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Client;


public class MirResEditor : EditorWindow
{
    public const int CellWidth = 48;
    public const int CellHeight = 32;
    private string mirResDataPath = @"E:\exp\unity-mir2\Assets\Resources\mir\Data";
    private string mirResMapPath = @"E:\exp\unity-mir2\Assets\Resources\mir\Map";
    [MenuItem("Window/传奇资源")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MirResEditor), false, "传奇资源");

    }
    private void OnGUI()
    {
        GUILayout.Label("C# lib资源路径", EditorStyles.boldLabel);
        //
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("c# lib Data路径:");
        mirResDataPath = EditorGUILayout.TextField(mirResDataPath);
        if (GUILayout.Button("浏览"))
        {
            EditorApplication.delayCall += openDataPath;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("c# lib Map路径:");
        mirResMapPath = EditorGUILayout.TextField(mirResMapPath);
        if (GUILayout.Button("浏览"))
        {
            EditorApplication.delayCall += openMapPath;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("导出地图资源"))
        {
            EditorApplication.delayCall += exportMapRes;
        }
        if (GUILayout.Button("anim 测试"))
        {
            EditorApplication.delayCall += AnimBuilder.testAnim;
        }

        if (GUILayout.Button("导出怪物动画资源"))
        {
            EditorApplication.delayCall += exportMonsterRes;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("导出NPC动画资源"))
        {
            EditorApplication.delayCall += exportNpcRes;
        }


        EditorGUILayout.EndHorizontal();
        GUILayout.Label("C# 人物动画资源");
        GUILayout.Label("C# 道士 战士 法师");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("衣服"))
        {
            EditorApplication.delayCall += exportBaseBody;
        }
        if (GUILayout.Button("衣服特效"))
        {

        }
        if (GUILayout.Button("武器"))
        {
            EditorApplication.delayCall += exportWeapon;
        }
        if (GUILayout.Button("武器特效"))
        {

        }
        if (GUILayout.Button("头发"))
        {
            EditorApplication.delayCall += exportHair;
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Label("技能");
        GUILayout.Label("法师");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("小火球"))
        {
            EditorApplication.delayCall += MirSpell.fireBall;
        }
        if (GUILayout.Button("大火球"))
        {
            EditorApplication.delayCall += MirSpell.greatFireBall;
        }
        if (GUILayout.Button("寒冰掌"))
        {
            EditorApplication.delayCall += MirSpell.frostCrunch;
        }
        if (GUILayout.Button("雷电术"))
        {
            EditorApplication.delayCall += MirSpell.ThunderBolt;
        }
        if (GUILayout.Button("抗拒火环"))
        {
            EditorApplication.delayCall += MirSpell.repulsion;
        }
        if (GUILayout.Button("诱惑之光"))
        {
            EditorApplication.delayCall += MirSpell.electricShock;
        }
        EditorGUILayout.EndHorizontal();
    }



    private void openDataPath()
    {
        openFolder(mirResDataPath);
    }

    private void openMapPath()
    {
        openFolder(mirResMapPath);
    }

    private void openFolder(string openDestPath)
    {
        mirResMapPath = EditorUtility.OpenFolderPanel("请选择", openDestPath, "");
    }

    //private const string resOut = "/Users/yangcai/Documents/unity-workspace/tmp";
    private const string resOut = "./Assets/Resources/mir";
    private void exportMapRes()
    {

        MapReader mapReader = new MapReader(mirResMapPath + "/0.map");
        var M2CellInfo = mapReader.MapCells;
        for (int x = 0; x < mapReader.Width; x++)
        {

            for (int y = 0; y < mapReader.Height; y++)
            {

                var cellInfo = M2CellInfo[x, y];
                if (cellInfo.hasBackImage(x, y))
                {
                    saveImage(cellInfo.BackIndex, cellInfo.getBackImageIndex());
                }
                if (cellInfo.hasMiddleImage())
                {


                    saveImage(cellInfo.MiddleIndex, cellInfo.getMiddleImageIndex());
                }


                if (cellInfo.hasFrontImage())
                {
                    saveImage(cellInfo.FrontIndex, cellInfo.getFrontImageIndex());
                    // door 图片
                    //var imageIndex = cellInfo.getFrontImageIndex();
                    //if (cellInfo.hasDoorImage())
                    //{
                    //    var doorImageIndex = imageIndex + cellInfo.DoorOffset;
                    //    if (depthBufferBits > 0)
                    //    {
                    //        saveImage(cellInfo.FrontIndex, doorImageIndex);
                    //    }
                    //    doorImageIndex = imageIndex + cellInfo.DoorOffset * 2;
                    //    if (depthBufferBits > 0)
                    //    {
                    //        saveImage(cellInfo.FrontIndex, doorImageIndex);
                    //    }
                    //}

                    if (cellInfo.hasFrontAnimation())
                    {
                        var frontAnimationIndex = cellInfo.getFrontImageIndex();
                        int size = cellInfo.getFrontAnimationFrame();
                        var anims = new string[size];
                        for (int i = frontAnimationIndex; i < frontAnimationIndex + size; i++)
                        {
                            anims[i - frontAnimationIndex] = saveImage(cellInfo.FrontIndex, i);
                        }
                        AnimBuilder.buildMapAnim(anims);
                    }
                }
            }
        }
        Libraries.close();

    }


    private string saveImage(MLibrary lib, int imageIndex)
    {
        var savePath = getFilePath(lib.getLibPath(), imageIndex);
        lib.writeInfo(getInfoFilePath(lib.getLibPath()));
        if (File.Exists(savePath))
        {
            return savePath;
        }
        //mir3 mid layer is same level as front layer not real middle + it cant draw index -1 so 2 birds in one stone :p
        var image = lib.CheckImage(imageIndex);
        if (image == null) return null;
        var bytes = image.Image.EncodeToPNG();
        saveBytes(savePath, bytes);
        lib.clearImage(imageIndex);
        return savePath;
    }

    private string saveImage(int libIndex, int imageIndex)
    {
        var lib = Libraries.MapLibs[libIndex];
        return saveImage(lib, imageIndex);
    }


    private string getInfoFilePath(string libPath)
    {
        var path = libPath.Replace(MLibrary.Extention, ".info");
        return path.Replace(Settings.resRootPath, resOut);
    }

    private string getFilePath(string libPath, int imageIndex)
    {

        var path = libPath.Replace(MLibrary.Extention, "");
        var outPath = path.Replace(Settings.resRootPath, resOut);

        var dirInfo = new DirectoryInfo(outPath);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        return dirInfo.FullName + "/" + imageIndex + ".png";
    }

    private void saveBytes(string filePath, byte[] bytes)
    {
        var saveFile = File.Open(filePath, FileMode.OpenOrCreate);
        var binary = new BinaryWriter(saveFile);
        binary.Write(bytes);
        binary.Flush();
        binary.Close();
        saveFile.Close();
    }


    private void exportNpcRes()
    {
        var alignOffsetes = new List<Vector2Int>();
        foreach (var npc in Libraries.NPCs)
        {
            var tmp = exportNpcResOne(npc);
            alignOffsetes.Add(tmp);
        }
        //Assets/Resources/mir/Data/NPC
        var npcOffsetInfoPath = resOut + "/Data/NPC/npc.info";

        saveOffsets(alignOffsetes, npcOffsetInfoPath);
    }

    //导出怪物 npc 装备(主要是衣服)
    private Vector2Int exportNpcResOne(MLibrary library)
    {
        library.Initialize();
        Vector2Int alignOffset;
        var images = library.checkImageAlignmentOffset(out alignOffset);

        var count = library.getCount();
        string resPath = "";
        for (int i = 0; i < count; i++)
        {
            resPath = saveObjectImage(library, images[i], i);
        }
        resPath = Directory.GetParent(resPath).FullName;

        AnimBuilder.buildNpc(library, resPath);
        library.close();
        return alignOffset;
    }



    private void exportMonsterRes()
    {
        //Assets/Resources/mir/Data/Monster
        var monsterOffsetInfoPath = resOut + "/Data/Monster/monster.info";
        var alignOffsetes = new List<Vector2Int>();
        var count = 6;

        for (int i = 0; i < count; i++)
        {
            var lib = Libraries.Monsters[i];

            var tmp = exportMonsterResOne(lib);
            alignOffsetes.Add(tmp);
        }
        saveOffsets(alignOffsetes, monsterOffsetInfoPath);
    }


    private void saveOffsets(List<Vector2Int> alignOffsetes, string savePath)
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

    private Vector2Int exportMonsterResOne(MLibrary monsterLib)
    {



        monsterLib.Initialize();
        Vector2Int alignOffset;
        var images = monsterLib.checkImageAlignmentOffset(out alignOffset);

        var count = monsterLib.getCount();
        string resPath = "";
        for (int i = 0; i < count; i++)
        {
            resPath = saveObjectImage(monsterLib, images[i], i);
        }
        resPath = Directory.GetParent(resPath).FullName;

        AnimBuilder.buildMonsterAnim(monsterLib, resPath);
        monsterLib.close();
        return alignOffset;
    }





    private string saveObjectImage(MLibrary lib, MImage image, int imageIndex)
    {

        var savePath = getFilePath(lib.getLibPath(), imageIndex);
        if (File.Exists(savePath))
        {
            return savePath;
        }
        //mir3 mid layer is same level as front layer not real middle + it cant draw index -1 so 2 birds in one stone :p

        if (image == null) return null;
        var bytes = image.Image.EncodeToPNG();
        saveBytes(savePath, bytes);
        lib.clearImage(imageIndex);
        return savePath;
    }


    private void exportBaseBody()
    {
        var alignOffsetes = new List<Vector2Int>();
        //int count = Libraries.CArmours.Length;
        var count = 1;
        for (int i = 0; i < count; i++)
        {
            var cArmour = Libraries.CArmours[i];
            var tmp = exportBaseBodyOne(cArmour);
            alignOffsetes.Add(tmp);
        }

        //foreach (var cArmour in Libraries.CArmours)
        //{
        //    var tmp = exportBaseBodyOne(cArmour);
        //    alignOffsetes.Add(tmp);
        //}
        //Assets/Resources/mir/Data/NPC
        /// Users / yangcai / Documents / unity - workspace / MirMobile / Assets / Resources / mir / Data / CArmour
        var npcOffsetInfoPath = resOut + "/Data/CArmour/carmour.info";

        saveOffsets(alignOffsetes, npcOffsetInfoPath);

    }


    private Vector2Int exportBaseBodyOne(MLibrary library)
    {
        library.Initialize();
        Vector2Int alignOffset;
        var images = library.checkImageAlignmentOffset(out alignOffset);

        var count = library.getCount();
        string resPath = "";
        for (int i = 0; i < count; i++)
        {
            resPath = saveObjectImage(library, images[i], i);
        }
        resPath = Directory.GetParent(resPath).FullName;

        AnimBuilder.buildPlayerAnim(library, resPath);
        library.close();
        return alignOffset;
    }



    private void exportHair()
    {
        var alignOffsetes = new List<Vector2Int>();

        var count = 1;
        for (int i = 0; i < count; i++)
        {
            var cHair = Libraries.CHair[i];
            var tmp = exportBaseBodyOne(cHair);
            alignOffsetes.Add(tmp);
        }

        //foreach (var cHair in Libraries.CHair)
        //{
        //    var tmp = exportBaseBodyOne(cHair);
        //    alignOffsetes.Add(tmp);
        //}
        //Assets/Resources/mir/Data/NPC
        /// Users / yangcai / Documents / unity - workspace / MirMobile / Assets / Resources / mir / Data / CArmour
        var npcOffsetInfoPath = resOut + "/Data/CHair/cHair.info";

        saveOffsets(alignOffsetes, npcOffsetInfoPath);

    }


    private void exportWeapon()
    {
        var alignOffsetes = new List<Vector2Int>();

        var count = 1;
        for (int i = 0; i < count; i++)
        {
            var cWeapon = Libraries.CWeapons[i];
            var tmp = exportBaseBodyOne(cWeapon);
            alignOffsetes.Add(tmp);
        }

        //foreach (var cHair in Libraries.CHair)
        //{
        //    var tmp = exportBaseBodyOne(cHair);
        //    alignOffsetes.Add(tmp);
        //}
        //Assets/Resources/mir/Data/NPC
        /// Users / yangcai / Documents / unity - workspace / MirMobile / Assets / Resources / mir / Data / CArmour
        var npcOffsetInfoPath = resOut + "/Data/CWeapon/cWeapon.info";

        saveOffsets(alignOffsetes, npcOffsetInfoPath);

    }
}
