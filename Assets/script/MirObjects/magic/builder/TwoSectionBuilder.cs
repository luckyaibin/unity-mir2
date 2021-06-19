using System.Collections;
using System.Collections.Generic;
using mir.objects.magic.controller;
using UnityEngine;
namespace mir.objects.magic.builder
{
    public abstract class TwoSectionBuilder : OneSectionBuilder
    {
        public virtual GameObject gameObject(Vector2 position, Vector2 target)
        {
            var magicPrefab = getPrefab("prefabs/npc");
            var anim = magicPrefab.GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(getAnimatorControllerPath());
            var mirGameObject = GameObject.Instantiate(magicPrefab, calcPosition(position, getOffset()), Quaternion.identity);
            var animController = addSpellController(mirGameObject);
            animController.targetPosition = target;
            animController.selfPostion = position;
            animController.spellBuilder = this;
            mirGameObject.name = "magic(" + GetSpell().ToString() + ")";
            mirGameObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("materials/blend_add");
            mirGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "map_front";
            mirGameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)position.y + 1000;
            mirGameObject.layer = 10;
            return mirGameObject;
        }
        public virtual OneSectionController addSpellController(GameObject gameObject)
        {
            return gameObject.AddComponent<TwoSectionController>();
        }
    }


}