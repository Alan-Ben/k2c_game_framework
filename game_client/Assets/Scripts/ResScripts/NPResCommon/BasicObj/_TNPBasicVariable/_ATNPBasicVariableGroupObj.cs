using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE.Variable
{
    /*********************
     * 参数对象序列号结构体
     **/
    [System.Serializable]
    public abstract class _ATNPBasicVariableGroupObj<E, V, G
#if NP_GAME 
        , D
#endif
        >
        where E : Enum
        where V : _ITNPBasicVariable<E
#if NP_GAME 
            , D
#endif
            >
        where G : _ATNPBasicVariableGroupObj<E, V, G
#if NP_GAME 
            , D
#endif
            >
#if NP_GAME 
        where D : _ITNPVariableDealerData<E>
#endif
    {
        /** 本对象的条件类型 */
        public V _m_vVariableObj;
        public List<G> _m_lChildGroupList;
        public ENPVariableCalType _m_eCalType = ENPVariableCalType.NONE;

        protected _ATNPBasicVariableGroupObj()
        {
            _m_vVariableObj = default(V);
            _m_lChildGroupList = new List<G>();
        }

        public bool hasVariable()
        {
            return (_m_vVariableObj != null)
                || (_m_lChildGroupList != null && _m_lChildGroupList.Count > 0);
        }

        /**************************************
         * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
         **/
        private static char[] _g_arrTotalSplitArr = { '+', '-', '*', '|', '<', '>' };


        //将字符串转化为本对象
        public void readString(string _str, string _err)
        {
            _readVariableGroup(_str, 0, _err);
        }
        protected void _readVariableGroup(string _str, int _depth, string _err)
        {
            //构造读取对象
            ALStringReader stringReader = new ALStringReader(_str);

            if (_depth > 5)
            {
                ALLog.Error(_err);
                ALLog.Error(" Notice!!! Single Variable depth more than 5!!! variableStr: " + _str + " depth: " + _depth);
            }

            if (stringReader.isEmpty)
                return;

            char tmpChar;
            //最后一个运算符，一般用于当前读取的对象设置
            ENPVariableCalType calType = ENPVariableCalType.NONE;

            //读取第一个分隔位置信息
            do
            {
                //读取字符串
                String splitStr = stringReader.readItem(_g_arrTotalSplitArr);
                //获取最后一个分隔符
                tmpChar = stringReader.lastSplitChar;

                //如果读取的数据和原数据长度一致，可以看做是一个独立的元数据
                if (null != splitStr && splitStr.Length == _str.Length)
                {
                    //取完字符串，使用截取的字符串读取对应条件
                    _m_vVariableObj = _readVariableStr(splitStr);
                    //设置计算方式
                    _m_eCalType = calType;
                    //重置计算方式
                    calType = ENPVariableCalType.NONE;
                }
                else
                {
                    //先读取前面的计算对象
                    G newObj = _createGroupObj();
                    newObj._readVariableGroup(splitStr, _depth + 1, _err);
                    if (null != newObj)
                    {
                        //设置计算方式为最后一个计算方式
                        newObj._m_eCalType = calType;
                        //重置计算方式
                        calType = ENPVariableCalType.NONE;

                        //如果队列不为空，而计算方式为none需要报错
                        if (_m_lChildGroupList.Count > 0 && newObj._m_eCalType == ENPVariableCalType.NONE)
                        {
                            ALLog.Error("Variable Cal type is none: " + _str);
                        }

                        //添加数据到集合
                        _m_lChildGroupList.Add(newObj);
                    }

                    //根据结束字符，设置下一个计算公式的计算方式
                    //判断此符号是否(，是则开始一个新集合的读取
                    if (tmpChar == '+')
                    {
                        calType = ENPVariableCalType.ADD;
                    }
                    else if (tmpChar == '-')
                    {
                        calType = ENPVariableCalType.SUB;
                    }
                    else if (tmpChar == '*')
                    {
                        calType = ENPVariableCalType.MUL;
                    }
                    else if (tmpChar == '|')
                    {
                        calType = ENPVariableCalType.DIV;
                    }
                    else if (tmpChar == '>')
                    {
                        calType = ENPVariableCalType.MAX;
                    }
                    else if (tmpChar == '<')
                    {
                        calType = ENPVariableCalType.MIN;
                    }
                }
            } while (!stringReader.isEmpty);
        }

#if NP_GAME
        /// <summary>
        /// 计算本数据集合的结果
        /// </summary>
        /// <param name="_variableInfo"></param>
        /// <returns></returns>
        public long CalculateVariableResult(D _data, NPVarInfo _variableInfo)
        {
            if (_m_lChildGroupList.Count > 0)
                return _CalculateVariableResult(_data, _m_lChildGroupList, _variableInfo);

            return _CalculateVariableResult(_data, _m_vVariableObj, _variableInfo);
        }

        protected long _CalculateVariableResult(D _data, List<G> _groupList, NPVarInfo _variableInfo)
        {
            if (null == _groupList || _groupList.Count <= 0)
                return 0;

            //遍历条件判断
            long result = 0;
            G tmpCond = null;
            for (int i = 0; i < _groupList.Count; i++)
            {
                tmpCond = _groupList[i];
                if (null == tmpCond)
                    continue;

                switch (tmpCond._m_eCalType)
                {
                    case ENPVariableCalType.ADD:
                    case ENPVariableCalType.NONE:
                        {
                            result += tmpCond.CalculateVariableResult(_data, _variableInfo);
                            break;
                        }
                    case ENPVariableCalType.SUB:
                        {
                            result -= tmpCond.CalculateVariableResult(_data, _variableInfo);
                            break;
                        }
                    case ENPVariableCalType.MUL:
                        {
                            result *= tmpCond.CalculateVariableResult(_data, _variableInfo);
                            break;
                        }
                    case ENPVariableCalType.DIV:
                        {
                            long tmpValue = tmpCond.CalculateVariableResult(_data, _variableInfo);
                            if (tmpValue != 0)
                                result /= tmpValue;
                            else
                                result = 0;

                            break;
                        }
                    case ENPVariableCalType.MAX:
                        {
                            long tmpValue = tmpCond.CalculateVariableResult(_data, _variableInfo);
                            if (result < tmpValue)
                                result = tmpValue;

                            break;
                        }
                    case ENPVariableCalType.MIN:
                        {
                            long tmpValue = tmpCond.CalculateVariableResult(_data, _variableInfo);
                            if (result > tmpValue)
                                result = tmpValue;

                            break;
                        }
                    default:
                        break;
                }
            }

            return result;
        }

        protected long _CalculateVariableResult(D _data, V _obj, NPVarInfo _variableInfo)
        {
            if (_obj == null)
            {
                return 0;
            }

            return _obj.calValue(_data, _variableInfo);
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
        protected abstract V _readVariableStr(string _str);
    }
}

