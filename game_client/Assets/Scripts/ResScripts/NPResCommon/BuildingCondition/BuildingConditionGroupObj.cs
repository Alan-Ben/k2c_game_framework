using Common.ConditionEnum;
using GOE.Condition;

namespace GOE
{
    [System.Serializable]
    public class BuildingConditionGroupObj : _ATNPBasicConditionGroupObj<EBuildingConditionType, _ABasicBuildingCondition, BuildingConditionGroupObj
#if NP_GAME
        , BusinessBuildingRefObj
#endif
        , NPVarInfo
    >
    {
        protected override BuildingConditionGroupObj _createGroupObj()
        {
            return new BuildingConditionGroupObj();
        }

        protected override _ABasicBuildingCondition _readConditionStr(string _str)
        {
            return _ABasicBuildingCondition.readCondition(_str);
        }
        
        /// <summary>
        /// 读取字符串并返回一个读取结束的groupObj对象
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_err"></param>
        /// <returns></returns>
        public static BuildingConditionGroupObj readConditionGroupList(string _str, string _err)
        {
            if (string.IsNullOrEmpty(_str))
                return new BuildingConditionGroupObj();

            BuildingConditionGroupObj obj = new BuildingConditionGroupObj();
            obj._readString(_str, 0, _err);

            return obj;
        }

        //对外统一调用的判断处理
        public static bool IsEnable(BuildingConditionGroupObj _condGroupList, BusinessBuildingRefObj _businessBuildingRefObj, NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            if (null == _condGroupList)
                return true;

            return _condGroupList.IsEnable(_businessBuildingRefObj, _varVariableInfo);
#else
            return false;
#endif
        }
    }
}