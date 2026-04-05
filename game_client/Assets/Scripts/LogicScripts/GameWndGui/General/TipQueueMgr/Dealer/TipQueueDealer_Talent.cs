using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 资质提示队列信息
    /// </summary>
    public class TipQueueDealer_Talent : _ABaseTipQueueDealer
    {
        //队列销毁接力时间
        private float _m_queueTime = 0.25f;
        //类型
        private EBasicAttrType _m_eType;
        //增加的值
        private long _m_lAddValue;

        public TipQueueDealer_Talent(EBasicAttrType _type, long _addValue)
        {
            _m_eType = _type;
            _m_lAddValue = _addValue;
            QueueDealerTipsRefObj queueDealerTipsRefObj = GRefdataCoreMgr.instance.queueDealerTipMap.getRef((long)ETipQueueType.TALENT);
            if (queueDealerTipsRefObj != null)
                _m_queueTime = queueDealerTipsRefObj.space_time;
        }

        /// <summary>
        /// 队列类型
        /// </summary>
        public override ETipQueueType tipType { get { return ETipQueueType.TALENT; } }
        /// <summary>
        /// 当队列里有多个相关类型tip时是否需要将展示的值合并，true的话需要重写_dealMarge
        /// </summary>
        public override bool needMarge { get { return false; } }

        /// <summary>
        /// 开始处理展示
        /// </summary>
        protected override void _onStart()
        {
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_eType);
            // NPGUIAddSceneCenterTip.instance.showIconTextTip(basicAttrRef.icon, TextTranslate.instance.getLanguage(TransKeyConst.hero_attrAddValue_name_num, _getTalentAttrName(), _m_lAddValue));
            ALCommonTaskController.CommonActionAddMonoTask(setDealDone, _m_queueTime);
        }

        /// <summary>
        /// 完成处理的事件处理函数
        /// </summary>
        protected override void _onDealDone()
        {
        }


        //获取资质类型名
        private string _getTalentAttrName()
        {
            switch (_m_eType)
            {
                // case EBasicAttrType.STR:
                //     return TextTranslate.instance.getLanguage(TransKeyConst.hero_strTalent_none);//武力资质
                // case EBasicAttrType.INT:
                //     return TextTranslate.instance.getLanguage(TransKeyConst.hero_intTalent_none);//智力资质
                // case EBasicAttrType.POL:
                //     return TextTranslate.instance.getLanguage(TransKeyConst.hero_polTalent_none);//政治资质
                // case EBasicAttrType.LEAD:
                //     return TextTranslate.instance.getLanguage(TransKeyConst.hero_leadTalent_none);//统帅资质
            }

            return null;
        }
    }
}