using System.Collections.Generic;
using ALPackage;
using Common.PrivilegeCardEnum;

namespace GOE
{
    [System.Serializable]
    public class GGUIPrivilegeCardType
    {
        [ALHeader("权益卡类型")]
        public EPrivilegeCardType type;
        [ALHeader("权益卡附加窗口")]
        public GGUIMonoSubPrivilegeCard monoSubPrivilegeCard;
    }

    /// <summary>
    /// 权益卡主页面
    /// </summary>
    public class GGUIMonoPrivilegeCardPage : _AALBasicUIWndMono
    {
        [ALHeader("权益卡列表")]
        public List<GGUIPrivilegeCardType> subPrivilegeCardList;
    }
}

