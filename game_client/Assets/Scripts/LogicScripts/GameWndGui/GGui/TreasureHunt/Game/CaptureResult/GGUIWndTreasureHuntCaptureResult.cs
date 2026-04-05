using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物结果窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureResult : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntCaptureResult>
    {
        private static GGUIWndTreasureHuntCaptureResult _g_instance;
        public static GGUIWndTreasureHuntCaptureResult instance { get { return _g_instance ??= new GGUIWndTreasureHuntCaptureResult(); } }

        private List<_ATreasureHuntCaptureHarvestItemInfo> _m_lHarvestItemInfoList;

        private GGUIWndTreasureHuntCaptureHarvestItemContainer _m_wHarvestContainer;


        public GGUIWndTreasureHuntCaptureResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCaptureResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCaptureResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化收获物列表容器
            if (wnd.monoHarvestContainer != null)
            {
                _m_wHarvestContainer = new GGUIWndTreasureHuntCaptureHarvestItemContainer(wnd.monoHarvestContainer);
            }

            // 绑定关闭按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            }

            _m_lHarvestItemInfoList = null;

            // 销毁收获物列表容器
            _m_wHarvestContainer?.discard();
            _m_wHarvestContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏收获物列表容器
            _m_wHarvestContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置收获物列表容器
            _m_wHarvestContainer?.resetWnd();
        }


        /// <summary>
        /// 设置收获物列表数据
        /// </summary>
        public void setData(List<_ATreasureHuntCaptureHarvestItemInfo> _harvestItemInfoList)
        {
            _m_lHarvestItemInfoList = _harvestItemInfoList;

            _refreshWnd();
        }


        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            // 刷新收获物列表容器
            if (_m_wHarvestContainer != null)
            {
                _m_wHarvestContainer.showWnd();
                _m_wHarvestContainer.setData(_m_lHarvestItemInfoList);
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onClickClose(UnityEngine.GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_CAPTURE_HARVEST_RESULT);
        }
    }
}