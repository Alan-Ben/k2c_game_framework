using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoCommonFadeImage : _ANPGGUIMonoCommonFadeObject
    {
        [ALHeader("效果图片")]
        public Image effectImage;
        
        // 被设置了透明度值
        protected override void _setAlphaValue(float _alphaValue)
        {
            ALUGUICommon.setGameObjEnable(effectImage, _alphaValue > 0);
            ALUGUICommon.setUIObjAlpha(effectImage, _alphaValue);
        }
    }
}