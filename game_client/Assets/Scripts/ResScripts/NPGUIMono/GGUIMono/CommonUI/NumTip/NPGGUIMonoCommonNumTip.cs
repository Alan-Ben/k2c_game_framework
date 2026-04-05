using UnityEngine;
using System.Collections;
using ALPackage;
using UnityEngine.UI;

public class NPGGUIMonoCommonNumTip : _AALBasicUIWndMono
{
    public Animation anim;//动画
    public RectTransform rectTransform;//窗口位置调整,当作为子窗口时，一般是窗口自己的rect
    public Text txtNum;// 资源文本
    public float disableTime; //无效的时间间隔
}
