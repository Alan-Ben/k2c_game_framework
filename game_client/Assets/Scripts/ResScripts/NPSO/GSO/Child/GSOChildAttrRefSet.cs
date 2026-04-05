
using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class ChildAttrRefObj : _IALBasicRefObj 
    {
        public long _refId { get { return (long)type; } }

        public ESpecAttrType type;
        public List<string> room_video_name_list;
        public List<NPGTextureIndex> bg_res_list;
        public long career_rand_group;
        
        public string getRoomVideoNameByStep(int _step)
        {
            if (room_video_name_list.Count == 0)
                return null;
            
            _step = Mathf.Clamp(_step, 0, room_video_name_list.Count - 1);
            return room_video_name_list[_step];
        }
        public NPGTextureIndex getBgResByStep(int _step)
        {
            if (bg_res_list.Count == 0)
                return null;

            _step -= 1;
            _step = Mathf.Clamp(_step, 0, bg_res_list.Count - 1);
            return bg_res_list[_step];
        }
        public NPGTextureIndex getLastBgRes()
        {
            if (bg_res_list.Count == 0)
                return null;

            return bg_res_list[^1];
        }
    }
    public class GSOChildAttrRefSet : _TALSOBasicRefSet<ChildAttrRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_attr"; } }
    }
}