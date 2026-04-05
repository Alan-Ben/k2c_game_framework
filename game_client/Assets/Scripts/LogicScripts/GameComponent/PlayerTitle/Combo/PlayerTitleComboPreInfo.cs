using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 组合称号前缀数据
    /// </summary>
    public class PlayerTitleComboPreInfo : _APlayerTitleComboInfo, _IPlayerTitleCombo
    {
        //称号前缀配置
        private PlayerTitlePrefixRefObj _m_titlePrefixRef;

        /// <summary>
        /// 配置id
        /// </summary>
        public long id { get { return _m_titlePrefixRef != null ? _m_titlePrefixRef.id : 0; } }
        /// <summary>
        /// 配置数据
        /// </summary>
        public PlayerTitlePrefixRefObj titlePrefixRef { get { return _m_titlePrefixRef; } }
        /// <summary>
        /// 解锁条件
        /// </summary>
        protected override _NPPlayerConditionSerializeInfo unlockCondition { get { return _m_titlePrefixRef != null ? _m_titlePrefixRef.unlock_condition : null; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_titlePrefixRef"></param>
        public PlayerTitleComboPreInfo(PlayerTitlePrefixRefObj _titlePrefixRef) : base(_titlePrefixRef?.add_msg_type_list)
        {
            _m_titlePrefixRef = _titlePrefixRef;
        }

        /// <summary>
        /// 处理解锁
        /// </summary>
        protected override void _dealUnlock(bool _popTip)
        {
            if (_m_titlePrefixRef == null)
                return;

            //解锁上浮提示
            long unlockCenterTipId = GRefdataCoreMgr.instance.npGeneral.player_combo_title_unlock_center_tip_id;
            if (unlockCenterTipId > 0 && _popTip)
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_gainTitlePrefix_str, GCommon.getItemName(ENPItemType.TITLE_PRE, id)), unlockCenterTipId);
            //请求解锁
            NPPlayer.instance.titleComp.reqUnlockComboTitlePre(_m_titlePrefixRef.id);
        }

        /// <summary>
        /// 处理已查看
        /// </summary>
        protected override void _dealSetIsViewed()
        {
            NPPlayer.instance.titleComp.setReadPrefixTitleRedTip(id);
        }
    }
}

