
using ALPackage;
using GOE;
using JetBrains.Annotations;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励品质容器Item
    /// </summary>
    public class GGUIWndNumMergeBoxRewardQualityContainerItem : _AHotfixBaseSubWnd<GGUIMonoNumMergeBoxRewardQualityContainerItem>
    {
        private GGUISubWndQualityShowGo _m_wQualityShowGo;


        public GGUIWndNumMergeBoxRewardQualityContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_wQualityShowGo?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wQualityShowGo?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wQualityShowGo?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.monoQualityShowGo != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(hotfixWnd.monoQualityShowGo);
        }


        /// <summary>
        /// 刷新显示
        /// </summary>
        /// <param name="_quality">品质</param>
        /// <param name="_probability">概率（万分比）</param>
        /// <param name="_isUpgrade">是否有提升</param>
        public void refreshWnd(EQuality _quality, int _probability, bool _isUpgrade)
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            // 设置品质显示
            _m_wQualityShowGo?.setData(_quality);

            // 设置概率文本
            float percent = _probability / 100f;
            ALUGUICommon.setLabelTxt(hotfixWnd.txtProbability, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percent));

            // 设置颜色（是否提升）
            hotfixWnd.setIsUpgrade(_isUpgrade);
        }
    }
}
