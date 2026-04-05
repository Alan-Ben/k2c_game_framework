using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤奖励展示
    /// </summary>
    public class GGUIPrefabWndPlayerSkinRewardShow : _ANPGGUIBasicLoadPrefabSubWnd<GGUIPrefabMonoPlayerSkinRewardShow>
    {
        public NPCommonAssetPathInfo _m_iAssetPathInfo;
        public long _m_lSkinId;

        public NPGGUIWndCommonShowCase _m_skinShowCase;//皮肤形象
        
        public GGUIPrefabWndPlayerSkinRewardShow(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoShowCase != null)
                _m_skinShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onDetailBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onDetailBtnClick);
            }
            
            _m_skinShowCase?.discard();
            _m_skinShowCase = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_skinShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_skinShowCase?.resetWnd();
        }

        public void setData(long _skinId)
        {
            _m_lSkinId = _skinId;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            PlayerSkinRefObj skinRefObj = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_m_lSkinId);
            if(skinRefObj == null)
                return;

            if (_m_skinShowCase != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(skinRefObj.td_show), 0);
                _m_skinShowCase.showWnd(showCaseUnitInfoObjList);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtSkinName, GCommon.getItemName(ENPItemType.PLAYER_SKIN, _m_lSkinId));
        }

        /// <summary>
        /// 点击详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onDetailBtnClick(GameObject _go)
        {
            
        }
    }
}