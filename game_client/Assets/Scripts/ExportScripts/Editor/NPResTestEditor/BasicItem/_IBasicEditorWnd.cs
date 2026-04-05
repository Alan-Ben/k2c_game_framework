using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础编辑对象
 **/
public interface _IBasicEditorItem
{
    void OnGUI();
    //处理函数
    void dealFunc();
    //是否需要最小宽度
    bool needMinWidth { get; }
}
