using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHomeYieldSpeedometer : _AALBasicUIWndMono
    {
        [ALHeader("指针")]
        public Transform transNeedle;
        [ALHeader("速度表进度条")]
        public Slider sliderSpeedometer;

        [ALHeader("指针的角度方位（度）")]
        public float startAngle = 0f;
        public float endAngle = 0f;

        [ALHeader("表盘值的范围")]
        public float minValue = 0;
        public float maxValue = 100;

        [ALHeader("刻度标签列表")
        ,ALInfo("会自动根据表盘值和列表内标签数量进行平均分配")]
        public List<Text> listTextSpeedometerLabels;

        [ALHeader("弹簧平滑时间")]
        public float smoothTime = 0.1f;
        [ALHeader("最大超调量（0表示不限制）")]
        public float maxOvershoot = 0f;
    }
}