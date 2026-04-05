using System;
using ALPackage;
using Common.ConditionEnum;
using GOE.Condition;
using NPEnum;

namespace GOE
{
    public abstract class _ABasicHeroCondition : _ITNPBasicCondition<EHeroConditionType
#if NP_GAME
        , HeroRefObj
#endif
        , HeroConditionVarInfo
    >
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public abstract EHeroConditionType conditionType { get; }
        
        /// <summary>
        /// 判断条件是否满足
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public abstract bool isEnable(HeroRefObj _data, HeroConditionVarInfo _varVariableInfo);
        
        /**************
            * 将字符串转化为本对象
            **/
        public static _ABasicHeroCondition readCondition(string _str)
        {
            ALStringReader stringReader = new ALStringReader(_str);
            string condTypeS = stringReader.readItem(':');

            if (null == condTypeS)
            {
                ALLog.Warning("空的条件字符串:" + _str);
                return null;
            }

            EHeroConditionType condType = EHeroConditionType.NONE;
            try
            {
                //读取类型枚举
                condType = (EHeroConditionType)ALCommon.EnumParse(typeof(EHeroConditionType), condTypeS, true);
            }
            catch (Exception) { }

            if (condType == EHeroConditionType.NONE)
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
        public static _ABasicHeroCondition readCondition(EHeroConditionType _conditionType, ALStringReader _reader)
        {
            switch (_conditionType)
            {
                case EHeroConditionType.NONE:
                    return new HeroConditionFalse();
                case EHeroConditionType.CS_LEVEL:
                    return HeroCondition_CS_LEVEL.readStr(_reader);
                case EHeroConditionType.CS_STAR:
                    return HeroCondition_CS_STAR.readStr(_reader);
                case EHeroConditionType.CS_ATTR:
                    return HeroCondition_CS_ATTR.readStr(_reader);
                case EHeroConditionType.CS_STEP:
                    return HeroCondition_CS_STEP.readStr(_reader);
                default:
#if UNITY_EDITOR
                    Debug.LogError($"找不到condition：{_conditionType}==={_reader.srcString}");
#endif
                    return new HeroConditionFalse();
            }
        }
    }
}