using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /** 按下Enter的处理函数 */
    public delegate void _onPressAction();
    public class ALGUIBaseTextArea : ALGUIBaseMouseStatWnd
    {
        protected string _m_sTxt;
        protected GUIStyle _m_sTxtStyle;
        protected bool _m_bIsSingleLine;
        protected bool _m_bIsPassword;

        /** 文字限制总长度 */
        protected int _m_iTextLengthLimit;

        /** 存储处理函数的对象 */
        protected _onPressAction _m_dPressEnterDelegate;
        protected _onPressAction _m_dPressEscDelegate;

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
        public bool singleLine
        {
            get { return _m_bIsSingleLine; }
            set { _m_bIsSingleLine = value; }
        }
        public bool isPassword
        {
            get { return _m_bIsPassword; }
            set { _m_bIsPassword = value; }
        }
        public int textLengthLimit
        {
            get { return _m_iTextLengthLimit; }
            set { _m_iTextLengthLimit = value; }
        }

        public ALGUIBaseTextArea(Rect _rect, string _defaultTxt)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = true;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(Rect _rect, string _defaultTxt, Color _txtColor)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = true;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(Rect _rect, string _defaultTxt, Color _txtColor, bool _isSingleLine)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = _isSingleLine;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(Rect _rect, string _defaultTxt, Color _txtColor, bool _isSingleLine, GUIStyle _txtStyle)
            : base(_rect)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = _isSingleLine;
            _m_sTxtStyle = _txtStyle;
            _m_sTxtStyle.normal.textColor = _txtColor;

            //去除背景
            _m_sTxtStyle.border = new RectOffset(0, 0, 0, 0);
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = true;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = true;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, bool _isSingleLine)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = _isSingleLine;
            _m_sTxtStyle = new GUIStyle(ALGUIMain.instance._m_sTextAreaStyle);

            _m_sTxtStyle.normal.textColor = _txtColor;
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }
        public ALGUIBaseTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, bool _isSingleLine, GUIStyle _txtStyle)
            : base(_posStyle)
        {
            _m_sTxt = _defaultTxt;
            _m_bIsSingleLine = _isSingleLine;
            _m_sTxtStyle = _txtStyle;
            _m_sTxtStyle.normal.textColor = _txtColor;

            //去除背景
            _m_sTxtStyle.border = new RectOffset(0, 0, 0, 0);
            _m_iTextLengthLimit = 0;

            _m_dPressEnterDelegate = null;

            //注册窗口事件
            ALGUIWndPrintActionDelegate = OnPain;
            ALGUIRegWndPressDelegate(OnPressKey);
        }

        /****************
         * 绘制函数
         ****************/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            PainTxtBackground();

            if (_m_bEnable && _m_bIsFouc)
            {
                //设置下一个GUI控件对象
                GUI.SetNextControlName(wndID.ToString());

                if (_m_bIsPassword)
                    _m_sTxt = GUI.PasswordField(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), _m_sTxt, '*', _m_sTxtStyle);
                else if (singleLine)
                    _m_sTxt = GUI.TextField(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), _m_sTxt, _m_sTxtStyle);
                else
                    _m_sTxt = GUI.TextArea(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), _m_sTxt, _m_sTxtStyle);

                //设置焦点
                GUI.FocusControl(wndID.ToString());
            }
            else
            {
                if (_m_bIsPassword)
                {
                    string passwordTxt = new string('*', _m_sTxt.Length);
                    GUI.Label(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), passwordTxt, _m_sTxtStyle);
                }
                else
                {
                    GUI.Label(new Rect(4, 4, _m_rWndRect.width - 8, _m_rWndRect.height - 8), _m_sTxt, _m_sTxtStyle);
                }
            }

            if (_m_iTextLengthLimit > 0)
            {
                //文字限制长度有效时
                if (_m_sTxt.Length > _m_iTextLengthLimit)
                {
                    _m_sTxt = _m_sTxt.Substring(0, _m_iTextLengthLimit);
                }
            }
        }

        /****************
         * 窗口在为焦点模式下按下Enter按键时的事件函数
         * 返回值表示是否截取对应消息
         **/
        public bool OnPressKey(ALGUIBaseWnd _wnd, KeyCode _key)
        {
            if (KeyCode.Return == _key)
            {
                if (null != onPressEnter)
                {
                    //调用事件函数
                    onPressEnter();
                }

                //失去焦点
                unfocus();

                return true;
            }
            else if (KeyCode.Escape == _key)
            {
                if (null != onPressEsc)
                {
                    //调用事件函数
                    onPressEsc();
                }

                //失去焦点
                unfocus();

                return true;
            }

            return false;
        }

        /*****************
         * 返回回调对象
         **/
        public _onPressAction onPressEnter
        {
            get { return _m_dPressEnterDelegate; }
            set { _m_dPressEnterDelegate = value; }
        }
        public _onPressAction onPressEsc
        {
            get { return _m_dPressEscDelegate; }
            set { _m_dPressEscDelegate = value; }
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
