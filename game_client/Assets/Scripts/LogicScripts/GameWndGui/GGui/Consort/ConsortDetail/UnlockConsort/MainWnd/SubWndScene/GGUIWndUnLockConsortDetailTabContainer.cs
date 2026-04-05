using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;

/************************
 * 可在Container框架内被包含或被容纳的scene对象
 **/
namespace GOE
{
    /// <summary>
    /// 这里用类合适，不要用struct其实更耗内存
    /// </summary>
    public class UnLockConsortDetailTabPageInfo
    {
        //页面类型
        public EUnLockConsortDetailWndTabType tabType;
        //显示窗口对象
        public _IUnLockConsortDetailTabPage tabPageWnd;

        public UnLockConsortDetailTabPageInfo(EUnLockConsortDetailWndTabType _tabType, _IUnLockConsortDetailTabPage _wnd)
        {
            tabType = _tabType;
            tabPageWnd = _wnd;
        }
    }

    /// <summary>
    /// 解锁妃子中，信息展示的子窗口的容器对象
    /// 用容器对象管理可以方便资源管理和释放
    /// </summary>
    public class GGUIWndUnLockConsortDetailTabContainer : _AALBasicSubContainerScene
    {
        //点击子窗口关闭的时候的响应函数
        private Action<EUnLockConsortDetailWndTabType> _m_aOnClickTabClose;

        //当前显示对象
        private UnLockConsortDetailTabPageInfo _m_piCurPageInfo;
        //每个页签显示的窗口对象，在构造的时候带入
        [NotNull] private List<UnLockConsortDetailTabPageInfo> _m_lTabPageList = new List<UnLockConsortDetailTabPageInfo>();//tab字典

        private _IGGUIWndUnLockConsortMainDetailWndParam _m_param;
        private GGottenConsortInfo _m_ShowConsortInfo;//当前显示的妃子信息
        
        public GGUIWndUnLockConsortDetailTabContainer(_IGGUIWndUnLockConsortMainDetailWndParam _param, List<GGUIMonoUnLockConsortDetailWndTabSetting> _tabSettingList, Action<EUnLockConsortDetailWndTabType> _onClickTabClose)
            : base(0)
        {
            _m_param = _param;
            _m_piCurPageInfo = null;
            _m_aOnClickTabClose = _onClickTabClose;

            foreach (var tabSetting in _tabSettingList)
            {
                if (tabSetting == null)
                    continue;

#if UNITY_EDITOR
                //这里只需要在editor做检查
                if (null != lookupTabPageStruct(tabSetting.tabType))
                {
                    Debug.LogWarning($"[GGUIWndUnLockConsortDetail _initTabAndPage] warning, tabSetting.tabType : {tabSetting.tabType} 重复配置");
                    continue;
                }
#endif

                //此处只构建结构体对象，避免窗口多次构造
                _IUnLockConsortDetailTabPage tabPageWnd = null;
                NPCommonAssetPathInfo uiResInfo = NPCommonAssetPathInfo.readFromUiResId(tabSetting.resPathId);
                switch (tabSetting.tabType)
                {
                    case EUnLockConsortDetailWndTabType.PROFILE:
                        tabPageWnd = (new GGUIWndUnlockConsortDetailProfilePage(uiResInfo, tabSetting.goParent));
                        break;

                    case EUnLockConsortDetailWndTabType.HALO:
                        tabPageWnd = (new GGUIWndUnLockConsortDetailHaloPage(_m_param.consortDetailWndHaloPageParam, uiResInfo, tabSetting.goParent));
                        break;

                    case EUnLockConsortDetailWndTabType.FETTER:
                        tabPageWnd = (new GGUIWndUnLockConsortDetailFetterPage(uiResInfo, tabSetting.goParent));
                        break;

                    case EUnLockConsortDetailWndTabType.BUSINESS:
                        tabPageWnd = (new GGUIWndUnLockConsortDetailBusinessPage(_m_param.consortDetailWndBusinessPageParam, uiResInfo, tabSetting.goParent));
                        break;

                    case EUnLockConsortDetailWndTabType.BLESS:
                        tabPageWnd = (new GGUIWndUnLockConsortDetailBlessPage(uiResInfo, tabSetting.goParent));
                        break;

                    case EUnLockConsortDetailWndTabType.INTERACTION:
                        tabPageWnd = (new GGUIWndUnlockConsortDetailInteractionPage(_m_param.consortDetailWndInteractionPageParam, uiResInfo, tabSetting.goParent));
                        break;

                    default:
                        //默认情况下直接报错返回
                        Debug.LogError($"错误的窗口类型配置{tabSetting.tabType}");
                        break;
                }

                _m_lTabPageList.Add(new UnLockConsortDetailTabPageInfo(tabSetting.tabType, tabPageWnd));
            }
        }

        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        /// <returns></returns>
        public override bool needDiscardOnSwitch { get { return true; } }

