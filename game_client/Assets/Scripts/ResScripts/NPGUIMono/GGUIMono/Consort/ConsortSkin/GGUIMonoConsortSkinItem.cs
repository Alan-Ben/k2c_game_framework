using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 妃子皮肤状态
/// </summary>
public enum EConsortSkinStateType
{
    [InspectorName("未解锁且不满足解锁条件")]
    CANNOT_UNLOCK,
    [InspectorName("可解锁, 但还未请求解锁")]
    CAN_UNLOCK,
    [InspectorName("已解锁未穿戴")]
    UNLOCK_NOT_WEARING,
    [InspectorName("已解锁穿戴中")]
    UNLOCK_WEARING,
}

//妃子皮肤 item
public class GGUIMonoConsortSkinItem : _ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("妃子皮肤名称")] 
    public TextEx txtName; 
    [ALHeader("妃子皮肤图片")] 
    public RawImage skinIcon;

    [ALHeader("星级")]
    public GGUIMonoHeroCommonStar monoStar;
    
    [ALHeader("等级")]
    public TextEx txtLevel;
    
    [ALHeader("皮肤解锁状态配置")] 
    public List<NPCommonEnumStatInfo<EConsortSkinStateType>> statInfos; 
    
    [ALHeader("默认皮肤时需要显示的GO列表")]
    public List<GameObject> goDefaultShowList;
    [ALHeader("默认皮肤时需要隐藏的GO列表")]
    public List<GameObject> goDefaultHideList;
}