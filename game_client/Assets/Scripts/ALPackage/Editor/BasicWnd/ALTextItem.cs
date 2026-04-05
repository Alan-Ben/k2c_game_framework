using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using ALPackage;

namespace ALPackage
{
    public class ALTextItem : _IALExportMenuInterface
    {
        private string _m_sText;
        private int _m_iHeight;

        public ALTextItem(string _text)
        {
            _m_sText = _text;
            _m_iHeight = 10;
        }
        public ALTextItem(string _text, int _height)
        {
            _m_sText = _text;
            _m_iHeight = _height;
        }

        public virtual bool needShow { get { return true; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            //输出文本信息
            GUILayout.Label(_m_sText, GUILayout.Height(_m_iHeight));
        }
    }
}
