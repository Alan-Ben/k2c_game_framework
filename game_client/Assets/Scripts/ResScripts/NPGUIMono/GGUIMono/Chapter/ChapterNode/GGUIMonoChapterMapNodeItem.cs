using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡 章状态
    /// </summary>
    public enum EChapterMapNodeState
    {
        [InspectorName("未达到")]
        UN_REACH,
        [InspectorName("进行中")]
        UNDERWAY,
        [InspectorName("已完成")]
        COMPLETED,
    }

    [Serializable]
    public class GGUIMonoChapterMapNodeShow
    {
        [ALHeader("章状态")]
        public EChapterMapNodeState state;
        
        [ALHeader("显示的物体列表")]
        public List<GameObject> showGoList;
    }
    
    /// <summary>
    /// 关卡 节点item
    /// </summary>
    public class GGUIMonoChapterMapNodeItem : _AALBasicUIWndMono
    {
        [ALHeader("章序号")]
        public TextEx txtChapterTag;
   
        [ALHeader("node图")]
        public RawImage nodeImg;
        [ALHeader("node图2")]
        public RawImage nodeImg2;
        [ALHeader("遮罩图")]
        public Slider sldMask;
        [ALHeader("遮罩图")]
        public Image maskImg;
        
        [ALHeader("游戏状态显隐配置")]
        public List<NPCommonEnumAniStatInfo<EChapterMapNodeState>> chapterStateShowList;

        [ALHeader("item动画")]
        public Animation ani;
        [ALHeader("默认状态的动画名称")]
        public string defaultStateAniName;
        [ALHeader("转化为已完成状态的动画名称")]
        public string chgToCompletedStateAniName;
        [ALHeader("转化为进行中状态的动画名称")]
        public string chgToUnderwayStateAniName;
        [ALHeader("boss战自动战斗动画名称")]
        public string bossAniName;
        
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        
        [ALHeader("粒子起点")]
        public RectTransform posStart;
        [ALHeader("自动前进特效挂点")]
        public Transform sfxParent;
        [ALHeader("自动前进特效挂id")]
        public long sfxId;

        public void refreshState(EChapterMapNodeState _state)
        {
            if(chapterStateShowList == null)
                return;

            NPCommonEnumAniStatInfo<EChapterMapNodeState>.setStat(chapterStateShowList, _state);
        }
    }
}