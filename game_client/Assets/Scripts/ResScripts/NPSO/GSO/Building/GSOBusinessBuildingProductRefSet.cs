using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingProductRefObj : _IALBasicRefObj, IComparable<BusinessBuildingProductRefObj>
    {
        public long _refId { get { return id; } }

        public long id;
        public long building_id;
        public long employee_required;
        public NPGTextureIndex icon;
        public string name;
        public _UnionBonusSerializeInfo add_bonus;
        public float fake_output_time;
        public float fake_output_time_offset;
        public long fake_output_count;

        public int CompareTo(BusinessBuildingProductRefObj _other)
        {
            if (_other == null)
                return 0;
            
            return employee_required.CompareTo(_other.employee_required);
        }
    }
    public class GSOBusinessBuildingProductRefSet : _TALSOBasicRefSet<BusinessBuildingProductRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building_product"; } }
    }
}
