using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 背包道具可合成道具列表页签
    /// </summary>
    public class GGUIWndBagConvertPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoBagConvertPage>
    {
        private long _m_resPathId;
        private GGUIWndBagConvertItemGrid _m_wConvertItemGrid;//可合成道具列表

        public GGUIWndBagConvertPage(long _resPathId, Transform _parent)
            : base(_parent)
        {
            _m_resPathId = _resPathId;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_resPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_resPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onAddItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onRemoveItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);
            _m_wConvertItemGrid?.showWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onAddItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onRemoveItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);
            _m_wConvertItemGrid?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wConvertItemGrid?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wConvertItemGrid?.discard();
            _m_wConvertItemGrid = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoConvertItemGrid)
                _m_wConvertItemGrid = new GGUIWndBagConvertItemGrid(wnd.monoConvertItemGrid);
        }

        //刷新和合成道具
        private void _refreshConvertItem(long _itemId)
        {
            ItemConvertRefObj itemConvertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_itemId);
            if (itemConvertRef != null && itemConvertRef.target_item != null)
            {
                if (_m_wConvertItemGrid != null)
                    _m_wConvertItemGrid.refreshConvertItem(itemConvertRef.target_item.itemId);
            }
        }


        #region 消息事件

        // 刷新物品
        private void _onUpdateItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow || _item == null)
                return;

            // 判断是否需要更新
            if (_m_wConvertItemGrid != null)
                _m_wConvertItemGrid.updateItem(_item);

            _refreshConvertItem(_item.itemId);
        }

        // 移除物品
        private void _onRemoveItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow || _item == null)
                return;

            // 判断是否需要更新
            if (_m_wConvertItemGrid != null)
                _m_wConvertItemGrid.removeItem(_item);

            _refreshConvertItem(_item.itemId);
        }

        // 添加物品
        private void _onAddItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow || _item == null)
                return;

            // 判断是否需要更新
            if (_m_wConvertItemGrid != null)
                _m_wConvertItemGrid.addItem(_item);

            _refreshConvertItem(_item.itemId);
        }

        #endregion
    }
}