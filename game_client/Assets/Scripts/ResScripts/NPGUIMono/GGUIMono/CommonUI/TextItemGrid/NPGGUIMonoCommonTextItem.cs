using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public class NPGGUIMonoCommonTextItem : _TALUGUIMonoGridItem
{
    [ALHeader("文本1")]
    public Text txtOne;

    [ALHeader("文本2")]
    public Text txtTwo;
    
    [ALHeader("图标")]
    public RawImage imgIcon;

    [ALHeader("额外展示的GO")]
    public GameObject goShow;
}
