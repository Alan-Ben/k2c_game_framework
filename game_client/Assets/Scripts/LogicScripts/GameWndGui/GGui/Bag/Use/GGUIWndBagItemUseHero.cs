using UnityEngine;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 背包使用物品-伙伴选择列表窗口
    /// </summary>
    public class GGUIWndBagItemUseHero : _ANPGGUIBasicWnd<GGUIMonoBagItemUseHero>
    {
        private static GGUIWndBagItemUseHero _g_instance = new GGUIWndBagItemUseHero();
        public static GGUIWndBagItemUseHero instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemUseHero();

                return _g_instance;
            }
        }

        //物品数据
        private BagItem _m_bagItem;
        //使用的物品
        private NPGGUIWndCommonItem _m_wItemWnd;
        //增加的属性类型图标
        private NPGGuiWndTexture _m_wTypeIcon;
        //骑士列表
        private GGUIWndBagItemUseHeroGrid _m_wHeroGrid;
        //使用回调
        private Action<BagItem,long> _m_aUseAction;

        public GGUIWndBagItemUseHero() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemUseHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemUseHero.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _bagItemCountChgMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemCountChgMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _bagItemCountChgMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemCountChgMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);

            _m_wItemWnd?.hideWnd();
            _m_wHeroGrid?.hideWnd();
            _m_wTypeIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemWnd?.resetWnd();
            _m_wHeroGrid?.resetWnd();
            _m_wTypeIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_bagItem = null;

            _m_wItemWnd?.discard();
            _m_wItemWnd = null;

            _m_wHeroGrid?.discard();
            _m_wHeroGrid = null;

            _m_wTypeIcon?.discard();
            _m_wTypeIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCommonItem != null)
                _m_wItemWnd = new NPGGUIWndCommonItem(wnd.monoCommonItem);

            if (wnd.monoHeroGrid != null)
                _m_wHeroGrid = new GGUIWndBagItemUseHeroGrid(wnd.monoHeroGrid);

            if(wnd.imgTypeIcon != null)
                _m_wTypeIcon = new NPGGuiWndTexture(wnd.imgTypeIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        // 初始化
        public void init(BagItem _item, Action<BagItem,long> _useAction)
        {
            if (null == wnd)
                return;

            _m_bagItem = _item;
            _m_aUseAction = _useAction;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || null == _m_bagItem)
                return;

            if (null != _m_wItemWnd)
            {
                CommonItemData itemData = new CommonItemData(_m_bagItem);
                _m_wItemWnd.showWnd();
                _m_wItemWnd.setItem(itemData);
            }

            if (null != _m_wHeroGrid)
            {
                _m_wHeroGrid.showWnd();
                _m_wHeroGrid.setInfo(_m_bagItem, _m_aUseAction);
            }

            NPCommonItem powerItem = GRefdataCoreMgr.instance.npGeneral.power_sys_info;
            if(powerItem != null && _m_wTypeIcon != null)
            {
                _m_wTypeIcon.showWnd();
                _m_wTypeIcon.setTexture(GCommon.getItemTexIcon(powerItem.itemType, powerItem.itemId));
            }

            BagItemHeroRefObj bagItemHeroRef = GRefdataCoreMgr.instance.bagItemHeroCore.getRef(_m_bagItem.itemId);
            if (bagItemHeroRef != null)
            {
                List<long> showValueList = bagItemHeroRef.show_value_list;
                if (showValueList != null)
                {
                    if (showValueList.Count == 1)
                        ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, showValueList[0].ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                    else if (showValueList.Count == 2)
                        ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                            TextTranslate.instance.getLanguage(TransKeyConst.common_interval_num_num,
                                showValueList[0].ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), showValueList[1].ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT))));
                }
            }
        }

        //背包物品变动
        private void _bagItemCountChgMsg(params object[] _objs)
        {
            if (null == _objs || _objs.Length == 0 || _m_bagItem == null)
                return;

            BagItem _item = (BagItem)_objs[0];
            // 判断是否需要更新
            if (null != _item && _item.itemId == _m_bagItem.itemId)
            {
                _refreshWnd();

                // 如果道具数量为0则关闭窗口
                if (_item.count <= 0)
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BAG_ITEM_USE_HERO);
            }
        }

        //伙伴实力变化
        private void _onHeroPowerChg(params object[] _objs)
        {
            _refreshWnd();
        }

        //关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BAG_ITEM_USE_HERO);
        }
    }
}
