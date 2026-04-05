using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;

#if NP_GAME
using GOE;
#endif

/// <summary>
/// 语言翻译
/// </summary>
public class NPLanguageArgsRefObj
{
    public string id; //key_id
    public string lan_key_id; //语言表的key_id
    public string args; //参数列表(列表可以是值,也可以是一个KEY)

    private List<string> _m_strList;
    
    public List<string> getArgsList()
    {
        if (null == _m_strList)
        {
            _m_strList = new List<string>();
            
            if (string.IsNullOrEmpty(args))
                return _m_strList;
            
            _m_strList = args.Split(';').ToList();
        }
        
        return _m_strList;
    }

    [Indexed]
    public string _id { get { return id; } set { id = value; } }
    public string _lan_key_id { get { return lan_key_id; } set { lan_key_id = value; } }
    public string _args { get { return args; } set { args = value; } }

    public static string assetPath { get { return NPABString.C_LanArgsDBAssetPath; } }
    public static string objName { get { return NPABString.C_LanArgsDBObjPath; } }
    public static string tableName { get { return "tb_lan_args"; } }
}

