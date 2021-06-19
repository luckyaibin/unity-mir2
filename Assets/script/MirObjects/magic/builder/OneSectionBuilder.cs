using System;
using mir.objects.magic.controller;
using UnityEngine;

namespace mir.objects.magic.builder
{
    public abstract class OneSectionBuilder : SpellBuilder
    {


        public GameObject gameObject(Vector2 position)
        {
            var magicPrefab = getPrefab("prefabs/npc");
            var anim = magicPrefab.GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(getAnimatorControllerPath());
            var mirGameObject = UnityEngine.Object.Instantiate(magicPrefab, calcPosition(position, getOffset()), Quaternion.identity);
            var animController = mirGameObject.AddComponent<OneSectionController>();
            animController.selfPostion = position;
            animController.spellBuilder = this;
            mirGameObject.name = "magic(" + GetSpell().ToString() + ")";
            mirGameObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("materials/blend_add");
            magicPrefab.GetComponent<SpriteRenderer>().sortingLayerName = "map_front";
            magicPrefab.GetComponent<SpriteRenderer>().sortingOrder = (int)position.y + 1000;
            magicPrefab.layer = 10;
            return mirGameObject;
        }


    }
}