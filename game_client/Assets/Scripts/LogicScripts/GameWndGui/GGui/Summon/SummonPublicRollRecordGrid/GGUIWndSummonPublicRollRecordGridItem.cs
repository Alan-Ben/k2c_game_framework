using ALPackage;

namespace GOE
{
    /// <summary>
    /// 公告召唤记录item
    /// </summary>
    public class GGUIWndSummonPublicRollRecordGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoSummonPublicRollRecordGridItem>
    {
        private Common.GachaObj.Gacha_PublicRecordInfo _m_iRecordInfo;
        
        public GGUIWndSummonPublicRollRecordGridItem(GGUIMonoSummonPublicRollRecordGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        public void setData(Common.GachaObj.Gacha_PublicRecordInfo _recordInfo)
        {
            _m_iRecordInfo = _recordInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iRecordInfo == null)
                return;

            GachaItemRefObj gachaItemRefObj = GRefdataCoreMgr.instance.gachaItemRefCore.getRef(_m_iRecordInfo.getItemId());
            if(gachaItemRefObj == null || gachaItemRefObj.item == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtRecord,
                TextTranslate.instance.getLanguage(TransKeyConst.summon_publicRollRecordDesc_str2_num1,
                    _m_iRecordInfo.getPlayerName(), gachaItemRefObj.item.getItemName(), gachaItemRefObj.item.count));
        }
    }
}