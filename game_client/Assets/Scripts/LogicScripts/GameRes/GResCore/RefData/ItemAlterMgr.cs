using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /**************
     * 替代物品管理对象
     **/
    public class ItemAlterMgr
    {
        //替换的映射关系表
        private Dictionary<long, List<ItemAlterRefObj>>[] _m_dAlterDic;

        public ItemAlterMgr()
        {
            int enumCount = ALCommon.getEnumCount(typeof(NPEnum.ENPItemType));
            _m_dAlterDic = new Dictionary<long, List<ItemAlterRefObj>>[enumCount];
        }

        //添加映射关系
        public void addAlter(ItemAlterRefObj _refObj)
        {
            if(null == _refObj)
                return;

            //获取对应映射表
            Dictionary<long, List<ItemAlterRefObj>> dicObj = _m_dAlterDic[(int)_refObj.src_item_type];
            if(null == dicObj)
            {
                dicObj = new Dictionary<long, List<ItemAlterRefObj>>();
                _m_dAlterDic[(int)_refObj.src_item_type] = dicObj;
            }

            //查询对应映射表
            List<ItemAlterRefObj> list = null;
            if(!dicObj.TryGetValue(_refObj.src_item_id, out list))
            {
                //获取失败则创建
                list = new List<ItemAlterRefObj>();
                dicObj.Add(_refObj.src_item_id, list);
            }

            //加入队列
            list.Add(_refObj);
        }

        /** 获取可替换队列 */
        public List<ItemAlterRefObj> getAlterList(NPEnum.ENPItemType _itemType, long _id)
        {
            //获取对应映射表
            Dictionary<long, List<ItemAlterRefObj>> dicObj = _m_dAlterDic[(int)_itemType];
            if(null == dicObj)
                return null;

            //查询对应映射表
            List<ItemAlterRefObj> list = null;
            if(!dicObj.TryGetValue(_id, out list))
            {
                return null;
            }

            //返回队列
            return list;
        }
    }
}
