using System;

namespace GOE
{
    /// <summary>
    /// 模拟cdn需要用的配置数据
    /// </summary>
    [Serializable]
    public class TempTestConfig
    {
        public string gameUpdatePath;//游戏远程资源更新路径
        public string refdataUpdatePath;//游戏远程资源更新路径
        public string injectFixUpdatePath;//InjectFix更新地址
        public string hotfixUpdatePath;//Hotfix更新地址
        public string audioUpdatePath;//音效资源更新路径
        public string videoUpdatePath;//视频资源更新路径

        public TempTestConfig() { }
        
    }
}