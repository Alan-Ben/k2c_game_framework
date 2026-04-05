using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 组合称号后缀数据
    /// </summary>
    public class PlayerTitleComboSfxInfo : _APlayerTitleComboInfo, _IPlayerTitleCombo
    {
        //称号后缀配置
        private PlayerTitleSuffixRefObj _m_titleSuffixRef;

        /// <summary>
        /// 配置id
        /// </summary>
        public long id { get { return _m_titleSuffixRef != null ? _m_titleSuffixRef.id : 0; } }
        /// <summary>
        /// 配置数据
        /// </summary>
        public PlayerTitleSuffixRefObj titleSuffixRef { get { return _m_titleSuffixRef; } }
        /// <summary>
        /// 解锁条件
        /// </summary>
        protected override _NPPlayerConditionSerializeInfo unlockCondition { get { return _m_titleSuffixRef != null ? _m_titleSuffixRef.unlock_condition : null; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_titleSuffixRef"></param>
        public PlayerTitleComboSfxInfo(PlayerTitleSuffixRefObj _titleSuffixRef) : base(_titleSuffixRef?.add_msg_type_list)
        {
            _m_titleSuffixRef = _titleSuffixRef;
        }

        /// <summary>
        /// 处理解锁
        /// </summary>
        protected override void _dealUnlock(bool _popTip)
        {
            if (_m_titleSuffixRef == null)
                return;

            //解锁上浮提示
            long unlockCenterTipId = GRefdataCoreMgr.instance.npGeneral.player_combo_title_unlock_center_tip_id;
            if (unlockCenterTipId > 0 && _popTip)
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_gainTitleSuffix_str, GCommon.getItemName(ENPItemType.TITLE_SFX, id)), unlockCenterTipId);
            //请求解锁
            NPPlayer.instance.titleComp.reqUnlockComboTitleSfx(_m_titleSuffixRef.id);
        }

        /// <summary>
        /// 处理已查看
        /// </summary>
        protected override void _dealSetIsViewed()
        {
            NPPlayer.instance.titleComp.setReadSuffixTitleRedTip(id);
        }
    }
}

