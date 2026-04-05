using System;
using Common.RankGiftPackObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行礼包入口点视图
    /// </summary>
    public class RankGiftPackPointView
    {
        //入口点的跟随实例
        private GGUICommonFollowTarget _m_followInstance;
        //入口点跟随的信息controller
        private GGUIRankGiftPackController _m_followInfoController;

        public RankGiftPackPointView()
        {
        }

        public void init(Action _complete)
        {
            _m_followInstance = new GGUICommonFollowTarget(MainAdditionBuildingTDScene.instance.getRankGiftPackPos(), Vector3.zero);
            //向UI展示窗口注册本对象
            GGUIWndHomeEntryFollow.instance.regInstance(_m_followInstance);
            
            WinMsg.RegisterMsg(WinMsgType.ON_RANK_GIFT_CHG, _onRankGiftPackChg);
            _refresh();

            if (_complete != null) 
                _complete();
        }

        //销毁
        public void discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_RANK_GIFT_CHG, _onRankGiftPackChg);

            GGUIWndHomeEntryFollow.instance.removeInstance(_m_followInstance);

            _m_followInstance = null;
            _discardInfoController();
        }

        private void _onRankGiftPackChg(params object[] _params)
        {
            _refresh();
        }

        private void _refresh()
        {
            RankGiftPackInfo rankGiftPackInfo = NPPlayer.instance.rankGiftPackComp.rankGiftPackInfo;
            if (null == rankGiftPackInfo || rankGiftPackInfo.dbId == 0)
            {
                _discardInfoController();
            }
            else
            {
                _showInfoController();
            }
        }
        
        //显示跟随信息
        private void _showInfoController()
        {
            if (null == _m_followInfoController)
            {
                _discardInfoController();
                _m_followInfoController = new GGUIRankGiftPackController();
                GGUIWndHomeEntryFollow.instance.addController(_m_followInstance, _m_followInfoController);
            }
            _m_followInfoController.setShowEntrance();
        }
        
        //销毁跟随信息
        private void _discardInfoController()
        {
            if (null != _m_followInfoController)
            {
                _m_followInfoController.discard();
            }
            _m_followInfoController = null;
        }
    }
}