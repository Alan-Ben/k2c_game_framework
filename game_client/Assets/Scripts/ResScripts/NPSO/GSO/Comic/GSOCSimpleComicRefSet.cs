
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 简易漫画配表
    /// </summary>
    [Serializable]
    public class SimpleComicRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // 唯一id
        public List<long> page_res_id_list;//每一页的资源路径id
        
        public long getResIdByIndex(int index)
        {
            if (null == page_res_id_list || page_res_id_list.Count == 0)
                return 0;
            
            if (index < 0 || index >= page_res_id_list.Count)
                return 0;
            return page_res_id_list[index];
        }
    }
    
    public class GSOCSimpleComicRefSet : _TALSOBasicRefSet<SimpleComicRefObj>
    {
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "simple_comic"; } }
    }
}