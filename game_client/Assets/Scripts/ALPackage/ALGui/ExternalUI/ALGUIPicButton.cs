using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIPicButton : ALGUIBaseMouseStatWnd
    {
        public ALBaseRsourceObj<Texture2D> _m_tNormalPic = null;
        public ALBaseRsourceObj<Texture2D> _m_tMouseOverPic = null;
        public ALBaseRsourceObj<Texture2D> _m_tPressDownPic = null;
        public ALBaseRsourceObj<Texture2D> _m_tDisablePic = null;

        public ALGUIPicButton(Rect _btnRect
                                , string _normalPicPath
                                , string _mouseOverPicPath
                                , string _pressDownPicPath
                                , string _disablePicPath)
            : base(_btnRect)
        {
            _m_tNormalPic = ALResourceDataCore.Instance.textureMgr.requestResource(_normalPicPath);
            _m_tMouseOverPic = ALResourceDataCore.Instance.textureMgr.requestResource(_mouseOverPicPath);
            _m_tPressDownPic = ALResourceDataCore.Instance.textureMgr.requestResource(_pressDownPicPath);
            _m_tDisablePic = ALResourceDataCore.Instance.textureMgr.requestResource(_disablePicPath);

            ALGUIWndPrintActionDelegate = OnPain;
        }

        public void OnPain(ALGUIBaseWnd _wnd)
        {
            if (!isEnable && null != _m_tDisablePic && null != _m_tDisablePic.obj)
            {
                GUI.DrawTexture(new Rect(0, 0, width, height), _m_tDisablePic.obj);
            }
            else if (isStat(ALGUIBaseMouseStat.OVER) && null != _m_tMouseOverPic && null != _m_tMouseOverPic.obj)
            {
                GUI.DrawTexture(new Rect(0, 0, width, height), _m_tMouseOverPic.obj);
            }
            else if (isStat(ALGUIBaseMouseStat.DOWN) && null != _m_tPressDownPic && null != _m_tPressDownPic.obj)
            {
                GUI.DrawTexture(new Rect(0, 0, width, height), _m_tPressDownPic.obj);
            }
            else if (null != _m_tNormalPic && null != _m_tNormalPic.obj)
            {
                GUI.DrawTexture(new Rect(0, 0, width, height), _m_tNormalPic.obj);
            }
        }
    }
}

#endif
