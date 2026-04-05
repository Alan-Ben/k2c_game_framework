using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public struct LockConsortDetailTabAndPageStruct
    {
        public GGUIWndLockConsortDetailTab tabWnd;
        public _ILockConsortDetailTabPage tabPageWnd;
    }
    
    /// <summary>
    /// 未解锁妃子详细信息
    /// </summary>
    public class GGUIWndLockConsortDetail : _ANPGGUIBasicResBarWnd<GGUIMonoLockConsortDetail>
    {
        private static GGUIWndLockConsortDetail _g_instance;
        public static GGUIWndLockConsortDetail instance { get { return _g_instance ??= new GGUIWndLockConsortDetail(); } }
        
        private List<_IConsortShowInfo> _m_lAllConsortShowInfoList = new List<_IConsortShowInfo>();//所有妃子列表
        private int _m_iCurConsortIndex = 0;//当前妃子索引
        private _IConsortShowInfo _m_curConsortShowData;//当前妃子配置数据
        
        private ELockConsortDetailWndTabType _m_eCurTabType = ELockConsortDetailWndTabType.NONE;//当前显示的tab类型
        [NotNull] private Dictionary<ELockConsortDetailWndTabType, LockConsortDetailTabAndPageStruct> _m_dTabAndPageDic = new Dictionary<ELockConsortDetailWndTabType, LockConsortDetailTabAndPageStruct>();//tab字典

        private GGUISubWndConsortDetailInfo _m_wndConsortDetailInfo;//妃子详细信息子窗口
        private GGUIWndConsortVoiceBubble _m_wConsortVoiceBubble;//妃子语音气泡mono
        
        public GGUIWndLockConsortDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoLockConsortDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoLockConsortDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get{return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.detailInfoSubWnd != null)
                _m_wndConsortDetailInfo = new GGUISubWndConsortDetailInfo(wnd.detailInfoSubWnd);
            
            if(wnd.monoVoiceBubble != null)
                _m_wConsortVoiceBubble = new GGUIWndConsortVoiceBubble(wnd.monoVoiceBubble);
            
            _initTabAndPage();
            
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPreBtn);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNextBtn);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _clickReturnBtn);
            
            ALUGUICommon.combineBtnClick(wnd.btnConsortSource, _clickSourceBtn);
            ALUGUICommon.combineBtnClick(wnd.btnFetterDetail, _clickFettersSkillDetailBtn);
        }
        
        protected override void _onDiscard()
        {
            _m_wndConsortDetailInfo?.discard();
            _m_wndConsortDetailInfo = null;
            
            _m_wConsortVoiceBubble?.discard();
            _m_wConsortVoiceBubble = null;
            
            _discardTabAndPage();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPreBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNextBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _clickReturnBtn);
            
                ALUGUICommon.uncombineBtnClick(wnd.btnConsortSource, _clickSourceBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnFetterDetail, _clickFettersSkillDetailBtn);
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wndConsortDetailInfo?.hideWnd();
            _m_wConsortVoiceBubble?.hideWnd();
            
            _dealTabAndPage(_tab => _tab?.hideWnd(), _tabPage => _tabPage?.hideWnd());
        }

        protected override void _onReset()
        {
            _m_wndConsortDetailInfo?.resetWnd();
            _m_wConsortVoiceBubble?.resetWnd();
            
            _dealTabAndPage(_tab => _tab?.resetWnd(), _tabPage => _tabPage?.resetWnd());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setConsortShowInfo(List<_IConsortShowInfo> _allConsortShowInfoList, int _curConsortIndex, bool _isPlayEnterAni)
        {
            if (_allConsortShowInfoList == null || _allConsortShowInfoList.Count <= 0)
            {
                Debug.LogError($"[GGUIWndLockConsortDetail setData] error, _allConsortShowInfoList is null or empty");
                return;
            }
            
            _m_lAllConsortShowInfoList = _allConsortShowInfoList;

            // 只有给定下标合法时才赋值, 否则直接使用上次坐标
            if (_curConsortIndex >= 0 && _curConsortIndex < _m_lAllConsortShowInfoList.Count)
                _m_iCurConsortIndex = _curConsortIndex;
            _m_iCurConsortIndex = Math.Clamp(_m_iCurConsortIndex, 0, _allConsortShowInfoList.Count - 1);
            
            _m_curConsortShowData = _m_lAllConsortShowInfoList[_m_iCurConsortIndex];
            if (_m_curConsortShowData == null)
            {
                Debug.LogError($"[GGUIWndLockConsortDetail setData] error, 传入的参数_allConsortShowInfoList中含有null元素, index:{_m_iCurConsortIndex}");
                return;
            }
            
            if (_m_curConsortShowData != null && _m_wConsortVoiceBubble != null)
            {
                _m_wConsortVoiceBubble.showWnd();
                _m_wConsortVoiceBubble.setInfo(_m_curConsortShowData.consortId, EConsortVoiceType.Locked);
                _m_wConsortVoiceBubble.refreshBubble();
            }
            
            if (_m_wndConsortDetailInfo != null)
            {
                _m_wndConsortDetailInfo.showWnd();
                _m_wndConsortDetailInfo.setData(_m_curConsortShowData, _isPlayEnterAni);
            }

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.lessThanOrEqualOneConsortHideGoList, _m_lAllConsortShowInfoList.Count > 1);
            }
            
            _refreshTabAndPageShow();
        }

        /// <summary>
        /// 当点击查看上一妃子按钮时
        /// </summary>
        private void _onClickPreBtn(GameObject _gameObject)
        {
            if (_m_lAllConsortShowInfoList == null || _m_lAllConsortShowInfoList.Count <= 1)//若妃子数量小于等于1, 则不处理
                return;

            _m_iCurConsortIndex = (_m_iCurConsortIndex - 1 + _m_lAllConsortShowInfoList.Count) % _m_lAllConsortShowInfoList.Count;
            setConsortShowInfo(_m_lAllConsortShowInfoList, _m_iCurConsortIndex, false);
        }

        private void _onClickNextBtn(GameObject _gameObject)
        {
            if (_m_lAllConsortShowInfoList == null || _m_lAllConsortShowInfoList.Count <= 1)//若妃子数量小于等于1, 则不处理
                return;

            _m_iCurConsortIndex = (_m_iCurConsortIndex + 1 + _m_lAllConsortShowInfoList.Count) % _m_lAllConsortShowInfoList.Count;
            setConsortShowInfo(_m_lAllConsortShowInfoList, _m_iCurConsortIndex, false);
        }

        private void _clickReturnBtn(GameObject _gameObject)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LOCK_CONSORT_DETAIL);
        }

        /// <summary>
        /// 点击获取来源按钮
        /// </summary>
        private void _clickSourceBtn(GameObject _gameObject)
        {
            if (_m_curConsortShowData == null || _gameObject == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                TextTranslate.instance.getLanguage(TransKeyConst.consort_accessWayDesc_str, TextTranslate.instance.getLanguage(GCommon.getItemSource(ENPItemType.CONSORT, _m_curConsortShowData.consortId))),
                (RectTransform)_gameObject.transform, 0, 0));
        }

        /// <summary>
        /// 点击羁绊技能详情按钮
        /// </summary>
        private void _clickFettersSkillDetailBtn(GameObject _gameObject)
        {
            if (_m_curConsortShowData == null || _m_curConsortShowData.consortRefObj == null || _gameObject == null || wnd == null)
                return;

            ConsortFettersSkillRefObj skillRefObj = GRefdataCoreMgr.instance.consortFettersSkillRefCore.getRef(_m_curConsortShowData.consortRefObj.consort_fetters_skill_id);
            if (skillRefObj == null)
            {
                Debug.LogError($"[GGUIWndLockConsortDetail _clickFettersSkillDetailBtn] error, 找不到妃子:{_m_curConsortShowData.consortId} 的羁绊技能:{_m_curConsortShowData.consortRefObj.consort_fetters_skill_id} 配表数据");
                return;
            }
            
            // 未解锁, 羁绊等级默认为-1
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortFetterSkillEffect(
                wnd.fetterDetailToolTipUIResId, -1, skillRefObj, (RectTransform)_gameObject.transform, 0, 0));
        }
        
        
        #region tab相关

        /// <summary>
        /// 初始化tab以及其page
        /// </summary>
        private void _initTabAndPage()
        {
            if(wnd == null || wnd.tabSettingList == null)
                return;

            _discardTabAndPage();//先销毁之前的窗口
            foreach (var tabSetting in wnd.tabSettingList)
            {
                if(tabSetting == null)
                    continue;

                if (_m_dTabAndPageDic.TryGetValue(tabSetting.tabType, out LockConsortDetailTabAndPageStruct _tabAndPageStruct))
                {
                    Debug.LogWarning($"[GGUIWndLockConsortDetail _initTabAndPage] warning, tabSetting.tabType : {tabSetting.tabType} 重复配置", wnd);
                    continue;
                }

                GGUIWndLockConsortDetailTab tabWnd = new GGUIWndLockConsortDetailTab(tabSetting);
                tabWnd.onTabClick += _onTabClick;
                
                _ILockConsortDetailTabPage tabPageWnd = null;
                switch (tabSetting.tabType)
                {
                    case ELockConsortDetailWndTabType.PROFILE:
                        tabPageWnd = new GGUIWndLockConsortDetailProfilePage(tabSetting.pagePathInfo, wnd.tabPageParent);
                        break;
                    
                    case ELockConsortDetailWndTabType.BLESS:
                        tabPageWnd = new GGUIWndLockConsortDetailBlessPage(tabSetting.pagePathInfo, wnd.tabPageParent);
                        break;
                }

                _m_dTabAndPageDic[tabSetting.tabType] = new LockConsortDetailTabAndPageStruct() { tabWnd = tabWnd, tabPageWnd = tabPageWnd };
            }
        }

        /// <summary>
        /// 销毁tab以及其page
        /// </summary>
        private void _discardTabAndPage()
        {
            foreach (var tabAndPageStruct in _m_dTabAndPageDic.Values)
            {
                if (tabAndPageStruct.tabWnd != null)
                {
                    tabAndPageStruct.tabWnd.onTabClick -= _onTabClick;
                    tabAndPageStruct.tabWnd.discard();
                }
                
                if(tabAndPageStruct.tabPageWnd != null)
                    tabAndPageStruct.tabPageWnd.discard();
            }
            _m_dTabAndPageDic.Clear();
        }

        private void _dealTabAndPage(Action<GGUIWndLockConsortDetailTab> _dealTabAction, Action<_ILockConsortDetailTabPage> _dealTabPageAction)
        {
            foreach (var tabAndPageStruct in _m_dTabAndPageDic.Values)
            {
                if(tabAndPageStruct.tabWnd != null)
                    _dealTabAction?.Invoke(tabAndPageStruct.tabWnd);
                
                if(tabAndPageStruct.tabPageWnd != null)
                    _dealTabPageAction?.Invoke(tabAndPageStruct.tabPageWnd);
            }
        }

        private void _onTabClick(GGUIWndLockConsortDetailTab _tabWnd)
        {
            if(_tabWnd == null)
                return;
            
            setTabAndPageShowType(_tabWnd.tabSetting.tabType, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_tabType"></param>
        public void setTabAndPageShowType(ELockConsortDetailWndTabType _tabType, bool _forceRefresh = true)
        {
            // 若当前选中tab与传入tab相同, 且不强制刷新, 则不进行刷新
            if((_m_eCurTabType == _tabType && !_forceRefresh))
                return;
            
            // 若传入tab为None, 显示默认tab
            if (_tabType == ELockConsortDetailWndTabType.NONE)
                _tabType = wnd.defaultTabType;
            _m_eCurTabType = _tabType;
            
            _refreshTabAndPageShow();
        }
        
        /// <summary>
        /// 刷新tab和page显示
        /// </summary>
        private void _refreshTabAndPageShow()
        {
            if(wnd == null || !isShow)
                return;

            if (!_m_dTabAndPageDic.ContainsKey(_m_eCurTabType))//若需要显示的tab不存在, 则默认显示UI配置
                _m_eCurTabType = wnd.defaultTabType;
                
            foreach (var tabAndPageKV in _m_dTabAndPageDic)
            {
                LockConsortDetailTabAndPageStruct tabAndPageStruct = _m_dTabAndPageDic[tabAndPageKV.Key];
                if (tabAndPageStruct.tabWnd != null)
                {
                    tabAndPageStruct.tabWnd.showWnd();
                    tabAndPageStruct.tabWnd.setSelected(tabAndPageKV.Key == _m_eCurTabType);
                }
                
                if (tabAndPageStruct.tabPageWnd != null)
                {
                    if (tabAndPageKV.Key == _m_eCurTabType)
                    {
                        //如未加载的需要补充加载操作
                        if (!tabAndPageStruct.tabPageWnd.isLoaded)
                            tabAndPageStruct.tabPageWnd.load();
                        
                        tabAndPageStruct.tabPageWnd.regLoadDoneDelegate(() =>
                        {
                            tabAndPageStruct.tabPageWnd.showWnd();
                            tabAndPageStruct.tabPageWnd.setData(_m_curConsortShowData);       
                        });
                    }
                    else
                    {
                        tabAndPageStruct.tabPageWnd.hideWnd();
                    }
                }
            }
        }
        
        #endregion
    }
}