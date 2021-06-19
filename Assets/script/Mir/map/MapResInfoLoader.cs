using System;
using System.Collections.Generic;
using Client.MirGraphics;
using UnityEngine;

namespace Assets.script.Mir.map
{
    public class MapResInfoLoader
    {

        private static MapResInfoLoader _instance = null;
        private static readonly object _syslock = new object();
        private MapResInfoLoader()
        {
        }

        public static MapResInfoLoader GetInstance()
        {
            // 最开始判断不存在的时候，该类从来未被实例化过 //
            if (_instance == null)
            {
                // 锁定状态，继续搜索是否存在该类的实例 //
                lock (_syslock)
                {
                    // 如果不存在，在锁定状态下实例化出一个实例 //
                    if (_instance == null)
                    {
                        _instance = new MapResInfoLoader();

                    }

                }
            }
            // 该实例本身就已经存在了，直接返回 //
            return _instance;
        }


        private Dictionary<String, MInfoLibrary> mlibs = new Dictionary<String, MInfoLibrary>();


        public MImage GetMImageInfo(string lib, int imageIndex)
        {

            lock (_syslock)
            {
                if (!mlibs.ContainsKey(lib))
                {
                    mlibs.Add(lib, new MInfoLibrary(lib + ".info"));
                }
                var info = mlibs[lib].getMImageInfo(imageIndex);
                info.imagePath = lib + "/" + imageIndex;
                info.imagePath = info.imagePath.Replace(MapConfigs.MIR_RES_PATH, "");
                return info;
            }

        }
    }

}

