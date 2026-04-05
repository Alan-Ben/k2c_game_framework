using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using CommonEnum;
using NPEnum;
using NPCommon;
using GOE;

// NPCommonCostItem的拓展方法
public static class NPCommonCostItemExternsion
{
#if NP_GAME
    
    public static CommonItemData toCommonItemData(this NPCommonItem _item)
    {
        return new CommonItemData(_item,0);
    }
    public static List<CommonItemData> toCommonItemDataList(this List<NPCommonItem> _itemList)
    {
        if(null == _itemList)
            return null;

        List<CommonItemData> result = new List<CommonItemData>();
        NPCommonItem forUnit = null;
        for (int i = 0; i < _itemList.Count; i++)
        {
            forUnit = _itemList[i];
            result.Add(forUnit.toCommonItemData());
        }
        return result;
    }
    
    public static CommonItemData toCommonItemData(this NPCommonCostItem _costItem)
    {
        return new CommonItemData(_costItem.item.itemType, _costItem.item.itemId, _costItem.count);
    }

    public static CommonItemData toCommonItemData(this NPCommon_ItemInfo _costItem)
    {
        return new CommonItemData((ENPItemType)_costItem.getItemType(), _costItem.getSubId(), _costItem.getCount());
    }

    public static List<CommonItemData> toCommonItemDataList(this List<NPCommonCostItem> _costItemList)
    {
        if(null == _costItemList)
            return null;

        List<CommonItemData> result = new List<CommonItemData>();
        NPCommonCostItem forUnit = null;
        for (int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            result.Add(forUnit.toCommonItemData());
        }
        return result;
    }

    public static List<NPCommon_ItemInfo> toRewardItemDataList(this List<NPCommonCostItem> _costItemList)
    {
        if(null == _costItemList)
            return null;

        List<NPCommon_ItemInfo> result = new List<NPCommon_ItemInfo>();
        NPCommonCostItem forUnit = null;
        for (int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            result.Add(forUnit.toCommon_ItemInfo());
        }
        return result;
    }

    public static List<_IItem> toItemDataList(this List<NPCommon_ItemInfo> _costItemList)
    {
        List<_IItem> result = new List<_IItem>();
        NPCommon_ItemInfo forUnit = null;
        for(int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            result.Add(forUnit.toCommonItemData());
        }
        return result;
    }

    public static List<_IItem> toItemDataList(this List<NPCommonCostItem> _costItemList)
    {
        List<_IItem> result = new List<_IItem>();
        NPCommonCostItem forUnit = null;
        for(int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            result.Add(forUnit.toCommonItemData());
        }
        return result;
    }
    
    public static NPCommon_ItemInfo toCommon_ItemInfo(this _IItem _item)
    {
        return new NPCommon_ItemInfo((int)_item.getItemType(), _item.subId, _item.getCount(), null);
    }
    public static List<NPCommon_ItemInfo> toCommon_ItemInfoList(this List<_IItem> _costItemList)
    {
        List<NPCommon_ItemInfo> result = new List<NPCommon_ItemInfo>();
        _IItem forUnit = null;
        for(int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            result.Add(forUnit.toCommon_ItemInfo());
        }
        return result;
    }
    
    /// <summary>
    /// 转成合并后的item列表
    /// </summary>
    /// <param name="_costItemList"></param>
    /// <returns></returns>
    public static List<_IItem> toMergeItemDataList(this List<NPCommonCostItem> _costItemList, bool _newItem)
    {
        List<NPCommonCostItem> result = new List<NPCommonCostItem>();
        NPCommonCostItem forUnit = null;
        NPCommonCostItem resultUnit = null;
        for(int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            resultUnit = result.Find((b) => { return b.item.itemType == forUnit.item.itemType && b.item.itemId == forUnit.item.itemId; });
            if (null == resultUnit)
            {
                if (_newItem)
                {
                    result.Add(new NPCommonCostItem(forUnit));
                }
                else
                {
                    result.Add(forUnit);
                }
            }
            else
            {
                resultUnit.count += forUnit.count;
            }
        }
        return result.toItemDataList();
    }
    
    /// <summary>
    /// 获取对应的大数显示类型
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public static PrimitiveExtension.ELargeStringType getLargeStringType(this _IItem _item)
    {
        if (_item == null)
            return PrimitiveExtension.ELargeStringType.DEFAULT;

        if (_item.getCurrencyType() == ECurrency.SILVER)
            return PrimitiveExtension.ELargeStringType.GOLD;
        
        return PrimitiveExtension.ELargeStringType.DEFAULT;
    }
    
