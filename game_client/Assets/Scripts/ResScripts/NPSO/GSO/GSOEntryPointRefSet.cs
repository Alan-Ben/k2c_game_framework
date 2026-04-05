using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using GOE;
using NPEnum;
using UnityEngine;

/// <summary>
/// 入口点表，定义几个主要场景的入口
/// </summary>
[Serializable]
public class EntryPointRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id; //唯一识别标记
    public string name; //入口点名称，跟随UI上展示
    public long red_id; //红点表的对应id
    public string entry_lock_desc;//场景入口未解锁描述
    public List<string> entry_lock_desc_args;//场景入口未解锁描述参数
    public _NPPlayerConditionSerializeInfo unlock_condition; //解锁条件（临时处理可以控制锁的显隐用）
    
    public _NPPlayerEffectSerializeInfo click_effect; //默认点击跳转效果
    public ENPFunctionType func_unlock_type;//系统功能类型

    public int follow_item_ui_res_id;//跟随窗口的ui资源id
    public int lock_desc_ui_res_id;//场景入口未解锁描述ui资源id
    public int guide_hand_ui_res_id;//场景入口引导手指资源id
}

public class GSOEntryPointRefSet : _TALSOBasicRefSet<EntryPointRefObj> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "entry_point"; } }
}
