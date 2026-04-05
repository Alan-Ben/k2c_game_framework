using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;
using ALPackage;

namespace GOE
{
    public class NPPlayerVariableRnd : _ANPBasicPlayerVariableObj
    {
        /** 具体上限值 */
        private int _m_iMaxValue;

        public int maxValue { get { return _m_iMaxValue; } }

        protected NPPlayerVariableRnd()
        {
            _m_iMaxValue = 0;
        }

        /******************
         * 获取条件类型
         */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.C_RND; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
#if NP_GAME
            return UnityEngine.Random.Range(0, _m_iMaxValue);
#else
        return 0L;
#endif
        }

        public static NPPlayerVariableRnd readVariable(ALStringReader _reader)
        {
            //解析字符串
            string maxVS = _reader.readItem('@');
            //逐个判断
            if (null == maxVS)
            {
                UnityEngine.Debug.LogError("高级公式——随机数 - C_RND example: ENPPlayerVariableType@maxNumber Error Str: " + _reader.srcString);
                return null;
            }

            NPPlayerVariableRnd variableObj = new NPPlayerVariableRnd();

            //解析字符串
            variableObj._m_iMaxValue = int.Parse(maxVS);

            return variableObj;
        }
    }
}

