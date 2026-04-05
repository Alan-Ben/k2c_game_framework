using System;
using System.Collections.Generic;

using UnityEngine;

using ALPackage;
using NPEnum;

namespace GOE.Condition
{
    /****************
     * 记录属性信息的具体对象
     **/
    public abstract class _ATNPBasicConditionGroupObj<E, C, G
#if NP_GAME 
        , D 
#endif
        , T_VarInfo
        > : _IBasicConditionGroupObj
        where E : Enum
        where C : _ITNPBasicCondition<E
#if NP_GAME 
            , D 
#endif
            , T_VarInfo
            >
        where G : _ATNPBasicConditionGroupObj<E, C, G
#if NP_GAME 
            , D 
#endif
            , T_VarInfo
            >
#if NP_GAME 
        where D : _ITNPConditionDealerData<E>
#endif
        where T_VarInfo : _IVarInfo
    {
        /** 本对象的条件类型 */
        protected internal bool _m_bTrueEnable;  //是否正向正确判断
        protected internal C _m_cConditionObj;

        protected internal List<G> _m_lChildConditionList;
        protected internal EWCGJudgeChar _m_eJudgeType = EWCGJudgeChar.NONE;

        public _ATNPBasicConditionGroupObj()
        {
            _m_bTrueEnable = true;
            _m_cConditionObj = default(C);
            _m_lChildConditionList = new List<G>();
        }

        public bool hasCondition()
        {
            return (_m_cConditionObj != null)
                    || (_m_lChildConditionList != null && _m_lChildConditionList.Count > 0);
        }

        /**************************************
        * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
        **/
        public static char _g_or = '|';
        public static char _g_and = '#';
        private static char _g_not = '!';
        public static char[] _g_arrTotalSplitArr = { _g_or, _g_and, _g_not };

        //将字符串转化为本对象
        public void ParseFromString(string _str)
        {
            _readString(_str, 1, string.Empty);
        }
        protected void _readString(string _str, int _depth, string _err)
        {
            //构造读取对象
            ALStringReader stringReader = new ALStringReader(_str);

            if (_depth > 5)
            {
                Debug.LogError(_err);
                Debug.LogError(" Notice!!! Single Condition depth more than 5!!! conditionStr: " + _str + " depth: " +
                                _depth);
            }

            if (stringReader.isEmpty)
                return;

            char tmpChar;
            bool isTrueEnable = true;

            //读取第一个分隔位置信息
            do
            {
                //读取字符串
                string splitStr = stringReader.readItem(_g_arrTotalSplitArr);
                //获取最后一个字符
                tmpChar = stringReader.lastSplitChar;

                //如果读取的数据和原数据长度一致，可以看做是一个独立的条件
                if (null != splitStr && splitStr.Length == _str.Length)
                {
                    //UnityEngine.ALServerLog.Error(index + " - " + startIdx + " single conddi: " + infoStr.ToString());
                    _m_cConditionObj = _readConditionStr(splitStr);
                    //设置正向还是反向判断
                    _m_bTrueEnable = isTrueEnable;
                    //重置正向
                    isTrueEnable = true;
                }
                else
                {
                    //此时表明有进行了字符拆分，先判断后续字符信息
                    if (tmpChar == _g_or)
                    {
                        //UnityEngine.ALServerLog.Error("set or");
                        //取出字符串进行处理
                        if (_m_eJudgeType == EWCGJudgeChar.AND)
                        {
                            ALLog.Error(_err);
                            ALLog.Error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                        }
                        _m_eJudgeType = EWCGJudgeChar.OR;
                    }
                    else if (tmpChar == _g_and)
                    {
                        //UnityEngine.ALServerLog.Error("set and");
                        //取出字符串进行处理
                        if (_m_eJudgeType == EWCGJudgeChar.OR)
                        {
                            ALLog.Error(_err);
                            ALLog.Error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                        }
                        _m_eJudgeType = EWCGJudgeChar.AND;
                    }
                    else if (tmpChar == _g_not)
                    {
                        //UnityEngine.ALServerLog.Error("set not");
                        //设置下一个判断条件反向
                        isTrueEnable = !isTrueEnable;
                    }//判断此符号是否(，是则开始一个新集合的读取

                    //继续进行条件组合读取
                    if (null != splitStr)
                    {
                        //读取内容对象
                        //UnityEngine.ALServerLog.Error(" sub conddi11: " + infoStr.ToString());
                        G newObj = _createGroupObj();
                        newObj._readString(splitStr, _depth + 1, _err);
                        if (null != newObj)
                        {
                            //设置正向还是反向判断
                            newObj._m_bTrueEnable = isTrueEnable;
                            //重置正向
                            isTrueEnable = true;
                            if (null != newObj)
                                _m_lChildConditionList.Add(newObj);
                        }
                    }
                }
            } while (!stringReader.isEmpty);
        }

#if NP_GAME
        /// <summary>
        /// 判断你条件是否匹配
        /// </summary>
        /// <param name="_conditionGroupObj"></param>
        /// <param name="_userData"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool IsEnable(D _userData, T_VarInfo _varVariableInfo)
        {
            if (!hasCondition())
                return true;

            //如果为none表示使用子队列条件
            if (null != _m_cConditionObj)
                return _isEnable(_userData, _m_cConditionObj, _varVariableInfo) == _m_bTrueEnable;

            return _isEnable(_userData, _m_lChildConditionList, _varVariableInfo, _m_eJudgeType) == _m_bTrueEnable;
        }
        private bool _isEnable(D _userData, C _condition, T_VarInfo _varVariableInfo)
        {
            if (null == _condition)
                return true;

            return _condition.isEnable(_userData, _varVariableInfo);
        }
        private bool _isEnable(D _userData, List<G> _condGroupList, T_VarInfo _varVariableInfo, EWCGJudgeChar _type)
        {
            if (null == _condGroupList || _condGroupList.Count <= 0)
                return true;

            //遍历条件判断
            G tmpCond = null;
            for (int i = 0; i < _condGroupList.Count; i++)
            {
                tmpCond = _condGroupList[i];
                if (null == tmpCond)
                    continue;

                if (_type == EWCGJudgeChar.OR)
                {
                    //只要一个组合通过则通过
                    if (tmpCond.IsEnable(_userData, _varVariableInfo))
                        return true;
                }
                else if (_type == EWCGJudgeChar.AND)
                {
                    //只要一个组合不通过则不通过
                    if (!tmpCond.IsEnable(_userData, _varVariableInfo))
                        return false;
                }
                else
                {
                    //只要一个组合不通过则不通过
                    if (!tmpCond.IsEnable(_userData, _varVariableInfo))
                        return false;
                }
            }

            if (_type == EWCGJudgeChar.OR)
                return false;
            else if (_type == EWCGJudgeChar.AND)
                return true;

            return true;
        }
#endif

        /// <summary>
        /// 创建一个Group对象
        /// </summary>
        /// <returns></returns>
        protected abstract G _createGroupObj();

        /// <summary>
        /// 从字符串读取出对应的条件
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        protected abstract C _readConditionStr(string _str);
    }
}

