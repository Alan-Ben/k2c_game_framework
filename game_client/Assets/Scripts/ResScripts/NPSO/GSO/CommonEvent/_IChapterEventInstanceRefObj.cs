using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用事件-事件子表
    /// </summary>
    public interface _ICommonEventInstanceSubRefObj
    {
        //事件类型
        public Common.EventEnum.ECommonEventType eventType { get; }
    }
}