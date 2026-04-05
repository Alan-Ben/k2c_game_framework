
using ALPackage;
using System;

namespace GOE
{
    [Serializable]
    public class AnecdoteEventRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public _NPPlayerConditionSerializeInfo show_condition; // 显示条件
        public long before_dialogue_id; // 事件前对话
        public long after_dialogue_id; // 事件后对话
        public NPGGoIndex event_res_index; // 事件资源
        public EAnecdoteEventType start_type; // 事件类型
        public EAnecdoteEventEndType end_type; // 结束类型

        public NPGTextureIndex event_entrance_view_icon;//事件入口view图标(texture)
        public NPGSpriteIndex event_entrance_view_icon_bg;//事件入口view图标背景(sprite)

        public int show_priority;
        
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public _IALBasicRefObj event_type_ref; // 具体事件类型的数据
    }
    public class GSOAnecdoteEventRefSet : _TALSOBasicRefSet<AnecdoteEventRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_event"; } }
    }
}