    public static PrimitiveExtension.ELargeStringType getLargeStringType(this NPCommon_ItemInfo _item)
    {
        if (_item == null)
            return PrimitiveExtension.ELargeStringType.DEFAULT;

        if (_item.getItemType() == (int)ENPItemType.CURRENCY && _item.getSubId() == (long)ECurrency.SILVER)
            return PrimitiveExtension.ELargeStringType.GOLD;
        
        return PrimitiveExtension.ELargeStringType.DEFAULT;
    }
    
    public static PrimitiveExtension.ELargeStringType getLargeStringType(this NPCommonItem _item)
    {
        if (_item == null)
            return PrimitiveExtension.ELargeStringType.DEFAULT;

        if (_item.itemType == ENPItemType.CURRENCY && _item.itemId == (long)ECurrency.SILVER)
            return PrimitiveExtension.ELargeStringType.GOLD;
        
        return PrimitiveExtension.ELargeStringType.DEFAULT;
    }

    public static PrimitiveExtension.ELargeStringType getLargeStringType(this ENPItemType _itemType, long _subId)
    {
        if (_itemType == ENPItemType.CURRENCY && _subId == (long)ECurrency.SILVER)
            return PrimitiveExtension.ELargeStringType.GOLD;
        
        return PrimitiveExtension.ELargeStringType.DEFAULT;
    }
    
#endif

    // 返回第一个匹配的costItem
    public static NPCommonCostItem getItem(this List<NPCommonCostItem> _costItemList, NPEnum.ENPItemType _itemType, long _itemId)
    {
        NPCommonCostItem forUnit = null;
        for (int i = 0; i < _costItemList.Count; i++)
        {
            forUnit = _costItemList[i];
            if (forUnit.item.itemType == _itemType && forUnit.item.itemId == _itemId)
                return forUnit;
        }

        return null;
    }
    
    /// <summary>
    /// 汇总计算，合并同类型
    /// </summary>
    /// <param name="_costItemList"></param>
    /// <returns></returns>
    public static List<NPCommonCostItem> collectCalc(this List<NPCommonCostItem> _costItemList)
    {
        if(null == _costItemList)
            return null;

        Dictionary<NPCommonItem, NPCommonCostItem> result = new Dictionary<NPCommonItem, NPCommonCostItem>();
        
        foreach (NPCommonCostItem costItem in _costItemList)
        {
            if(null == costItem)
                continue;
            
            if (result.ContainsKey(costItem.item))
            {
                result[costItem.item].count += costItem.count;
            }
            else
            {
                result.Add(costItem.item, new NPCommonCostItem(costItem.item, costItem.count));
            }
        }
        
        return result.Values.ToList();
    }

    /// <summary>
    /// 汇总计算，合并同类型
    /// </summary>
    /// <param name="_costItemList"></param>
    /// <returns></returns>
    public static List<NPCommon_ItemInfo> collectCalc(this List<NPCommon_ItemInfo> _itemList)
    {
        if (null == _itemList)
            return null;

        List<NPCommon_ItemInfo> flagList = new List<NPCommon_ItemInfo>();
        bool isHas = false;
        NPCommon_ItemInfo temp = null;
        for (int i = 0; i < _itemList.Count; i++)
        {
            temp = _itemList[i];
            if (null == temp)
                continue;

            isHas = false;
            NPCommon_ItemInfo tempJ = null;
            for (int j = 0; j < flagList.Count; j++)
            {
                tempJ = flagList[j];
                if (tempJ.getItemType() == temp.getItemType() && tempJ.getSubId() == temp.getSubId())
                {
                    tempJ.setCount(temp.getCount() + tempJ.getCount());
                    isHas = true;
                    break;
                }
            }
            if (!isHas)
                flagList.Add(temp);
        }
        return flagList;
    }

}

/********************
 * 信息结构体
 **/
[System.Serializable]
    public class NPCommonCostItem
#if NP_GAME
        : _IItem, _IConditionDescShow
