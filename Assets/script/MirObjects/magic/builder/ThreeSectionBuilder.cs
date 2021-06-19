using System;
using mir.objects.magic.controller;
using UnityEngine;

namespace mir.objects.magic.builder
{
    public abstract class ThreeSectionBuilder : TwoSectionBuilder
    {
        public override OneSectionController addSpellController(GameObject gameObject)
        {
            return gameObject.AddComponent<ThreeSectionController>();
        }
    }
}