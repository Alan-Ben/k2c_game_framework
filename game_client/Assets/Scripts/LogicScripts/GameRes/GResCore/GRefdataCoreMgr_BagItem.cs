using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        [NotNull] private Dictionary<long, List<BagItemRefObj>> _m_dDefectItemDic = new Dictionary<long, List<BagItemRefObj>>();//获取缺少类型的物品列表字典
        [NotNull] private Dictionary<NPCommonItem, ItemConvertRefObj> _m_dConvertItemDic = new Dictionary<NPCommonItem, ItemConvertRefObj>();//可合成物品列表字典<目标道具，配置数据>

        private void _initBagItem()
        {
            // 初始化获取缺少类型的物品列表字典
            _m_dDefectItemDic.Clear();
            bagItemCore.dealAllRef((_bagItemRef) =>
            {
                if(_bagItemRef == null || _bagItemRef.defect_common_item == null || _bagItemRef.defect_common_item.Count <= 0)
                    return;

                foreach (var defectItem in _bagItemRef.defect_common_item)
                {
                    if(defectItem == null || !defectItem.IsValid)
                        continue;

                    long commonItemId = defectItem.getId();
                    List<BagItemRefObj> list = null;
                    if (!_m_dDefectItemDic.TryGetValue(commonItemId, out list) || list == null)
                    {
                        list = new List<BagItemRefObj>();
                        _m_dDefectItemDic[commonItemId] = list;
                    }
                    
                    list.Add(_bagItemRef);
                }
            });

            //初始化可合成物品列表字典
            itemConvertCore.dealAllRef(_convertRef =>
            {
                if (_convertRef != null && _convertRef.target_item != null)
                    _m_dConvertItemDic[_convertRef.target_item] = _convertRef;
            });
        }

        /// <summary>
        /// 可获取缺少类型的物品列表
        /// </summary>
        /// <returns></returns>
        public List<BagItemRefObj> getCommonItemDefectBagItemRefList(NPCommonItem _commonItem)
        {
            if(_commonItem == null)
                return null;
            
            return _getCommonItemDefectBagItemRefList(_commonItem.getId());
        }

        public List<BagItemRefObj> getCommonItemDefectBagItemRefList(ENPItemType _itemType, long _subId)
        {
            return _getCommonItemDefectBagItemRefList(NPCommonItem.getId(_itemType, _subId));
        }

        private List<BagItemRefObj> _getCommonItemDefectBagItemRefList(long _commonItemId)
        {
            if(_m_dDefectItemDic.TryGetValue(_commonItemId, out List<BagItemRefObj> list))
            {
                return list;
            }

            return null;
        }

        /// <summary>
        /// 根据目标道具获取可合成物品配置
        /// </summary>
        /// <param name="_targetItem"></param>
        /// <returns></returns>
        public ItemConvertRefObj getItemConvertRefByTargetItem(NPCommonItem _targetItem)
        {
            if (_targetItem == null)
                return null;

            if (_m_dConvertItemDic.TryGetValue(_targetItem, out ItemConvertRefObj itemConvertRef))
            {
                return itemConvertRef;
            }
            return null;
        }

        /// <summary>
        /// 根据提升目标类型获取物品使用配置列表
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public void getBagItemUseRefByImproveTargetType(EImproveTargetType _type, List<BagItemUseRefObj> _list)
        {
            if (_list == null)
                return;

            _list.Clear();

            bagItemUseCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.improve_target_type_list != null && _ref.improve_target_type_list.Contains(_type))
                    _list.Add(_ref);
            });
        }

    }
}