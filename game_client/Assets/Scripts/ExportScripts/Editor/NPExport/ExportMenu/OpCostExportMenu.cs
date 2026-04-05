using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 操作消耗表导出
    /// </summary>
    public class OpCostExportMenu : NPBasicExportMenuItem<TmpOpCostRefObj, OpCostGroupRefObj, GSOOpCostGroupRefSet>
    {
        public OpCostExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc) : base("op_cost", ENPExportSettingEnum.OP_COST, GSOOpCostGroupRefSet.assetPath, GSOOpCostGroupRefSet.objName, _tag, _judgeCanShowFunc)
        {
        }

        protected override string _menuText { get { return "操作消耗表（op_cost）"; } }
        
        public override void _readRefInfo()
        {
            obj.id = GetLong("id");
            obj.cost_group_id = GetLong("cost_group_id");
            obj.op_count = GetInt("op_count");
            obj.cost_item = NPCommonCostItem.readFromStr(GetString("cost_item"));
        }

        public override List<OpCostGroupRefObj> _exchangeTemplate(List<TmpOpCostRefObj> _tempList)
        {
            List<OpCostGroupRefObj> resList = new List<OpCostGroupRefObj>();
            if (_tempList == null || _tempList.Count <= 0)
                return resList;
            
            Dictionary<long, List<TmpOpCostRefObj>> groupOpCostDic = new Dictionary<long, List<TmpOpCostRefObj>>();//操作消耗表分组字典
            // 将所有消耗分组按照其分组放入对应字典的列表中
            foreach (TmpOpCostRefObj item in _tempList)
            {
                if(item == null)
                    continue;
                
                if(!groupOpCostDic.TryGetValue(item.cost_group_id, out List<TmpOpCostRefObj> groupOpCostList) || groupOpCostList == null)
                {
                    groupOpCostList = new List<TmpOpCostRefObj>();
                    groupOpCostDic.Add(item.cost_group_id, groupOpCostList);
                }
                
                groupOpCostList.Add(item);
            }

            // 遍历所有操作消耗分组
            foreach (var groupOpCostKV in groupOpCostDic)
            {
                OpCostGroupRefObj opCostGroupRefObj = new OpCostGroupRefObj();
                opCostGroupRefObj.cost_group_id = groupOpCostKV.Key;
                opCostGroupRefObj.opCostRefObjList = new List<OpCostRefObj>();
                resList.Add(opCostGroupRefObj);

                List<TmpOpCostRefObj> tmpOpCostRefObjList = groupOpCostKV.Value;
                if(tmpOpCostRefObjList == null || tmpOpCostRefObjList.Count <= 0)
                    continue;
                
                // 按照操作次数小到大排序
                tmpOpCostRefObjList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    // 操作次数从小到大排序
                    if (_a.op_count.CompareTo(_b.op_count) != 0)
                        return _a.op_count.CompareTo(_b.op_count);

                    // 若操作次数一样(配表不应该出现这种配置情况, 但代码还是按照id排一下序)
                    return _a.id.CompareTo(_b.id);
                });

                // 将同组的tmpOpCostRefObj填入对应分组opCostGroupRefObj的add_list中
                OpCostRefObj opCostRefObj = null;
                foreach (TmpOpCostRefObj tmpOpCostRefObj in tmpOpCostRefObjList)
                {
                    if(tmpOpCostRefObj == null)
                        continue;

                    opCostRefObj = new OpCostRefObj();
                    opCostRefObj.id = tmpOpCostRefObj.id;
                    opCostRefObj.op_count = tmpOpCostRefObj.op_count;
                    opCostRefObj.cost_item = tmpOpCostRefObj.cost_item;
                    
                    opCostGroupRefObj.opCostRefObjList.Add(opCostRefObj);
                }
            }

            return resList;
        }
    }
}