using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    /// 妃子入口
    /// </summary>
    public class CustomMonoGuildMarsHelpDeal : MonoBehaviour
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("显示数量")]
        public Text txtCount;
        
        [ALHeader("1个求助的状态隐藏的列表")]
        public List<GameObject> oneAssistHideList;
        [ALHeader("没有互助需要处理隐藏的go列表")]
        public List<GameObject> hideGoList;

#if NP_GAME
        
        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnClick, _onBtnClick);
        }

        private void OnEnable()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _refreshWnd);
        }
        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _refreshWnd);
        }
        

        private void _refreshWnd()
        {
            int canDealCount = NPPlayer.instance.guildMarsHelpComp.canDealCount;
            bool oneAssist = canDealCount <= 1;

            ALUGUICommon.setLabelTxt(txtCount,  oneAssist ? "" : canDealCount.ToString());
            if (NPPlayer.instance.guildComp.isJoinGuild()&& canDealCount > 0)
            {
                ALUGUICommon.setGameObjEnable(hideGoList,true);
                ALUGUICommon.setGameObjEnable(oneAssistHideList, !oneAssist);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(hideGoList,false);
                ALUGUICommon.setGameObjEnable(oneAssistHideList,false);
            }
        }

        
        private void _onBtnClick(GameObject _)
        {
            NPPlayer.instance.guildMarsHelpComp.reqDealMarsHelp(_suc =>
            {
                _refreshWnd();
            });
        }
#endif
    }
}