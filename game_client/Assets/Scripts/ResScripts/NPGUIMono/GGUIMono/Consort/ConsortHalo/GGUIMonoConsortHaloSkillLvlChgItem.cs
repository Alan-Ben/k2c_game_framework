using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 星辉技能等级变更Item
    /// </summary>
    public class GGUIMonoConsortHaloSkillLvlChgItem : _AALBasicUIWndMono
    {
        [ALHeader("技能图标")]
        public RawImage skillIcon;

        [ALHeader("技能名")]
        public TextEx txtSkillName;

        [ALHeader("上一等级")]
        public TextEx txtPreLvl;
        [ALHeader("下一等级")]
        public TextEx txtNextLvl;

        [ALHeader("加成类型描述")]
        public TextEx txtAddTypeDesc;

        [ALHeader("上一等级加成值")]
        public TextEx txtPreLvlAddValue;
        [ALHeader("下一等级加成值")]
        public TextEx txtNextLvlAddValue;
    }
}