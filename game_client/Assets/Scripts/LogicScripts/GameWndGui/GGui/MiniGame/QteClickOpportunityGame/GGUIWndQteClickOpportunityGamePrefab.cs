using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndQteClickOpportunityGamePrefab : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoQteClickOpportunityGamePrefab>
    {
        private NPCommonAssetPathInfo _m_assetPathInfo;
        
        [NotNull] private List<GGUIWndQteClickOpportunityItem> _m_lItemWndList = new List<GGUIWndQteClickOpportunityItem>();
        
        protected override string _monoAssetPath { get { return _m_assetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GGUIWndQteClickOpportunityGamePrefab(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_parent)
        {
            _m_assetPathInfo = _assetPath;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.itemList != null)
            {
                GGUIWndQteClickOpportunityItem itemWnd = null;
                for(int i = 0;i < wnd.itemList.Count; i++)
                {
                    GGUIMonoQteClickOpportunityItem itemMono = wnd.itemList[i];
                    if(itemMono == null)
                        continue;

                    itemWnd = new GGUIWndQteClickOpportunityItem(itemMono);
                    _m_lItemWndList.Add(itemWnd);
                }
            }
        }
        
        protected override void _onDiscard()
        {
            foreach (GGUIWndQteClickOpportunityItem itemWnd in _m_lItemWndList)
            {
                if(itemWnd != null)
                    itemWnd.discard();
            }
            _m_lItemWndList.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 对所有item窗口的操作
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllItemWnd(Action<GGUIWndQteClickOpportunityItem> _action)
        {
            if (_action == null)
                return;

            _m_lItemWndList.ForEach(_action);
        }

        public void setGameState(EQteClickOpportunityGameState _gameState)
        {
            if (wnd == null || wnd.stateShow == null)
                return;
            
            wnd.stateShow.setShowData(_gameState);
        }
    }
}