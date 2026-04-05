using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历结果窗口
    /// </summary>
    public class GGUIWndAkeyTravelResult : _ANPGGUIBasicWnd<GGUIMonoAkeyTravelResult>
    {
        private static GGUIWndAkeyTravelResult _g_instance;
        public static GGUIWndAkeyTravelResult instance { get { return _g_instance ??= new GGUIWndAkeyTravelResult(); } }

        private List<_ITravelEventResultInfo> _m_lResultInfoList;
        private long _m_lAddExp;
        
        /// <summary>
        /// 结果列表
        /// </summary>
        private GGUIWndAkeyTravelResultItemContainer _m_wResultItemContainer;
        
        /// <summary>
        /// 经验进度条
        /// </summary>
        private NPGGUIWndProgress _m_ExpProgress;
        
        public GGUIWndAkeyTravelResult() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoAkeyTravelResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAkeyTravelResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoResultItemContainer != null)
                _m_wResultItemContainer = new GGUIWndAkeyTravelResultItemContainer(wnd.monoResultItemContainer);

            if (wnd.expProgress != null)
                _m_ExpProgress = new NPGGUIWndProgress(wnd.expProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
            }
            
            _m_wResultItemContainer?.discard();
            _m_wResultItemContainer = null;
            
            _m_ExpProgress?.discard();
            _m_ExpProgress = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wResultItemContainer?.hideWnd();
            _m_ExpProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wResultItemContainer?.resetWnd();
            _m_ExpProgress?.resetWnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_resultInfoList">结果列表</param>
        /// <param name="_addExp"></param>
        public void setData(List<_ITravelEventResultInfo> _resultInfoList, long _addExp)
        {
            _m_lResultInfoList = _resultInfoList;
            _m_lAddExp = _addExp;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            string travelCountKey = string.IsNullOrEmpty(wnd.txtTravelCountKey) ? TransKeyConst.common_value : wnd.txtTravelCountKey;
            ALUGUICommon.setLabelTxt(wnd.txtTravelCount, TextTranslate.instance.getLanguage(travelCountKey, _m_lResultInfoList?.Count ?? 0));

            string addExpKey = string.IsNullOrEmpty(wnd.txtAddExpKey) ? TransKeyConst.common_add_num : wnd.txtAddExpKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddExp, TextTranslate.instance.getLanguage(addExpKey, _m_lAddExp));
            //设置玩家经验条，用经验值减去当前等级的初始经验
            if (null != _m_ExpProgress)
            {
                long showExp = NPPlayer.instance.rescourceComp.getValue(ECurrency.P_EXP) - NPPlayer.instance.playerInfo.curLevelRef.exp;
                if (showExp < 0)
                    showExp = 0;

                _m_ExpProgress.showWnd();
                if(NPPlayer.instance.playerInfo.nextLevelRef != null && NPPlayer.instance.playerInfo.curLevelRef != null)
                    _m_ExpProgress.setProgress(showExp, (NPPlayer.instance.playerInfo.nextLevelRef.exp - NPPlayer.instance.playerInfo.curLevelRef.exp), EValueFormatType.NORMAL_NOT_LARGE_STR);
                else
                    _m_ExpProgress.setProgress(showExp, showExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }

            if (_m_wResultItemContainer != null)
            {
                _m_wResultItemContainer.showWnd();
                _m_wResultItemContainer.setData(_m_lResultInfoList);
            }
        }

        /// <summary>
        /// 确认按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_AKEY_RESULT);
        }
    }
}