using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public class NPGGUIMonoPlayerLvUpPropItem : _TALUGUIMonoGridItem
{
    [ALHeader("属性名字")]
    public Text propNameTxt;

    [ALHeader("属性旧值")]
    public Text propOldValueTxt;

    [ALHeader("属性新值")]
    public Text propNewValueTxt;

}
