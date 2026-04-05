using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 组合称号底框数据
    /// </summary>
    public class PlayerTitleComboBgInfo : _APlayerTitleComboInfo, _IPlayerTitleCombo
    {
        //称号后缀配置
        private PlayerTitleBgRefObj _m_titleBgRef;

        /// <summary>
        /// 配置id
        /// </summary>
        public long id { get { return _m_titleBgRef != null ? _m_titleBgRef.id : 0; } }
        /// <summary>
        /// 配置数据
        /// </summary>
        public PlayerTitleBgRefObj titleBgRef { get { return _m_titleBgRef; } }
        /// <summary>
        /// 解锁条件
        /// </summary>
        protected override _NPPlayerConditionSerializeInfo unlockCondition { get { return _m_titleBgRef != null ? _m_titleBgRef.unlock_condition : null; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_titleBgRef"></param>
        public PlayerTitleComboBgInfo(PlayerTitleBgRefObj _titleBgRef) : base(_titleBgRef?.add_msg_type_list)
        {
            _m_titleBgRef = _titleBgRef;
        }

        /// <summary>
        /// 处理解锁
        /// </summary>
        protected override void _dealUnlock(bool _popTip)
        {
            if (_m_titleBgRef == null)
                return;

            //解锁上浮提示
            long unlockCenterTipId = GRefdataCoreMgr.instance.npGeneral.player_combo_title_unlock_center_tip_id;
            if(unlockCenterTipId > 0 && _popTip)
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_gainTitleBg_str,GCommon.getItemName(ENPItemType.TITLE_BG,id)), unlockCenterTipId);
            //请求解锁
            NPPlayer.instance.titleComp.reqUnlockComboTitleBg(_m_titleBgRef.id);
        }

        /// <summary>
        /// 处理已查看
        /// </summary>
        protected override void _dealSetIsViewed()
        {
            NPPlayer.instance.titleComp.setReadBgTitleRedTip(id);
        }
    }
}

