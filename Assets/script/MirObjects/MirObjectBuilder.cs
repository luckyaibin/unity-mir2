using System;
using System.Collections.Generic;
using System.IO;
using Assets.script.Mir.map;
using mir.objects.magic;
using mir.objects.magic.builder;
using UnityEngine;

namespace script.mir.objects
{
    public abstract class MirObjectBuilder<T> : BaseBuilder where T : Packet
    {


        private static GameObject prefab;


        public virtual GameObject gameObject(T packet)
        {

            return null;
        }


        public virtual GameObject gameObject(T packet, Transform parent)
        {
            return null;
        }
    }
}


