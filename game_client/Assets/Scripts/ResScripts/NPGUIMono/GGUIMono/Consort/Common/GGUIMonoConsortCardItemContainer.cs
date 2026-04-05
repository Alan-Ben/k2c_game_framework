using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

//妃子卡牌item 容器
public class GGUIMonoConsortCardItemContainer:_ATNPGGUIMonoSingleChoiceContainer<GGUIMonoConsortCardItem>
{
    public RectTransform areaMaskObj;
    [ALHeader("列表完整滚动的时间")]
    public float itemMoveTime = 0.2f;
    [ALHeader("空列表的时候显示,反之隐藏")]
    public List<GameObject> goListEmpterShow;
    [ALHeader("空列表的时候隐藏,反之显示")]
    public List<GameObject> goListEmpterHide;
}