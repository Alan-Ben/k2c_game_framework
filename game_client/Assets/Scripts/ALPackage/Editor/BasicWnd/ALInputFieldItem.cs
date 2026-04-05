using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using ALPackage;

namespace ALPackage
{
    public class ALInputFieldItem : _IALExportMenuInterface
    {
        private string _m_sText;
        private string _m_sInputText;
        private int _m_iHeight;

        public ALInputFieldItem(string _title, string _defaultValue)
        {
            _m_sText = _title;
            _m_sInputText = _defaultValue;
            _m_iHeight = 10;
        }
        public ALInputFieldItem(string _text, string _defaultValue, int _height)
        {
            _m_sText = _text;
            _m_sInputText = _defaultValue;
            _m_iHeight = _height;
        }

        public virtual bool needShow { get { return true; } }
        public string inputText { get { return _m_sInputText; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            //输出文本信息
            GUILayout.Label(_m_sText, GUILayout.Height(_m_iHeight));
            _m_sInputText = GUILayout.TextField(_m_sInputText, GUILayout.Height(_m_iHeight));
        }
    }
}
