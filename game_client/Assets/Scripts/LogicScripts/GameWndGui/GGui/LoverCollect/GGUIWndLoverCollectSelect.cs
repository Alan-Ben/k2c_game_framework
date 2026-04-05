using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndLoverCollectSelect : _ATALBasicUIWnd<GGUIMonoLoverCollectSelect>
    {
        [NotNull] public static GGUIWndLoverCollectSelect instance { get { return _g_instance ??= new GGUIWndLoverCollectSelect(); } }
        private static GGUIWndLoverCollectSelect _g_instance;


        [NotNull, ItemNotNull] private readonly List<GGUISubWndLoverCollectSelectItem> _m_itemWndList;


        public GGUIWndLoverCollectSelect()
            : base(EALUIWndLayer.ADDITION)
        {
            _m_itemWndList = new List<GGUISubWndLoverCollectSelectItem>();
        }


        protected override string _monoAssetPath { get { return GGUIMonoLoverCollectSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoLoverCollectSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public long performGroupId { get { return wnd != null ? wnd.performGroupId : 0; } }


        protected override void _onShowWnd()
        {
            foreach (GGUISubWndLoverCollectSelectItem item in _m_itemWndList)
                item.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            foreach (GGUISubWndLoverCollectSelectItem item in _m_itemWndList)
                item.hideWnd();
        }
        protected override void _onReset()
        {
            foreach (GGUISubWndLoverCollectSelectItem item in _m_itemWndList)
                item.resetWnd();
        }
        protected override void _onDiscard()
        {
            foreach (GGUISubWndLoverCollectSelectItem item in _m_itemWndList)
                item.discard();
            _m_itemWndList.Clear();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);

            if (wnd.listLoverItems != null)
            {
                foreach (GGUIMonoLoverCollectSelectItem mono in wnd.listLoverItems)
                {
                    if (mono != null)
                        _m_itemWndList.Add(new GGUISubWndLoverCollectSelectItem(mono));
                }
            }
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            List<long> loverIds = GRefdataCoreMgr.instance.npGeneral.lover_collect_lover_ids;
            if (loverIds == null)
                return;

            for (int i = 0; i < _m_itemWndList.Count && i < loverIds.Count; i++)
                _m_itemWndList[i].setInfo(loverIds[i]);
        }


        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LOVER_COLLECT_SELECT);
        }
    }
}
