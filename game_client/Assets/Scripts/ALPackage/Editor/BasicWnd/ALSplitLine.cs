using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using ALPackage;

namespace ALPackage
{
    public class ALSplitLine : _IALExportMenuInterface
    {
        public ALSplitLine()
        {
        }

        public virtual bool needShow { get { return true; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            //输出文本信息
            GUILayout.Label("-----------------------------------", GUILayout.Height(10));
        }
    }
}
