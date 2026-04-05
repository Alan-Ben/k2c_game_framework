using ALPackage;
using Common.NpPlayerInfoObj;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家称号展示附加窗口
    /// </summary>
    public class GGUIWndSubPlayerTitle : _ATALBasicUISubWnd<GGUIMonoSubPlayerTitle>
    {
        //加载的item
        private GGUIWndPrefabSubDressItem _m_wLoadItem;

        public GGUIWndSubPlayerTitle(GGUIMonoSubPlayerTitle _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wLoadItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLoadItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wLoadItem?.discard();
            _m_wLoadItem = null;
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置当前称号信息
        /// </summary>
        public void setInfo(PlayerInfo_CurTitle _curTitle)
        {
            if (wnd == null)
                return;

            //如果没有佩戴，清空展示
            if (_curTitle == null || _curTitle.getType() == ENPPlayerTitleType.NONE)
            {
                _m_wLoadItem?.discard();
                _m_wLoadItem = null;
                ALUGUICommon.setLabelTxt(wnd.txtComboTitle, "");
                return;
            }

            if (_curTitle.getType() == ENPPlayerTitleType.COMBO)
            {
                PlayerInfo_ComboTitle comboTitle = new PlayerInfo_ComboTitle();
                comboTitle.readPackage(_curTitle.getInfo());
                setInfo(comboTitle);
            }
            else if(_curTitle.getType() == ENPPlayerTitleType.COMMON)
            {
                PlayerInfo_Title comTitle = new PlayerInfo_Title();
                comTitle.readPackage(_curTitle.getInfo());
                setInfo(comTitle);
            }
        }

        /// <summary>
        /// 设置组合称号信息
        /// </summary>
        public void setInfo(PlayerInfo_ComboTitle _comboTitle)
        {
            if (wnd == null)
                return;

            //先清除已加载的item
            _m_wLoadItem?.discard();
            _m_wLoadItem = null;

            if (_comboTitle == null)
            {
                //重置文本
                ALUGUICommon.setLabelTxt(wnd.txtComboTitle, "");
                return;
            }

            //设置文本
            ALUGUICommon.setLabelTxt(wnd.txtComboTitle, GCommon.getPlayerComboTitleString(_comboTitle.getPreId(), _comboTitle.getSfxId(), _comboTitle.getBgId()));

            //设置底图
            long bgId = _comboTitle.getBgId();
            //如果没有设置组合称号底图，读取默认底图
            if (bgId <= 0)
                bgId = GRefdataCoreMgr.instance.npGeneral.default_player_combo_title_bg;
            PlayerTitleBgRefObj bgRef = GRefdataCoreMgr.instance.playerTitleBgRefCore.getRef(bgId);
            if (bgRef == null)
                return;

            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wLoadItem, bgRef.asset_path_id, wnd.transParent, (_prefabItem) =>
            {
                _m_wLoadItem = _prefabItem;
                _m_wLoadItem?.showWnd();
                _m_wLoadItem?.setIcon(ENPItemType.TITLE_BG, bgId);
            });
        }

        /// <summary>
        /// 设置固定称号信息
        /// </summary>
        public void setInfo(PlayerInfo_Title _comTitle)
        {
            if (wnd == null || _comTitle == null)
                return;
            setInfo(_comTitle.getId(), _comTitle.getGainCount());
        }

        /// <summary>
        /// 设置固定称号信息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_gainCount"></param>
        public void setInfo(long _id, int _gainCount)
        {
            //先清除已加载的item
            _m_wLoadItem?.discard();
            _m_wLoadItem = null;
            //重置文本
            ALUGUICommon.setLabelTxt(wnd?.txtComboTitle, "");

            long uiResId = GCommon.getPlayerTitleUIResId(_id, _gainCount);
            if (uiResId <= 0)
                return;

            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wLoadItem, uiResId, wnd.transParent, (_prefabItem) =>
            {
                _m_wLoadItem = _prefabItem;
                _m_wLoadItem?.showWnd();
                _m_wLoadItem?.setIcon(ENPItemType.TITLE, _id);
                _m_wLoadItem?.setName(GCommon.getItemName(ENPItemType.TITLE, _id));
            });
        }
    }
}
