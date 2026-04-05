using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟委托处理记录item
    /// </summary>
    public class GGUIMonoGuildEntrustRecordItem : _TALUGUIMonoGridItem
    {
        [ALHeader("不同排名的显示配置")]
        public List<RankShowSetting> rankShowSettingList;
        [ALHeader("排行")]
        public TextEx txtRank;

        [ALHeader("自己的委托处理记录显示")]
        public List<GameObject> selfItemShowList;
        [ALHeader("其他人的委托处理记录显示")]
        public List<GameObject> otherItemShowList;
        
        [ALHeader("联盟成员信息")]
        public GGUIMonoGuildMemberInfo monoMemberInfo;

        [ALHeader("委托处理的计数")]
        public TextEx txtEntrustDealCount;
        
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