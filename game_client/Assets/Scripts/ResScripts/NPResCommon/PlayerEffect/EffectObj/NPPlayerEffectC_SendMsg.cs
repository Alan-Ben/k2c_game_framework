using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 发送客户端自定义消息
    /// </summary>
    public class NPPlayerEffectC_SendMsg : _ANPPlayerEffectInfo
    {
        private WinMsgType _m_winMsgType;//舞台资源下标字符串

        public NPPlayerEffectC_SendMsg()
        {
            _m_winMsgType = WinMsgType.NONE;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SEND_MSG; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            WinMsg.SendMsg(_m_winMsgType);
#endif
        }

        public static NPPlayerEffectC_SendMsg readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 1)
            {
                Debug.LogError("配置错误 - C_SEND_MSG   example: C_SEND_MSG:WinMsgType Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_SendMsg effectObj = new NPPlayerEffectC_SendMsg();

            try
            {
                effectObj._m_winMsgType = (WinMsgType)ALCommon.EnumParse(typeof(WinMsgType), strs[0], true);;

                return effectObj;
            }
            catch (Exception)
            {
                Debug.LogError("配置错误 - C_SEND_MSG   example: C_SEND_MSG:WinMsgType Error Str: " + _str);
                return null;
            }
        }
    }
}

