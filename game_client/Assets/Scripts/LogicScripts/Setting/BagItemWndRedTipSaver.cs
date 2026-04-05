using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GOE
{
    [System.Serializable]
    public class BagItemRedTipSaverData
    {
        public List<BagItemRedTipTypeListData> dataList;//红点数据列表
    }
    [System.Serializable]
    public class BagItemRedTipTypeListData
    {
        public long itemId;//物品ID
        public List<BagItemRedTipTypeData> dataList;//红点数据
    }
    [System.Serializable]
    public class BagItemRedTipTypeData
    {
        public EBagItemRedTipType type;//红点类型
        public bool canShow;//是否可显示
    }


    /// <summary>
    /// 背包窗口红点相关本地保存
    /// </summary>
    public class BagItemWndRedTipSaver : _AALBasicSettingInfo
    {
        [NotNull] private BagItemRedTipSaverData _m_redTipData = new BagItemRedTipSaverData();

        public BagItemWndRedTipSaver(long _accountCID) : base($"{_accountCID}_cache_account_bag_red_tip_saver")
        {
        }
        
        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            return JsonUtility.ToJson(_m_redTipData);
        }

        /**************
        * 读取保存的字符串
        **/
        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;

            _m_redTipData = JsonUtility.FromJson<BagItemRedTipSaverData>(_infoStr);

        }

        /// <summary>
        /// 根据物品id获取红点类型列表数据
        /// </summary>
        /// <param name="_itemId"></param>
        /// <returns></returns>
        public BagItemRedTipTypeListData getRedTipTypeListDataByItemId(long _itemId)
        {
            if(_m_redTipData.dataList == null)
                return null;

            foreach(var data in _m_redTipData.dataList)
            {
                if(data.itemId == _itemId)
                    return data;
            }
            return null;
        }

        /// <summary>
        /// 新增物品红点记录
        /// </summary>
        /// <param name="_item"></param>
        public void recordItem(BagItem _item)
        {
            if (_item == null)
                return;

            BagItemRedTipTypeListData typeListData = getRedTipTypeListDataByItemId(_item.itemId);
            if (typeListData != null)
            {
                if (typeListData.dataList == null)
                    typeListData.dataList = new List<BagItemRedTipTypeData>();

                //更新或添加红点类型数据
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.NEW, _item.isNew);
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.ADD, _item.isNewAddItem);
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.COMBINE, GCommon.isItemCanCombine(_item.itemId, 1));
                //对应物品是否可被合成
                _recordBeCombine(_item);
            }
            else
            {
                typeListData = new BagItemRedTipTypeListData();
                typeListData.itemId = _item.itemId;
                //更新或添加红点类型数据
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.NEW, _item.isNew);
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.ADD, _item.isNewAddItem);
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.COMBINE, GCommon.isItemCanCombine(_item.itemId, 1));
                //对应物品是否可被合成
                _recordBeCombine(_item);

                //记录红点数据
                if (_m_redTipData.dataList == null)
                    _m_redTipData.dataList = new List<BagItemRedTipTypeListData>();
                _m_redTipData.dataList.Add(typeListData);
            }
            saveSetting();
        }

        /// <summary>
        /// 记录可被合成红点
        /// </summary>
        /// <param name="_item">原材料</param>
        private void _recordBeCombine(BagItem _item)
        {
            if (_item == null)
                return;

            ItemConvertRefObj itemConvertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_item.itemId);
            if (itemConvertRef == null)
                return;

            NPCommonItem targetItem = itemConvertRef.target_item;
            if (targetItem == null || targetItem.itemType != ENPItemType.BAG_ITEM)
                return;

            BagItemRedTipTypeListData typeListData = getRedTipTypeListDataByItemId(targetItem.itemId);
            if (typeListData != null)
            {
                if (typeListData.dataList == null)
                    typeListData.dataList = new List<BagItemRedTipTypeData>();

                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.BE_COMBINE, (_item.isNew || _item.isNewAddItem) && GCommon.isItemCanBeCombine(ENPItemType.BAG_ITEM, targetItem.itemId));
            }
            else
            {
                typeListData = new BagItemRedTipTypeListData();
                typeListData.itemId = targetItem.itemId;
                typeListData.dataList = new List<BagItemRedTipTypeData>();
                _updateOrAddRedTipType(typeListData, EBagItemRedTipType.BE_COMBINE, (_item.isNew || _item.isNewAddItem) && GCommon.isItemCanBeCombine(ENPItemType.BAG_ITEM, targetItem.itemId));

                //记录红点数据
                if (_m_redTipData.dataList == null)
                    _m_redTipData.dataList = new List<BagItemRedTipTypeListData>();
                _m_redTipData.dataList.Add(typeListData);
            }
        }

        /// <summary>
        /// 更新或添加红点类型数据
        /// </summary>
        private void _updateOrAddRedTipType(BagItemRedTipTypeListData _typeListData, EBagItemRedTipType _type, bool _canShow)
        {
            if (_typeListData == null)
                _typeListData = new BagItemRedTipTypeListData();
            if (_typeListData.dataList == null)
                _typeListData.dataList = new List<BagItemRedTipTypeData>();

            BagItemRedTipTypeData existingData = null;
            foreach (BagItemRedTipTypeData data in _typeListData.dataList)
            {
                if (data.type == _type)
                {
                    existingData = data;
                    break;
                }
            }

            if (existingData != null)
                existingData.canShow = existingData.canShow || _canShow;
            else
                _typeListData.dataList.Add(new BagItemRedTipTypeData { type = _type, canShow = _canShow });
        }

        /// <summary>
        /// 获取红点记录字典
        /// </summary>
        /// <param name="_itemId"></param>
        /// <returns></returns>
        public bool  getCanShowRedTip(long _itemId, EBagItemRedTipType _type)
        {
            if (_m_redTipData.dataList == null)
                return false;

            for (int i = 0; i < _m_redTipData.dataList.Count; i++)
            {
                if (_m_redTipData.dataList[i].itemId == _itemId)
                {
                    for (int j = 0; j < _m_redTipData.dataList[i].dataList.Count; j++)
                    {
                        if (_m_redTipData.dataList[i].dataList[j].type == _type)
                        {
                            return _m_redTipData.dataList[i].dataList[j].canShow;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 设置对应红点类型为已读
        /// </summary>
        /// <param name="_itemId"></param>
        /// <param name="_types"></param>
        public void setReadRedTipByType(long _itemId, params EBagItemRedTipType[] _types)
        {
            if (_types == null || _types.Length == 0)
                return;

            if (_m_redTipData.dataList == null)
                return;

            for (int i = 0; i < _m_redTipData.dataList.Count; i++)
            {
                if (_m_redTipData.dataList[i].itemId == _itemId)
                {
                    for (int j = 0; j < _m_redTipData.dataList[i].dataList.Count; j++)
                    {
                        if (_types.Contains(_m_redTipData.dataList[i].dataList[j].type))
                        {
                            _m_redTipData.dataList[i].dataList[j].canShow = false;
                        }
                    }
                }
            }
            saveSetting();
        }
    }
}