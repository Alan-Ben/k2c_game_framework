using ALPackage;
using UnityEngine;
using System;

namespace GOE
{
    /// <summary>
    /// 邮件列表item-- 常规
    /// </summary>
    public class GGUISubWndMailItemPrefab_Normal : GGUISubWndMailItemPrefab_Base<GGUISubMonoMailItemPrefab>
    {


        public GGUISubWndMailItemPrefab_Normal(long _uiPathId, Transform _parent)
            : base(_uiPathId, _parent)
        {
            
        }

        protected override void _onDiscardEx()
        {
            
        }

        protected override void _onSetDataInfo(GMailDataInfo _mailDataInfo)
        {
            
        }

        protected override string _getSender()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.mail_sender_str, _m_miMailDataInfo.getSender());
        }

        protected override string _getTitle()
        {
            if(null != _m_miMailDataInfo)
                return _m_miMailDataInfo.getTitle();
            return null;
        }

        protected override void _refreshWndEx()
        {
            
        }
    }
}
