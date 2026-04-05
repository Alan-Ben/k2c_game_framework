using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办历史页面
    /// </summary>
    public class GGUIWndDinnerStartLogPage: _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoDinnerStartLogPage>
    {
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        private GGUIWndDinnerStartItemGrid _m_startItemGrid;
        private List<DinnerStartLogIdx> _m_itemDataList;

        public GGUIWndDinnerStartLogPage(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerStartLogPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerStartLogPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            _m_startItemGrid?.discard();
            _m_startItemGrid = null;
            if (_m_itemDataList != null) 
                _m_itemDataList.Clear();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.logItemGrid)
            {
                _m_startItemGrid = new GGUIWndDinnerStartItemGrid(wnd.logItemGrid);
            }
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            //请求log列表
            NPPlayer.instance.dinnerComp.reqGetStartLogIdxList((info) =>
            {
                _m_startItemGrid?.showWnd();
                if (_m_itemDataList == null)
                    _m_itemDataList = new List<DinnerStartLogIdx>();
                _m_itemDataList.Clear();
                foreach (Dinner_StartLogIdx startLogIdx in info.getIdxList())
                {
                    _m_itemDataList.Add(new DinnerStartLogIdx(startLogIdx));
                }
                _m_startItemGrid?.showItemList(_m_itemDataList);
            });
        }
    }
}