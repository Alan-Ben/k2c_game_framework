using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-免费宝箱页面
    /// </summary>
    public class GGUIWndGuildFreeBoxPage : _AGGUIWndGuildBoxPageBase<GGUIMonoGuildFreeBoxPage>
    {
        public GGUIWndGuildFreeBoxPage(Transform _parent) : base(EGuildBoxType.GUILD_FREE_BOX, GGUIMonoGuildFreeBoxPage.assetPath, GGUIMonoGuildFreeBoxPage.objName,  _parent)
        {
        }
    
        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixCDChg);
        }
    
        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixCDChg);
        }
    
        protected override void _onResetEx()
        {
        }
    
        protected override void _onDiscardEx()
        {
        }
    
        protected override void _onWndInitDoneEx()
        {
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        protected override void _onRefreshWnd()
        {
            _refreshCanGetCount();
        }

        /// <summary>
        /// 刷新可领取次数
        /// </summary>
        private void _refreshCanGetCount()
        {
            if (wnd == null)
                return;

            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.guild_box_claim_fixed_cd_id);

            long maxCount = 0;
            long curCount = 0;
            if (fixedCdInfo != null)
            {
                maxCount = fixedCdInfo.getMaxCount();
                curCount = maxCount - fixedCdInfo.getCount();
            }
            ALUGUICommon.setLabelTxt(wnd.txtFreeBoxCount, TextTranslate.instance.getLanguage(TransKeyConst.guild_box_dailyFreeBoxNum_num_num, curCount, maxCount));
        }

        /// <summary>
        /// 可领取次数变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onFixCDChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            long refId = (long)_objects[0];
            if(GRefdataCoreMgr.instance.npGeneral.guild_box_claim_fixed_cd_id == refId)
            {
                _refreshCanGetCount();
            }
        }
    }
}