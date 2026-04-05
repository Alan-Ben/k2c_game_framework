using System.Collections.Generic;
using GOE;
using UnityEngine.UI;

/// <summary>
/// 聊天表情组item
/// </summary>
public class GGUIMonoChatEmoteGroupContainerItem : _ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("分组名")]
    public TextEx txtName;
    [ALHeader("分组图片")]
    public RawImage icon;
    [ALHeader("解锁状态配置")]
    public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfos;

}