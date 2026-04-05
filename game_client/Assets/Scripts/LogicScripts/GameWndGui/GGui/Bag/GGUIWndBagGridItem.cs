using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GOE
{
    // 背包物品
    public class GGUIWndBagGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoBagGridItem>
    {
        // 物品
        private NPGGUIWndCommonItem _m_wItem = null;
        //点击事件
        private Action<int> _m_selectDelegate;

        public GGUIWndBagGridItem(GGUIMonoBagGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onDiscard()
        {
            if(_m_wItem != null)
                _m_wItem.discard();
            _m_wItem = null;
            _m_selectDelegate = null;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if(_m_wItem != null)
                _m_wItem.resetWnd();
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            if(null != _m_wItem)
                _m_wItem.resetWnd();
        }

        protected override void _onShowWnd()
        {
        }

        // 初始化
        protected override void _onWndInitDone()
        {
            // 物品
            if(wnd.item != null)
                _m_wItem = new NPGGUIWndCommonItem(wnd.item);

            //绑定点击处理
            ALUGUICommon.combineBtnClick(wnd.clickGo, _onClick);
        }

        public void setSelectDelegate(Action<int> _delegate)
        {
            if (null == _delegate)
                return;
            _m_selectDelegate = _delegate;
        }

        // 初始化UI
        public void refreshItem(BagItemShowInfo _item, bool _isSelected)
        {
            if (_item == null || null == _item.bagItemRef)
                return;

            // 显示物品
            _m_wItem?.showWnd(_item.itemData);
            //设置是否选中
            ALUGUICommon.setGameObjEnable(wnd.selectTag, _isSelected);
            //物品数量为空时显隐
            ALUGUICommon.setGameObjEnable(wnd.unableShow, _item.bagItem == null || _item.bagItem.count == 0);
            //刷新红点
            _refreshRedTip(_item);
        }

        //刷新红点
        private void _refreshRedTip(BagItemShowInfo _item)
        {
            if (wnd == null || _item == null || _item.bagItemRef == null)
                return;

            //设置红点
            if (wnd.goRedTipList != null && wnd.goRedTipList.Count > 0)
            {
                bool isSet = false;
                for (int i = 0; i < wnd.goRedTipList.Count; i++)
                {
                    if (wnd.goRedTipList[i] == null)
                        continue;

                    //只要有设置红点了，就不需要继续设置了
                    if (isSet)
                    {
                        ALUGUICommon.setGameObjEnable(wnd.goRedTipList[i].redTipGo, false);
                        continue;
                    }

                    // 记录窗口红点状态
                    isSet = AccountSettingMgr.instance.bagItemWndRedTipSaver.getCanShowRedTip(_item.bagItemRef.id, wnd.goRedTipList[i].redTipType);
                    // //根据类型设置红点
                    // switch (wnd.goRedTipList[i].redTipType)
                    // {
                    //     case EBagItemRedTipType.NEW:
                    //         isSet = _item.isNew;
                    //         break;
                    //     case EBagItemRedTipType.COMBINE:
                    //         isSet = GCommon.isItemCanCombine(_item.bagItemRef.id, 1);
                    //         break;
                    //     case EBagItemRedTipType.BE_COMBINE:
                    //         isSet = GCommon.isItemCanBeCombine(ENPItemType.BAG_ITEM, _item.bagItemRef.id);
                    //         break;
                    //     case EBagItemRedTipType.ADD:
                    //         isSet = _item.isNewAddItem;
                    //         break;
                    // }
                    ALUGUICommon.setGameObjEnable(wnd.goRedTipList[i].redTipGo, isSet);
                }
            }
        }

        /// <summary>
        /// 点击处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClick(GameObject _go)
        {
            //设置选中对象
            if (null != _m_selectDelegate)
                _m_selectDelegate(itemIdx);
        }
    }
}
