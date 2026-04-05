using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/***************
 * 通用的单个信息接口对象
 **/
namespace ALPackage
{
    public interface _IALBasicRefObj
    {
        long _refId { get; }
    }
}