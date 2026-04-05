using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /*******************
     *登录界面的功能按钮界面
     **/
    public class NPPGUIWndOutGameFuncs : _ANPGGUIBasicWnd<NPPGUIMonoOutGameFuncs>
    {
        private static NPPGUIWndOutGameFuncs _g_instance = new NPPGUIWndOutGameFuncs();
        public static NPPGUIWndOutGameFuncs instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUIWndOutGameFuncs();
                return _g_instance;
            }
        }

        private int _m_iGameBeforeNoticeIndex = 0;
        private bool _m_bIsShowNotice;
        //LOGO
        private NPPGUIWndLoginLogo _m_wLogo;

        protected NPPGUIWndOutGameFuncs()
            : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoOutGameFuncs.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoOutGameFuncs.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_START_GAME_WND_SHOW, _onStartGameWndShow);
            if (wnd == null)
                return;

            //默认隐藏切换账号按钮，到startGame才显示
            ALUGUICommon.setGameObjEnable(wnd.switchAccountBtn, false);

            _refreshWnd();
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            _m_wLogo?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_START_GAME_WND_SHOW, _onStartGameWndShow);
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_wLogo?.resetWnd();
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_wLogo?.discard();
            _m_wLogo = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.noticBtn, _onClickNoticeBtn);
            ALUGUICommon.uncombineBtnClick(wnd.aiHelpBtn, _onClickAihelpBtn);
            ALUGUICommon.uncombineBtnClick(wnd.switchAccountBtn, _onClickSwitchAccountBtn);
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.noticBtn, _onClickNoticeBtn);
            ALUGUICommon.combineBtnClick(wnd.aiHelpBtn, _onClickAihelpBtn);
            ALUGUICommon.combineBtnClick(wnd.switchAccountBtn, _onClickSwitchAccountBtn);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //设置logo
            ENPLanguage language = GameSetting.instance.getCurrentLanguage();
            NPCommonAssetPathInfo logoAsset = PLoginCommonInfo.instance.obj.getLanguageLogoAssetPath(language);
            if (_m_wLogo != null)
                _m_wLogo.discard();

            if (wnd.logoParent != null && logoAsset != null)
            {
                _m_wLogo = new NPPGUIWndLoginLogo(logoAsset, wnd.logoParent);
                _m_wLogo.load(_m_wLogo.showWnd);
            }
        }

        /// <summary>
        /// 当点击公告按钮时的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickNoticeBtn(GameObject _go)
        {
            //每次点击从第一个开始看
            _m_iGameBeforeNoticeIndex = 0;
            //标记还没打开公告弹窗
            _m_bIsShowNotice = false;
            _checkAndShowNotice();
        }

        /// <summary>
        /// 检查并显示公告
        /// </summary>
        private void _checkAndShowNotice()
        {
            if (!Game.instance.isUseCdn)
            {
                _showNoticeWnd(null, null, null);
                return;
            }

            CDNSetting_GameBeforeNoticeInfo.instance.requestData(_noticeInfo =>
            {
                //判断数据是否有效
                if (null == _noticeInfo || _noticeInfo.Count <= 0)
                {
                    _showNoticeWnd(null, null, null);
                    return;
                }

                //显示索引
                if (_m_iGameBeforeNoticeIndex < _noticeInfo.Count)
                {
                    GameBeforeNoticeInfo tmpInfo = _noticeInfo[_m_iGameBeforeNoticeIndex];

                    //是否有效，并且是否自动打开
                    if (null != tmpInfo && tmpInfo.inValid())
                    {

                        //获取语言公告
                        GameNoticeContent content = tmpInfo.getLanguage(GameSetting.instance.getCurrentLanguage());
                        //展示公告
                        if (null != content)
                        {
                            _m_bIsShowNotice = true;
                            _showNoticeWnd(content.content, content.title, () =>
                            {
                                //增加索引
                                _m_iGameBeforeNoticeIndex++;
                                _checkAndShowNotice();
                            });
                            return;
                        }
                    }
                    //增加索引
                    _m_iGameBeforeNoticeIndex++;
                    _checkAndShowNotice();
                }
                else
                {
                    //如果没有公告或者遍历完所有公告没有展示过一个有效的公告，弹tip提示
                    if(_m_iGameBeforeNoticeIndex == 0 || (_m_iGameBeforeNoticeIndex >= _noticeInfo.Count && !_m_bIsShowNotice))
                        _showNoticeWnd(null, null, null);
                }
            });
        }

        /// <summary>
        /// 打开notice窗口
        /// </summary>
        /// <param name="_content"></param>
        /// <param name="_title"></param>
        /// <param name="_onClick"></param>
        private void _showNoticeWnd(string _content, string _title, Action _onClick)
        {
            NPGUIAddSceneSingleWndScene oneBtnScene = new NPGUIAddSceneSingleWndScene(NPPGUIWndGameBeforeNotice.instance, false);
            oneBtnScene.regInitDelegate(() =>
            {
                NPPGUIWndGameBeforeNotice.instance.setInfo(_title, _content, TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        _onClick?.Invoke();
                        oneBtnScene.quitScene();
                    }, () =>
                    {
                        oneBtnScene.quitScene();
                    });
            });
            oneBtnScene.enterScene();
        }
        
        /// <summary>
        /// 当点击客服按钮时的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickAihelpBtn(GameObject _go)
        {
            GCommon.showAIHelp();
        }

        /// <summary>
        /// 当点击切换账号按钮时的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickSwitchAccountBtn(GameObject _go)
        {
            if (SDKMgr.instance.isUseSDK && Game.instance.isUseCdn)
            {
                NPGGUIWndLoginWay.instance.setInfo(null, true);
                QueueMgr.instance.addNode_Login_MainUIAddWnd(NPGGUIWndLoginWay.instance, UINodeTagConst.C_Login_AllWay);
            }
            else
            {
                Game.instance.relogin();
            }
        }

        //开始游戏窗口展示
        private void _onStartGameWndShow()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.switchAccountBtn, true);
        }
    }
}
