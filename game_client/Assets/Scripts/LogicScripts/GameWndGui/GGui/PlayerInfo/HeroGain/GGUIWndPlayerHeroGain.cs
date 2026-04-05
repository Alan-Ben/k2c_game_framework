using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPlayerHeroGain : _ATALBasicUIWnd<GGUIMonoPlayerHeroGain>
    {
        [NotNull] public static GGUIWndPlayerHeroGain instance { get { return _g_instance ??= new GGUIWndPlayerHeroGain(); } }
        private static GGUIWndPlayerHeroGain _g_instance;


        [NotNull] private readonly List<PlayerHeroUnlockShowRefObj> _m_heroRefList;  
        private GGUISubWndPlayerHeroGainGrid _m_heroGrid;
        
        
        public GGUIWndPlayerHeroGain() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_heroRefList = new List<PlayerHeroUnlockShowRefObj>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoPlayerHeroGain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerHeroGain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }


        protected override void _onShowWnd()
        {
            _m_heroGrid?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_heroGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_heroGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_heroGrid?.discard();
            _m_heroGrid = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoHeroGrid != null)
                _m_heroGrid = new GGUISubWndPlayerHeroGainGrid(wnd.monoHeroGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_heroRefList.Clear();
            _m_heroRefList.AddRange(GRefdataCoreMgr.instance.getPlayerHeroUnlockShowRefListBySort());

            _m_heroGrid?.refreshWnd(_m_heroRefList);
        }
        
        
        private void _onCloseBtnClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYER_HERO_GAIN);
        }
        
    }
}