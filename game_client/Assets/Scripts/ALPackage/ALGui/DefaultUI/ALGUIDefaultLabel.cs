using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIDefaultLabel : ALGUIBaseLabel
    {
        public ALGUIDefaultLabel(Rect _rect, string _defaultTxt)
            : base(_rect, _defaultTxt)
        {
        }
        public ALGUIDefaultLabel(Rect _rect, string _defaultTxt, Color _txtColor)
            : base(_rect, _defaultTxt, _txtColor)
        {
        }
        public ALGUIDefaultLabel(Rect _rect, string _defaultTxt, Color _txtColor, GUIStyle _txtStyle)
            : base(_rect, _defaultTxt, _txtColor, _txtStyle)
        {
        }
        public ALGUIDefaultLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt)
            : base(_posStyle, _defaultTxt)
        {
        }
        public ALGUIDefaultLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor)
            : base(_posStyle, _defaultTxt, _txtColor)
        {
        }
        public ALGUIDefaultLabel(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, GUIStyle _txtStyle)
            : base(_posStyle, _defaultTxt, _txtColor, _txtStyle)
        {
        }

        /******************
         * 绘制文字填写区域背景图
         ******************/
        public override void PainTxtBackground()
        {
            if (null != ALGUIDefaultSetting.defaultTextAreaBKTexture)
            {
                ALGUIDefaultSetting.defaultTextAreaBKTexture.GUIDraw(new Rect(0, 0, _m_rWndRect.width, _m_rWndRect.height));
            }
        }
    }
}

#endif
