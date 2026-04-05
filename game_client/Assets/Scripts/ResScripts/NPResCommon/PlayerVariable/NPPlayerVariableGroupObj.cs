using System;
using System.Text;
using System.Collections.Generic;
using GOE.Variable;
using UnityEngine;
using NPEnum;

namespace GOE
{
    /*********************
     * 参数对象序列号结构体
     **/
    [System.Serializable]
    public class NPPlayerVariableGroupObj : _ATNPBasicVariableGroupObj<ENPPlayerVariableType, _ANPBasicPlayerVariableObj, NPPlayerVariableGroupObj
#if NP_GAME
            , NPPlayer
#endif
        >
    {

        /// <summary>
        /// 创建一个Group对象
        /// </summary>
        /// <returns></returns>
        protected override NPPlayerVariableGroupObj _createGroupObj()
        {
            return new NPPlayerVariableGroupObj();
        }

        /// <summary>
        /// 从字符串读取出对应的条件
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        protected override _ANPBasicPlayerVariableObj _readVariableStr(string _str)
        {
            return _ANPBasicPlayerVariableObj.readVariable(_str);
        }

        /// <summary>
        /// 计算本数据集合的结果
        /// </summary>
        /// <param name="_variableInfo"></param>
        /// <returns></returns>
        public long CalculateVariableResult(NPVarInfo _variableInfo)
        {
#if NP_GAME
            return CalculateVariableResult(NPPlayer.instance, _variableInfo);
#else
            return 0;
#endif
        }

        /// <summary>
        /// 读取字符串并返回一个读取结束的groupObj对象
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_err"></param>
        /// <returns></returns>
        public static NPPlayerVariableGroupObj readVariableGroup(string _str, string _err)
        {
            if (string.IsNullOrEmpty(_str))
                return new NPPlayerVariableGroupObj();

            NPPlayerVariableGroupObj obj = new NPPlayerVariableGroupObj();

            obj._readVariableGroup(_str, 0, _err);

            return obj;
        }

        /**************
         * 根据AI主体判断条件是否匹配
         **/
        public static long CalculateVariableResult(NPPlayerVariableGroupObj _group, NPVarInfo _variableInfo)
        {
#if NP_GAME
            if (null == _group)
                return 0;

            return _group.CalculateVariableResult(NPPlayer.instance, _variableInfo);
#else
            return 0;
#endif
        }
    }
}

