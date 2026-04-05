using System;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;



namespace GOE
{
    //第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
    public class NPItemConvertRefExportMenu : NPBasicExportMenuItem<ItemConvertRefObj, ItemConvertRefObj, GSOItemConvertRefSet>
    {

        public NPItemConvertRefExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
            : base("item_convert", ENPExportSettingEnum.ITEM_CONVERT, GSOItemConvertRefSet.assetPath, GSOItemConvertRefSet.objName, _tag, _judgeCanShowFunc)
        {
        }

        /****************
	     * 显示的菜单文字
	     **/
        protected override string _menuText { get { return "item_convert 物品兑换表"; } }

        public override List<ItemConvertRefObj> _exchangeTemplate(List<ItemConvertRefObj> _tempList)
        {
            _checkItemConvert(_tempList);
            return _tempList;
        }

        public override void _readRefInfo()
        {
            obj.bag_item_id = GetLong("bag_item_id");
            obj.ori_item_num = GetLong("ori_item_num");
            obj.cost_item_list = NPCommonCostItem.readList(GetString("cost_item_list"));
            obj.target_item = NPCommonItem.readFromStr(GetString("target_item"));
            obj.sort_id = GetInt("sort_id");
            obj.convert_condition = _NPPlayerConditionSerializeInfo.ReadFromString(GetString("convert_condition"), "convert_condition");
            obj.convert_condition_desc = GetString("convert_condition_desc");
            obj.convert_condition_desc_args = GetList<string>("convert_condition_desc_args");
        }

        private void _checkItemConvert(List<ItemConvertRefObj> _list)
        {
            ItemConvertRefObj temp = null;
            List<long> resIdList = new List<long>();
            for (int i = 0; i < _list.Count; i++)
            {
                resIdList.Clear();
                temp = _list[i];
                if (null == temp)
                    continue;

                if (!_check(temp, resIdList, _list))
                    break;
            }
        }
        private bool _check(ItemConvertRefObj _refObj, List<long> _resIdList, List<ItemConvertRefObj> _list)
        {
            _resIdList.Add(_refObj.bag_item_id);

            ItemConvertRefObj temp = null;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;

                if (temp.bag_item_id == _refObj.target_item.itemId)
                {
                    if (_resIdList.Contains(temp.target_item.itemId))
                    {
                        ALLog.Error($"!!! item_convert 数据填写错误 会导致循环合成_refObj.id = {temp.bag_item_id}");
                        return false;
                    }
                    return _check(temp, _resIdList, _list);
                }
            }
            return true;
        }
    }

}