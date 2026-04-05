using ALPackage;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NPGGUIMonoCommonTab : MonoBehaviour
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;  // 点击按钮

    [ALHeader("选中状态时显示")]
    public List<GameObject> isOnShow;//选中状态时显示
    [ALHeader("非选中状态显示")]
    public List<GameObject> isOffShow;//非选中状态显示
    [ALHeader("无效时显示的对象")]
    public List<GameObject> disableShow;//无效时显示的对象
    [ALHeader("无效时置灰列表")] 
    public List<MaskableGraphic> disableGrayList;
    
    [ALHeader("选中未选中时是否调整尺寸，注意自动布局的不要调整")]
    public bool selectChgSizeScale = true;
    [ALHeader("未选中时的尺寸调整")]
    public Vector2 noSelectSizeScale = Vector2.one;

    [ALHeader("小红点对象")]
    public NPGGUIMonoCommonRedTip monoRedTip;
}
