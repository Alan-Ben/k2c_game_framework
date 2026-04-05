using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineInfo : _AALBasicUIWndMono
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
        [ALHeader("资源量")]
        public Text txtResourceAmount;
        [ALHeader("队伍实力")]
        public TextEx txtTeamPower;
        [ALHeader("采集玩家名称")]
        public Text txtPlayerName;
        [ALHeader("自己名称颜色")]
        public Color selfNameColor = Color.white;
        [ALHeader("盟友名称颜色")]
        public Color guildMemberNameColor = Color.white;
        [ALHeader("其他玩家名称颜色")]
        public Color enemyNameColor = Color.white;
        [ALHeader("采集进度条")]
        public Slider sldCollectProgress;
        [ALHeader("剩余采集资源数量")]
        public Text txtRemainResourceNum;
        [ALHeader("剩余采集时间")]
        public Text txtRemainCollectTime;
        [ALHeader("预测的采集时间")]
        public Text txtPredictCollectTime;
        [ALHeader("采集速度")]
        public Text txtCollectSpeed;
        [ALHeader("前往采集按钮")]
        public GameObject btnGoCollect;
        [ALHeader("返回按钮")]
        public GameObject btnReturnFromCollect;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("各种状态下显示的内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listCollectingShow;
        public List<GameObject> listEnemyOccupyShow;
        public List<GameObject> listGuildMemberShow;
        [ALHeader("选择队伍时相关的动画")]
        public Animation anim;
        public string teamSelectStartAnimName;
        public string teamSelectCancelAnimName;
        [ALHeader("加载中显示的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        [ALHeader("分享按钮")]
        public GameObject btnShare;
        
        
        public void setState(bool _isEmpty, bool _isSelf, bool _isGuildMember)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listCollectingShow, false);
            ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, false);

            if (_isEmpty)
                ALUGUICommon.setGameObjEnable(listEmptyShow, true);
            else if (_isSelf)
                ALUGUICommon.setGameObjEnable(listCollectingShow, true);
            else if(_isGuildMember)
                ALUGUICommon.setGameObjEnable(listGuildMemberShow, true);
            else
                ALUGUICommon.setGameObjEnable(listEnemyOccupyShow, true);
        }
        public void setLoadingShow(bool _isLoading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isLoading ? listLoadingShow : listLoadingHide, true);
        }


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7411); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7411); } }
    }
}