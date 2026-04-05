using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingVideoGroupRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;
        public NPGGoIndex video_res_index;
        public List<string> video_anim_name_list;
        public List<string> video_name_list;
        public int need_show_lock_mask_index;
        public int show_split_line_index;
    }
    public class GSOBusinessBuildingVideoGroupRefSet : _TALSOBasicRefSet<BusinessBuildingVideoGroupRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building_video_group"; } }
    }
}