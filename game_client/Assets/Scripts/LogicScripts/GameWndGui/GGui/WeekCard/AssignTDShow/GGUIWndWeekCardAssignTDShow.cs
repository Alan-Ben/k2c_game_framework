using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 周卡委派形象选择
    /// </summary>
    public class GGUIWndWeekCardAssignTDShow  : _ANPGGUIBasicWnd<GGUIMonoWeekCardAssignTDShow>
    {
        private static GGUIWndWeekCardAssignTDShow _m_instance = new GGUIWndWeekCardAssignTDShow();

        public static GGUIWndWeekCardAssignTDShow instance
        {
            get
            {
                if (null == _m_instance)
                    _m_instance = new GGUIWndWeekCardAssignTDShow();
                return _m_instance;
            }
        }
        
        private NPGGUIWndCommonShowCase _m_assignShowcase;
        private NPGGUIWndCommonTab _m_tabHero;
        private NPGGUIWndCommonTab _m_tabConsort;
        private GGUIWndWeekCardAssignTDShowItemContainer _m_iconItemContainer;
        private _IAssignShowData _m_selectedItemData;
        private EWeekCardNPCType _m_weekCardNpcType = EWeekCardNPCType.NONE;
        private EWeekCardNPCType _m_selectedNpcType = EWeekCardNPCType.NONE;

        public GGUIWndWeekCardAssignTDShow() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoWeekCardAssignTDShow.assetPath; }
        protected override string _monoObjName { get => GGUIMonoWeekCardAssignTDShow.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_assignShowcase?.hideWnd();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_assignShowcase?.discard();
            _m_assignShowcase = null;
            
            _m_tabHero?.discard();
            _m_tabHero = null;
            
            _m_tabConsort?.discard();
            _m_tabConsort = null;

            _m_selectedItemData = null;
            _m_weekCardNpcType = EWeekCardNPCType.NONE;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _clickConfirm);

            if (null != wnd.assignShowcase)
            {
                _m_assignShowcase = new NPGGUIWndCommonShowCase(wnd.assignShowcase);
            }

            if (null != wnd.tabHero)
            {
                _m_tabHero = new NPGGUIWndCommonTab(wnd.tabHero);
                _m_tabHero.clickDelegate += _clickTabHero;
                _m_tabHero.setSelected(false);
            }

            if (null != wnd.tabConsort)
            {
                _m_tabConsort = new NPGGUIWndCommonTab(wnd.tabConsort);
                _m_tabConsort.clickDelegate += _clickTabConsort;
                _m_tabConsort.setSelected(false);
            }

            if (null != wnd.iconItemContainer)
            {
                _m_iconItemContainer = new GGUIWndWeekCardAssignTDShowItemContainer(wnd.iconItemContainer);
                _m_iconItemContainer.onSelectItemChg += _onSelectItemChg;
            }
        }

        private void _clickConfirm(GameObject obj)
        {
            if (null == _m_selectedItemData)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.week_card_hero_no_selected_none));
                return;
            }
            //确定更换
            //判断有没有变化，没有变化就弹tip？
            if (_m_selectedNpcType == NPPlayer.instance.weekCardComp.npcType
                && _m_selectedItemData?.id == NPPlayer.instance.weekCardComp.npcId)
            {
                QueueMgr.instance.DoUIRollBackByEsc();
                return;
            }
            
            //有变化就发消息变更
            NPPlayer.instance.weekCardComp.reqWeekCardChgNPC(_m_selectedNpcType, _m_selectedItemData.id);
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        /// <summary>
        /// 选中item变化
        /// </summary>
        /// <param name="obj"></param>
        private void _onSelectItemChg(GGUIWndWeekCardAssignTDShowItem obj)
        {
            _IAssignShowData selectedItemData = obj.showData  as _IAssignShowData;
            if(selectedItemData == _m_selectedItemData)
                return;
            _m_selectedNpcType = _m_weekCardNpcType;
            _m_selectedItemData = selectedItemData;
            // _m_iconItemContainer?.setSelectItem(_m_selectedIconItem);
            _refreshTdShow(_m_selectedItemData?.td_show);
        }

        /// <summary>
        /// 点击知己页签
        /// </summary>
        /// <param name="obj"></param>
        private void _clickTabConsort(bool obj)
        {
            _m_weekCardNpcType = EWeekCardNPCType.CONSORT;
            _m_tabHero?.setSelected(false);
            _m_tabConsort?.setSelected(true);
            List<_ICommonIconShowData> showList = new List<_ICommonIconShowData>();
            _ICommonIconShowData defaultSelected = null;
            _ICommonIconShowData curSelected = null;
            List<GGottenConsortInfo> consortList = new List<GGottenConsortInfo>();
            consortList.AddRange(NPPlayer.instance.consortComp.consortList);
            consortList.Sort(_sortConsort);
            for (int i = 0; i < consortList.Count; i++)
            {
                GGottenConsortInfo gottenConsortInfo = consortList[i];
                CommonIconShowData_Consort consortIconItem = new CommonIconShowData_Consort(gottenConsortInfo.consortId);
                showList.Add(consortIconItem);
                if (_m_selectedNpcType == EWeekCardNPCType.CONSORT && null != _m_selectedItemData)
                {
                    if (_m_selectedItemData.id == gottenConsortInfo.consortId)
                    {
                        defaultSelected = consortIconItem;
                    }
                }
                if (NPPlayer.instance.weekCardComp.npcType == EWeekCardNPCType.CONSORT && gottenConsortInfo.consortId == NPPlayer.instance.weekCardComp.npcId)
                {
                    curSelected = consortIconItem;
                }
            }

            _showIconContainer(showList, defaultSelected, curSelected);
        }

        private int _sortConsort(GGottenConsortInfo x, GGottenConsortInfo y)
        {
            if (x.intimacy > y.intimacy)
                return -1;
            if (x.intimacy < y.intimacy)
                return 1;
            if (x.charm > y.charm)
                return -1;
            if (x.charm < y.charm)
                return 1;
            if (x.consortId > y.consortId)
                return 1;
            if (x.consortId < y.consortId)
                return -1;
            return 0;
        }

        /// <summary>
        /// 点击骑士页签
        /// </summary>
        /// <param name="obj"></param>
        private void _clickTabHero(bool obj)
        {

            _m_weekCardNpcType = EWeekCardNPCType.HERO;
            _m_tabHero?.setSelected(true);
            _m_tabConsort?.setSelected(false);
            
            List<_ICommonIconShowData> showList = new List<_ICommonIconShowData>();
            _ICommonIconShowData defaultSelected = null;
            _ICommonIconShowData curSelected = null;
            List<HeroInfo> heroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(heroList);
            heroList.Sort(_sortHero);
            for (int i = 0; i < heroList.Count; i++)
            {
                HeroInfo heroInfo = heroList[i];
                CommonIconShowData_Hero heroIconItem = new CommonIconShowData_Hero(heroInfo.id);
                showList.Add(heroIconItem);
                if (_m_selectedNpcType == EWeekCardNPCType.HERO && null != _m_selectedItemData)//已经有选择
                {
                    if (_m_selectedItemData.id == heroInfo.id)
                    {
                        defaultSelected = heroIconItem;
                    }
                }
                if (NPPlayer.instance.weekCardComp.npcType == EWeekCardNPCType.HERO && heroInfo.id == NPPlayer.instance.weekCardComp.npcId)
                {
                    curSelected = heroIconItem;
                }

            }
            _showIconContainer(showList, defaultSelected, curSelected);
        }

        private int _sortHero(HeroInfo x, HeroInfo y)
        {
            // if (x.power > y.power)
            //     return -1;
            // if (x.power < y.power)
            //     return 1;
            if (GCommon.getItemQuality(ENPItemType.HERO, x.id) > GCommon.getItemQuality(ENPItemType.HERO, y.id))
                return -1;
            if (GCommon.getItemQuality(ENPItemType.HERO, x.id) < GCommon.getItemQuality(ENPItemType.HERO, y.id))
                return 1;
            if (x.id < y.id)
                return -1;
            if (x.id > y.id)
                return 1;
            
            return 0;
        }

        /// <summary>
        /// 显示icon列表
        /// </summary>
        /// <param name="showList"></param>
        /// <param name="defaultSelected"></param>
        private void _showIconContainer(List<_ICommonIconShowData> showList, _ICommonIconShowData defaultSelected, _ICommonIconShowData curSelected)
        {
            if (null == _m_iconItemContainer)
                return;
            _m_iconItemContainer.showWnd();
            _m_iconItemContainer.showItemList(showList);
            _m_iconItemContainer.refreshAllItem((_itemWnd) =>
            {
                _itemWnd.setSelectShow(null != defaultSelected && _itemWnd.showData == defaultSelected);
                _itemWnd.setIsCurSelected(null != curSelected && _itemWnd.showData == curSelected);
            });
        }

        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        private void _refreshWnd()
        {
            _refreshTdShow(NPPlayer.instance.weekCardComp.getTDShowIndex());
            _clickTabHero(true);
        }
        
        /// <summary>
        /// 刷新执政官形象
        /// </summary>
        private void _refreshTdShow(NPGGoIndex _goIndex)
        {
            _m_assignShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(_goIndex));
        }
    }
}