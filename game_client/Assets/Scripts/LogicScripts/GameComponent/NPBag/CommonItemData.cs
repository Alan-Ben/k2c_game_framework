using UnityEngine;
using System;
using NPCommon;
using NPEnum;
using Object = System.Object;

namespace GOE
{
    public interface _IItem
    {
        long subId { get; }
        string getItemName();
        string getItemDesc();

        long getCount();
        string getShowNum();
        EQuality getQuality();

        NPGTextureIndex getIcon();
        NPGSpriteIndex getQualityIcon();

        NPEnum.ENPItemType getItemType();

        CommonEnum.ECurrency getCurrencyType();
    }

    //物品数据结构体
    public class CommonItemData : _IItem
    {
        //属性
        public long subId { get { return _m_lSubId; } }

        public string getShowNum() { return null == _m_sShowNum ? _m_iCount.ToString() : TextTranslate.instance.getLanguage(_m_sShowNum, _m_iCount.ToString()); }

        public long getCount() { return _m_iCount; }

        public long getSrcCount() { return _m_iSrcCount; }

        public string getItemName() { return GCommon.getItemName(getItemType(), _m_lSubId); }

        public string getItemDesc() { return GCommon.getItemDesc(getItemType(), _m_lSubId); }

        public EQuality getQuality() { return GCommon.getItemQuality(getItemType(), _m_lSubId); }
        
        public NPGTextureIndex getIcon() { return GCommon.getItemTexIcon(getItemType(), _m_lSubId); }

        public NPGSpriteIndex getQualityIcon() { return GCommon.getItemQualityIcon(getItemType(), _m_lSubId); }

        public NPEnum.ENPItemType getItemType() { return _m_eItemType; }

        public NPEnum.ENPBagItemType bagItemType { get { return _m_eBagItemType; } }

        public CommonEnum.ECurrency getCurrencyType()
        {
            if(_m_eItemType == NPEnum.ENPItemType.CURRENCY)
                return (CommonEnum.ECurrency)_m_lSubId;
            return CommonEnum.ECurrency.NONE;
        }


        //成员变量
        private NPEnum.ENPItemType _m_eItemType;//物品类型
        private long _m_lSubId;//子ID
        private string _m_sShowNum;//显示数量
        private long _m_iCount;//真实数量
        private long _m_iSrcCount;//原本数量
        private NPEnum.ENPBagItemType _m_eBagItemType;//背包物品子类型

        //构造函数
        public CommonItemData()
        {
            _m_eItemType = NPEnum.ENPItemType.NONE;
            _m_lSubId = 0;
            _m_sShowNum = null;
            _m_iCount = 0;
            _m_iSrcCount = 0;
            _m_eBagItemType = NPEnum.ENPBagItemType.NONE;
        }

        public CommonItemData(NPCommon_ItemInfo _commonItemInfo, string _transKey = null)
        {
            if (_commonItemInfo == null)
                return;

            setData((ENPItemType)_commonItemInfo.getItemType(), _commonItemInfo.getSubId(), _commonItemInfo.getCount(), _commonItemInfo.getCount(), _transKey);
        }
        
        public CommonItemData(NPEnum.ENPItemType _type, long _subId, long _count, string _transKey = null)
        {
            setData(_type, _subId, _count, _count, _transKey);
        }

        public CommonItemData(NPEnum.ENPItemType _type, long _subId, long _count, long _srcCount, string _transKey = null)
        {
            setData(_type, _subId, _count, _srcCount, _transKey);
        }

        public CommonItemData(NPCommonItem _cItem, long _count, string _transKey = null)
        {
            setData(_cItem.itemType, _cItem.itemId, _count, _count, _transKey);
        }
        public CommonItemData(NPCommonCostItem _cItem, string _transKey = null)
        {
            setData(_cItem.item.itemType, _cItem.item.itemId, _cItem.count, _cItem.count, _cItem.count.ToString());
        }
        public CommonItemData(BagItem _bItem)
        {
            if (null == _bItem)
                return;
            _m_eBagItemType = _bItem.itemRefObj.bag_item_type;
            setData(NPEnum.ENPItemType.BAG_ITEM, _bItem.itemId, _bItem.count, _bItem.count, _bItem.count.ToString());
        }
        //设置数据
        public void setData(NPEnum.ENPItemType _type, long _subId, long _count, long _srcCount, string _transKey = null)
        {
            //设置一些成员变量
            _m_eItemType = _type;
            _m_lSubId = _subId;
            if(null != _transKey)
                _m_sShowNum = _transKey;
            else
                _m_sShowNum = null;
            _m_iCount = _count;
            _m_iSrcCount = _srcCount;

            if(_m_eItemType == NPEnum.ENPItemType.BAG_ITEM)
            {
                //如果设置了就不需要查询了
                if (_m_eBagItemType > 0)
                    return;
                BagItemRefObj itemRef = GRefdataCoreMgr.instance.bagItemCore.getRef(_m_lSubId);
                if(itemRef != null)
                {
                    _m_eBagItemType = itemRef.bag_item_type;
                }
                else
                {
                    _m_eBagItemType = NPEnum.ENPBagItemType.NONE;
                }
            }
            else
            {
                _m_eBagItemType = NPEnum.ENPBagItemType.NONE;
            }
        }



        //------------------------------其他方法----------------------------------
        public void setCount(long _count)
        {
            if(_m_iCount == _count)
                return;

            _m_iCount = _count;
            _m_sShowNum = _count.ToString();
        }
        public void setSrcCount(long _srcCount)
        {
            if(_m_iSrcCount == _srcCount)
                return;
            _m_iSrcCount = _srcCount;
        }

        //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
        public static bool operator ==(CommonItemData a, CommonItemData b)
        {
            // If both are null, or both are same instance, return true.
            if(Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if(((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            if(a._m_eItemType != b._m_eItemType)
                return false;
            if(a._m_lSubId != b._m_lSubId)
                return false;
            if(a._m_iCount != b._m_iCount)
                return false;

            return true;
        }

        public static bool operator !=(CommonItemData a, CommonItemData b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if(obj is CommonItemData)
            {
                CommonItemData item = obj as CommonItemData;
                // Return true if the fields match:
                if(item._m_eItemType != _m_eItemType)
                    return false;
                if(item._m_lSubId != _m_lSubId)
                    return false;
                if(item._m_iCount != _m_iCount)
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _m_eItemType.GetHashCode() ^ _m_lSubId.GetHashCode() ^ _m_iCount.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("物品类型: {0}, 子id： {1}, 数量： {2}", _m_eItemType, _m_lSubId, _m_iCount);
        }
    }
}
