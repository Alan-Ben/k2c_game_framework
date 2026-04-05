using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingDevelopRefObj : _IALBasicRefObj, IComparable<BusinessBuildingDevelopRefObj>
    {
        public long _refId { get { return id; } }

        public long id;
        public long building_id;
        public long level_required;
        public NPGTextureIndex icon;
        public string name;
        public string desc;
        public List<string> desc_args;
        public long video_group_id;
        public _UnionBonusSerializeInfo add_bonus;
        
        
        [NonSerialized, ALAutoExportVariableAttr(true, true, true)]
        public BusinessBuildingVideoGroupRefObj video_group_ref;
        

        public int CompareTo(BusinessBuildingDevelopRefObj _other)
        {
            if (_other == null)
                return 0;
            
            return level_required.CompareTo(_other.level_required);
        }
    }
    public class GSOBusinessBuildingDevelopRefSet : _TALSOBasicRefSet<BusinessBuildingDevelopRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building_develop"; } }
    }
}