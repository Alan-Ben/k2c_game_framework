using System;

namespace GOE
{
    /// <summary>
    /// 登录服务器地址数据
    /// </summary>
    [Serializable]
    public class LoginServerUrlInfo
    {
        public string loginServerIp;//登录ip
        public string loopLSIpList;//轮询登录ip列表

        public int loginServerPort;//登录端口
        public string loopLSPortList;//轮询登录端口列表

        public string loginServerDelayTime;//延迟登录时间
        public string loopLSDelayTimeList;//轮询延迟登录时间

        public string callback_id;//支付回调地址ID



        public LoginServerUrlInfo() { }

        public override string ToString()
        {
            return $"[{nameof(loginServerIp)}: {loginServerIp}], [{nameof(loopLSIpList)}: {loopLSIpList}], [{nameof(loginServerPort)}: {loginServerPort}], [{nameof(loopLSPortList)}: {loopLSPortList}], [{nameof(loginServerDelayTime)}: {loginServerDelayTime}], [{nameof(loopLSDelayTimeList)}: {loopLSDelayTimeList}], [{nameof(callback_id)}: {callback_id}]";
        }
    }
}