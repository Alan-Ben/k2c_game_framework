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
    public class CustomMonoConsortChatEntry : MonoBehaviour
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("没有消息需要隐藏的go列表")]
        public List<GameObject> hideGoList;
        [ALHeader("妃子头像")]
        public RawImage imgIcon;//头像

#if NP_GAME
        // 头像
        private NPGGuiWndTexture _m_wtIconWnd;
        
        private void Awake()
        {
            if (imgIcon != null)
                _m_wtIconWnd = new NPGGuiWndTexture(imgIcon);
            
            ALUGUICommon.combineBtnClick(btnClick, _onBtnClick);
        }

        private void OnEnable()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND, _refreshWnd);
        }
        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND, _refreshWnd);

            if (_m_wtIconWnd != null) 
                _m_wtIconWnd.discardTexture();
        }
        

        private void _refreshWnd()
        {
            List<long> newFriendList = NPPlayer.instance.consortChatComp.getChatNewFriendList();

            if (newFriendList != null && newFriendList.Count > 0)
            {
                if (_m_wtIconWnd != null)
                {
                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(newFriendList.GetFirst());
                    if (consortInfo != null)
                    {
                        _m_wtIconWnd.setTexture(consortInfo.consortSkinShowInfo?.consortHeadIcon);
                        _m_wtIconWnd.showWnd();
                        ALUGUICommon.setGameObjEnable(hideGoList,true);
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(hideGoList,false);
                    }
                }   
            }
            else
            {
                ALUGUICommon.setGameObjEnable(hideGoList,false);
            }
        }

        
        private void _onBtnClick(GameObject _)
        {
            QueueMgr.instance.AddNode(new GNodeConsortChatMain(EConsortChatMainPage.CHAT));
        }
#endif
    }
}