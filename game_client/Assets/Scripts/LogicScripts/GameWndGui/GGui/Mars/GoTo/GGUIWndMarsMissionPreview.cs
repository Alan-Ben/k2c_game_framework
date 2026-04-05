using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星任务预览界面
    /// </summary>
    public class GGUIWndMarsMissionPreview : _ANPGGUIBasicWnd<GGUIMonoMarsMissionPreview>
    {
        private static GGUIWndMarsMissionPreview _g_instance;
        public static GGUIWndMarsMissionPreview instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsMissionPreview();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndMarsMissionPreviewTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndMarsMissionPreviewTab _m_wSelectTabWnd;

        public GGUIWndMarsMissionPreview() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsMissionPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsMissionPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndMarsMissionPreviewTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }
            _m_wSelectTabWnd = null;
        }

        protected override void _onDiscard()
        {
            _m_wSelectTabWnd = null;

            if (_m_lTabWndList != null)
            {
                GGUIWndMarsMissionPreviewTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_wSelectTabWnd = null;

            _m_lTabWndList = new List<GGUIWndMarsMissionPreviewTab>();
            if (null != wnd.monoTabList)
            {
                GGUIMarsMissionPreviewTabMono tempTabMono = null;
                GGUIWndMarsMissionPreviewTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndMarsMissionPreviewTab(tempTabMono.monoTab, tempTabMono.tabType, tempTabMono.goShowList);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        //刷新列表
        private void _refreshWnd()
        {
            //设置选择页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndMarsMissionPreviewTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == EMarsMissionType.GO_TO)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
            }
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndMarsMissionPreviewTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            _m_wSelectTabWnd?.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            _m_wSelectTabWnd?.setSelected(true);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_MISSION_PREVIEW);
        }
    }
}