using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱
    /// </summary>
    public class GGUIWndGuildBoxMain : _ATALBasicUIWnd<GGUIMonoGuildBoxMain>
    {
        private static GGUIWndGuildBoxMain _g_instance = new GGUIWndGuildBoxMain();
    
        public static GGUIWndGuildBoxMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildBoxMain();
                return _g_instance;
            }
        }
        
        private EGuildBoxPage _m_curPage = EGuildBoxPage.GiftBox;
        private bool _m_bIsSelectPage;
        private GGUIWndGuildFreeBoxPage _m_freeBoxPage;
        private GGUIWndGuildGiftBoxPage _m_giftBoxPage;
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonTab _m_freeBoxTabWnd;  //
        private NPGGUIWndCommonTab _m_girtBoxTabWnd;  //
        private GGUIWndLongProgress _m_activeBoxProgressWnd;  // 活跃宝箱进度条
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildBoxMain() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildBoxMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildBoxMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override bool isShowAniPlayOnlyOne { get => true; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_BOX_ACTIVE_POINT_CHG, _onGuildBoxActivePointChange);
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_BOX_COUNT_CHG, _onGuildBoxAddCountChg);
            //先检查清除无效宝箱数据
            NPPlayer.instance.guildBoxComp.checkAndRemoveInvalidBox();
            //刷新界面
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_BOX_ACTIVE_POINT_CHG, _onGuildBoxActivePointChange);
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_BOX_COUNT_CHG, _onGuildBoxAddCountChg);
            _m_activeBoxProgressWnd?.hideWnd();
            _m_freeBoxPage?.hideWnd();
            _m_giftBoxPage?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_activeBoxProgressWnd?.resetWnd();
            _m_freeBoxPage?.resetWnd();
            _m_giftBoxPage?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_freeBoxTabWnd?.discard();
            _m_freeBoxTabWnd = null;
            _m_girtBoxTabWnd?.discard();
            _m_girtBoxTabWnd = null;
            _m_activeBoxProgressWnd?.discard();
            _m_activeBoxProgressWnd = null;
            _m_freeBoxPage?.discard();
            _m_freeBoxPage = null;
            _m_giftBoxPage?.discard();
            _m_giftBoxPage = null;
            _m_bIsSelectPage = false;

            if (null == wnd)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnGetActiveBox, _onClickbtnGetActiveBox);
            ALUGUICommon.uncombineBtnClick(wnd.btnActiveBoxDetail, _onClickbtnActiveBoxDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnActiveBoxScoreDetail, _onClickbtnActiveBoxScoreDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.freeBoxTab != null)
            {
                _m_freeBoxTabWnd = new NPGGUIWndCommonTab(wnd.freeBoxTab);         
                _m_freeBoxTabWnd.clickDelegate += _onTabClickfreeBoxTab;
            }

            if (wnd.girtBoxTab != null)
            {
                _m_girtBoxTabWnd = new NPGGUIWndCommonTab(wnd.girtBoxTab);
                _m_girtBoxTabWnd.clickDelegate += _onTabClickgirtBoxTab;
            }
            
            if (wnd.activeBoxProgress != null)
                _m_activeBoxProgressWnd = new GGUIWndLongProgress(wnd.activeBoxProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnGetActiveBox, _onClickbtnGetActiveBox);
            ALUGUICommon.combineBtnClick(wnd.btnActiveBoxDetail, _onClickbtnActiveBoxDetail);
            ALUGUICommon.combineBtnClick(wnd.btnActiveBoxScoreDetail, _onClickbtnActiveBoxScoreDetail);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            //是否有选中页面，已选中不再设置，避免跳转出去后返回页面选择被重置
            if (!_m_bIsSelectPage)
            {
                //两边都有奖励的话，默认选中付费宝箱的
                //只有一个页签有奖励，默认选中有奖励的
                //如果都没有奖励，默认选中付费宝箱
                int giftBoxCount = NPPlayer.instance.guildBoxComp.getBoxCount(EGuildBoxType.GUILD_GIFT_BOX);
                int freeBoxCount = NPPlayer.instance.guildBoxComp.getBoxCount(EGuildBoxType.GUILD_FREE_BOX);
                if (giftBoxCount > 0)
                    _m_curPage = EGuildBoxPage.GiftBox;
                else if (freeBoxCount > 0)
                    _m_curPage = EGuildBoxPage.FreeBox;
                else
                    _m_curPage = EGuildBoxPage.GiftBox;
                _m_bIsSelectPage = true;
            }

            _refreshActiveBox();
            _refreshPage();
        }

        /// <summary>
        /// 刷新活跃宝箱状态
        /// </summary>
        private void _refreshActiveBox()
        {
            if (null == wnd)
                return;

            long curActiveBoxScore = NPPlayer.instance.guildBoxComp.getActiveBoxCurScore();
            long maxActiveBoxScore = NPPlayer.instance.guildBoxComp.getActiveBoxMaxScore();
            bool canGetActiveBox = NPPlayer.instance.guildBoxComp.getBoxCount(EGuildBoxType.GUILD_ACTIVE_BOX) > 0;

            if (_m_activeBoxProgressWnd != null)
            {
                string _getCommonSliderTxtStr(string _cur, string _max)
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
                }
                _m_activeBoxProgressWnd.showWnd();
                _m_activeBoxProgressWnd.initSld(0, maxActiveBoxScore, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_activeBoxProgressWnd.setNowValue(curActiveBoxScore);
            }

            ALUGUICommon.setGameObjEnable(wnd.activeBoxCanGetShowList, canGetActiveBox);
            ALUGUICommon.setGameObjEnable(wnd.activeBoxCanGetHideList, !canGetActiveBox);
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        private void _refreshPage()
        {
            if (wnd == null)
                return;

            switch (_m_curPage)
            {
                case EGuildBoxPage.FreeBox:
                    _m_freeBoxTabWnd?.setSelected(true);
                    _m_girtBoxTabWnd?.setSelected(false);
                    ALUGUICommon.setGameObjEnable(wnd.freeBoxTabShowList, true);
                    ALUGUICommon.setGameObjEnable(wnd.giftBoxTabShowList, false);
                    if (_m_freeBoxPage != null)
                    {
                        _m_freeBoxPage.showWnd();
                    }
                    else
                    {
                        _m_freeBoxPage = new GGUIWndGuildFreeBoxPage(wnd.pageParent);
                        _m_freeBoxPage.load(_m_freeBoxPage.showWnd);
                    }
                    _m_giftBoxPage?.hideWnd();

                    break;
                case EGuildBoxPage.GiftBox:
                    _m_freeBoxTabWnd?.setSelected(false);
                    _m_girtBoxTabWnd?.setSelected(true);
                    ALUGUICommon.setGameObjEnable(wnd.freeBoxTabShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.giftBoxTabShowList, true);
                    if (_m_giftBoxPage != null)
                    {
                        _m_giftBoxPage.showWnd();
                    }
                    else
                    {
                        _m_giftBoxPage = new GGUIWndGuildGiftBoxPage(wnd.pageParent);
                        _m_giftBoxPage.load(_m_giftBoxPage.showWnd);
                    }
                    _m_freeBoxPage?.hideWnd();
                    break;
            }
            
        }

        /// <summary>
        /// 联盟宝箱活跃积分变化事件
        /// </summary>
        private void _onGuildBoxActivePointChange()
        {
            _refreshActiveBox();
        }

        /// <summary>
        /// 新增宝箱数量推送
        /// </summary>
        /// <param name="_objects"></param>
        private void _onGuildBoxAddCountChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            EGuildBoxType boxType = (EGuildBoxType)_objects[0];
            if (boxType == EGuildBoxType.GUILD_ACTIVE_BOX)
            {
                _refreshActiveBox();
            }
        }

        // <AutoGen:Method>

        private void _onTabClickfreeBoxTab(bool _tab)
        {
            if(_m_curPage == EGuildBoxPage.FreeBox)
                return;
            _m_curPage = EGuildBoxPage.FreeBox;
            _refreshPage();
        }

        private void _onTabClickgirtBoxTab(bool _tab)
        {
            if(_m_curPage == EGuildBoxPage.GiftBox)
                return;
            _m_curPage = EGuildBoxPage.GiftBox;
            _refreshPage();
        }


        // 领取活跃宝箱奖励按钮点击事件
        private void _onClickbtnGetActiveBox(GameObject go)
        {
            bool canGetActiveBox = NPPlayer.instance.guildBoxComp.getBoxCount(EGuildBoxType.GUILD_ACTIVE_BOX) > 0;
            if (!canGetActiveBox)
            {
                _onClickbtnActiveBoxDetail(null);
                return;
            }

            NPPlayer.instance.guildBoxComp.reqGainGuildRewardBoxList(EGuildBoxType.GUILD_ACTIVE_BOX, _suc =>
            {
                _refreshActiveBox();
            });

        }

        // 活跃宝箱奖励详情点击事件
        private void _onClickbtnActiveBoxDetail(GameObject go)
        {
            long targetActiveBoxId = NPPlayer.instance.guildBoxComp.getTargetActiveBoxId();
            GuildBoxRefObj activeBoxInfo = GRefdataCoreMgr.instance.guildBoxRefCore.getRef(targetActiveBoxId);
            GGUIWndGuildBoxDetail.instance.setInfo(activeBoxInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildBoxDetail.instance, GGUIWndGuildBoxDetail.instance.showWnd, UINodeTagConst_Guild.C_GUILD_BOX_ACTIVE_BOX_DETAIL);
        }

        // 活跃宝箱积分获取详情点击事件
        private void _onClickbtnActiveBoxScoreDetail(GameObject go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildBoxScore.instance, GGUIWndGuildBoxScore.instance.showWnd, UINodeTagConst_Guild.C_GUILD_BOX_SCORE);

        }

        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_BOX);
        }

     
        // </AutoGen:Method>
    }
}