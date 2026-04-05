using ALPackage;
using UnityEngine.UI;

public class NPGGUIMonoCommonImageProcess:_AALBasicUIWndMono
{
    [ALHeader("进度条")] 
    public Image imageProcess;
    [ALHeader("进度条范围最小限制")] 
    public float minRange = 0f;
    [ALHeader("进度条范围最大限制")] 
    public float maxRange = 1f;
}