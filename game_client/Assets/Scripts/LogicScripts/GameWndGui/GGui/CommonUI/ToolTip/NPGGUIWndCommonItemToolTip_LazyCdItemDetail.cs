using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndCommonItemToolTip_LazyCdItemDetail : _ATNPGGUIWndCommonItemToolTipCheckPosition<NPGGUIMonoCommonToolTip_LazyCdItemDetail>
    {
        private long _m_lItemId;//物品id
        private PlayerLazyCDInfo _m_lazyCdInfo;//CD信息
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片
        private ALCommonEnableTaskController _m_refreshTask;

        public NPGGUIWndCommonItemToolTip_LazyCdItemDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {

        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            _m_refreshTask.setDisable();
            _m_refreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshWithTime, 1f);
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_refreshTask.setDisable();

        }

        protected override void _onReset()
        {
            base._onReset();

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discardTexture();

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discard();
            _m_wTexIconWnd = null;

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discard();
            _m_wQualityWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.imgItemIcon != null)
            {
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.imgItemIcon);
            }

            if (wnd.imgQualityBg != null)
            {
                _m_wQualityWnd = new GGuiWndSprite(wnd.imgQualityBg);
            }
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_ItemDetail));
        }


        #region 窗体事件

        /// <summary>
        /// 刷新窗体显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_lazyCdInfo == null)
                return;

            //物品名称
            ALUGUICommon.setLabelTxt(wnd.txtItemName, GCommon.getItemName(ENPItemType.LAZY_CD, _m_lItemId));
            //获取途径
            ALUGUICommon.setLabelTxt(wnd.txtAccess, TextTranslate.instance.getLanguage(TransKeyConst.common_resource_production_tip, GCommon.getItemSource(ENPItemType.LAZY_CD, _m_lItemId)));
            //物品描述
            ALUGUICommon.setLabelTxt(wnd.txtItemDesc, GCommon.getItemDesc(ENPItemType.LAZY_CD, _m_lItemId));

            //物品图片
            if (_m_wTexIconWnd != null)
            {
                _m_wTexIconWnd.setTexture(GCommon.getItemTexIcon(ENPItemType.LAZY_CD, _m_lItemId));
                _m_wTexIconWnd.showWnd();
            }

            //品质底图
            if (_m_wQualityWnd != null)
            {
                _m_wQualityWnd.setTexture(GCommon.getItemQualityIcon(ENPItemType.LAZY_CD, _m_lItemId));
                _m_wQualityWnd.showWnd();
            }

            string txtAddPerNeedTimeKey = string.IsNullOrEmpty(wnd.txtAddPerNeedTimeKey)
                ? TransKeyConst.common_value : wnd.txtAddPerNeedTimeKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddPerNeedTime,
                TextTranslate.instance.getLanguage(txtAddPerNeedTimeKey, TimeUtil.millisecondsToTime_Two_NoSec(_m_lazyCdInfo.CdDurationMs)));
            
            _refreshWithTime();
        }
        
        /// <summary>
        /// 刷新随时间更新数据
        /// </summary>
        private void _refreshWithTime()
        {
            if(_m_lazyCdInfo == null || wnd == null)
                return;

            int cdCount = _m_lazyCdInfo.getCount();
            
            //持有数量
            string txtNumKey = string.IsNullOrEmpty(wnd.txtNumKey) ? TransKeyConst.common_resource_haveNum : wnd.txtNumKey;
            string txtNumStr = !wnd.needShowMaxCount ? cdCount.ToString() : 
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, cdCount, _m_lazyCdInfo.MaxCount);
            ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(txtNumKey, txtNumStr));
            
            //CD计数满时
            if (_m_lazyCdInfo.MaxCount <= cdCount)
            {
                ALUGUICommon.setGameObjEnable(wnd.cdCountFullShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.cdCountEmptyShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.cdCountNeitherFullNorEmptyShowList, false);
                
                ALUGUICommon.setLabelTxt(wnd.txtTimeNext, TextTranslate.instance.getLanguage(TransKeyConst.lazy_cd_max_desc, GCommon.getItemName(ENPItemType.LAZY_CD, _m_lItemId)));
                ALUGUICommon.setLabelTxt(wnd.txtTimeMax, TextTranslate.instance.getLanguage(TransKeyConst.lazy_cd_duration_desc, _m_lazyCdInfo.CdDurationMs / 1000, _m_lazyCdInfo.AddCountPerTime, GCommon.getItemName(ENPItemType.LAZY_CD, _m_lItemId)));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.cdCountFullShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.cdCountEmptyShowList, cdCount <= 0);
                ALUGUICommon.setGameObjEnable(wnd.cdCountNeitherFullNorEmptyShowList, cdCount > 0);
                
                string txtTimeNextKey = string.IsNullOrEmpty(wnd.txtTimeNextKey) ? TransKeyConst.lazy_cd_next_remain_desc : wnd.txtTimeNextKey;
                ALUGUICommon.setLabelTxt(wnd.txtTimeNext, TextTranslate.instance.getLanguage(txtTimeNextKey, TimeUtil.millisecondsToTime_Two(_m_lazyCdInfo.getRemainMs())));
                
                string txtTimeMaxKey = string.IsNullOrEmpty(wnd.txtTimeMaxKey) ? TransKeyConst.lazy_cd_max_remain_desc : wnd.txtTimeMaxKey;
                ALUGUICommon.setLabelTxt(wnd.txtTimeMax, TextTranslate.instance.getLanguage(txtTimeMaxKey, TimeUtil.millisecondsToTime_Two(_m_lazyCdInfo.getMaxRemainMs())));
            }
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_itemId"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        public void setShowData(long _itemId, RectTransform _targetTransRoot, float _interval)
        {
            _m_lItemId = _itemId;
            _m_lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(_m_lItemId);
            if (_m_lazyCdInfo == null)
            {
                Debug.LogError($"[NPGGUIWndCommonItemToolTip_LazyCdItemDetail setShowData] 无法从lazyCdComp中获取到cd:{_m_lItemId}的数据");
            }

            //刷新窗体
            _refreshWnd();

            //设置位置
            setPos(_targetTransRoot, _interval);
        }
        

        #endregion
    }
}
