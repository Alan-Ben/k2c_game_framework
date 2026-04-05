
using ALPackage;
using GOE;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励容器Item
    /// </summary>
    public class GGUIWndNumMergeBoxRewardContainerItem : _AHotfixBaseSubWnd<GGUIMonoNumMergeBoxRewardContainerItem>
    {
        private GGUIWndNumMergeBoxRewardItemContainer _m_wItemContainer;
        private GGUISubWndQualityShowGo _m_wQualityShowGo;


        public GGUIWndNumMergeBoxRewardContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_wItemContainer?.showWnd();
            _m_wQualityShowGo?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
            _m_wQualityShowGo?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
            _m_wQualityShowGo?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            // 初始化嵌套的奖励Item容器
            if (hotfixWnd.monoItemContainer != null)
                _m_wItemContainer = new GGUIWndNumMergeBoxRewardItemContainer(hotfixWnd.monoItemContainer);
            // 初始化品质显示
            if (hotfixWnd.monoQualityShowGo != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(hotfixWnd.monoQualityShowGo);
        }


        /// <summary>
        /// 刷新显示
        /// </summary>
        /// <param name="_info">品质概率信息</param>
        public void refreshWnd(QualityProbabilityInfo _info)
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            // 设置品质显示
            _m_wQualityShowGo?.setData(_info.quality);

            // 设置品质名称文本
            NPQualityRefObj qualityRefObj = GRefdataCoreMgr.instance.getQuality(ENPItemType.BAG_ITEM, _info.quality);
            string name = TextTranslate.instance.getLanguage(qualityRefObj?.name);
            
            ALUGUICommon.setLabelTxt(hotfixWnd.txtQualityName, name);
            // 设置总概率文本
            float percent = _info.probability / 100f;
            string percentStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percent);
            ALUGUICommon.setLabelTxt(hotfixWnd.txtProbability, percentStr);
            // 设置品质名称和概率文本
            ALUGUICommon.setLabelTxt(hotfixWnd.txtQualityNameAndProbability, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_boxRewardQualityNameAndProbability_name_percent, name, percentStr));

            // 刷新物品容器
            _m_wItemContainer?.refreshWnd(_info.itemList, _info.probabilityList);
        }
    }
}
