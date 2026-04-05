using System.Collections.Generic;
using ALPackage;
using Common.ChildEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultUnmarriedGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成年未婚子嗣的信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALHeader("申请联谊按钮")]
        public GameObject btnSearching;
        [ALHeader("联谊倒计时和取消按钮")]
        public Text txtSearchingTime;
        public GameObject btnCancel;
        [ALHeader("分别是全服联姻，指定联姻，等待联姻的显示对象")]
        public List<GameObject> listServerSearchingShow;
        public List<GameObject> listCustomSearchingShow;
        public List<GameObject> listWaitingShow;

        [ALHeader("有最低村庄收益时限制显示对象")]
        public List<GameObject> hasLowerEarningsLimitShow;
        [ALHeader("有最低村庄收益时限制隐藏对象")]
        public List<GameObject> hasLowerEarningsLimitHide;
        [ALHeader("限制最低村庄收益")]
        public TextEx txtLimitLowerEarnings;
        [ALHeader("限制最低村庄收益key(一个参数, 限制值, 程序会附上大数值单位, 如K,M等)")]
        public string limitLowerEarningsKey;

        public void setSearchingType(EAdultStatus _status)
        {
            ALUGUICommon.setGameObjEnable(listServerSearchingShow, false);
            ALUGUICommon.setGameObjEnable(listCustomSearchingShow, false);
            ALUGUICommon.setGameObjEnable(listWaitingShow, false);
            
            switch (_status)
            {
                case EAdultStatus.APPLY_SERVER:
                    ALUGUICommon.setGameObjEnable(listServerSearchingShow, true);
                    break;
                case EAdultStatus.APPLY_PLAYER:
                    ALUGUICommon.setGameObjEnable(listCustomSearchingShow, true);
                    break;
                case EAdultStatus.NONE:
                    ALUGUICommon.setGameObjEnable(listWaitingShow, true);
                    break;
            }
        }
    }
}