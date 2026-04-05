using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 不同技能状态显示列表
    /// </summary>
    [Serializable]
    public class TreasureHuntSkillStateShow
    {
        [ALHeader("技能状态")]
        public ETreasureHuntSkillState state;

        [ALHeader("显示的物体列表")]
        public List<GameObject> showGoList;
        
        [ALHeader("置灰列表")]
        public List<MaskableGraphic> grayList;
        
        [ALHeader("技能效果描述颜色")]
        public Color skillDescColor = Color.white;
    }
    
    /// <summary>
    /// 太空寻宝 - 技能信息子窗口
    /// </summary>
    public class GGUIMonoTreasureHuntSkillInfo : _AALBasicUIWndMono
    {
        [ALHeader("技能图标")]
        public RawImage skillIcon;

        [ALHeader("当前等级")]
        public TextEx txtLevel;

        [ALHeader("当前等级技能效果描述")]
        public TextEx nowLevelSkillDesc;
        
        [ALHeader("下一级技能效果描述")]
        public TextEx nextLevelSkillDesc;
        [ALHeader("下一级技能效果描述key(一个参数, 下一等级技能加成数值)")]
        public string nextLevelSkillDescKey;
        
        [ALHeader("技能状态显示列表")]
        public List<TreasureHuntSkillStateShow> skillStateShowList;

        [ALHeader("操作按钮")]
        public GameObject btnOp;

        [ALHeader("技能点物品")]
        public NPGGUIMonoCommonItem monoSkillPointItem;
        [ALHeader("可升级时显示的物体列表")]
        public List<GameObject> canLevelUpShowGoList;
        [ALHeader("不可升级时显示的物体列表")]
        public List<GameObject> cannotLevelUpShowGoList;

        [ALHeader("激活时播放动画名")]
        public string activeAnimationName;
        [ALHeader("升级时播放动画名")]
        public string levelUpAnimationName;

#if NP_GAME
        public void refreshSkillState(ETreasureHuntSkillState _state, out TreasureHuntSkillStateShow _nowStateShow)
        {
            _nowStateShow = null;
            if (skillStateShowList == null)
                return;

            foreach (var stateShow in skillStateShowList)
            {
                if(stateShow == null)
                    continue;

                if (stateShow.state != _state)
                {
                    ALUGUICommon.setGameObjEnable(stateShow.showGoList, false);
                    GGameCommonInfo.disgrayImage(stateShow.grayList);
                }
                else
                {
                    _nowStateShow = stateShow;
                }
            }

            if (_nowStateShow != null)
            {
                ALUGUICommon.setGameObjEnable(_nowStateShow.showGoList, true);
                GGameCommonInfo.grayImage(_nowStateShow.grayList);
            }
            
            return;
        }
#endif
        
    }
}