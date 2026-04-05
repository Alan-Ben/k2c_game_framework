using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using GS2GC.p019_DinnerOp;
using UnityEngine;

namespace GOE
{
    
    /// <summary>
    /// 宴会内的席位的位置mono
    /// </summary>
    public class GTDDinnerMainPosItemView
    {
        private GTDDinnerMainPosItemMono _m_posItemMono;
        private GDinnerJoinerInfo _m_joinInfo;
        private GDinnerFollowInstance _m_followInstance;
        private GGUIDinnerSeatFollowItemController _m_followInfoController;
        private GTDDinnerMainPlayerView _m_playerView;
        private int _m_dealSerialize;

        public GTDDinnerMainPosItemView(GTDDinnerMainPosItemMono _mono)
        {
            _m_posItemMono = _mono;
            if(null != _m_posItemMono && null != _m_posItemMono.clickMono)
                _m_posItemMono.clickMono.onClick += _onClickMono;
            _m_followInstance = new GDinnerFollowInstance(_m_posItemMono.uiFollowParent);
            
            //向UI展示窗口注册本对象
            GGUIWndDinnerFollowShow.instance.regInstance(_m_followInstance);
        }

        public GDinnerJoinerInfo joinInfo { get => _m_joinInfo; }

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            if(null != _m_posItemMono && null != _m_posItemMono.clickMono)
                _m_posItemMono.clickMono.onClick -= _onClickMono;
            _m_posItemMono = null;
            GGUIWndDinnerFollowShow.instance.removeInstance(_m_followInstance);
            _discardPlayerView();
            _discardInfoController();

            _m_dealSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="_joinerInfo"></param>
        public void show(GDinnerJoinerInfo _joinerInfo)
        {
            _m_joinInfo = _joinerInfo;
            _refreshView();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void hide()
        {
            _m_joinInfo = null;
            _discardInfoController();
            _discardPlayerView();
            _m_dealSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 点击处理
        /// </summary>
        private void _onClickMono()
        {
            if(_m_joinInfo != null)
                return;
            
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshView()
        {
            if (null == _m_joinInfo)
            {
                _discardInfoController();
                _discardPlayerView();
                return;
            }
            
            //展示跟随信息
            _showInfoController();

            // _showPlayerView();
        }

        /// <summary>
        /// 显示玩家信息
        /// </summary>
        private void _showPlayerView()
        {
            if (null == _m_joinInfo)
            {
                _discardPlayerView();
                return;
            }
            if (null == _m_playerView && null != _m_posItemMono.loadPlayerParent)
                _m_playerView = new GTDDinnerMainPlayerView(_m_posItemMono.loadPlayerParent);
            if (_m_joinInfo.joinerType == EDinnerJoinerType.PLAYER)
            {
                //根据player的数据做显示
                GCommon.reqPlayerInfo(_m_joinInfo.joinerId, (playerInfo) =>
                {
                    _m_playerView?.show(playerInfo?.skinRef?.td_show);
                });
            }
            else if(joinInfo.joinerType == EDinnerJoinerType.HERO)
            {
                var heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(joinInfo.joinerId);
                _m_playerView.show(heroRefObj.td_show);
            }
          
        }
        private void _discardPlayerView()
        {
            _m_playerView?.discard();
            _m_playerView = null;
        }


        //显示跟随信息
        private void _showInfoController()
        {
            if (null == _m_joinInfo)
                return;
            
            if (null == _m_followInfoController)
            {
                _m_followInfoController = new GGUIDinnerSeatFollowItemController();
                GGUIWndDinnerFollowShow.instance.addController(_m_followInstance, _m_followInfoController);
            }
            _m_followInfoController.setShowData(_m_joinInfo);
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

        public void dealClickSeatItem()
        {
            _onClickMono();
        }
    }
}