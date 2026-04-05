using System;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public abstract class _ATBasicConditionSerializeInfo<T_ConditionGroupObj, T_VarInfo
#if NP_GAME
        , T_ConditionDealerData
#endif
    >
        where T_ConditionGroupObj : _IBasicConditionGroupObj
#if NP_GAME
        where T_VarInfo : _IVarInfo
#endif
    {
        public string s_condition;

        //是否已经初始化过数据
        [System.NonSerialized]
        protected bool _m_bIsInited = false;
        //玩家条件集合数据对象
        [System.NonSerialized]
        protected T_ConditionGroupObj _m_cgConditionGroup;

        public _ATBasicConditionSerializeInfo()
        {
            s_condition = string.Empty;

            _m_bIsInited = false;
            _m_cgConditionGroup = default;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool hasCondition
        {
            get
            {
                if (null == condition)
                    return false;

                return condition.hasCondition();
            }
        }
        public bool isEmpty
        {
            get
            {
                if (null == condition)
                    return true;

                return !condition.hasCondition();
            }
        }

        /// <summary>
        /// 使用实际的条件对象，根据是否已经初始化过进行判断和处理
        /// </summary>
        public virtual T_ConditionGroupObj condition
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgConditionGroup;

                _m_bIsInited = true;

                try
                {
                    _m_cgConditionGroup = getConditionGroupObj(s_condition, string.Empty);
                }
                catch(Exception )
                {
                    ALLog.Crush($"read condition:[{s_condition}] Error!!!");
                    _m_cgConditionGroup = default;
                }

                //如果已经读取，则在非编辑器环境下需要设置数据为空，注意这里服务器不能设置为空
#if !UNITY_EDITOR
            s_condition = null;
#endif

                return _m_cgConditionGroup;
            }
        }

        protected abstract T_ConditionGroupObj getConditionGroupObj(string s_condition, string _err);

        /// <summary>
        /// 判断本对象的条件是否匹配
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public abstract bool IsEnable(
#if NP_GAME
            T_ConditionDealerData _conditionDealerData, 
#endif
            T_VarInfo _varVariableInfo);

        /// <summary>
        /// 是否没有条件 或 条件成立(若对这个方法结果取非, 就代表 是否存在条件 且 条件不满足)
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool isNoConditionOrEnable(
#if NP_GAME
            T_ConditionDealerData _conditionDealerData, 
#endif
            T_VarInfo _varVariableInfo)
        {
            return isEmpty || IsEnable(
#if NP_GAME
                _conditionDealerData,
#endif
                _varVariableInfo);
        }
        
        /******************
         * 从带入的字符串内读取属性加成信息
         * 
         * @author alzq.z
         * @time   Aug 27, 2013 10:57:11 PM
         */
        public void ParseFromString(string _str)
        {
            ParseFromString(_str, string.Empty);
        }
        public void ParseFromString(string _str, string _fieldName)
        {
            s_condition = _str;

            _m_bIsInited = false;
            _m_cgConditionGroup = default;
        }
    }
}