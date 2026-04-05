using System;
using System.Text;
using System.Collections.Generic;
using GOE.Condition;
using UnityEngine;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class NPPlayerConditionGroupObj : _ATNPBasicConditionGroupObj<ENPPlayerConditionType, _ANPBasicPlayerCondition, NPPlayerConditionGroupObj
        #if NP_GAME
            , NPPlayer
        #endif
            , NPVarInfo
        >
    {
        /// <summary>
        /// 创建一个Group对象
        /// </summary>
        /// <returns></returns>
        protected override NPPlayerConditionGroupObj _createGroupObj()
        {
            return new NPPlayerConditionGroupObj();
        }

        /// <summary>
        /// 从字符串读取出对应的条件
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        protected override _ANPBasicPlayerCondition _readConditionStr(string _str)
        {
            return _ANPBasicPlayerCondition.readCondition(_str);
        }

        /// <summary>
        /// 读取字符串并返回一个读取结束的groupObj对象
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_err"></param>
        /// <returns></returns>
        public static NPPlayerConditionGroupObj readConditionGroupList(string _str, string _err)
        {
            if (string.IsNullOrEmpty(_str))
                return new NPPlayerConditionGroupObj();

            NPPlayerConditionGroupObj obj = new NPPlayerConditionGroupObj();

            obj._readString(_str, 0, _err);

            return obj;
        }

        //对外统一调用的判断处理
        public bool IsEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return IsEnable(NPPlayer.instance, _varVariableInfo);
#else
            return false;
#endif
        }

        //对外统一调用的判断处理
        public static bool IsEnable(NPPlayerConditionGroupObj _condGroupList, NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            if (null == _condGroupList)
                return true;

            return _condGroupList.IsEnable(NPPlayer.instance, _varVariableInfo);
#else
            return false;
#endif
        }
    }
}
