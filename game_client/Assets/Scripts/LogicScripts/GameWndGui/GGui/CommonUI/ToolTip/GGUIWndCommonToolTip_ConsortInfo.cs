using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子信息跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_ConsortInfo : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ConsortInfo>
    {
        private long _m_consortId;
        private long _m_intimacy;
        private long _m_charm;

        public GGUIWndCommonToolTip_ConsortInfo(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_ConsortInfo));
        }
        
        public void setInfo(long _consortId, long _intimacy, long _charm, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _m_consortId = _consortId;
            _m_intimacy = _intimacy;
            _m_charm = _charm;
            _refreshWnd();
            setPos(_targetTransRoot,_intervalX, _intervalY);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_consortId);
            if (null == consortRefObj)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtName, consortRefObj.transName);
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, _m_intimacy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtCharm, _m_charm.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
        }
    }
}