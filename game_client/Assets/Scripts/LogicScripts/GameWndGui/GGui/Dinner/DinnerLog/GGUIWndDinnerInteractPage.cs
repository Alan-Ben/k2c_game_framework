using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会参与历史页面
    /// </summary>
    public class GGUIWndDinnerInteractPage: _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoDinnerInteractPage>
    {
        private GGUIWndDinnerInteractItemGrid _m_joinItemGrid;
        
        public GGUIWndDinnerInteractPage(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerInteractPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerInteractPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_joinItemGrid?.resetWnd();
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.logItemGrid)
            {
                _m_joinItemGrid = new GGUIWndDinnerInteractItemGrid(wnd.logItemGrid);
            }
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            //请求log列表
            NPPlayer.instance.dinnerComp.reqJoinedPlayerList((info) =>
            {
                List<Dinner_JoinerLogList> itemDataList = info.getJoinerList();
                _m_joinItemGrid?.showWnd();
                _m_joinItemGrid?.showItemList(itemDataList);
            });
        }
    }
}