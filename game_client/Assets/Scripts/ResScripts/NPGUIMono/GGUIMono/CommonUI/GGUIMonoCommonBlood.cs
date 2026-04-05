using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoCommonBlood : _AALBasicUIWndMono
    {
        [ALHeader("真正的血条和渐变条")]
        [ALInfo(
            "✦将下面三个Transform放在同一个父对象下；\n" +
            "✦把下面三个Transform的Anchor调成上下左右全铺满；\n" +
            "✦把父对象调整到血条是满的程度；\n" +
            "代码是通过修改anchor来改变血条的长度的（和UGUI的Slider一样），有问题＠Coda")]
        [ALHeader("Mono配置方法")]
        public RectTransform transBlood;
        public RectTransform transFadeDamage;
        public RectTransform transFadeCure;

        [ALHeader("当受伤或回血时播放的动画")]
        public Animation animDamage;
        public Animation animCure;
        public string damageAnimName;
        public string cureAnimName;

        [ALHeader("渐变前的等待时间")]
        public float fTimeBeforeFade;
        [ALInfo("每秒移动的血量为，总血量乘以fFadeSpeed，来保证不同血量下的移动是一致的")]
        [ALHeader("血条渐变效果的速度")]
        public double fFadeSpeed = 0.4f;

        [ALHeader("总血量描述")]
        public Text txtBloodValue;
        [ALInfo("勾上了后上面的Text会显示100/100这种，不勾就只显示剩余血量")]
        [ALHeader("展示最大血量")]
        public bool bShowMaxBlood;
        [ALInfo("勾上后上面的Text的数字会变成大数字的缩写")]
        [ALHeader("展示大数字的缩写")]
        public bool bShowLargeNum;

        [ALHeader("血条震动的根节点")]
        public RectTransform transShakeRoot;
        [ALInfo("填入多组震动数据代表不同的震动级别，index越大认为振幅越大")]
        [ALHeader("震动数据")]
        public List<ShakeData> listShakeData;
        [ALHeader("默认震动级别，0是不震动")]
        public int defaultShakeLevel = 0;

        // ============ 下面是表现逻辑相关成员

        // 血条Bar的显示血量
        public double bloodCurShow { get { return _m_lBloodCurShow; } }
        public double bloodMaxShow { get { return _m_lBloodMaxShow; } }
        public double bloodFadeDamageShow { get { return _m_lBloodFadeDamageShow; } }
        public double bloodFadeCureShow { get { return _m_lBloodFadeCureShow; } }
        private double _m_lBloodCurShow;
        private double _m_lBloodMaxShow;
        private double _m_lBloodFadeDamageShow;
        private double _m_lBloodFadeCureShow;


        /// <summary>
        /// 更新血条Bar的显示
        /// </summary>
        public void updateBloodBarShow(double _current, double _max, double _damage, double _cure)
        {
            _m_lBloodCurShow = _current;
            _m_lBloodMaxShow = _max;
            _m_lBloodFadeDamageShow = _damage;
            _m_lBloodFadeCureShow = _cure;

            if (_m_lBloodCurShow < 0) _m_lBloodCurShow = 0;
            if (_m_lBloodFadeDamageShow < 0) _m_lBloodFadeDamageShow = 0;
            if (_m_lBloodFadeCureShow < 0) _m_lBloodFadeCureShow = 0;
            if (_m_lBloodMaxShow <= 0) return;

            double bloodRate = _m_lBloodCurShow / _m_lBloodMaxShow;
            _setBloodValue((float)(bloodRate));
            _setDamageFadeValue((float)(bloodRate + _m_lBloodFadeDamageShow / _m_lBloodMaxShow));
            _setCureFadeValue((float)(bloodRate + _m_lBloodFadeCureShow / _m_lBloodMaxShow));
        }


        /// <summary>
        /// 根据震动等级获取震动的数据，如果没配置数据返回Null
        /// </summary>
        public ShakeData safeGetShakeData(int _shakeLevel)
        {
            // 0级震动为不震动
            _shakeLevel--;

            return listShakeData.SafeGet(_shakeLevel);
        }

        // 更新血条长度，取值0到1
        private void _setBloodValue(float _bloodWidthRate)
        {
            // 传值做限制
            _bloodWidthRate = Mathf.Clamp01(_bloodWidthRate);

            if (transBlood == null)
                return;
            transBlood.localScale = transBlood.localScale.SetX(_bloodWidthRate);

//            // 修改血条的anchor来控制长度
//            transBlood.anchorMax = transBlood.anchorMax.SetX(_bloodWidthRate);
//
//            // 始终保持渐变条顶住血条
//            if (transFadeDamage != null) transFadeDamage.anchorMin = transFadeDamage.anchorMin.SetX(_bloodWidthRate);
//            // 始终保持渐变条顶住血条 
//            if (transFadeCure != null) transFadeCure.anchorMin = transFadeCure.anchorMin.SetX(_bloodWidthRate);
        }

        // 设置伤害渐变血条的长度
        private void _setDamageFadeValue(float _fadeWidthRate)
        {
            if (transFadeDamage == null)
                return;

            float fadeRightValue = Mathf.Clamp01(_fadeWidthRate);
            transFadeDamage.localScale = transFadeDamage.localScale.SetX(fadeRightValue);

//            // 传进来的值是条的长度比例，所以真实的右端点，得从左边的起点开始算
//            float fadeRightValue = transFadeDamage.anchorMin.x + _fadeWidthRate;
//            fadeRightValue = Mathf.Clamp01(fadeRightValue);
//
//            // 修改渐变条的右端点
//            transFadeDamage.anchorMax = transFadeDamage.anchorMax.SetX(fadeRightValue);
        }

        // 设置治愈渐变血条的长度
        private void _setCureFadeValue(float _fadeWidthRate)
        {
            if (transFadeCure == null)
                return;
            float fadeRightValue = Mathf.Clamp01(_fadeWidthRate);
            transFadeCure.localScale = transFadeCure.localScale.SetX(fadeRightValue);

//            // 传进来的值是条的长度比例，所以真实的右端点，得从左边的起点开始算
//            float fadeRightValue = transFadeCure.anchorMin.x + _fadeWidthRate;
//            fadeRightValue = Mathf.Clamp01(fadeRightValue);
//
//            // 修改渐变条的右端点
//            transFadeCure.anchorMax = transFadeCure.anchorMax.SetX(fadeRightValue);
        }
    }

    [Serializable]
    public class ShakeData
    {
        [Header("震动的剧烈程度")]
        public float fShakeIntensity;
        [Header("震动的持续时间")]
        public float fShakeDuration;
    }
}