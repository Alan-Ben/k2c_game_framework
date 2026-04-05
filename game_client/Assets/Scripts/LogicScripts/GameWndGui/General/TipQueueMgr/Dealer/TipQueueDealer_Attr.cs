using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 基础属性提示队列信息
    /// </summary>
    public class TipQueueDealer_Attr : _ABaseTipQueueDealer
    {
        //旧属性
        private PlayerAttrPropertyContainer _m_oriAttrPropertyContainer;
        //新属性
        private PlayerAttrPropertyContainer _m_curAttrPropertyContainer;
        //队列销毁接力时间
        private float _m_queueTime = 1f;
        //旧的属性值
        private long _m_lOriStrValue;
        private long _m_lOriIntValue;
        private long _m_lOriPolValue;
        private long _m_lOriLeadValue;
        //新的属性值
        private long _m_lCurStrValue;
        private long _m_lCurIntValue;
        private long _m_lCurPolValue;
        private long _m_lCurLeadValue;

        public TipQueueDealer_Attr(PlayerAttrPropertyContainer _oriAttrPropertyContainer, PlayerAttrPropertyContainer _curAttrPropertyContainer)
        {
            _m_oriAttrPropertyContainer = _oriAttrPropertyContainer;
            _m_curAttrPropertyContainer = _curAttrPropertyContainer;

            QueueDealerTipsRefObj queueDealerTipsRefObj = GRefdataCoreMgr.instance.queueDealerTipMap.getRef((long)ETipQueueType.ATTR);
            if (queueDealerTipsRefObj != null)
                _m_queueTime = queueDealerTipsRefObj.space_time;

            // //旧的属性值
            // if (_m_oriAttrPropertyContainer != null)
            // {
            //     _m_lOriStrValue = _m_oriAttrPropertyContainer.getValue(EBasicAttrType.STR);
            //     _m_lOriIntValue = _m_oriAttrPropertyContainer.getValue(EBasicAttrType.INT);
            //     _m_lOriPolValue = _m_oriAttrPropertyContainer.getValue(EBasicAttrType.POL);
            //     _m_lOriLeadValue = _m_oriAttrPropertyContainer.getValue(EBasicAttrType.LEAD);
            // }
            //
            // //新的属性值
            // if(_m_curAttrPropertyContainer != null)
            // {
            //     _m_lCurStrValue = _m_curAttrPropertyContainer.getValue(EBasicAttrType.STR);
            //     _m_lCurIntValue = _m_curAttrPropertyContainer.getValue(EBasicAttrType.INT);
            //     _m_lCurPolValue = _m_curAttrPropertyContainer.getValue(EBasicAttrType.POL);
            //     _m_lCurLeadValue = _m_curAttrPropertyContainer.getValue(EBasicAttrType.LEAD);
            // }
        }

        /// <summary>
        /// 队列类型
        /// </summary>
        public override ETipQueueType tipType { get { return ETipQueueType.ATTR; } }
        /// <summary>
        /// 当队列里有多个相关类型tip时是否需要将展示的值合并，true的话需要重写_dealMarge
        /// </summary>
        public override bool needMarge { get { return true; } }
        /// <summary>
        /// 旧属性
        /// </summary>
        public PlayerAttrPropertyContainer oriAttrPropertyContainer { get => _m_oriAttrPropertyContainer; }
        /// <summary>
        /// 新属性
        /// </summary>
        public PlayerAttrPropertyContainer curAttrPropertyContainer { get => _m_curAttrPropertyContainer; }


        /// <summary>
        /// 开始处理展示
        /// </summary>
        protected override void _onStart()
        {
            // //开始展示tip
            // long showCount = 0;
            // if (_showAttrChgTip(EBasicAttrType.STR, _m_lOriStrValue, _m_lCurStrValue))
            //     showCount++;
            //
            // if (_showAttrChgTip(EBasicAttrType.INT, _m_lOriIntValue, _m_lCurIntValue))
            //     showCount++;
            //
            // if (_showAttrChgTip(EBasicAttrType.POL, _m_lOriPolValue, _m_lCurPolValue))
            //     showCount++;
            //
            // if (_showAttrChgTip(EBasicAttrType.LEAD, _m_lOriLeadValue, _m_lCurLeadValue))
            //     showCount++;
            //
            // ALCommonTaskController.CommonActionAddMonoTask(setDealDone, _m_queueTime * showCount);
        }

        /// <summary>
        /// 处理合并操作
        /// </summary>
        /// <param name="_dealerList"></param>
        protected override void _dealMarge(List<_ABaseTipQueueDealer> _dealerList)
        {
            if (_dealerList == null)
                return;

            for (int i = 0; i < _dealerList.Count; i++)
            {
                TipQueueDealer_Attr attrDealer = _dealerList[i] as TipQueueDealer_Attr;
                if(attrDealer == null)
                    continue;

                // //获取武力起始值
                // long oriStrValueTemp = attrDealer.oriAttrPropertyContainer.getValue(EBasicAttrType.STR);
                // if (_m_lOriStrValue > oriStrValueTemp)
                //     _m_lOriStrValue = oriStrValueTemp;
                //
                // long curStrValueTemp = attrDealer.curAttrPropertyContainer.getValue(EBasicAttrType.STR);
                // if (_m_lCurStrValue < curStrValueTemp)
                //     _m_lCurStrValue = curStrValueTemp;
                //
                // //获取智力起始值
                // long oriIntValueTemp = attrDealer.oriAttrPropertyContainer.getValue(EBasicAttrType.INT);
                // if (_m_lOriIntValue > oriIntValueTemp)
                //     _m_lOriIntValue = oriIntValueTemp;
                //
                // long curIntValueTemp = attrDealer.curAttrPropertyContainer.getValue(EBasicAttrType.INT);
                // if (_m_lCurIntValue < curIntValueTemp)
                //     _m_lCurIntValue = curIntValueTemp;
                //
                // //获取政治起始值
                // long oriPolValueTemp = attrDealer.oriAttrPropertyContainer.getValue(EBasicAttrType.POL);
                // if (_m_lOriPolValue > oriPolValueTemp)
                //     _m_lOriPolValue = oriPolValueTemp;
                //
                // long curPolValueTemp = attrDealer.curAttrPropertyContainer.getValue(EBasicAttrType.POL);
                // if (_m_lCurPolValue < curPolValueTemp)
                //     _m_lCurPolValue = curPolValueTemp;
                //
                // //获取统帅起始值
                // long oriLeadValueTemp = attrDealer.oriAttrPropertyContainer.getValue(EBasicAttrType.LEAD);
                // if (_m_lOriLeadValue > oriLeadValueTemp)
                //     _m_lOriLeadValue = oriLeadValueTemp;
                //
                // long curLeadValueTemp = attrDealer.curAttrPropertyContainer.getValue(EBasicAttrType.LEAD);
                // if (_m_lCurLeadValue < curLeadValueTemp)
                //     _m_lCurLeadValue = curLeadValueTemp;
            }
        }

        /// <summary>
        /// 完成处理的事件处理函数
        /// </summary>
        protected override void _onDealDone()
        {
        }

        //属性变化tip展示
        private bool _showAttrChgTip(EBasicAttrType _type, long _oriValue, long _curValue)
        {
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_type);
            if (_oriValue < _curValue)
            {
                long showValue = _curValue - _oriValue;
                long centerTipId = GRefdataCoreMgr.instance.npGeneral.hero_attr_chg_center_tip_id;
                // NPGUIAddSceneCenterTip.instance.showIconTextTip(basicAttrRef.icon, TextTranslate.instance.getLanguage(TransKeyConst.hero_attrAddValue_name_num, TextTranslate.instance.getLanguage(basicAttrRef.name), showValue), centerTipId);
                return true;
            }

            return false;
        }
    }
}