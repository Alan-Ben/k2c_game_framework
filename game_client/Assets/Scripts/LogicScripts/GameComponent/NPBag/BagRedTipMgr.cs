using ALPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOE
{
    // 背包小红点管理类
    public class BagRedTipMgr : WCGSingleton<BagRedTipMgr>
    {
        // 页签小红点数字
        private int[] _m_aTabRedTipsNum = new int[ALCommon.getEnumCount(typeof(NPEnum.ENPBagItemType))];
        // 背包小红点数字
        private int _m_iBagRedTipsNum = 0;

        // 响应新增物品
        public void onAddItem(BagItem _item)
        {
            if(_item == null)
                return;

            _m_iBagRedTipsNum++;


            _m_aTabRedTipsNum[(int)_item.itemRefObj.bag_item_type]++;
        }

        // 响应移除新物品
        public void onRemoveItem(BagItem _item)
        {
            if(_item == null || _item.itemRefObj == null)
                return;

            if(_m_iBagRedTipsNum > 0)
                _m_iBagRedTipsNum--;

            if(getTabRedTipNum(_item.itemRefObj.bag_item_type) > 0)
                _m_aTabRedTipsNum[(int)_item.itemRefObj.bag_item_type]--;
        }

        // 物品访问后，小红点减1
        public void onItemVisited(BagItem _item)
        {
            if(null == _item || _item.itemRefObj == null)
                return;

            if(getTabRedTipNum(_item.itemRefObj.bag_item_type) > 0)
            {
                _m_aTabRedTipsNum[(int)_item.itemRefObj.bag_item_type]--;
            }
        }

        // 获取页签红点
        public int getTabRedTipNum(NPEnum.ENPBagItemType _type)
        {
            return _m_aTabRedTipsNum[(int)_type];
        }

        //获取多个页签红点总和
        public int getTabRedTipNum(List<NPEnum.ENPBagItemType> _typeList)
        {
            if (null == _typeList)
                return 0;

            int redAllNum = 0;
            for (int i = 0; i < _typeList.Count; i++)
            {
                redAllNum += _m_aTabRedTipsNum[(int)_typeList[i]];
            }
            return redAllNum;
        }

        // 获取背包红点
        public int getBagRedTipNum()
        {
            return _m_iBagRedTipsNum;
        }

        // 清除页签红点
        public void clearTabRedTipNum(NPEnum.ENPBagItemType _type)
        {
            int redTipCount = getTabRedTipNum(_type);
            _m_aTabRedTipsNum[(int)_type] = 0;

            _m_iBagRedTipsNum -= redTipCount;
            if(_m_iBagRedTipsNum < 0)
                _m_iBagRedTipsNum = 0;
        }


        // 清除多个页签红点
        public void clearTabRedTipNum(List<NPEnum.ENPBagItemType> _typeList)
        {
            if (null == _typeList)
                return;

            for (int i = 0; i < _typeList.Count; i++)
            {
                clearTabRedTipNum(_typeList[i]);
            }
        }

        public void clear()
        {
            // 页签小红点归零
            for(int i = 0, length = _m_aTabRedTipsNum.Length; i < length; i++)
            {
                _m_aTabRedTipsNum[i] = 0;
            }

            // 背包小红点归零
            _m_iBagRedTipsNum = 0;
        }
    }
}
