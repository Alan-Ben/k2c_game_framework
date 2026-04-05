
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class AnecdoteEventChoiceRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public List<long> option_id_list;
        public string event_desc;

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public List<AnecdoteEventChoiceOptionRefObj> option_ref_list;
    }
    public class GSOAnecdoteEventChoiceRefSet : _TALSOBasicRefSet<AnecdoteEventChoiceRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_event_choice"; } }
    }
}