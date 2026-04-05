using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ALPackage;
using NPEnum;

namespace GOE
{
    //指定类型的队伍资源
    public class NPPlayerVariableVarValue : _ANPBasicPlayerVariableObj
    {
        private ENPPlayerVariableVarType _m_eType;

        public ENPPlayerVariableVarType vType { get { return _m_eType; } }

        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_VAR_V; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
#if NP_GAME
            return _variableInfo.getValue(_m_eType);
#else
        return 0L;
#endif
        }

        public static NPPlayerVariableVarValue readVariable(ALStringReader _reader)
        {
            //解析字符串
            string typeS = _reader.readItem('@');
            //逐个判断
            if (null == typeS)
            {
                UnityEngine.Debug.LogError("高级公式配置错误 - VAR_VALUE example: enum:varType Error Str: " + _reader.srcString);
                return null;
            }

            NPPlayerVariableVarValue variableObj = new NPPlayerVariableVarValue();

            try
            {
                variableObj._m_eType = (ENPPlayerVariableVarType)ALCommon.EnumParse(typeof(ENPPlayerVariableVarType), typeS, true);

                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("高级公式配置错误 - VAR_VALUE example: enum:varType Error Str: " + _reader.srcString);
                return null;
            }
        }

    }
}

