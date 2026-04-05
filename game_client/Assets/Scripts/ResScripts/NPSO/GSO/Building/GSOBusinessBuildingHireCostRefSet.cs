
using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingHireCostRefObj : _IALBasicRefObj, IComparable<BusinessBuildingHireCostRefObj>
    {
        public long _refId { get { return employee_num; } }

        public long employee_num;
        public float hire_cost_coeff_A;
        public float hire_cost_coeff_B;
        public float hire_cost_coeff_C;
        public float hire_cost_coeff_D;
        
        public int CompareTo(BusinessBuildingHireCostRefObj _other)
        {
            if (_other == null)
                return -1;
            
            return employee_num.CompareTo(_other.employee_num);    
        }
    }
    public class GSOBusinessBuildingHireCostRefSet : _TALSOBasicRefSet<BusinessBuildingHireCostRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building_hire_cost"; } }
    }
}