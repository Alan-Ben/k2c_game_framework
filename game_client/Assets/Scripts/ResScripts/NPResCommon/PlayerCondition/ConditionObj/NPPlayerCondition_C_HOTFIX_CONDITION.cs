using System;
using System.Text;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 47 === 热更条件类型 C_HOTFIX_CONDITION:热更条件类型:参数
    /// </summary>
    public class NPPlayerCondition_C_HOTFIX_CONDITION : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_HOTFIX_CONDITION; } }

        public string condition_type;//条件类型
        public string condition_info_str;//条件参数
        
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
            object result = null;
            try
            {
#if NP_GAME
                //调用热更的条件判断处理接口
                result = ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixPlayerCondition", "isEnable",_varVariableInfo, condition_type, condition_info_str);
#endif
            }
            catch(Exception _e)
            {
                UnityEngine.Debug.LogError("deal ENPPlayerConditionType.C_HOTFIX_CONDITION error" + _e.ToString());
            }

            if (result != null && result is bool)
            {
                return (bool)result;
            }

            return false;
        }
        
        /// <summary>
        /// 主工程调用读取热更条件字符串
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_HOTFIX_CONDITION readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_HOTFIX_CONDITION cond = new NPPlayerCondition_C_HOTFIX_CONDITION();
            try
            {
                if (null == _reader)
                    return null;

                string judgeFuncS = _reader.readItem(':');
                string infoS = _reader.readItem(':');

                if (null == judgeFuncS || null == infoS)
                {
                    UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.C_HOTFIX_CONDITION[" + _reader.srcString + "]");
                    return null;
                }

                cond.condition_type = judgeFuncS;
                cond.condition_info_str = infoS;

                return cond;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError($"条件配置错误 - NPPlayerCondition_C_HOTFIX_CONDITION example: C_HOTFIX_CONDITION:HOTFIX_TYPE:PARAM， Error Str: C_HOTFIX_CONDITION:{_reader.srcString}");
                return null;
            }
        }
       
    }
}