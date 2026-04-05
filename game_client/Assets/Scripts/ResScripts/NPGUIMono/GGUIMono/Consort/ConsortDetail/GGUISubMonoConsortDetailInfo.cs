using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息子窗口
    /// </summary>
    public class GGUISubMonoConsortDetailInfo : _AALBasicUIWndMono
    {
        [ALHeader("妃子名TextEx")]
        public TextEx txtConsortName;
        [ALHeader("妃子名TextMesh")]
        public TMP_Text txtConsortNameMesh;
        
        [ALHeader("妃子称号")]
        public TextEx txtConsortTitle;
        [ALHeader("出生地")]
        public TextEx txtConsortBirthPlace;
        [ALHeader("妃子描述")]        
        public TextEx txtDesc;
        
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        
        [ALHeader("皮肤名")]
        public TextEx txtConsortSkinName;
        [ALHeader("当穿着默认皮肤时显示的物体列表")]
        public List<GameObject> onWearingDefaultSkinShow;
        [ALHeader("当穿着默认皮肤时隐藏的物体列表(可理解为穿着非默认皮肤时显示的物体列表)")]
        public List<GameObject> onWearingDefaultSkinHide;
        
        [ALHeader("妃子半身像")]
        public RawImage consortRawImage;
        
        [ALHeader("妃子td showcase")]
        public GGUIMonoCommonShowCase monoShowcase;
        [ALHeader("妃子形象在td showcase中的index(配置小于0的值时不显示)")]
        public int consortActorInShowCaseIndex = 0;
        [ALHeader("背景在td showcase中的index(配置小于0的值时不显示)")]
        public int bgInShowCaseIndex = 3;
        [ALHeader("进入时候妃子播放的动画名")]
        public string enterAniName; 
        
        
        [ALHeader("亲密度翻译key，放空仅展示数值")]
        public string intimacyTransKey; 
        [ALHeader("亲密度")]
        public TextEx txtIntimacy; 
        [ALHeader("魅力翻译key，放空仅展示数值")]
        public string charmTransKey; 
        [ALHeader("魅力")] 
        public TextEx txtCharm;
        [ALHeader("妃子产出来源")] 
        public TextEx txtConsortSource; 
    
        [ALHeader("解锁状态配置")] 
        public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> unlockStatInfos;
        [ALHeader("未解锁置灰列表")]
        public List<MaskableGraphic> lockGrayList;


#if NP_GAME
        public void setUnlockState(EGameCommonUnlockType _lockState)
        {
            if(unlockStatInfos == null)
                return;
            
            NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(unlockStatInfos, _lockState);
            
            if (_lockState == EGameCommonUnlockType.LOCK)
            {
                GGameCommonInfo.grayImage(lockGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(lockGrayList);
            }
        }  
#endif
    }
}