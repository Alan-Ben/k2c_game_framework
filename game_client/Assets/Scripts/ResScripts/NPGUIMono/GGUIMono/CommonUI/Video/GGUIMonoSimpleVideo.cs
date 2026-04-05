using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GOE
{
    public class GGUIMonoSimpleVideo : _AALBasicUIWndMono
    {
        public RawImage showRawImage;
        [ALHeader("是否独立背景音乐")]
        public bool isBgMusic;
    }
}