using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineDetailInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("横幅图片")]
        public RawImage imgBanner;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("资源图标")]
        public NPGGUIMonoCommonItem monoResourceIcon;
        [ALHeader("采集玩家名称")]
        public Text txtPlayerName;
        [ALHeader("自己名称颜色")]
        public Color selfNameColor = Color.white;
        [ALHeader("盟友名称颜色")]
        public Color guildMemberNameColor = Color.white;
        [ALHeader("其他玩家名称颜色")]
        public Color enemyNameColor = Color.white;
        
        [ALHeader("剩余采集资源数量")]
        public Text txtRemainResourceNum;
        [ALHeader("已采集资源数量")]
        public Text txtHasCollectResourceNum;
        [ALHeader("剩余采集时间")]
        public Text txtRemainCollectTime;
        
        [ALHeader("采集速度")]
        public Text txtCollectSpeed;

        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon playerHeadIcon;
        [ALHeader("队伍序号")]
        public TextEx txtTeamOrder;
        [ALHeader("队伍带兵量")]
        public TextEx txtTroopNum;
        [ALHeader("队伍实力")]
        public TextEx txtTeamPower;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7404); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7404); } }
    }
}