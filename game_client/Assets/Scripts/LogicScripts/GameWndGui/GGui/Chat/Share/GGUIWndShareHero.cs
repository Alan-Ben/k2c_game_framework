using ALPackage;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴分享弹窗
    /// </summary>
    public class GGUIWndShareHero : _ANPGGUIBasicWnd<GGUIMonoShareHero>
    {
        private static GGUIWndShareHero _g_instance;

        public static GGUIWndShareHero instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareHero();
                return _g_instance;
            }
        }

        private _IShareIconSHow _m_curSelected = null;
        private GGUIWndShareIconItemGrid _m_shareIconGrid;
        private GGUIWndHeroCommonCardItem _m_wHeroCardItem;

        public GGUIWndShareHero() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareHero.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareHero.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_shareIconGrid?.hideWnd();
            _m_wHeroCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_shareIconGrid?.resetWnd();
            _m_wHeroCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curSelected = null;
            
            _m_shareIconGrid?.discard();
            _m_shareIconGrid = null;

            _m_wHeroCardItem?.discard();
            _m_wHeroCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _clickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.shareBtn, _clickShareBtn);
            if (null != wnd.shareIconGrid)
            {
                _m_shareIconGrid = new GGUIWndShareIconItemGrid(wnd.shareIconGrid);
                _m_shareIconGrid.selectedItem += _selectedItem;
            }

            if (null != wnd.monoHeroCardItem)
                _m_wHeroCardItem = new GGUIWndHeroCommonCardItem(wnd.monoHeroCardItem);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            List<_IShareIconSHow> showList = new List<_IShareIconSHow>();
            List<HeroInfo> heroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(heroList);
            heroList.Sort(_sortHeroList);
            for (int i = 0; i < heroList.Count; i++)
            {
                showList.Add(new ShareIconShow_Hero(heroList[i]));
            }
            
            _m_shareIconGrid?.showItemList(showList);
        }

        //默认排序，品质从高到低 实力从高到低 角色id
        private int _sortHeroList(HeroInfo _a, HeroInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                return -_a.isUnlock.CompareTo(_b.isUnlock);

            EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
            if (qualityA.CompareTo(qualityB) != 0)
                return -qualityA.CompareTo(qualityB);

            long powerA = _a.power;
            long powerB = _b.power;
            if (powerA.CompareTo(powerB) != 0)
                return -powerA.CompareTo(powerB);

            return _a.id.CompareTo(_b.id);
        }

        #region 点击事件

        //选中item
        private void _selectedItem(_IShareIconSHow _showData)
        {
            if (null == _showData)
                return;

            ShareIconShow_Hero heroInfo = _showData as ShareIconShow_Hero;
            if(null == heroInfo)
                return;

            _m_curSelected = _showData;

            _m_wHeroCardItem?.showWnd();
            _m_wHeroCardItem?.setInfo(heroInfo.info);
        }

        //点击关闭
        private void _clickCloseBtn(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_HERO);
        }

        //点击分享
        private void _clickShareBtn(GameObject obj)
        {
            if (null == _m_curSelected)
                return;

            GChatUtil.sendChatShareMsg(EChatShareType.HERO, _m_curSelected.creatShareInfo());
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_HERO);
        }

        #endregion
    }
}