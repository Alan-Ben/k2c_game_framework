
using ALPackage;
using System;

namespace GOE
{
    [Serializable]
    public class ChildResRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; //形象id
        public NPGTextureIndex icon; //图片
        public NPGTextureIndex card_icon; //半身像
        public NPGGoIndex td_show; //全身形象
        public NPGGoIndex td_mini_show; //小人形象
    }

    public class GSOChildResRefSet : _TALSOBasicRefSet<ChildResRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_res"; } }
    }
}