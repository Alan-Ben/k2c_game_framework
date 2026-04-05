using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟排行界面
    /// </summary>
    public class GGUIMonoGuildRank : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("前三名排行列表（排名按照配置顺序）")]
        public List<GGUIMonoGuildRankGridItem> monoTopRankList;

        [ALHeader("排行列表")]
        public GGUIMonoGuildRankGrid monoRankGrid;
        
        [ALHeader("搜索输入框")]
        public InputField inputSearch;
        [ALHeader("需要搜索功能时显示的GO列表")]
        public List<GameObject> searchFuncOnShowGoList;
        [ALHeader("找到指定联盟时显示(只在有搜索联盟时生效, 没有搜索联盟时隐藏)")]
        public List<GameObject> guildFindShow;
        [ALHeader("未找到指定联盟时显示(只在有搜索联盟时生效, 没有搜索联盟时隐藏)")]
        public List<GameObject> guildNotFindShow;
        
        [ALHeader("自身联盟排行信息")]
        public GGUIMonoGuildRankGridItem selfGuildRankInfo;
    }
}
