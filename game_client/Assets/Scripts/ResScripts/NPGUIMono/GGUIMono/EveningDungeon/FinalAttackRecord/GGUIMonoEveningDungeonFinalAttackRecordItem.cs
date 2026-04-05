using ALPackage;

namespace GOE
{
    /// <summary>
    /// 晚间副本尾刀记录item
    /// </summary>
    public class GGUIMonoEveningDungeonFinalAttackRecordItem : _AALBasicUIWndMono
    {
        [ALHeader("玩家头像信息")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;

        [ALHeader("尾刀时间")]
        public TextEx txtTime;
    }
}