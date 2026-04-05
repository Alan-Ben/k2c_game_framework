using System;

namespace GOE
{
    /// <summary>
    /// 可用登入类型数据
    /// </summary>
    [Serializable]
    public class ClientPlatformConfig
    {
        public string[] login;//登入类型列表
        public string[] charge;//充值类型列表

        public ClientPlatformConfig() { }
        
        public override string ToString()
        {
            return $"[{nameof(login)}: [{string.Join(", ", login)}] ], [{nameof(charge)}: [{string.Join(", ", charge)}] ]";
        }
    }
}