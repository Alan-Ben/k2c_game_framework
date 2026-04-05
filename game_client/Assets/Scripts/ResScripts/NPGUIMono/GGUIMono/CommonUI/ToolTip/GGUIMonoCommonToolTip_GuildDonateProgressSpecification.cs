using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠进度说明 跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_GuildDonateProgressSpecification : NPGGUIMonoCommonToolTip
    {
        [ALHeader("描述使用的key(两个参数, 1.捐赠方式名 2.增加进度值)")]
        public string descKey;
        
        [ALHeader("捐赠类型列表")]
        public NPGGUIMonoCommonTextItemGrid donateTypeGrid;
    }
}