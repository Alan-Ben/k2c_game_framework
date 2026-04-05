using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public interface _IALExportMenuInterface
    {
        //具体的gui绘制函数
        void onGUI();

        //是否需要显示
        bool needShow { get; }
    }
}
