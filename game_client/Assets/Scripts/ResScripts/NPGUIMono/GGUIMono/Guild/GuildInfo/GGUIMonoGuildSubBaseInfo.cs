using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟基础信息附加窗口
    /// </summary>
    public class GGUIMonoGuildSubBaseInfo : _AALBasicUIWndMono
    {
        [ALHeader("联盟旗帜")]
        public RawImage imgFlagIcon;
        [ALHeader("联盟名称")]
        public Text txtGuildName;
        [ALHeader("联盟名称，前面带服务器名")]
        public Text txtGuildNameWithServerName;
        [ALHeader("联盟等级")]
        public Text txtGuildLevel;
        [ALHeader("联盟成员数量")]
        public Text txtGuildMemberCount;
        [ALHeader("联盟经验(当前/升级所需)")]
        public Text txtGuildExp;
        [ALHeader("联盟经验(当前)")]
        public Text txtGuildExpCur;
        [ALHeader("联盟总国力(赚速)")]
        public Text txtGuildNationPower;
        [ALHeader("联盟ID")]
        public Text txtGuildId;
        [ALHeader("联盟宣言")]
        public Text txtGuildDeclaration;
        [ALHeader("盟主名称")]
        public Text txtGuildLeaderName;
        [ALHeader("联盟财富值")]
        public Text txtGuildWealth;
        [ALHeader("联盟经验条")]
        public Slider sldGuildExp;
        [ALHeader("服务器信息")]
        public Text txtServer;
        [ALHeader("服务器信息不带翻译key")]
        public Text txtServerEx;

        [ALHeader("加入条件描述")]
        public TextEx txtJoinLimitDesc;
        [ALHeader("多加入条件描述连接符")]
        public string joinLimitDescConnectKey;
    }
}
