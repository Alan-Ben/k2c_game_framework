using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /*********************
     * 主城部分的操作监听处理对象
     **/
    public abstract class _ACityCameraMonitor
    {
        //摄像头位置变更处理
        public abstract void OnCameraPosChg();

        //缩放变更的处理
        public abstract void OnScaleChg(float _targetScale);

        //进入时的操作处理
        public abstract void OnEnter();
        //退出操作时的处理
        public abstract void OnExit();
    }
}
