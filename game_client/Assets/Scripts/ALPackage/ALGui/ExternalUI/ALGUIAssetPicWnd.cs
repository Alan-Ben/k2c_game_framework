using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    class ALGUIAssetPicWnd : ALGUIBaseWnd
    {
        private string _m_sAssetImgPath;
        private ALBaseRsourceObj<Texture2D> _m_tPic;

        public ALGUIAssetPicWnd(Rect _wndRect, string _assetImgPath)
            : base(_wndRect)
        {
            _m_sAssetImgPath = _assetImgPath;
            _m_tPic = ALResourceDataCore.Instance.textureMgr.requestResource(_m_sAssetImgPath);

            //清空本窗口的鼠标点击等响应
            ALGUIClearAllMouseDelegate();

            ALGUIWndPrintActionDelegate = OnPain;
        }

        public void OnPain(ALGUIBaseWnd _wnd)
        {
            //绘制图片
            GUI.DrawTexture(new Rect(0, 0, _m_rWndRect.width, _m_rWndRect.height), _m_tPic.obj);
        }
    }
}
#endif
