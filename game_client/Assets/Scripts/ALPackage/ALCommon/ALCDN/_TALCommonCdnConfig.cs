using System;

namespace ALPackage
{
    /// <summary>
    /// 通用CDN配置结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class _TALCommonCdnConfig<T>
    {
        public string version;//最新的客户端配置版本
        public string next_version;//下一个客户端配置版本
        public T config;//具体配置

        public _TALCommonCdnConfig() {}
    }
}