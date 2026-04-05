using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class RankShowSetting
    {
        [ALHeader("排行范围, 配置-1代表无限")]
        public WCGIntRange rankRange;

        public List<GameObject> showGoList;
    }
    
    /// <summary>
    /// 联盟排行列表item
    /// </summary>
    public class GGUIMonoGuildRankGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("不同排名的显示配置")]
        public List<RankShowSetting> rankShowSettingList;
        
        [ALHeader("排名")]
        public Text txtRank;
        
        [ALHeader("联盟基础信息脚本")]
        public GGUIMonoGuildSubBaseInfo guildBaseInfoMono;
        
        [ALHeader("加入联盟按钮")]
        public GGUIMonoJoinGuildBtn joinGuildBtn;

        [ALHeader("数据为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("数据为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
        
        /// <summary>
        /// 设置排名
        /// </summary>
        /// <param name="_rank"></param>
        public void setRank(int _rank)
        {
            RankShowSetting rankShowSetting = null;
            foreach (RankShowSetting item in rankShowSettingList)
            {
                if(item == null)
                    continue;

                ALUGUICommon.setGameObjEnable(item.showGoList, false);
                
                if(item.rankRange != null && item.rankRange.inRange(_rank))
                {
                    rankShowSetting = item;
                }
            }
            
            if(rankShowSetting != null)
                ALUGUICommon.setGameObjEnable(rankShowSetting.showGoList, true);
            
            //设置排名
            ALUGUICommon.setLabelTxt(txtRank, _rank);
        }
    }
}
