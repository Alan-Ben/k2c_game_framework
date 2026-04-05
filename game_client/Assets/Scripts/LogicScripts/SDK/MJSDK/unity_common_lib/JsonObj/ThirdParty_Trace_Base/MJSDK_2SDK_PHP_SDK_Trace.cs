

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(第三方平台-Php SDK埋点信息)消息结构体
    /// </summary>
    [System.Serializable]
    public class MJSDK_2SDK_PHP_SDK_Trace : MJSDK_2SDK_Base
    {
        //事件节点
        public string step;
        //版本信息
        public string login_tag;
        //扩展字段
        public string extend;
    }
}