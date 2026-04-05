using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndMarsExploreMineCollectReward : _ANPGGUIBasicWnd<GGUIMonoMarsExploreMineCollectReward>
    {
        private static GGUIWndMarsExploreMineCollectReward _g_instance;
        [NotNull] public static GGUIWndMarsExploreMineCollectReward instance { get { return _g_instance ??= new GGUIWndMarsExploreMineCollectReward(); } }

        private NPGGUIWndCommonItemContainer _m_rewardContainerWnd;
        [CanBeNull] private List<CommonItemData> _m_rewardList;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreMineCollectReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreMineCollectReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExploreMineCollectReward() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_rewardContainerWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_rewardContainerWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_rewardContainerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_rewardContainerWnd?.discard();
            _m_rewardContainerWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardItemContainer != null)
                _m_rewardContainerWnd = new NPGGUIWndCommonItemContainer(wnd.monoRewardItemContainer);
        }


        public void refreshWnd([CanBeNull] string _desc, [CanBeNull] List<CommonItemData> _rewardList)
        {
            _m_rewardList = _rewardList;

            if (wnd != null)
                ALUGUICommon.setLabelTxt(wnd.txtDesc, _desc ?? string.Empty);

            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_rewardContainerWnd?.showItemList(_m_rewardList);
        }
    }
}
