using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子星辉等级列表item
    /// </summary>
    public class GGUIMonoConsortHaloLvlContainerItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("icon")]
        public RawImage icon;
        
        [ALHeader("等级")]
        public List<Text> txtLevelList;
        [ALHeader("等级描述key")]
        public string txtLevelKey;

        [ALHeader("星辉解锁后显示")]
        public List<GameObject> haloUnlockShowList;
        [ALHeader("星辉未解锁时隐藏")]
        public List<GameObject> haloLockShowList;
        [ALHeader("未解锁时的置灰列表")]
        public List<MaskableGraphic> lockGrayList;
        
        [ALHeader("等级达到时需要展示的GO列表")]
        public List<GameObject> goLevelReachShowList;
        [ALHeader("等级达到时需要隐藏的GO列表")]
        public List<GameObject> goLevelReachHideList;
    }
}