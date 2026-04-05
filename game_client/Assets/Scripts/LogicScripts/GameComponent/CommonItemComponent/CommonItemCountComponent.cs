using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 通用道具数量Component
    /// </summary>
    public class CommonItemCountComponent : _ANPBasicPlayerComponent
    {
        private Dictionary<NPCommonItem, long> _m_dCommonItemCountDic;//通用道具字典
        
        private bool _m_bHasCommonItemChgTask; // 是否有通用道具变化任务
        [NotNull] private ObjectPool<NPCommonCostItem> _m_commonItemPool = new ObjectPool<NPCommonCostItem>(() => new NPCommonCostItem(), null, null, null, false, 1); // 通用道具对象池
        [NotNull] private List<NPCommonCostItem> _m_lTickTaskChgItemList = new List<NPCommonCostItem>(); // tick任务中的变化道具列表
        
        public CommonItemCountComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COMMON_ITEM_COUNT; } }
        public override ENPPlayerCompType[] dependCompList { get; }
        public override void presendInitProtocol()
        {
        }

        protected override void _dealInit()
        {
            if (_m_dCommonItemCountDic == null)
                _m_dCommonItemCountDic = new Dictionary<NPCommonItem, long>();
            _m_dCommonItemCountDic.Clear();
            
            setInitDone();
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            
            _clear();
            _m_dCommonItemCountDic = null;

            _m_bHasCommonItemChgTask = false;
            _pushBackAllCommonItem();
            _m_commonItemPool.Clear();
        }

        private void _clear()
        {
            _m_dCommonItemCountDic?.Clear();
        }
        
        public long getCount(NPCommonItem _commonItem)
        {
            if (_commonItem == null || _m_dCommonItemCountDic == null)
                return 0;

            _m_dCommonItemCountDic.TryGetValue(_commonItem, out long _count);
            return _count;
        }
        
        public long getCount(ENPItemType _itemType, long _subId)
        {
            if (_m_dCommonItemCountDic == null)
                return 0;

            foreach (var kv in _m_dCommonItemCountDic)
            {
                NPCommonItem commonItem = kv.Key;
                if(commonItem != null && commonItem.itemType == _itemType && commonItem.itemId == _subId)
                    return kv.Value;
            }

            return 0;
        }
        
        /// <summary>
        /// 设置数量
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void setCount(NPCommonItem _commonItem, long _value)
        {
            if (_commonItem == null)
                return;

            if (_m_dCommonItemCountDic == null)
                _m_dCommonItemCountDic = new Dictionary<NPCommonItem, long>();

            _m_dCommonItemCountDic[_commonItem] = _value;
            
            sendCommonItemCountChgMsg(_commonItem.itemType, _commonItem.itemId, _value);
        }
        
        /// <summary>
        /// 设置数量
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void setCount(ENPItemType _itemType, long _subId, long _value)
        {
            if (_m_dCommonItemCountDic == null)
                _m_dCommonItemCountDic = new Dictionary<NPCommonItem, long>();

            NPCommonItem commonItem = null;
            long preValue = 0;
            foreach (var kv in _m_dCommonItemCountDic)
            {
                commonItem = kv.Key;
                if (commonItem != null && commonItem.itemType == _itemType && commonItem.itemId == _subId)
                {
                    preValue = _value;
                    break;
                }
            }
            
            // 若不存在，则创建新的
            if(commonItem == null)
                commonItem = new NPCommonItem() {itemType = _itemType, itemId = _subId};
            _m_dCommonItemCountDic[commonItem] = _value;
            
            sendCommonItemCountChgMsg(_itemType, _subId, _value);
        }

        public void addCount(NPCommonItem _commonItem, long _addCount)
        {
            if (_commonItem == null)
                return;

            if (_m_dCommonItemCountDic == null)
                _m_dCommonItemCountDic = new Dictionary<NPCommonItem, long>();

            if (_m_dCommonItemCountDic.TryGetValue(_commonItem, out long preCount))
            {
                _m_dCommonItemCountDic[_commonItem] = preCount + _addCount;
            }
            else
            {
                _m_dCommonItemCountDic[_commonItem] = _addCount;
            }
            
            sendCommonItemCountChgMsg(_commonItem.itemType, _commonItem.itemId, preCount + _addCount);
        }

        public void addCount(ENPItemType _itemType, long _subId, long _addCount)
        {
            if (_m_dCommonItemCountDic == null)
                _m_dCommonItemCountDic = new Dictionary<NPCommonItem, long>();

            NPCommonItem commonItem = null;
            long preCount = 0;
            foreach (var kv in _m_dCommonItemCountDic)
            {
                commonItem = kv.Key;
                if (commonItem != null && commonItem.itemType == _itemType && commonItem.itemId == _subId)
                {
                    preCount = kv.Value;
                    break;
                }
            }

            // 若不存在，则创建新的
            if(commonItem == null)
                commonItem = new NPCommonItem() {itemType = _itemType, itemId = _subId};
            _m_dCommonItemCountDic[commonItem] = preCount + _addCount;

            sendCommonItemCountChgMsg(_itemType, _subId, preCount + _addCount);
        }

        public void sendCommonItemCountChgMsg(ENPItemType _itemType, long _subId, long _count)
        {
            _addToTickTaskChgItemList(_itemType, _subId, _count);
            WinMsg.SendMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _itemType, _subId, _count);
        }

        public void sendCommonItemCountChgMsg(NPCommonItem _commonItem, long _count)
        {
            if(_commonItem == null)
                return;
            
            sendCommonItemCountChgMsg(_commonItem.itemType, _commonItem.itemId, _count);
        }

        #region 背包相关数据变化

        /// <summary>
        /// 背包物品数量变化
        /// </summary>
        private void _onBagItemChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem bagItem))
                return;

            sendCommonItemCountChgMsg(ENPItemType.BAG_ITEM, bagItem.itemId, bagItem.count);
        }

        #endregion

        #region 道具变化任务

        /// <summary>
        /// 创建道具变化任务
        /// </summary>
        private void _createCommonItemChgTask()
        {
            if (_m_bHasCommonItemChgTask)
                return;

            _m_bHasCommonItemChgTask = true;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (!_m_bHasCommonItemChgTask)
                    return;

                _m_bHasCommonItemChgTask = false;

                _dealCommonItemChgTask();
            }, 1f); // 延迟一会处理道具变化任务, 防止频繁触发
        }

        /// <summary>
        /// 处理道具变化任务
        /// </summary>
        private void _dealCommonItemChgTask()
        {
            // 发送累计变化消息
            if (_m_lTickTaskChgItemList.Count > 0)
            {
                // 发送累计变化消息
                WinMsg.SendMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG_TICK_TOTAL, _m_lTickTaskChgItemList);

                // 回收对象
                _pushBackAllCommonItem();
            }
        }

        /// <summary>
        /// 添加变化的道具到列表
        /// </summary>
        private void _addToTickTaskChgItemList(ENPItemType _itemType, long _subId, long _count)
        {
            NPCommonCostItem chgItem = _m_commonItemPool.Get();
            if (chgItem == null)
                return;

            if (chgItem.item == null)
                chgItem.item = new NPCommonItem();
            
            chgItem.item.itemType = _itemType;
            chgItem.item.itemId = _subId;
            chgItem.count = _count;
            
            _m_lTickTaskChgItemList.Add(chgItem);

            _createCommonItemChgTask();
        }

        /// <summary>
        /// 放回所有通用道具对象
        /// </summary>
        private void _pushBackAllCommonItem()
        {
            foreach (var item in _m_lTickTaskChgItemList)
            {
                if (item != null)
                    _m_commonItemPool.Release(item);
            }
            _m_lTickTaskChgItemList.Clear();
        }

        #endregion
    }
}