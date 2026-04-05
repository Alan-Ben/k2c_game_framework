using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 加成概率表导出
    /// </summary>
    public class ProAddExportMenu : NPBasicExportMenuItem<TmpProAddRefObj, ProAddGroupRefObj, GSOProAddGroupRefSet>
    {
        public ProAddExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc) : base("pro_add", ENPExportSettingEnum.PRO_ADD, GSOProAddGroupRefSet.assetPath, GSOProAddGroupRefSet.objName, _tag, _judgeCanShowFunc)
        {
        }

        protected override string _menuText { get { return "加成概率表（pro_add）"; } }
        
        public override void _readRefInfo()
        {
            obj.id = GetLong("id");
            obj.group_id = GetLong("group_id");
            obj.add = GetLong("add");
            obj.random_weight = GetLong("random_weight");
        }

        public override List<ProAddGroupRefObj> _exchangeTemplate(List<TmpProAddRefObj> _tempList)
        {
            List<ProAddGroupRefObj> resList = new List<ProAddGroupRefObj>();
            if (_tempList == null || _tempList.Count <= 0)
                return resList;
            
            Dictionary<long, List<TmpProAddRefObj>> groupProAddDic = new Dictionary<long, List<TmpProAddRefObj>>();//概率加成表分组字典
            // 将所有加成分组按照其分组放入对应字典的列表中
            foreach (TmpProAddRefObj item in _tempList)
            {
                if(item == null)
                    continue;
                
                if(!groupProAddDic.TryGetValue(item.group_id, out List<TmpProAddRefObj> groupProAddList) || groupProAddList == null)
                {
                    groupProAddList = new List<TmpProAddRefObj>();
                    groupProAddDic.Add(item.group_id, groupProAddList);
                }
                
                groupProAddList.Add(item);
            }

            // 遍历所有加成概率分组
            foreach (var groupProAddKV in groupProAddDic)
            {
                ProAddGroupRefObj proAddGroupRefObj = new ProAddGroupRefObj();
                proAddGroupRefObj.group_id = groupProAddKV.Key;
                proAddGroupRefObj.add_list = new List<ProAddRefObj>();
                proAddGroupRefObj.total_weight = 0;
                resList.Add(proAddGroupRefObj);

                List<TmpProAddRefObj> tmpProAddRefObjList = groupProAddKV.Value;
                if(tmpProAddRefObjList == null || tmpProAddRefObjList.Count <= 0)
                    continue;
                
                // 按照加成万分比小到大排序
                tmpProAddRefObjList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    // 加成从小到大排序
                    if (_a.add.CompareTo(_b.add) != 0)
                        return _a.add.CompareTo(_b.add);

                    // 若加成一样(配表不应该出现这种配置情况, 但代码还是按照id排一下序)
                    return _a.id.CompareTo(_b.id);
                });

                // 将同组的tmpProAddRefObj填入对应分组proAddGroupRefObj的add_list中
                ProAddRefObj proAddRefObj = null;
                foreach (TmpProAddRefObj tmpProAddRefObj in tmpProAddRefObjList)
                {
                    if(tmpProAddRefObj == null)
                        continue;

                    proAddGroupRefObj.total_weight += tmpProAddRefObj.random_weight;//累加到当前为止的权重

                    proAddRefObj = new ProAddRefObj();
                    proAddRefObj.id = tmpProAddRefObj.id;
                    proAddRefObj.add = tmpProAddRefObj.add;
                    proAddRefObj.random_weight = tmpProAddRefObj.random_weight;
                    proAddRefObj.fromBeginToNowWeight = proAddGroupRefObj.total_weight;
                    
                    proAddGroupRefObj.add_list.Add(proAddRefObj);
                }
            }

            return resList;
        }
    }
}