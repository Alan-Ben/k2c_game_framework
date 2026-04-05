using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠进度说明 跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_GuildDonateProgressSpecification : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_GuildDonateProgressSpecification>
    {
        private NPGGUIWndCommonTextItemGrid _m_wDonateTypeGrid;
        
        public GGUIWndCommonToolTip_GuildDonateProgressSpecification(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
            
            if(wnd.donateTypeGrid != null)
                _m_wDonateTypeGrid = new NPGGUIWndCommonTextItemGrid(wnd.donateTypeGrid);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_wDonateTypeGrid?.discard();
            _m_wDonateTypeGrid = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
            
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wDonateTypeGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wDonateTypeGrid?.resetWnd();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            if(null == wnd)
                return;

            if (_m_wDonateTypeGrid != null)
            {
                List<CommonTextItemStruct> _itemStructList = new List<CommonTextItemStruct>();
                foreach (var item in GRefdataCoreMgr.instance.guildConstructRefCore.refList)
                {
                    if (item != null)
                    {
                        string desc = "";
                        if (string.IsNullOrEmpty(wnd.descKey))
                        {
                            desc = TextTranslate.instance.getLanguage(TransKeyConst.common_str_colon_str, item.name,
                                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                                    item.add_reward_point));
                        }
                        else
                        {
                            desc = TextTranslate.instance.getLanguage(wnd.descKey, item.name, item.add_reward_point);
                        }
                        
                        _itemStructList.Add(new CommonTextItemStruct(){needShowGo = false, strOne = desc, strTwo = desc});
                    }
                }
                
                _m_wDonateTypeGrid.showWnd();
                _m_wDonateTypeGrid.setItemList(_itemStructList);
            }

            //设置位置
            setPos(_targetTransRoot, _intervalX,_intervalY);
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_GuildDonateProgressSpecification));
        }
    }
}