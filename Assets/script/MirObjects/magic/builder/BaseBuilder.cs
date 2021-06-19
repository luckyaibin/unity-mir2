using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.script.Mir.map;
using UnityEngine;
namespace mir.objects.magic.builder
{
    public class BaseBuilder
    {
        private static GameObject prefab;


        public Vector3 calcPosition(Vector2 npcPosition, Vector2 offset)
        {
            return new Vector3(calcX(npcPosition) + offset.x, -(calcY(npcPosition) + offset.y), 0);
        }


        public GameObject getPrefab(string prefabPath)
        {
            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            return prefab;
        }


        public float calcX(Vector2 vector)
        {
            return vector.x * MapConfigs.MAP_TILE_WIDTH;
        }

        public float calcY(Vector2 vector)
        {
            return vector.y * MapConfigs.MAP_TILE_HEIGHT;
        }


        public static List<Vector2Int> readOffsets(string offsetPath)
        {
            var offsets = new List<Vector2Int>();
            var fileStream = File.Open(offsetPath, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            var count = binaryReader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                offsets.Add(new Vector2Int(binaryReader.ReadInt32(), binaryReader.ReadInt32()));
            }
            binaryReader.Close();
            fileStream.Close();
            return offsets;
        }
    }
}