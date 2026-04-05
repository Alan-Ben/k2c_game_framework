using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作日志弹窗列表
    /// </summary>
    public class GGUIMonoGuildCooperateLogGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildCooperateLogGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
        [ALHeader("正在请求数据时的提示")]
        public GameObject goRequestingShow;
    }
}