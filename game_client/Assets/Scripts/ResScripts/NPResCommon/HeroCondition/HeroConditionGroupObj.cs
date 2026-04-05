using Common.ConditionEnum;
using GOE.Condition;

namespace GOE
{
    [System.Serializable]
    public class HeroConditionGroupObj : _ATNPBasicConditionGroupObj<EHeroConditionType, _ABasicHeroCondition, HeroConditionGroupObj
#if NP_GAME
        , HeroRefObj
#endif
        , HeroConditionVarInfo
    >
    {
        protected override HeroConditionGroupObj _createGroupObj()
        {
            return new HeroConditionGroupObj();
        }

        protected override _ABasicHeroCondition _readConditionStr(string _str)
        {
            return _ABasicHeroCondition.readCondition(_str);
        }
        
        /// <summary>
        /// 读取字符串并返回一个读取结束的groupObj对象
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_err"></param>
        /// <returns></returns>
        public static HeroConditionGroupObj readConditionGroupList(string _str, string _err)
        {
            if (string.IsNullOrEmpty(_str))
                return new HeroConditionGroupObj();

            HeroConditionGroupObj obj = new HeroConditionGroupObj();
            obj._readString(_str, 0, _err);

            return obj;
        }

        //对外统一调用的判断处理
        public static bool IsEnable(HeroConditionGroupObj _condGroupList, HeroRefObj _heroRefObj, HeroConditionVarInfo _varVariableInfo)
        {
#if NP_GAME
            if (null == _condGroupList)
                return true;

            return _condGroupList.IsEnable(_heroRefObj, _varVariableInfo);
#else
            return false;
#endif
        }
    }
}