using System;
using ALPackage;
using Common.ConditionEnum;
using GOE.Condition;

namespace GOE
{
    public abstract class _ABasicBuildingCondition : _ITNPBasicCondition<EBuildingConditionType
#if NP_GAME
        , BusinessBuildingRefObj
#endif
        , NPVarInfo
    >
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public abstract EBuildingConditionType conditionType { get; }
        
        /// <summary>
        /// 判断条件是否满足
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public abstract bool isEnable(BusinessBuildingRefObj _data, NPVarInfo _varVariableInfo);
        
        /**************
            * 将字符串转化为本对象
            **/
        public static _ABasicBuildingCondition readCondition(string _str)
        {
            ALStringReader stringReader = new ALStringReader(_str);
            string condTypeS = stringReader.readItem(':');

            if (null == condTypeS)
            {
                ALLog.Warning("空的条件字符串:" + _str);
                return null;
            }

            EBuildingConditionType condType = EBuildingConditionType.NONE;
            try
            {
                //读取类型枚举
                condType = (EBuildingConditionType) ALCommon.EnumParse(typeof(EBuildingConditionType), condTypeS, true);
            }
            catch (Exception)
            {
                Debug.LogError($"建筑条件配置错误！！不存在类型:{condTypeS}，str:{_str}");
            }

            if (condType == EBuildingConditionType.NONE)
            {
#if UNITY_EDITOR
                ALLog.Warning("错误的条件类型:" + _str);
#endif
                return null;
            }

            //创建对象
            return readCondition(condType, stringReader);
        }

        /********************
        * 从节点中读取相关信息
        */
        public static _ABasicBuildingCondition readCondition(EBuildingConditionType _conditionType, ALStringReader _reader)
        {
            switch (_conditionType)
            {
                case EBuildingConditionType.NONE:
                    return new BuildingConditionFalse();
                case EBuildingConditionType.CS_BUILDING_ID:
                    return BuildingCondition_CS_BUILDING_ID.readStr(_reader);
                case EBuildingConditionType.CS_SPEC_ATTR_TYPE:
                    return BuildingCondition_CS_SPEC_ATTR_TYPE.readStr(_reader);
                default:
#if UNITY_EDITOR
                    Debug.LogError($"找不到condition：{_conditionType}==={_reader.srcString}");
#endif
                    return new BuildingConditionFalse();
            }
        }
    }
}