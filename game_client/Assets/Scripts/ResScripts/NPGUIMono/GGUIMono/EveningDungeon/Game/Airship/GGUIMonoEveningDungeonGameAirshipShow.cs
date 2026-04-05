using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 飞船显示
    /// </summary>
    public class GGUIMonoEveningDungeonGameAirshipShow : _AALBasicUIWndMono
    {
        [ALHeader("飞船形象管理器")]
        public GGUIMonoEveningDungeonGameAirshipActorMgr<EQuality> airshipActorMgr;
        
        [ALHeader("无飞船时的显示动画名称")]
        public string noAirShipShowAnimName;
        
        [ALHeader("当前主体飞船状态动画信息")]
        public List<NPCommonEnumAniStatInfo<EEveningDungeonGameAirshipState>> airshipStateAniStatInfoList;

        [ALHeader("飞船入场状态持续时间(秒), 时间到后会进行状态切换")]
        public float airshipEntryStateTime;
        [ALHeader("飞船攻击状态持续时间(秒), 时间到后会进行状态切换")]
        public float airshipAttackStateTime;
        [ALHeader("飞船离场状态持续时间(秒), 时间到后会进行状态切换")]
        public float airshipDepartureStateTime;
    }
}