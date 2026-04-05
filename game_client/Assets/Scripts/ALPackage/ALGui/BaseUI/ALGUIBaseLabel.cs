using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIBaseLabel : ALGUIBaseWnd
    {
        protected string _m_sTxt;
        protected GUIStyle _m_sTxtStyle;

        /** 文字限制总长度 */
        protected int _m_iTextLengthLimit;

        public string text
        {
            get { return _m_sTxt; }
            set { _m_sTxt = value; }
        }
        public GUIStyle txtStyle
        {
            get { return _m_sTxtStyle; }
            set
            {
                _m_sTxtStyle = value;
                _m_sTxtStyle.border = new RectOffset(0, 0, 0, 0);
            }
        }
        public Color txtColor
        {
            get { return _m_sTxtStyle.normal.textColor; }
            set { _m_sTxtStyle.normal.textColor = value; }
        }
        public int textLengthLimit
        {
            get { return _m_iTextLengthLimit; }
            set { _m_iTextLengthLimit = value; }
        }
        public TextAnchor textAlign
        {
            get { return _m_sTxtStyle.alignment; }
            set { _m_sTxtStyle.alignment = value; }
        }

        public ALGUIBaseLabel(Rect _rect, string _defaultTxt)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIBaseLabel(Rect _rect, string _defaultTxt, Color _txtColor)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIBaseLabel(Rect _rect, string _defaultTxt, Color _txtColor, GUIStyle _txtStyle)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = _txtStyle;
            _m_sTxtStyle.normal.textColor = _txtColor;

            //去除背景
            _m_sTxtStyle.border = new RectOffset(0, 0, 0, 0);
            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIBaseLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIBaseLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIBaseLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, GUIStyle _txtStyle)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_sTxtStyle = _txtStyle;
            _m_sTxtStyle.normal.textColor = _txtColor;

            //去除背景
            _m_sTxtStyle.border = new RectOffset(0, 0, 0, 0);
            _m_iTextLengthLimit = 0;

            //注册绘制函数
            ALGUIWndPrintActionDelegate = OnPain;
        }

        /****************
         * 绘制函数
         ****************/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            PainTxtBackground();

            if (_m_sTxt.Length > 0)
                GUI.Label(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), _m_sTxt, _m_sTxtStyle);
        }

        /******************
         * 绘制文字填写区域背景图
         ******************/
        public virtual void PainTxtBackground()
        {
        }
    }
}

#endif
