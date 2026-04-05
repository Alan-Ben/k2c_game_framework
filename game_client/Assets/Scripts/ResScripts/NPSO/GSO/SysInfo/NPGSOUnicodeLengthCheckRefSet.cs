using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

/// <summary>
/// Unicode对应的编码段的字符长度
/// </summary>
[Serializable]
public class NPUnicodeLengthCheckRefObj : _IALBasicRefObj
{
    public long _refId { get { return 0; } }

    [ALAutoExportVariableAttr(false, false)]
    public ushort min_range_include;
    [ALAutoExportVariableAttr(false, false)]
    public ushort max_range_include;
    [ALAutoExportVariableAttr(false, false)]
    public int length;
}

public class NPGSOUnicodeLengthCheckRefSet : _TALSOBasicRefSet<NPUnicodeLengthCheckRefObj>
{
    /************
    * 资源加载路径
    **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "unicode_length_check"; } }
}

