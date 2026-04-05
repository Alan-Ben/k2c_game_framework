using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;
using ALPackage;

namespace GOE
{
    public class NPPlayerVariableNum : _ANPBasicPlayerVariableObj
    {
        /** 具体数字 */
        private int _m_value;

        public int Value { get { return _m_value; } }

        protected NPPlayerVariableNum()
        {
            _m_value = 0;
        }

        /******************
         * 获取条件类型
         */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_NUM; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
#if NP_GAME
            return _m_value;
#else
        return 0L;
#endif
        }

        public static NPPlayerVariableNum readVariable(ALStringReader _reader)
        {
            //解析字符串
            string valueS = _reader.readItem('@');
            //逐个判断
            if (null == valueS)
            {
                UnityEngine.Debug.LogError("高级公式——数字 - CS_NUM example: ENPPlayerVariableType@number Error Str: " + _reader.srcString);
                return null;
            }

            NPPlayerVariableNum variableObj = new NPPlayerVariableNum();

            //解析字符串
            variableObj._m_value = int.Parse(valueS);

            return variableObj;
        }
    }
}

