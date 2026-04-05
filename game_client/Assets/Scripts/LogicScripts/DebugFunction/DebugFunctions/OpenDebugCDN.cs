using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GOE
{
    /// <summary>
    /// {"cdnUrl":"aaa","channelId":"123"}这种格式
    /// </summary>
    public class OpenDebugCDN : AFileDebugFunction
    {
        public OpenDebugCDN(string _debugFileName) : base(_debugFileName)
        {
        }

        /// <summary>
        /// 调试文件是否有内容
        /// </summary>
        protected override bool hasContent
        {
            get { return true; }
        }

        /// <summary>
        /// 根据Debug文件内的内容，刷新自己需要的信息
        /// </summary>
        /// <param name="_content"></param>
        protected override void _doRefreshStatus(string _content)
        {
            Dictionary<string, string> settingDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(_content);
            //不能用字典的StringComparer.OrdinalIgnoreCase，因为JsonConvert创建的字典，key还是会大写，用了ignoreCase，只是把外面要比较的值强行转成小写
            //内部key是大写的话就永远匹配不到了
            foreach (KeyValuePair<string,string> keyValuePair in settingDict)
            {
                if(keyValuePair.Key != null)
                {
                    Game.instance.debugCdnSetting.Add(keyValuePair.Key.ToLowerInvariant(), keyValuePair.Value);
                }
            }
        }

        /// <summary>
        /// 调试功能开启时，要如何调用其他逻辑
        /// </summary>
        protected override void _doOpenFunction()
        {
            Game.instance.IsDebugCDN = true;

            if (MainCameraMono.selfInstance != null)
            {
                if (Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.CDN_URL))
                {
                    MainCameraMono.selfInstance.platInfo.phpUrlForLoginList = new List<string>(){Game.instance.debugCdnSetting[DebugCdnConst.CDN_URL]};
                }
                if (Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.CHANNEL_ID))
                {
                    Game.instance.mainCamera.platInfo.channelId = int.Parse(Game.instance.debugCdnSetting[DebugCdnConst.CHANNEL_ID]);   
                }
            }
        }

        /// <summary>
        /// 调试功能关闭时，要如何调用其他逻辑
        /// </summary>
        protected override void _doCloseFunction()
        {
            Game.instance.IsDebugCDN = false;
        }
    }
}