using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIDefaultTextArea : ALGUIBaseTextArea
    {
        public ALGUIDefaultTextArea(Rect _rect, string _defaultTxt)
            : base(_rect, _defaultTxt)
        {
        }
        public ALGUIDefaultTextArea(Rect _rect, string _defaultTxt, Color _txtColor)
            : base(_rect, _defaultTxt, _txtColor)
        {
        }
        public ALGUIDefaultTextArea(Rect _rect, string _defaultTxt, Color _txtColor, bool _isSingleLine)
            : base(_rect, _defaultTxt, _txtColor, _isSingleLine)
        {
        }
        public ALGUIDefaultTextArea(Rect _rect, string _defaultTxt, Color _txtColor, bool _isSingleLine, GUIStyle _txtStyle)
            : base(_rect, _defaultTxt, _txtColor, _isSingleLine, _txtStyle)
        {
        }
        public ALGUIDefaultTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt)
            : base(_posStyle, _defaultTxt)
        {
        }
        public ALGUIDefaultTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor)
            : base(_posStyle, _defaultTxt, _txtColor)
        {
        }
        public ALGUIDefaultTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, bool _isSingleLine)
            : base(_posStyle, _defaultTxt, _txtColor, _isSingleLine)
        {
        }
        public ALGUIDefaultTextArea(ALGUIWndPositionStyle _posStyle, string _defaultTxt, Color _txtColor, bool _isSingleLine, GUIStyle _txtStyle)
            : base(_posStyle, _defaultTxt, _txtColor, _isSingleLine, _txtStyle)
        {
        }

        /******************
         * 绘制文字填写区域背景图
         ******************/
        public override void PainTxtBackground()
        {
            if (isFocus)
            {
                if (null != ALGUIDefaultSetting.defaultTextAreaFocusBKTexture)
                    ALGUIDefaultSetting.defaultTextAreaFocusBKTexture.GUIDraw(new Rect(0, 0, _m_rWndRect.width, _m_rWndRect.height));
            }
            else if (isStat(ALGUIBaseMouseStat.OVER))
            {
                if (null != ALGUIDefaultSetting.defaultTextAreaHoverBKTexture)
                    ALGUIDefaultSetting.defaultTextAreaHoverBKTexture.GUIDraw(new Rect(0, 0, _m_rWndRect.width, _m_rWndRect.height));
            }
            else
            {
                if (null != ALGUIDefaultSetting.defaultTextAreaBKTexture)
                    ALGUIDefaultSetting.defaultTextAreaBKTexture.GUIDraw(new Rect(0, 0, _m_rWndRect.width, _m_rWndRect.height));
            }
        }
    }
}

#endif