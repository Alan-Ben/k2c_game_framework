using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

//妃子卡牌item 容器
public class GGUIMonoConsortIconItemContainer:_ATNPGGUIMonoSingleChoiceContainer<GGUIMonoConsortIconItem>
{
    /// <summary>
    /// 选中之后的缩放配置，第一个是选中的缩放值，后面的是往两边依次的缩放值，超出列表的以最后一个缩放值为准
    /// </summary>
    [ALHeader("选中之后的缩放配置，第一个是选中的缩放值，\n" +
              "后面的是往两边依次的缩放值，超出列表的以最后一个缩放值为准")]
    [Space(15)]
    public List<float> scaleList;
    [ALHeader("遮罩部分")]
    public RectTransform areaMaskObj;
    [ALHeader("拖动列表引用")]
    public NPScrollRect commonScrollRect;

    [ALHeader("是否需要移动选中的item到中心")]
    public bool needMoveSelectItemCenter;

}