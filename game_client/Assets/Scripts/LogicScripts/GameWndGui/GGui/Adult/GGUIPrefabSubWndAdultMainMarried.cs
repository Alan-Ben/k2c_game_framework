using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndAdultMainMarried : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoAdultMainMarried>
    {
        [ItemNotNull, NotNull] private readonly List<MarriedInfo> _m_marriedInfoList;
        
        private GGUISubWndAdultMarriedGrid _m_adultGrid;
        
        
        public GGUIPrefabSubWndAdultMainMarried(Transform _parent) 
            : base(_parent)
        {
            _m_marriedInfoList = new List<MarriedInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultMainMarried.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultMainMarried.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_adultGrid?.showWnd();

            refreshWnd();
            
            NPPlayer.instance.childComp.onMarriedAdultChg += refreshWnd;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.childComp.onMarriedAdultChg -= refreshWnd;
            
            _m_adultGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultGrid?.discard();
            _m_adultGrid = null;

            if (wnd == null)
                return;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultGrid != null)
                _m_adultGrid = new GGUISubWndAdultMarriedGrid(wnd.monoAdultGrid);
        }
        
        
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTotalNum, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARRIED_COUNT));
            ALUGUICommon.setLabelTxt(wnd.txtGridMaxCount, TextTranslate.instance.getLanguage(TransKeyConst.adult_marriedAdultMaxShowCount_num, GRefdataCoreMgr.instance.npGeneral.married_adult_limit));
            NPPlayer.instance.childComp.getMarriedChildListNonAlloc(_m_marriedInfoList);
            _m_adultGrid?.refreshWnd(_m_marriedInfoList);
        }
    }
}