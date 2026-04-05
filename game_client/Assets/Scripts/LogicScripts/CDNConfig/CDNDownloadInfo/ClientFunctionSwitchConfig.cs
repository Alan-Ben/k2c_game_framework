using System;

namespace GOE
{
    /// <summary>
    /// 功能开关数据
    /// </summary>
    [Serializable]
    public class ClientFunctionSwitchConfig
    {
        public string id;//客户端控件
        public int is_open;//开关状态：0：开启；1:关闭入口（可见不可点）；2:不可见
        public string min_version;//最小兼容版本号，  仅开启状态时存在
        public string max_version;//最大兼容版本号，  仅开启状态时存在

        public ClientFunctionSwitchConfig() { }
        
        public override string ToString()
        {
            return $"[{nameof(id)}: {id}], [{nameof(is_open)}: {is_open}], [{nameof(min_version)}: {min_version}], [{nameof(max_version)}: {max_version}]";
        }
    }
}