        /** 在进入本场景时调用的事件函数 */
        protected override void _onEnterScene()
        {
            //本场景是空的，因此直接设置完成
            setSceneInited();
        }


        /** 在初始化本场景完成后调用的事件函数 */
        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 由于本类重载之后需要处理的退出scene的处理
        /// </summary>
        protected override void _dealQuitScene()
        {
            _m_lTabPageList.Clear();
        }

        /// <summary>
        /// 初始化的显示窗口操作，显示完成则调用回调
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
        }

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void _dealHideScene(Action _delegate)
        {
        }

        /// <summary>
        /// 离开本附加Scene时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene()
        {
        }

        /// <summary>
        /// 在尝试切换Scene的时候触发的函数
        /// </summary>
        protected override void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }

        /// <summary>
        /// 在切换Scene完成的时候触发的函数
        /// </summary>
        protected override void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }

        public void setShowConsortInfo(GGottenConsortInfo _consortInfo)
        {
            _m_ShowConsortInfo = _consortInfo;
            
            if(_m_piCurPageInfo == null || _m_piCurPageInfo.tabPageWnd == null)
                return;
            
            _m_piCurPageInfo.tabPageWnd.setData(_consortInfo, _onCloseTabPage);
        }
        
        /// <summary>
        /// 显示对应窗口类型
        /// </summary>
        /// <param name="_tabType"></param>
        public void setShowPage(EUnLockConsortDetailWndTabType _tabType)
        {
            //查询显示对象，直接调用显示
            //使用底层接口，将窗口生命周期交给本窗口
            UnLockConsortDetailTabPageInfo info = lookupTabPageStruct(_tabType);
            if(null != info)
            {
                if (null != info.tabPageWnd)
                {
                    _m_piCurPageInfo = info;
                    //设置数据
                    info.tabPageWnd.setData(_m_ShowConsortInfo, _onCloseTabPage);
                    //调用本窗口显隐
                    showMainWnd(info.tabPageWnd.tabWndObj);
                }
            }
        }

        /// <summary>
        /// 直接设置显示交互页面
        /// </summary>
        /// <param name="_interactionPageTab"></param>
        public void showInteractionPage(EUnlockConsortDetailWndInteractionPageTabType _interactionPageTab)
        {
            // 当前正显示交互页面
            if (_m_piCurPageInfo != null && _m_piCurPageInfo.tabType == EUnLockConsortDetailWndTabType.INTERACTION)
            {
                if (_m_piCurPageInfo.tabPageWnd != null && _m_piCurPageInfo.tabPageWnd is GGUIWndUnlockConsortDetailInteractionPage)
                {
                    GGUIWndUnlockConsortDetailInteractionPage interactionPage = _m_piCurPageInfo.tabPageWnd as GGUIWndUnlockConsortDetailInteractionPage;
                    // 设置选中页签
                    interactionPage?.setSelectTab(_interactionPageTab);
                }
            }
            // 当前没有在显示交互页面
            else
            {
                // 设置交互页面显示参数
                if (_m_param != null && _m_param.consortDetailWndInteractionPageParam != null)
                {
                    _m_param.consortDetailWndInteractionPageParam.interactionPageTabType = _interactionPageTab;
                }
                
                // 显示交互页面
                setShowPage(EUnLockConsortDetailWndTabType.INTERACTION);
            }
        }

        /// <summary>
        /// 查询对应Tab类型的显示数据对象
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public UnLockConsortDetailTabPageInfo lookupTabPageStruct(EUnLockConsortDetailWndTabType _type)
        {
            for(int i = 0; i < _m_lTabPageList.Count; i++)
            {
                if (_m_lTabPageList[i]?.tabType == _type)
                    return _m_lTabPageList[i];
            }

            return null;
        }

        /// <summary>
        /// 关闭子窗口调用的函数
        /// </summary>
        protected void _onCloseTabPage(EUnLockConsortDetailWndTabType _tabType)
        {
            //调用响应函数
            if(null != _m_aOnClickTabClose)
                _m_aOnClickTabClose(_tabType);
        }
        
        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="_show"></param>
        public void onlyShowActor(bool _show)
        {
            if (_m_piCurPageInfo != null)
            {
                _m_piCurPageInfo.tabPageWnd?.onlyShowActor(_show);
            }
        }
    }
}

