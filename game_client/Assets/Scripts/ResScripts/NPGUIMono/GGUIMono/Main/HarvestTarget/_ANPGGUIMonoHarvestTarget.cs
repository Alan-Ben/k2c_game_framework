
using ALPackage;
using NPEnum;
using UnityEngine;

public abstract class _ANPGGUIMonoHarvestTarget : _AALBasicUIWndMono
{
    [ALHeader("对应的货币类型")]
    public CommonEnum.ECurrency currencyType;
    [ALHeader("粒子的终点")]
    public RectTransform particleTarget;
}
