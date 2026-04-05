using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注册到位置管理器中的接口
/// 不同的对象（场景对象、UI对象）根据本身逻辑不同进行不同的实现
/// </summary>
namespace GOE
{
    public interface _INPShowPos
    {
        /// <summary>
        /// 获取对应显示的UI坐标
        /// </summary>
        /// <returns></returns>
        Vector2 getUIPos();
    }
}

