
using ALPackage;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励物品容器Item
    /// </summary>
    public class GGUIWndNumMergeBoxRewardItemContainerItem : _AHotfixBaseSubWnd<GGUIMonoNumMergeBoxRewardItemContainerItem>
    {
        private NPGGUIWndCommonItem _m_wItem;


        public GGUIWndNumMergeBoxRewardItemContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_wItem?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wItem?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wItem?.discard();
            _m_wItem = null;
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.monoItem != null)
                _m_wItem = new NPGGUIWndCommonItem(hotfixWnd.monoItem);
        }


        /// <summary>
        /// 刷新物品显示
        /// </summary>
        /// <param name="_item">物品</param>
        /// <param name="_probability">概率（万分比）</param>
        public void refreshWnd(NPCommonCostItem _item, int _probability)
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            // 设置物品
            _m_wItem?.setItem(_item);
            // 设置概率文本（转换为百分比）
            float percent = _probability / 100f;
            ALUGUICommon.setLabelTxt(hotfixWnd.txtProbability, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percent));
        }
    }
}
