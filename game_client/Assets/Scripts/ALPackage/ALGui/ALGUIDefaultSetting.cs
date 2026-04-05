using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    class ALGUIDefaultSetting
    {
        /** 默认的字体显示 */
        public static Font defaultTxtFont = null;
        /** 默认的字体颜色 */
        public static Color defaultTxtColor = Color.white;
        /** 默认标题背景(九宫格模式) */
        public static SquaredTextureObj defaultTitleBKTexture = null;
        /** 默认文本背景(九宫格模式) */
        public static SquaredTextureObj defaultTextAreaBKTexture = null;
        public static SquaredTextureObj defaultTextAreaHoverBKTexture = null;
        public static SquaredTextureObj defaultTextAreaFocusBKTexture = null;
        /** 默认按钮背景(九宫格模式) */
        public static SquaredTextureObj defaultButtonNormalBKTexture = null;
        public static SquaredTextureObj defaultButtonHoverBKTexture = null;
        public static SquaredTextureObj defaultButtonDownBKTexture = null;
        public static SquaredTextureObj defaultButtonDisableBKTexture = null;

        /** 默认的滑动条图片信息 */
        public static ALSOGUIScrollBarStyle defaultVScrollBarStyle = null;
        public static ALSOGUIScrollBarStyle defaultHScrollBarStyle = null;

        /** 默认的图标对象 */
        public static Texture2D defaultDownloadTexture = null;
    }
}
#endif
