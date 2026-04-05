using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _IGGUIWndUnLockConsortMainDetailWndParam
    {
        /// <summary>
        /// 妃子详情页面选中的tab类型
        /// </summary>
        EUnLockConsortDetailWndTabType consortDetailWndSelectTabType { get; set; }
        
        int consortDetailWndShowConsortIndex { get; set; }
        
        /// <summary>
        /// 经营技能页面参数
        /// </summary>
        _IGGUIWndUnLockConsortDetailBusinessPageParam consortDetailWndBusinessPageParam { get; }
        
        /// <summary>
        /// 互动页面参数
        /// </summary>
        _IGGUIWndUnlockConsortDetailInteractionPageParam consortDetailWndInteractionPageParam { get; }
        
        /// <summary>
        /// 星辉页面参数
        /// </summary>
        _IGGUIWndUnLockConsortDetailHaloPageParam consortDetailWndHaloPageParam { get; }
    }
    
    /// <summary>
    /// 这里用类合适，不要用struct其实更耗内存
    /// </summary>
    public class UnLockConsortDetailTabItemInfo
    {
        public EUnLockConsortDetailWndTabType tabType;
        public GGUIWndUnLockConsortDetailTab tabWnd;
        public EUnLockConsortDetailWndTabSelectMode selectMode;

        public UnLockConsortDetailTabItemInfo(GGUIMonoUnLockConsortDetailWndTabSetting _tabSetting, Action<GGUIWndUnLockConsortDetailTab> _onTabClick)
        {
            tabType = _tabSetting.tabType;
            tabWnd = new GGUIWndUnLockConsortDetailTab(_tabSetting);
            //设置回调
            tabWnd.onClickTab = _onTabClick;
        }
    }

    //public struct UnLockConsortDetailTabAndPageStruct
    //{
    //    public EUnLockConsortDetailWndTabType tabType;
    //    public GGUIWndUnLockConsortDetailTab tabWnd;
    //    public _IUnLockConsortDetailTabPage tabPageWnd;
    //    public EUnLockConsortDetailWndTabSelectMode selectMode;
    //}

    /// <summary>
    /// 已解锁妃子详情页面tab
    /// </summary>
    public class GGUIWndUnLockConsortMainDetailWnd : _ATALBasicUIWnd<GGUIMonoUnLockConsortMainDetailWnd>
    {
        private static GGUIWndUnLockConsortMainDetailWnd _g_instance;
        public static GGUIWndUnLockConsortMainDetailWnd instance { get { return _g_instance ??= new GGUIWndUnLockConsortMainDetailWnd(); } }

        //当前妃子信息
        private GGottenConsortInfo _m_curConsortShowInfo;

        private _IGGUIWndUnLockConsortMainDetailWndParam _m_param;
        //管理本窗口中页签的列表
        private List<UnLockConsortDetailTabItemInfo> _m_lTabInfoList = new List<UnLockConsortDetailTabItemInfo>();
        private UnLockConsortDetailTabItemInfo _m_iCurSelectTabInfo;//当前选中的tab信息

        //alzq: 暂时不需要返回，如果Tab不存在直接返回默认Tab。默认写死是INTERACTION
        //[NotNull] private List<EUnLockConsortDetailWndTabType> _m_lSelectedTabTypeList = new List<EUnLockConsortDetailWndTabType>();//选中的tab类型列表(不清除数据, 以便实现跳转后返回保持tab选中)

        private GGUIWndConsortVoiceBubble _m_wVoiceBubble;
        
        // 仅显示形象tab
        private NPGGUIWndCommonToggleEx _m_wOnlyShowActorToggle;
        
        //页签显示的子窗口的管理Scene
        private GGUIWndUnLockConsortDetailTabContainer _m_tabPageWndScene;

        public GGUIWndUnLockConsortMainDetailWnd() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoUnLockConsortMainDetailWnd.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoUnLockConsortMainDetailWnd.objName; } }
        protected override _AALResourceCore _resourceCore { get{return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.onlyShowActorToggle != null)
            {
                _m_wOnlyShowActorToggle = new NPGGUIWndCommonToggleEx(wnd.onlyShowActorToggle);
                _m_wOnlyShowActorToggle.clickDelegate += _onOnlyShowActorTabClick;
            }

            if (wnd.monoConsortVoiceBubble != null)
            {
                _m_wVoiceBubble = new GGUIWndConsortVoiceBubble(wnd.monoConsortVoiceBubble);
            }
            
            // 初始化tab
            _initTabAndPage();

            //构造页签窗口管理器
            if (null != _m_tabPageWndScene)
                _m_tabPageWndScene.quitScene();

            //构造页签窗口管理器
            _m_tabPageWndScene = new GGUIWndUnLockConsortDetailTabContainer(_m_param, wnd.tabSettingList, _onClickTabClose);
            _m_tabPageWndScene.setShowConsortInfo(_m_curConsortShowInfo);
            //这里根据类型展示子窗口
            _m_tabPageWndScene.enterAndShowScene();
            
            ALUGUICommon.combineBtnClick(wnd.btnConsortProfile, _onClickConsortProfile);
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPreBtn);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNextBtn);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _clickReturnBtn);
        }
        
        protected override void _onDiscard()
        {
            if (_m_wOnlyShowActorToggle != null)
            {
                _m_wOnlyShowActorToggle.clickDelegate -= _onOnlyShowActorTabClick;
                _m_wOnlyShowActorToggle.discard();
            }
            _m_wOnlyShowActorToggle = null;

            _m_wVoiceBubble?.discard();
            _m_wVoiceBubble = null;
            
            //释放Tab对象列表
            _discardTabAndPage();

            //释放子窗口Scene
            if (null != _m_tabPageWndScene)
                _m_tabPageWndScene.quitScene();
            _m_tabPageWndScene = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnConsortProfile, _onClickConsortProfile);
            ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPreBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNextBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _clickReturnBtn);
        }
        
        protected override void _onShowWnd()
        {
            _m_wOnlyShowActorToggle?.setState(false);
            
            _m_tabPageWndScene?.showScene();
            
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, _simulateClickTab);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_GIVE_GIFT, _onSimulateClickOpenGive);
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshTabRed);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, _simulateClickTab);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_GIVE_GIFT, _onSimulateClickOpenGive);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshTabRed);
            
            _m_wVoiceBubble?.hideWnd();
            
            _m_wOnlyShowActorToggle?.hideWnd();

            _m_tabPageWndScene?.hideScene();   
        }

        protected override void _onReset()
        {
            _m_wVoiceBubble?.resetWnd();

            _m_wOnlyShowActorToggle?.resetWnd();

            _m_tabPageWndScene?.hideScene();
        }
        
        /// <summary>
        /// 这里只设置妃子信息，不刷新窗口
        /// 一般在部分前置调用数据初始化时使用
        /// </summary>
        public void initConsortInfo(GGottenConsortInfo _consortInfo)
        {
            _m_curConsortShowInfo = _consortInfo;
            if (_m_curConsortShowInfo == null)
            {
                Debug.LogError($"[GGUIWndUnLockConsortDetail setData] error, 传入的参数_consortInfo为null");
                return;
            }
        }

        /// <summary>
        /// 初始化选中的类型
        /// </summary>
        public void initWndParam(_IGGUIWndUnLockConsortMainDetailWndParam _param)
        {
            _m_param = _param;
        }

        /// <summary>
        /// 初始化设置妃子气泡
        /// </summary>
        /// <param name="_voiceType"></param>
        private void _initConsortVoiceBubble(EConsortVoiceType _voiceType)
        {
            if (_m_wVoiceBubble != null && _m_curConsortShowInfo != null)
            {
                _m_wVoiceBubble.showWnd();
                _m_wVoiceBubble.setInfo(_m_curConsortShowInfo.consortId, _voiceType);
            }
        }

        private void _showConsortVoiceBubble(EConsortVoiceType _voiceType)
        {
            _m_wVoiceBubble?.refreshBubble(_voiceType);
        }
        
        /// <summary>
        /// 当点击妃子详情按钮
        /// </summary>
        private void _onClickConsortProfile(GameObject _gameObject)
        {
            if(_m_curConsortShowInfo == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndUnlockConsortDetailProfile.instance, () =>
            {
                GGUIWndUnlockConsortDetailProfile.instance.showWnd();
                GGUIWndUnlockConsortDetailProfile.instance.setData(_m_curConsortShowInfo.consortRefObj);
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_PROFILE_WND, false, false);
        }
        
        /// <summary>
        /// 当点击查看上一妃子按钮时
        /// </summary>
        private void _onClickPreBtn(GameObject _gameObject)
        {
            //TODO 调用Scene选择上一个
            GMainGUIMainSceneConsortDetail.instance.showPre();
        }

        private void _onClickNextBtn(GameObject _gameObject)
        {
            //TODO 调用Scene选择下一个
            GMainGUIMainSceneConsortDetail.instance.showNext();
        }

        private void _clickReturnBtn(GameObject _gameObject)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL);
        }

        /// <summary>
        /// 点击对应页签子窗口的关闭按钮
        /// </summary>
        /// <param name="_tabType"></param>
        private void _onClickTabClose(EUnLockConsortDetailWndTabType _tabType)
        {
            //判断是否INTERACTION，如果不是则直接设置为INTERACTION
            if (_tabType == EUnLockConsortDetailWndTabType.INTERACTION)
                return;

            //设置显示页签
            setSelectTabType(EUnLockConsortDetailWndTabType.INTERACTION);
        }

        #region 仅显示形象tab

        /// <summary>
        /// 仅显示形象tab被点击时
        /// </summary>
        private void _onOnlyShowActorTabClick(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if(_toggleWnd == null)
                return;
            
            GMainGUIMainSceneConsortDetail.instance.onlyShowActor(!_toggleWnd.isOn);
        }
        
        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="__show"></param>
        public void onlyShowActor(bool _show)
        {
            _m_wOnlyShowActorToggle?.setSelected(_show);
            
            _m_tabPageWndScene?.onlyShowActor(_show);
        }

        #endregion
        
        #region 页签

        /// <summary>
        /// 初始化tab以及其page
        /// </summary>
        private void _initTabAndPage()
        {
            if(wnd == null || wnd.tabSettingList == null)
                return;

            _discardTabAndPage();//先销毁之前的窗口
            if (_m_lTabInfoList == null)
                _m_lTabInfoList = new List<UnLockConsortDetailTabItemInfo>();
            foreach (GGUIMonoUnLockConsortDetailWndTabSetting tabSetting in wnd.tabSettingList)
            {
                if(tabSetting == null)
                    continue;

#if UNITY_EDITOR
                if (lookupTabInfo(tabSetting.tabType) != null)
                {
                    Debug.LogWarning($"[GGUIWndUnLockConsortDetail _initTabAndPage] warning, tabSetting.tabType : {tabSetting.tabType} 重复配置", wnd);
                    continue;
                }
#endif

                //构造页签点击管理器
                //放入结构列表
                _m_lTabInfoList.Add(new UnLockConsortDetailTabItemInfo(tabSetting, _onTabClick));
            }
        }

        /// <summary>
        /// 销毁tab以及其page
        /// </summary>
        private void _discardTabAndPage()
        {
            //清空列表
            foreach (UnLockConsortDetailTabItemInfo tabAndPageStruct in _m_lTabInfoList)
            {
                //释放窗口对象
                if (tabAndPageStruct != null && tabAndPageStruct.tabWnd != null)
                {
                    tabAndPageStruct.tabWnd.onClickTab -= _onTabClick;
                    tabAndPageStruct.tabWnd.discard();
                }
               
                // tabPage窗口销毁由_m_tabPageWndScene管理, 这里不进行销毁
                // if(tabAndPageStruct.tabPageWnd != null)
                //     tabAndPageStruct.tabPageWnd.discard();
            }
            _m_lTabInfoList.Clear();
            _m_iCurSelectTabInfo = null;
        }

        /// <summary>
        /// 
        /// 遍历所有Tab执行行为
        /// </summary>
        /// <param name="_dealTabAndPageAction"></param>
        private void _dealTabAndPage(Action<UnLockConsortDetailTabItemInfo> _dealTabAndPageAction)
        {
            foreach (var tabAndPageStruct in _m_lTabInfoList)
            {
                _dealTabAndPageAction?.Invoke(tabAndPageStruct);
            }
        }

        private void _onTabClick(GGUIWndUnLockConsortDetailTab _tabWnd)
        {
            if(_tabWnd == null)
                return;
            
            //设置显示页签
            setSelectTabType(_tabWnd.tabSetting.tabType);
        }
        
        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_tabType"></param>
        public void setSelectTabType(EUnLockConsortDetailWndTabType _tabType, bool _forceRefresh = false)
        {
            if (_m_param == null)
            {
                Debug.LogError("[GGUIWndUnLockConsortDetail setSelectTabType] error, _m_param为null");
                return;
            }
            //如当前页签一致则不刷新
            if (_m_param.consortDetailWndSelectTabType == _tabType && !_forceRefresh)
                return;

            //设置显示类型
            _m_param.consortDetailWndSelectTabType = _tabType;

            _refreshSelectedTab();//刷新选中页签
            //刷新显示数据
            _refreshCurPageInfo();//刷新当前显示page
            
            switch (_tabType)
            {
                case EUnLockConsortDetailWndTabType.HALO:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_HALO_PAGE);
                    break;
                case EUnLockConsortDetailWndTabType.BUSINESS:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_BUSINESS_PAGE);
                    break;
                case EUnLockConsortDetailWndTabType.BLESS:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_BLESS_PAGE);
                    break;
                case EUnLockConsortDetailWndTabType.FETTER:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_FETTER_PAGE);
                    break;
            }
            
        }

        /// <summary>
        /// 设置当前妃子信息
        /// </summary>
        /// <param name="_consortInfo"></param>
        public void setConsortInfo(GGottenConsortInfo _consortInfo)
        {
            if (_m_curConsortShowInfo == _consortInfo)
            {
                _initConsortVoiceBubble(EConsortVoiceType.Viewed);
                _showConsortVoiceBubble(EConsortVoiceType.Viewed);
                return;
            }

            _m_curConsortShowInfo = _consortInfo;
            _m_tabPageWndScene?.setShowConsortInfo(_m_curConsortShowInfo);//设置页签子窗口Scene显示的妃子信息
            
            _initConsortVoiceBubble(EConsortVoiceType.Viewed);
            _showConsortVoiceBubble(EConsortVoiceType.Viewed);

            //刷新Tab是否显示
            _refreshTabShow();

            //刷新显示数据
            setSelectTabType(_m_param?.consortDetailWndSelectTabType ?? EUnLockConsortDetailWndTabType.INTERACTION, true);
        }

        /// <summary>
        /// 查询对应Tab类型的显示数据对象
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public UnLockConsortDetailTabItemInfo lookupTabInfo(EUnLockConsortDetailWndTabType _type)
        {
            if (_m_lTabInfoList == null)
                return null;
            
            for (int i = 0; i < _m_lTabInfoList.Count; i++)
            {
                if (_m_lTabInfoList[i] != null && _m_lTabInfoList[i].tabType == _type)
                    return _m_lTabInfoList[i];
            }

            return null;
        }

        private void _refreshWnd()
        {
            _refreshTabShow();

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.lessThanOrEqualOneConsortHideGoList, GMainGUIMainSceneConsortDetail.instance.showConsortListCount > 1);
            }

            //什么都没传用原始的
            if (null == _m_param)
            {
                _refreshSelectedTab();//刷新选中页签
                //刷新显示数据
                _refreshCurPageInfo();//刷新当前显示page
            }
            else if(_m_param.consortDetailWndSelectTabType == EUnLockConsortDetailWndTabType.NONE)
            {
                setSelectTabType(EUnLockConsortDetailWndTabType.INTERACTION, true);
            }
            else
            {
                setSelectTabType(_m_param.consortDetailWndSelectTabType, true);
            }
        }

        /// <summary>
        /// 刷新页签显示
        /// </summary>
        private void _refreshTabShow()
        {
            if(wnd == null || !isShow)
                return;
            
            //此处根据实际数据情况，显隐不同的Tab对象
            // 先将所有tab取消选中, 并且判断是否需要显示
            _dealTabAndPage((_tabAndPageStruct) =>
            {
                if (null == _tabAndPageStruct.tabWnd)
                    return;

                bool needShow = _checkTabNeedShow(_tabAndPageStruct.tabType);
                // 若tab不需要显示
                if (!needShow)
                {
                    //需要隐藏tab按钮
                    _tabAndPageStruct.tabWnd.hideWnd();
                    
                    //如果当前页签是对应tab则切换到默认
                    if(_m_param != null && _m_param.consortDetailWndSelectTabType == _tabAndPageStruct.tabType)
                        _m_param.consortDetailWndSelectTabType = EUnLockConsortDetailWndTabType.INTERACTION;
                    
                    return;
                }

                _m_iCurSelectTabInfo = null;
                
                _tabAndPageStruct.tabWnd.showWnd();
                _tabAndPageStruct.tabWnd.setState(false);
            });

            _refreshTabRed();
        }

        /// <summary>
        /// 刷新页签红点
        /// </summary>
        private void _refreshTabRed()
        {
            _dealTabAndPage((_tabAndPageStruct) =>
            {
                if (null == _tabAndPageStruct.tabWnd)
                    return;

                long showRedTipNum = 0;
                if (_m_curConsortShowInfo != null)
                {
                    switch (_tabAndPageStruct.tabType)
                    {
                        case EUnLockConsortDetailWndTabType.BUSINESS:
                            showRedTipNum = NPPlayer.instance.consortComp.needShowConsortBusinessSkillRed(_m_curConsortShowInfo.consortId) ? 1 : 0;
                            break;
                    
                        case EUnLockConsortDetailWndTabType.BLESS:
                            showRedTipNum = NPPlayer.instance.consortComp.needShowConsortBlessSkillRed(_m_curConsortShowInfo.consortId) ? 1 : 0;
                            break;
                    
                        case EUnLockConsortDetailWndTabType.FETTER:
                            showRedTipNum = NPPlayer.instance.consortComp.getConsortFetterRedCount(_m_curConsortShowInfo.consortId);
                            break;
                        
                        case EUnLockConsortDetailWndTabType.INTERACTION:
                            showRedTipNum = NPPlayer.instance.consortComp.needShowConsortSendGiftRootRedTip() ? 1 : 0;
                            break;
                    }    
                }
                
                _tabAndPageStruct.tabWnd.showRedTipNum((int)showRedTipNum);
            });
        }

        /// <summary>
        /// 校验tab是否需要显示
        /// </summary>
        /// <returns></returns>
        private bool _checkTabNeedShow(EUnLockConsortDetailWndTabType _tab)
        {
            switch (_tab)
            {
                //星辉tab需要特殊判断, 只有有星辉的妃子才要显示星辉tab
                case EUnLockConsortDetailWndTabType.HALO:
                    return _m_curConsortShowInfo != null && _m_curConsortShowInfo.consortRefObj != null && _m_curConsortShowInfo.consortRefObj.hasHalo();
                
                // 加护技能tab需要特殊判断, 只有有对应关联大臣 且 有加护技能的妃子才要显示加护技能tab
                case EUnLockConsortDetailWndTabType.BLESS:
                    return _m_curConsortShowInfo != null && _m_curConsortShowInfo.consortRefObj != null && _m_curConsortShowInfo.consortRefObj.needShowBlessSkill();
            }

            return true;
        }
        
        /// <summary>
        /// 刷新选中页签
        /// </summary>
        private void _refreshSelectedTab()
        {
            if(_m_param == null)
                return;
            
            if(_m_iCurSelectTabInfo != null && _m_iCurSelectTabInfo.tabWnd != null)
                _m_iCurSelectTabInfo.tabWnd.setSelected(false);
                
            // 刷新tab选中状态
            _dealTabAndPage((_tabItem) =>
            {
                if (_tabItem != null && _tabItem.tabWnd != null && _m_param.consortDetailWndSelectTabType == _tabItem.tabType)
                {
                    _m_iCurSelectTabInfo = _tabItem;
                    _tabItem.tabWnd.setSelected(true);
                }
            });
        }
        
        /// <summary>
        /// 刷新当前子页签显示的妃子信息
        /// </summary>
        private void _refreshCurPageInfo()
        {
            //刷新当前页签显示的妃子信息
            _m_tabPageWndScene?.setShowPage(_m_param?.consortDetailWndSelectTabType ?? EUnLockConsortDetailWndTabType.INTERACTION);
        }
        
        /// <summary>
        /// 模拟点击页签
        /// </summary>
        /// <param name="_tabType"></param>
        private void _simulateClickTab(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || _objs[0] == null || !(_objs[0] is EUnLockConsortDetailWndTabType) || _m_param == null)
                return;

            EUnLockConsortDetailWndTabType clickTab = (EUnLockConsortDetailWndTabType) _objs[0];

            switch (clickTab)
            {
                case EUnLockConsortDetailWndTabType.INTERACTION://若是点击交互页签, 可以多判断一层, 打开交互页面页签
                    if (_objs.Length >= 2 && _objs[1] != null && _objs[1] is EUnlockConsortDetailWndInteractionPageTabType)
                    // 若有传入交互页面显示页签类型
                    {
                        EUnlockConsortDetailWndInteractionPageTabType interactionPageTab = (EUnlockConsortDetailWndInteractionPageTabType) _objs[1];
                        if (_m_param.consortDetailWndSelectTabType == EUnLockConsortDetailWndTabType.INTERACTION)//若当前已经在交互页签, 不需要修改点击tab
                        {
                            // 不进行任何操作
                        }
                        else//若当前不在交互页签
                        {
                            // 设置选中页签
                            setSelectTabType(clickTab);
                        }
                        
                        _m_tabPageWndScene?.showInteractionPage(interactionPageTab);//直接设置显示交互页面
                    }
                    else
                    // 没有传入交互页面显示页签类型
                    {
                        if (_m_param.consortDetailWndSelectTabType == EUnLockConsortDetailWndTabType.INTERACTION)//若当前已经在交互页签, 不需要修改点击tab
                        {
                            // 不需要进行任何操作
                        }
                        else//若不在交互页签
                        {
                            // 设置选中页签
                            setSelectTabType(clickTab);
                        }
                    }
                    break;
                
                default:
                    //设置显示页签
                    setSelectTabType(clickTab);
                    break;
            }
        }

        //模拟点击打开送礼
        private void _onSimulateClickOpenGive()
        {
            _simulateClickTab(EUnLockConsortDetailWndTabType.INTERACTION, EUnlockConsortDetailWndInteractionPageTabType.SEND_GIFT);
        }

        #endregion

        public void refreshBubble(EConsortVoiceType _voiceType, Action _showComplete = null)
        {
            if (_m_wVoiceBubble == null)
            {
                _showComplete?.Invoke();
                return;
            }
            
            _m_wVoiceBubble.refreshBubble(_voiceType, _showComplete);
        }
        
        #region 妃子操作表现

        #endregion
    }
}