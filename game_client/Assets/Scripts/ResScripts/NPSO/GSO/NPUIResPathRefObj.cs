using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using SQLite4Unity3d;

#if NP_GAME
using GOE;
#endif

/// <summary>
/// UI资源路径信息
/// </summary>
public class NPUIResPathRefObj : NPCommonAssetPathInfo
{
    public long id; //对应id

    [Indexed]
    public long Id { get { return id; } set { id = value; } }
    public string AssetPath { get { return asset_path; } set { asset_path = value; } }
    public string ObjName { get { return obj_name; } set { obj_name = value; } }  


    public static string assetPath { get { return NPABString.C_UIResPathDBAssetPath; } }
    public static string objName { get { return NPABString.C_UIResPathDBObjPath; } }
    public static string tableName { get { return "tb_ui_res_path"; } }
}

