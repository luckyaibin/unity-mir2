using System;
using UnityEngine;

namespace Assets.script.Mir.log
{

    public class LogUtil
    {
        private const bool enable = true;

        public static void log(string tag, string msg)
        {
            if (enable)
            {
                Debug.Log(tag + " : " + msg);
            }
        }
    }
}