using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ALPackage;
using GOE;

/**************
 * 服务器列表item。。。
 **/
public class NPGGUIMonoServerGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("服务器显示对象")]
    public NPGGUIMonoServerSingleItem serverItem;
    [ALHeader("点击按钮")]
    public GGUIMonoCommonSetColorTab clickBtn;
    [ALHeader("已经有角色的玩家信息")] 
    public NPGGUIMonoPlayerIcon playerIcon;
}
