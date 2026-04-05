using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIDefaultTxtButton : ALGUIBaseMouseStatWnd
    {
        private ALGUIBaseLabel _m_glTextGuiObj;

        public ALGUIDefaultTxtButton(Rect _btnRect, string _txt)
            : base(_btnRect)
        {
            ALGUIWndPositionStyle posStyle = new ALGUIWndPositionStyle();
            posStyle.width.lengthType = ALGUILengthType.PARENT_PERCENT;
            posStyle.height.lengthType = ALGUILengthType.PARENT_PERCENT;
            posStyle.width.num = 100;
            posStyle.height.num = 100;
            _m_glTextGuiObj = new ALGUIBaseLabel(posStyle, _txt);
            //设置文字对齐
            _m_glTextGuiObj.textAlign = TextAnchor.MiddleCenter;
            _m_glTextGuiObj.txtColor = Color.white;

            ALGUIRegChildWnd(_m_glTextGuiObj);

            ALGUIWndPrintActionDelegate = OnPain;
        }
        public ALGUIDefaultTxtButton(ALGUIWndPositionStyle _posStyle, string _txt)
            : base(_posStyle)
        {
            ALGUIWndPositionStyle posStyle = new ALGUIWndPositionStyle();
            posStyle.width.lengthType = ALGUILengthType.PARENT_PERCENT;
            posStyle.height.lengthType = ALGUILengthType.PARENT_PERCENT;
            posStyle.width.num = 100;
            posStyle.height.num = 100;
            _m_glTextGuiObj = new ALGUIBaseLabel(posStyle, _txt);
            //设置文字对齐
            _m_glTextGuiObj.textAlign = TextAnchor.MiddleCenter;
            _m_glTextGuiObj.txtColor = Color.white;

            ALGUIRegChildWnd(_m_glTextGuiObj);

            ALGUIWndPrintActionDelegate = OnPain;
        }

        public string text
        {
            get
            {
                return _m_glTextGuiObj.text;
            }
            set
            {
                _m_glTextGuiObj.text = value;
            }
        }

        public void OnPain(ALGUIBaseWnd _wnd)
        {
            if (!isEnable)
            {
                if (null != ALGUIDefaultSetting.defaultButtonDisableBKTexture)
                    ALGUIDefaultSetting.defaultButtonDisableBKTexture.GUIDraw(new Rect(0, 0, width, height));
            }
            else if (isStat(ALGUIBaseMouseStat.DOWN))
            {
                if (null != ALGUIDefaultSetting.defaultButtonDownBKTexture)
                    ALGUIDefaultSetting.defaultButtonDownBKTexture.GUIDraw(new Rect(0, 0, width, height));
            }
            else if (isStat(ALGUIBaseMouseStat.OVER))
            {
                if (null != ALGUIDefaultSetting.defaultButtonHoverBKTexture)
                    ALGUIDefaultSetting.defaultButtonHoverBKTexture.GUIDraw(new Rect(0, 0, width, height));
            }
            else
            {
                if (null != ALGUIDefaultSetting.defaultButtonNormalBKTexture)
                    ALGUIDefaultSetting.defaultButtonNormalBKTexture.GUIDraw(new Rect(0, 0, width, height));
            }
        }
    }
}

#endif
