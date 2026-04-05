using ALPackage;
using UnityEngine;
using _AMineShareItemViewData = GOE.GGUIWndMarsExploreMineShare._AMineShareItemViewData;

namespace GOE
{
    /// <summary>
    /// 矿分享列表GridItem子窗口
    /// 负责显示单个矿分享项的UI并触发按需数据加载
    /// </summary>
    public class GGUISubWndMarsExploreMineShareGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMarsExploreMineShareGridItem>
    {
        /// <summary>当前item的表现数据</summary>
        private _AMineShareItemViewData _m_data;
        /// <summary>刷新序列号，用于控制回调的有效性</summary>
        private int _m_refreshSerialize;
        /// <summary>矿图标窗口</summary>
        private NPGGuiWndTexture _m_iconWnd;


        public GGUISubWndMarsExploreMineShareGridItem(GGUIMonoMarsExploreMineShareGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }
        protected override void _resetGridItem()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.txtMineIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.txtMineIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }


        /// <summary>
        /// 刷新GridItem（由Grid调用）
        /// </summary>
        public void refreshWnd(_AMineShareItemViewData _data)
        {
            _m_data = _data;
            refreshWnd();
        }
        /// <summary>
        /// 刷新UI显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_data == null)
                return;

            // 生成新的刷新序列号
            int refreshSerialize = _m_refreshSerialize = ALSerializeOpMgr.next();

            // 设置加载状态
            wnd.setLoading(true);

            // 触发按需数据加载，使用闭包捕获序列号
            _m_data.loadData(() =>
            {
                // 检查序列号，确保是最新的刷新
                if (refreshSerialize != _m_refreshSerialize)
                    return;

                // 数据加载完成，刷新UI
                _refreshUI();
            });
        }
        

        /// <summary>
        /// 刷新UI显示（私有方法）
        /// </summary>
        private void _refreshUI()
        {
            if (wnd == null || !_m_bIsShow || _m_data == null)
                return;

            // 设置加载状态
            wnd.setLoading(false);

            // 显示矿的信息（配置数据）
            if (_m_data.mineRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtMineName, TextTranslate.instance.getLanguage(_m_data.mineRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtMineLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_data.mineRef.mine_lvl));
                _m_iconWnd?.setTexture(_m_data.mineRef.icon);
            }

            // 显示发现者名字
            if (_m_data.finderInfo != null)
                ALUGUICommon.setLabelTxt(wnd.txtOwnerName, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineShareFinderName_name, _m_data.finderInfo.getPlayerName()));

            // 显示占领者名字
            if (_m_data.occupierInfo != null)
                ALUGUICommon.setLabelTxt(wnd.txtOccupierName, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineShareOccupierName_name, _m_data.occupierInfo.getPlayerName()));

            // 显示剩余资源数量
            if (_m_data.mineDynamic != null)
            {
                long remainNum = _m_data.remainNum;
                long maxNum = _m_data.mineRef?.res_num ?? remainNum;
                ALUGUICommon.setLabelTxt(wnd.txtRemainResNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, remainNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), maxNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            }

            // 根据关系设置颜色（同联盟、好友、其他联盟）
            wnd.setColor(_m_data.isAlliance, _m_data.isFriend, _m_data.isOtherAlliance);
            wnd.setOccupied(_m_data.occupierInfo != null);
            wnd.setHadAttackedByOthers(_m_data.isAttackedByOthers);
        }
        /// <summary>
        /// 点击item，打开矿的详细信息窗口
        /// </summary>
        private void _onClickItem(GameObject _go)
        {
            if (_m_data?.mineDynamic == null)
                return;

            // 判断矿是否已经被自己占领
            if (_m_data.mineDynamic.getOccupiedCid() == NPPlayer.instance.playerInfo.CID)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineOccupiedBySelf_none);
                return;
            }
            
            if (_m_data.isAlliance)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineOccupiedByAlliance_none);
                return;
            }

            // 获取矿实例ID和分享矿数据库ID
            long mineInstanceId = _m_data.mineDynamic.getId();

            _AMineShareItemViewData data = _m_data;
            // 设置信息并传入派遣队伍的回调
            GGUIWndMarsExploreMineInfo.instance.refreshWnd(mineInstanceId, (_teamId) =>
            {
                data.forwardCollectMine(_teamId);
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_MINE_SHARE);
            });
            // 打开详细信息窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineInfo.instance, GGUIWndMarsExploreMineInfo.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_MINE_INFO);
        }
    }
}
