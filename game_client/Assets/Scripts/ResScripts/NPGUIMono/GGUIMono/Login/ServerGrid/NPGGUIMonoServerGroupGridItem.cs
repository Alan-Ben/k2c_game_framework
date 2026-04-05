using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ALPackage;
using GOE;

/**************
 * 服务器组列表item。。。
 **/
public class NPGGUIMonoServerGroupGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("服务器组名")]
    public TextEx textGroupName;
    
    [ALHeader("点击按钮")]
    public GGUIMonoCommonSetColorTab clickBtn;

}
