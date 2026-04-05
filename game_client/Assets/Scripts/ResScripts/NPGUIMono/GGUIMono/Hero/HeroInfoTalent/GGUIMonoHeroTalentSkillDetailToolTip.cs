using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴资质技能详情跟随窗口
    /// </summary>
    public class GGUIMonoHeroTalentSkillDetailToolTip : NPGGUIMonoCommonToolTip
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("解锁星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("伙伴未解锁时需要显示的GO列表")]
        public List<GameObject> goHeroLockShowList;
        [ALHeader("伙伴未解锁时需要隐藏的GO列表")]
        public List<GameObject> goHeroLockHideList;
        [ALHeader("技能未解锁时需要显示的GO列表")]
        public List<GameObject> goSkillLockShowList;
        [ALHeader("技能未解锁时需要隐藏的GO列表")]
        public List<GameObject> goSkillLockHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1029); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1029); } }
    }
}