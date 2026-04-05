using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /*********************
    * 系统信息显示窗口
    **/
    public class ALDebugWnd : ALGUIBaseWnd
    {
        private static ALDebugWnd _g_instance = null;
        public static ALDebugWnd instance { get { return _g_instance; } }
        public static void init()
        {
            if (null == _g_instance)
                _g_instance = new ALDebugWnd();
        }
        public static void appendInfo(string _str)
        {
            if (null == _g_instance)
                return;

            _g_instance.outputTxt = _str + "\n" + _g_instance.outputTxt;

            //限制长度
            if (_g_instance.outputTxt.Length > 10240)
                _g_instance.outputTxt = _g_instance.outputTxt.Substring(0, 10240);
        }

        /** 其他信息显示部分 */
        private ALGUIBaseLabel _m_lDebugOutputLabel;

        public ALDebugWnd()
            : base()
        {
            //设置本窗口的位置信息
            positionStyle.width.lengthType = ALGUILengthType.PARENT_PERCENT;
            positionStyle.height.lengthType = ALGUILengthType.PARENT_PERCENT;
            positionStyle.width.num = 100;
            positionStyle.height.num = 100;
            positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
            positionStyle.x = 0;
            positionStyle.y = 0;
            //设置本窗口无效
            disable();

            ALGUIWndPositionStyle outputPosStyle = new ALGUIWndPositionStyle();
            outputPosStyle.width.lengthType = ALGUILengthType.PARENT_PERCENT;
            outputPosStyle.height.lengthType = ALGUILengthType.PARENT_PERCENT;
            outputPosStyle.width.num = 100;
            outputPosStyle.height.num = 100;
            outputPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
            outputPosStyle.x = 0;
            outputPosStyle.y = 0;

            _m_lDebugOutputLabel = new ALGUIBaseLabel(outputPosStyle, "");
            _m_lDebugOutputLabel.textAlign = TextAnchor.UpperLeft;
            _m_lDebugOutputLabel.txtColor = Color.red;
            _m_lDebugOutputLabel.disable();

            _m_lDebugOutputLabel.hide();

            //注册文字显示对象
            ALGUIRegChildWnd(_m_lDebugOutputLabel);
        }

        /** 设置显示的文字 */
        public string outputTxt
        {
            get
            {
                return _m_lDebugOutputLabel.text;
            }
            set
            {
                _m_lDebugOutputLabel.text = value;

                if (null != value && value.Length > 0)
                    _m_lDebugOutputLabel.show();
                else
                    _m_lDebugOutputLabel.hide();
            }
        }
    }
}

#endif
