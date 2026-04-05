using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 星辉技能item
    /// </summary>
    public class GGUIMonoConsortHaloSkillLvlItem : _AALBasicUIWndMono
    {
        [ALHeader("技能图标")]
        public RawImage skillIcon;
        
        [ALHeader("技能名")]
        public TextEx txtSkillName;

        [ALHeader("技能等级")]
        public TextEx txtSkillLvl;

        [ALHeader("技能加成类型描述")]
        public TextEx txtSKillAddTypeDesc;
        [ALHeader("技能加成值描述")]
        public TextEx txtSkillAddValue;
    }
}