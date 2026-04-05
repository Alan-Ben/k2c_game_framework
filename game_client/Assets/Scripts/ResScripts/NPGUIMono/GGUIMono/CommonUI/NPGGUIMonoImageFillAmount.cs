using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 用于控制ImageFillAmount只使用某一范围的时候用的脚本
    /// </summary>
    public class NPGGUIMonoImageFillAmount:MonoBehaviour
    {
        [ALHeader("用于显示fillAmount的图片")]
        public Image fillImage;
        
        [ALHeader("用于显示fillAmount的范围的最小值")]
        public float minRangValue = 0;
        [ALHeader("用于显示fillAmount的范围的最大值")]
        public float maxRangValue = 1;

        /// <summary>
        /// 设置图片的FillAmount
        /// </summary>
        /// <param name="_fillAmount"></param>
        public void setFillAmount(float _fillAmount)
        {
            if(null == fillImage)
                return;
            fillImage.fillAmount = minRangValue + (maxRangValue - minRangValue) * _fillAmount;
            fillImage.fillAmount = Mathf.Min(fillImage.fillAmount, maxRangValue);
        }
    }
}