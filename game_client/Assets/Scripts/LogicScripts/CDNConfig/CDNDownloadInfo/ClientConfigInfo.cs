using System;

namespace GOE
{
    
    /// <summary>
    /// 客户端配置数据
    /// </summary>
    [Serializable]
    public class ClientConfigInfo
    {
        public string resUpdateCDNUrlRootList;//资源更新CDN URL root列表（每家cdn提供商一个地址）      数据来源：平台列表中的CDN地址，可以配置多个CDN地址用'||'双竖线间隔
        public string resUpdateRawUrlRootList;//资源更新源站URL root列表                             数据来源：平台列表中的源站地址，可以配置多个源站地址用'||'双竖线间隔
        public string areaUpdatePath;//游戏区域资源更新路径（相对url root）                  数据来源：客户端管理列表中区域资源版本号和指定文件夹(areares)  如：areares\0.0.0.1
        public string gameUpdatePath;//游戏远程资源更新路径（相对url root）                  数据来源：客户端管理列表中区游戏资源版本号和指定文件夹(gameres)  如：gameres\0.0.0.1
        public string hotfixUpdatePath;//游戏ILR热更代码更新路径（相对url root）               数据来源：客户端管理列表中"Script代码资源版本号"和指定文件夹(hotfix)  如：hotfix\0.0.0.1
        public int platformId;//后台平台ID（可能用不到，打印记得打）                   数据来源：平台列表中"自定义ID"
        public string customerServiceURL;//客服URL                                             数据来源：渠道列表中"客服页面URL"
        public string customerServiceEmail;//客服邮箱                                            数据来源：渠道列表中"客服email"
        public bool isOpenPHPAD;//是否开启PHP埋点，客户端开关（具体判断是否打开的逻辑在PHP判断）（先传空）
        public string phpADUrl;//游戏埋点地址
        public string injectFixUpdatePath;//InjectFix更新地址（相对url root） 
        public string refdataUpdatePath;
        public string videoUpdatePath;
        public string audioUpdatePath;//游戏远程音效资源更新路径（相对url root）                  数据来源：客户端管理列表中区游戏音效资源版本号和指定文件夹(audio)  如：audio\0.0.0.1
        public string noticeUpdatePath;
        public int clientHotfixUpdateType;//客户端热更提示类型  1 强；2：否；3：提示
        public int clientUpdateType;//客户端包更新方式  1 强；2：否；3：提示
        public string newClientVersion;//客户端最新版本
        public string newClientUpdateUrl;//客户端新包更新地址
        
        public string phpLoopADURL;
        public int phpADLevel;
        
        public bool isFormalPlatform;//是否是正式服平台
        public bool isShowPolicy;//是否展示隐私协议
        public bool isShowATT;//是否展示ATT协议（Apple AppTracking Transparency）
        
        public ClientConfigInfo() { }

        public override string ToString()
        {
            return $"{nameof(resUpdateCDNUrlRootList)}: {resUpdateCDNUrlRootList}, {nameof(resUpdateRawUrlRootList)}: {resUpdateRawUrlRootList}, {nameof(areaUpdatePath)}: {areaUpdatePath}, {nameof(gameUpdatePath)}: {gameUpdatePath}, {nameof(hotfixUpdatePath)}: {hotfixUpdatePath}, {nameof(platformId)}: {platformId}, {nameof(customerServiceURL)}: {customerServiceURL}, {nameof(customerServiceEmail)}: {customerServiceEmail}, {nameof(isOpenPHPAD)}: {isOpenPHPAD}, {nameof(phpADUrl)}: {phpADUrl}, {nameof(injectFixUpdatePath)}: {injectFixUpdatePath}, {nameof(refdataUpdatePath)}: {refdataUpdatePath}, {nameof(audioUpdatePath)}: {audioUpdatePath}, {nameof(clientHotfixUpdateType)}: {clientHotfixUpdateType}, {nameof(phpLoopADURL)}: {phpLoopADURL}, {nameof(phpADLevel)}: {phpADLevel}, {nameof(isFormalPlatform)}: {isFormalPlatform}, {nameof(isShowPolicy)}: {isShowPolicy}, {nameof(isShowATT)}: {isShowATT}, {nameof(noticeUpdatePath)}: {noticeUpdatePath}";
        }
    }
}