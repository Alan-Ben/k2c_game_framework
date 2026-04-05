using System;
using System.Collections.Generic;
using ALPackage;
using GOE.Variable;
using NPEnum;

namespace GOE
{
    /*******************
     * 变量结构体基类
     **/
    public abstract class _ANPBasicPlayerVariableObj : _ITNPBasicVariable<ENPPlayerVariableType
#if NP_GAME
            , NPPlayer
#endif
        >
    {
        /******************
         * 获取条件类型
         */
        public abstract ENPPlayerVariableType variableType { get; }

        public abstract long calPlayerValue(NPVarInfo _variableInfo);

#if NP_GAME
        /// <summary>
        /// 判断条件是否匹配
        /// </summary>
        /// <param name="_cond"></param>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public long calValue(NPPlayer _data, NPVarInfo _varVariableInfo)
        {
            return calPlayerValue(_varVariableInfo);
        }
#endif


        /**************
         * 将字符串转化为本对象
         **/
        public static _ANPBasicPlayerVariableObj readVariable(string _str)
        {
            ALStringReader stringReader = new ALStringReader(_str);
            string varTypeS = stringReader.readItem('@');

            if (null == varTypeS)
            {
                ALLog.Warning("空的公式字符串:" + _str);
                return null;
            }

            ENPPlayerVariableType varType = ENPPlayerVariableType.NONE;
            try
            {
                //读取类型枚举
                varType = (ENPPlayerVariableType)ALCommon.EnumParse(typeof(ENPPlayerVariableType), varTypeS, true);
            }
            catch (Exception) { }

            if (varType == ENPPlayerVariableType.NONE)
            {
#if UNITY_EDITOR
                ALLog.Error("错误的公式类型:" + _str);
#endif
                return null;
            }

            return readVariable(varType, stringReader);
        }

        /********************
         * 从节点中读取相关信息
         */
        public static _ANPBasicPlayerVariableObj readVariable(ENPPlayerVariableType _variableType, ALStringReader _reader)
        {
            switch (_variableType)
            {
                case ENPPlayerVariableType.C_RND:
                    return NPPlayerVariableRnd.readVariable(_reader);

                case ENPPlayerVariableType.CS_NUM:
                    return NPPlayerVariableNum.readVariable(_reader);
                case ENPPlayerVariableType.CS_VALUE:
                    return NPPlayerVariablePlayerValue.readVariable(_reader);
                case ENPPlayerVariableType.CS_PROPERTY:
                    return NPPlayerVariablePlayerPro.readVariable(_reader);
                case ENPPlayerVariableType.CS_VAR_V:
                    return NPPlayerVariableVarValue.readVariable(_reader);
                case ENPPlayerVariableType.CS_BUF_L:
                    return NPPlayerVariableBufL.readVariable(_reader);
                case ENPPlayerVariableType.CS_BUFF_HAD_ACTIVE_DAY:
                    return NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY.readVariable(_reader);
                case ENPPlayerVariableType.CS_ITEM_COUNT:
                    return NPPlayerVariableItemCount.readVariable(_reader);

                case ENPPlayerVariableType.CS_PARAM:
                    return NPPlayerVariablePlayerParam.readVariable(_reader);
                case ENPPlayerVariableType.CS_VALUE_ID:
                    return NPPlayerVariableIdValue.readVariable(_reader);
                // case ENPPlayerVariableType.CS_PET_NUM_L:
                //     return NPPlayerVariablePetNumL.readVariable(_reader);
                // case ENPPlayerVariableType.CS_PET_NUM_S:
                //     return NPPlayerVariablePetNumS.readVariable(_reader);
                // case ENPPlayerVariableType.CS_PET_NUM_E:
                //     return NPPlayerVariablePetNumE.readVariable(_reader);
                case ENPPlayerVariableType.CS_CONDITION_BOOLEAN_V:
                    return NPPlayerVariableConditionBooleanV.readVariable(_reader);
                case ENPPlayerVariableType.CS_EVENT_RECORD:
                    return NPPlayerVariableEventRecord.readVariable(_reader);
                case ENPPlayerVariableType.CS_SCOPE_ITEM_COUNT:
                    return NPPlayerVariableScopeItemCount.readVariable(_reader);
                case ENPPlayerVariableType.CS_HERO_NUM:
                    return NPPlayerVariableHeroNum.readVariable(_reader);
                case ENPPlayerVariableType.CS_RECORD_PARAM:
                    return NPPlayerVariableRecordParam.readVariable(_reader);
                case ENPPlayerVariableType.CS_CONSORT_RES_COUNT:
                    return NPPlayerVariable_CS_CONSORT_RES_COUNT.readVariable(_reader);
                case ENPPlayerVariableType.CS_BUILDING_LEVEL:
                    return NPPlayerVariable_CS_BUILDING_LEVEL.readVariable(_reader);
                case ENPPlayerVariableType.CS_HAS_BUILDING:
                    return NPPlayerVariable_CS_HAS_BUILDING.readVariable(_reader);
                case ENPPlayerVariableType.CS_BUSINESS_BUILDING_WORKER_NUM:
                    return NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM.readVariable(_reader);
                case ENPPlayerVariableType.CS_CONSORT_FETTER_LEVLE:
                    return NPPlayerVariable_CS_CONSORT_FETTER_LEVLE.readVariable(_reader);
                case ENPPlayerVariableType.CS_MARS_BUILDING_LVL:
                    return NPPlayerVariable_CS_MARS_BUILDING_LVL.readVariable(_reader);
                case ENPPlayerVariableType.CS_MARS_BUILDING_EQUIP_LVL:
                    return NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL.readVariable(_reader);
                case ENPPlayerVariableType.CS_MARS_BUILDING_DISPATCH_NUM:
                    return NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM.readVariable(_reader);
                default:
                    return null;
            }
        }
    }
}

