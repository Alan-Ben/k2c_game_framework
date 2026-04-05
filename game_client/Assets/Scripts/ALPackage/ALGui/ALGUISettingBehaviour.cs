using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /*********************
     * 可通过本脚本方便的在Unity中设置默认的GUI相关属性
     * 此对象仅允许创建并赋值一次，超出一次会导致第一次设置的相关信息无效
     *********************/
    public class ALGUISettingBehaviour : MonoBehaviour
    {
        /** 默认的字体显示 */
        public Font defaultTxtFont = null;
        /** 默认的字体颜色 */
        public Color defaultTxtColor = Color.black;
        /** 默认标题背景(九宫格模式) */
        public ALSquaredTextureObj defaultTitleBKTexture = null;
        /** 默认文字背景(九宫格模式) */
        public ALSquaredTextureObj defaultTextAreaBKTexture = null;
        public ALSquaredTextureObj defaultTextAreaHoverBKTexture = null;
        public ALSquaredTextureObj defaultTextAreaFocusBKTexture = null;

        /** 默认按钮背景(九宫格模式) */
        public ALSquaredTextureObj defaultButtonNormalBKTexture = null;
        public ALSquaredTextureObj defaultButtonHoverBKTexture = null;
        public ALSquaredTextureObj defaultButtonDownBKTexture = null;
        public ALSquaredTextureObj defaultButtonDisableBKTexture = null;

        /** 默认的滑动条图片信息 */
        public ALSOGUIScrollBarStyle defaultVScrollBarStyle = null;
        public ALSOGUIScrollBarStyle defaultHScrollBarStyle = null;

        /** 默认的图标对象 */
        public Texture2D defaultDownloadTexture = null;

        void Awake()
        {
            ALGUIDefaultSetting.defaultTxtFont = defaultTxtFont;
            ALGUIDefaultSetting.defaultTxtColor = defaultTxtColor;
            ALGUIDefaultSetting.defaultTitleBKTexture = new SquaredTextureObj(defaultTitleBKTexture);
            ALGUIDefaultSetting.defaultTextAreaBKTexture = new SquaredTextureObj(defaultTextAreaBKTexture);
            ALGUIDefaultSetting.defaultTextAreaHoverBKTexture = new SquaredTextureObj(defaultTextAreaHoverBKTexture);
            ALGUIDefaultSetting.defaultTextAreaFocusBKTexture = new SquaredTextureObj(defaultTextAreaFocusBKTexture);

            ALGUIDefaultSetting.defaultButtonNormalBKTexture = new SquaredTextureObj(defaultButtonNormalBKTexture);
            ALGUIDefaultSetting.defaultButtonHoverBKTexture = new SquaredTextureObj(defaultButtonHoverBKTexture);
            ALGUIDefaultSetting.defaultButtonDownBKTexture = new SquaredTextureObj(defaultButtonDownBKTexture);
            ALGUIDefaultSetting.defaultButtonDisableBKTexture = new SquaredTextureObj(defaultButtonDisableBKTexture);

            ALGUIDefaultSetting.defaultVScrollBarStyle = defaultVScrollBarStyle;
            ALGUIDefaultSetting.defaultHScrollBarStyle = defaultHScrollBarStyle;

            ALGUIDefaultSetting.defaultDownloadTexture = defaultDownloadTexture;
        }

        // Use this for initialization
        void Start()
        {
        }
    }
}

#endif
