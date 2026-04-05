using System;

namespace GOE
{
    /// <summary>
    /// 用户热更功能备注的header
    /// </summary>
    public class HotfixMonoAttribute : Attribute
    {
        //是否序列化，是的化会生成mono策划可以拖
        public bool isSerialize;
        //注释
        public string notes;
        
        public HotfixMonoAttribute(string _notes, bool _isSerialize = true)
        {
            notes = _notes;
            isSerialize = _isSerialize;
        }
    }
}