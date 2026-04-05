using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择实验室窗口
    /// </summary>
    public class GGUIWndTreasureHuntLabSelect : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntLabSelect>
    {
        private static GGUIWndTreasureHuntLabSelect _g_instance;
        public static GGUIWndTreasureHuntLabSelect instance { get { return _g_instance ??= new GGUIWndTreasureHuntLabSelect(); } }
        
        private TreasureHuntLabRefObj _m_rSelectLabRefObj;//选中的实验室配表数据

        private GGUIWndTreasureHuntLabSelectContainer _m_wLabContainer;
        
        public GGUIWndTreasureHuntLabSelect() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntLabSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntLabSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoLabContainer != null)
            {
                _m_wLabContainer = new GGUIWndTreasureHuntLabSelectContainer(wnd.monoLabContainer);
                _m_wLabContainer.onLabSelect += _onLabSelect;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }

            if (_m_wLabContainer != null)
            {
                _m_wLabContainer.onLabSelect -= _onLabSelect;
                _m_wLabContainer.discard();
            }
            _m_wLabContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wLabContainer?.hideWnd();   
        }

        protected override void _onReset()
        {
            _m_wLabContainer?.resetWnd();   
        }

        public void setData(TreasureHuntLabRefObj _selectLabRefObj)
        {
            _m_rSelectLabRefObj = _selectLabRefObj;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_wLabContainer != null)
            {
                _m_wLabContainer.showWnd();
                _m_wLabContainer.setData(GRefdataCoreMgr.instance.treasureHuntLabRefCore.refList, _m_rSelectLabRefObj);
            }
        }

        private void _onLabSelect(TreasureHuntLabRefObj _selectLabRefObj)
        {
            if(_selectLabRefObj == null || _selectLabRefObj == _m_rSelectLabRefObj)
                return;
            
            if(!_selectLabRefObj.isUnlock(true))
                return;
            
            _m_rSelectLabRefObj = _selectLabRefObj;
            
            _onCloseBtnClick(null);
            GNodeTreasureHuntLab.addNode(_m_rSelectLabRefObj, GGUIWndTreasureHuntLab.instance.canPutInTreasureInfo);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SELECT_LAB);
        }
    }
}