#endif
    {
        public NPCommonItem item = new NPCommonItem();
        public long count;

        
        public NPCommonCostItem()
        {
        }
        
        public NPCommonCostItem(ENPItemType _type,long _itemId,long _count)
        {
            item = new NPCommonItem(_type,_itemId);
            count = _count;
        }
        
        public NPCommonCostItem(NPCommonCostItem _item)
        {
            item = new NPCommonItem(_item.item);
            count = _item.count;
        }
        
        public NPCommonCostItem(NPCommonItem _item, long _count)
        {
            item = new NPCommonItem(_item);
            count = _count;
        }
        
        public NPCommonCostItem(NPCommon_ItemInfo _info)
        {
            if (_info == null)
                return;

            item = new NPCommonItem((ENPItemType)_info.getItemType(), _info.getSubId());
            count = _info.getCount();
        }

        public bool IsValid { get { return item?.IsValid ?? false; } }

        public void setCount(long _count)
        {
            count = _count;
        }
        
        
#if NP_GAME
        public long subId
        {
            get { return item.itemId; }
        }
        public NPGTextureIndex conditionIcon
        {
            get { return getIcon(); }
        }
        public string conditionDesc
        {
            get
            {
                long targetCount = getCount();
                long currentCount = GCommon.getItemCount(item);

                PrimitiveExtension.ELargeStringType largeStringType = PrimitiveExtension.ELargeStringType.DEFAULT;
                if (item != null && item.itemType == ENPItemType.CURRENCY && item.itemId == (long)ECurrency.SILVER)
                    largeStringType = PrimitiveExtension.ELargeStringType.GOLD;
                
                string current = currentCount.ToLargeString(largeStringType);
                if (currentCount < targetCount)
                    current = GCommon.addColorForRichText(current, GRefdataCoreMgr.instance.npGeneral.item_not_enough_color);

                return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, current, targetCount.ToLargeString(largeStringType));
            }
        }
        public bool conditionIsEnable
        {
            get { return GCommon.isItemEnough(this, false); }
        }
        public void jumpFunc() 
        {
            GCommon.dealItemNotEnough(item);
        }
        public long getCount()
        {
            return count;
        }

        public string getShowNum()
        {
            return count.ToString();
        }
        
        public string getItemName()
        {
            return GCommon.getItemName(getItemType(), subId);
        }

        public string getItemDesc()
        {
            return GCommon.getItemDesc(getItemType(), subId);
        }

        public EQuality getQuality()
        {
            return GCommon.getItemQuality(getItemType(), subId);
        }

        public NPGTextureIndex getIcon()
        {
            return GCommon.getItemTexIcon(getItemType(), subId); 
        }

        public NPGSpriteIndex getQualityIcon()
        {
            return GCommon.getItemQualityIcon(getItemType(), subId);
        }

        public ENPItemType getItemType()
        {
            return item.itemType;
        }

        public CommonEnum.ECurrency getCurrencyType()
        {
            if (item.itemType == NPEnum.ENPItemType.CURRENCY)
                return (CommonEnum.ECurrency)subId;
            return CommonEnum.ECurrency.NONE;
        }
#endif   
        
        /************
        * 读取字符串
        **/
        public static NPCommonCostItem readFromStr(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { "-",":" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 3)
            {
                UnityEngine.Debug.LogError($"NPCommonCostItem 解析失败，字符串格式错误：{_str}，正确格式：itemType:itemId:count");
                return null;
            }

            NPCommonCostItem ret = new NPCommonCostItem();

            ret.item.itemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), strs[0], true);
            ret.item.itemId = long.Parse(strs[1]);
            ret.count = long.Parse(strs[2]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<NPCommonCostItem> switchList(List<NPCommon_ItemInfo> _switchList)
        {
            List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
            NPCommon_ItemInfo itemInfo = null;
            for (int i = 0; i < _switchList.Count; i++)
            {
                itemInfo = _switchList[i];
                if (null == itemInfo)
                    continue;

                itemList.Add(new NPCommonCostItem(itemInfo));
            }

        return itemList;
        }
        public static List<NPCommonCostItem> MakeListFromString(string _str)
        {
            return readList(_str);
        }
        public static List<NPCommonCostItem> readList(string _str)
        {
            List<NPCommonCostItem> list = new List<NPCommonCostItem>();
            if (null == _str || _str.Length <= 0)
                return list;

            //string[] strs = _str.Split('|');
            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                NPCommonCostItem newItem = NPCommonCostItem.readFromStr(strs[i]);
                if(null == newItem)
                    continue;

                list.Add(newItem);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", item.itemType, item.itemId, count);
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":"}, StringSplitOptions.RemoveEmptyEntries);
            if(strs.Length < 1)
            {
                UnityEngine.Debug.LogError($"NPCommonCostItem解析失败：{_str}");
                return;
            }

            this.item = NPCommonItem.readFromStr(strs[0]);
            try
            {
                //支持数量不配置默认为1
                if (strs.Length > 1)
                    this.count = long.Parse(strs[1]);
                else
                    this.count = 1;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"CommonItem【{_str}】的count解析错误:{e}");
                throw;
            }
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static NPCommonCostItem[] MakeArrayFromString(string _str)
        {
            return readList(_str).ToArray();
        }
    }