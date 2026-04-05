using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndInnGuestListNormalPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoInnGuestListNormalPage>
    {
        [ItemNotNull, NotNull] private readonly List<InnNormalGuestHandbookInfo> _m_guestList;
        private GGUISubWndInnGuestListNormalPageGrid _m_itemGrid;
        
        
        public GGUIPrefabSubWndInnGuestListNormalPage(Transform _parent) 
            : base(_parent)
        {
            _m_guestList = new List<InnNormalGuestHandbookInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnGuestListNormalPage.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnGuestListNormalPage.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_itemGrid?.showWnd();
            
            refreshWnd();
            
            NPPlayer.instance.innComp.onNormalGuestHandbookInfoChg += refreshWnd;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onNormalGuestHandbookInfoChg -= refreshWnd;
            
            _m_itemGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_itemGrid?.discard();
            _m_itemGrid = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoItemGrid != null)
                _m_itemGrid = new GGUISubWndInnGuestListNormalPageGrid(wnd.monoItemGrid);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            NPPlayer.instance.innComp.getNormalGuestHandbookInfoListNonAlloc(_m_guestList);
            int unlockNum = 0;
            foreach (InnNormalGuestHandbookInfo guestInfo in _m_guestList)
            {
                if (guestInfo.isUnlock)
                    unlockNum++;
            }
            ALUGUICommon.setLabelTxt(wnd.txtCollectTip, TextTranslate.instance.getLanguage(TransKeyConst.inn_normalGuestCollectTip_num_num, unlockNum, _m_guestList.Count));
            _m_itemGrid?.refreshWnd();
        }
    }
}