using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单个服务器item
    /// </summary>
    public class NPGGUIWndServerGridItem : _ATALUGUIBasicGridItemWnd<NPGGUIMonoServerGridItem>
    {
        private ServerShowData _m_serverData;//服务器数据
        private NPGGUIWndPlayerIcon _m_playerIcon;// 玩家相关信息
        private GGUIWndCommonSetColorTab _m_clickToggle;//点击tab

        public NPGGUIWndServerGridItem(NPGGUIMonoServerGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (null != _m_playerIcon)
            {
                _m_playerIcon.resetWnd();
            }
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_playerIcon)
            {
                _m_playerIcon.discard();
                _m_playerIcon = null;
            }

            if (null != _m_clickToggle)
            {
                _m_clickToggle.discard();
                _m_clickToggle = null;
            }
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }

            if (null != wnd.clickBtn)
            {
                _m_clickToggle = new GGUIWndCommonSetColorTab(wnd.clickBtn);
                _m_clickToggle.clickDelegate = _onClickTab;
                setSelected(false);
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_onClick"></param>
        public void setInfo(ServerShowData _data)
        {
            _m_serverData = _data;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            //刷新显示
            if (null != wnd.serverItem && null != _m_serverData)
                wnd.serverItem.setServerInfo(_m_serverData.serverDataInfo);

            //形象展示
            if (null == _m_serverData || null == _m_serverData.playerCharacterInfo)
            {
                if (_m_playerIcon != null)
                    _m_playerIcon.hideWnd();
            }
            else
            {
                if (null != _m_playerIcon)
                {
                    _m_playerIcon.showWnd();
                    _m_playerIcon.setPlayerInfo(new NPCommonSimplePlayerInfo(_m_serverData.playerCharacterInfo.getIconShowInfo()));
                }
            }
        }

        /// <summary>
        /// 点击item的处理
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickTab(bool _obj)
        {
            if (_m_serverData == null || _m_serverData.serverDataInfo == null)
                return;

            //如果服务器关闭并且不是白名单账号，则提示服务器维护
            if (!GameSetting.instance.isWhite && (EServerOnlineState) _m_serverData.serverDataInfo.onlineState != EServerOnlineState.OPEN)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.server_isUnderMaintenance_none);//服务器维护中
                return;
            }

            //弹窗提示文本
            string transValue = string.Empty;
            //在队列中时提示信息
            if (GameInit_SelectServer.instance.isInQueue)
            {
                transValue = TextTranslate.instance.getLanguage(TransKeyConst.login_queueChgServer,
                    GameInit_SelectServer.instance.curQueueSize, GCommon.getLoginQueueTime(GameInit_SelectServer.instance.curQueueSize),
                    _m_serverData.serverDataInfo.serverName);
            }
            else
            {
                transValue = TextTranslate.instance.getLanguage(TransKeyConst.login_serverSelect, _m_serverData.serverDataInfo.serverName);
            }
            
            //同个大区下不用重登
            if (_m_serverData.areaTag == CDNSetting_AreaInfo.instance.areaTag)
            {
                GameInit_SelectServer.instance.getCurSelectServerItem(serverId =>
                {
                    //服务器是当前选中的
                    if (serverId == _m_serverData.serverDataInfo.serverId)
                    {
                        //设置选中状态
                        _m_clickToggle?.setSelected(true);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.server_inCurServer_none);//已在当前服务器
                        return;
                    }
                    else
                    {
                        //设置未选中状态
                        _m_clickToggle?.setSelected(false);
                        //提示确认是否切换服务器
                        NPMesMgr.instance.showTwoBtnMes(transValue,
                            TextTranslate.instance.getLanguage(TransKeyConst.cancel), null, TextTranslate.instance.getLanguage(TransKeyConst.confirm), () =>
                            {
                                //设置数据集部分选中服务器
                                GameInit_SelectServer.instance.setCurSelectServerItem(_m_serverData.serverDataInfo);
                                //退出服务器列表节点
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Login_Server_List);
                            });
                    }
                });
            }
            else
            {
                //不同大区，需要重登
                NPMesMgr.instance.showTwoBtnMes(transValue,
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel), null, TextTranslate.instance.getLanguage(TransKeyConst.confirm), () =>
                    {
                        //设置数据集部分选中服务器
                        GameInit_SelectServer.instance.setCurSelectServerItem(_m_serverData.serverDataInfo);
                        
                        //设置选中的大区
                        CDNSetting_AreaInfo.instance.areaId = CDNSetting_AreaInfo.instance.getAreaIdByTag(_m_serverData.areaTag);
                        
                        //记录登录服务器id
                        LoginTokenSetting.instance.setUidServerId(Game.instance.uid, _m_serverData.serverDataInfo.serverId);
                        
                        //清除cdn配置
                        GameInit_CDN.instance.forceClearAreaCdn();
                        
                        //退出服务器列表节点
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Login_Server_List);
                        
                        //开始重登
                        Game.instance.relogin();
                    });
            }
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (null == wnd)
                return;
            _m_clickToggle?.setSelected(_isSelected);
        }
    }
}
