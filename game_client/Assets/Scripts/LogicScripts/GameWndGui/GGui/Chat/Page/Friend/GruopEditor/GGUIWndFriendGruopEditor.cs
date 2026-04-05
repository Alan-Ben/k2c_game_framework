using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友分组编辑
    /// </summary>
    public class GGUIWndFriendGruopEditor:_ANPGGUIBasicWnd<GGUIMonoFriendGruopEditor>
    {
        private static GGUIWndFriendGruopEditor _g_instance;
        public static GGUIWndFriendGruopEditor instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndFriendGruopEditor();
                return _g_instance;
            }
        }

        private GGUIWndFriendGruopEditorFriendListGrid _m_friendListGrid;
        private GGUIWndFriendGruopEditorGroupItemContainer _m_friendGroupItemContainer;
        private PlayerFriendGroup _m_curGroupItem;
        private NPGGUIWndCommonToggleEx _m_togMoveFriend;

        public GGUIWndFriendGruopEditor() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoFriendGruopEditor.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendGruopEditor.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_friendListGrid?.discard();
            _m_friendListGrid = null;
            
            _m_friendGroupItemContainer?.discard();
            _m_friendGroupItemContainer = null;

            if (null != _m_togMoveFriend)
            {
                _m_togMoveFriend.clickDelegate -= _clickMoveFriend;
                _m_togMoveFriend?.discard();   
            }
            _m_togMoveFriend = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseClick);
            if (null != wnd.togMoveFriend)
            {
                _m_togMoveFriend = new NPGGUIWndCommonToggleEx(wnd.togMoveFriend);
                _m_togMoveFriend.clickDelegate += _clickMoveFriend;
            }
            ALUGUICommon.combineBtnClick(wnd.btnCloseGroupContainer, _closeGroupContainer);
            ALUGUICommon.combineBtnClick(wnd.btnRemoveGroup, _clickRemoveGroup);
            ALUGUICommon.combineBtnClick(wnd.btnChgGroupName, _clickChgGroupName);

            if (null != wnd.groupNameInputField)
            {
                wnd.groupNameInputField.onValueChanged.AddListener(_onValueChanged);
            }

            if (null != wnd.friendListGrid)
            {
                _m_friendListGrid = new GGUIWndFriendGruopEditorFriendListGrid(wnd.friendListGrid);
                _m_friendListGrid.onSelectedItemChg += _refreshSelectedCount;
            }

            if (null != wnd.groupItemContainer)
            {
                _m_friendGroupItemContainer = new GGUIWndFriendGruopEditorGroupItemContainer(wnd.groupItemContainer);
                _m_friendGroupItemContainer.clickDelegate += _clickGroupItem;
            }
        }

        /// <summary>
        /// 点击关闭分组列表
        /// </summary>
        /// <param name="obj"></param>
        private void _closeGroupContainer(GameObject obj)
        {
            _refreshGroupContainer(false);
        }

        private void _onValueChanged(string _text)
        {
            string curText = _text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref curText);
            ALUGUICommon.setInputTxt(wnd.groupNameInputField, curText);
            
            _showNameCharCount(curText);
        }
        
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onCloseClick(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        
        /// <summary>
        /// 选中移动的分组
        /// </summary>
        /// <param name="_group"></param>
        private void _clickGroupItem(PlayerFriendGroup _group)
        {
            if(null == wnd)
                return;
            if(null == _m_friendListGrid)
                return;
            List<PlayerFriendItemData> itemList = _m_friendListGrid.getSelectedItemList();
            if (null == itemList || itemList.Count == 0) //没有选中好友
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friend_editgroup_move_none_tip));
                _refreshGroupContainer(false);
                return;
            }
            List<long> cidList = new List<long>();
            foreach (PlayerFriendItemData itemData in itemList)
            {
                if(null == itemData)
                    continue;
                cidList.Add(itemData.cid);
            }

            //发消息移动好友到分组_group
            NPPlayer.instance.friendsComp.reqChgBelongFriendGroup(cidList, _group.dbId, (_info) =>
            {
                //换完分组，刷新界面
                _refreshWnd();
            });
        }

        /// <summary>
        /// 点击移动好友按钮，弹出可选的分组列表
        /// </summary>
        /// <param name="obj"></param>
        private void _clickMoveFriend(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            _refreshGroupContainer(!_toggleWnd.isOn);
        }

        /// <summary>
        /// 刷新分组列表显示
        /// </summary>
        /// <param name="_isShowContainer"></param>
        private void _refreshGroupContainer(bool _isShowContainer)
        {
            if (!_isShowContainer)
            {
                _m_friendGroupItemContainer?.hideWnd();
                _m_togMoveFriend?.setSelected(false);
            }
            else
            {
                List<PlayerFriendGroup> groupList = new List<PlayerFriendGroup>();
                NPPlayer.instance.friendsComp.getFriendGroupList(groupList, _filtGroup);
                if (groupList.Count == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_need_add_tip));
                    return;
                }
                
                if (null != _m_friendGroupItemContainer)
                {
                    _m_friendGroupItemContainer.showWnd();
                    _m_friendGroupItemContainer.showItemList(groupList);
                }
                _m_togMoveFriend?.setSelected(true);
            }
        }

        /// <summary>
        /// 是否过滤分组
        /// </summary>
        /// <param name="_group"></param>
        /// <returns></returns>
        private bool _filtGroup(PlayerFriendGroup _group)
        {
            //看是否当前选中的分组
            return (_group.dbId == _m_curGroupItem.dbId);
        }

        private void _clickRemoveGroup(GameObject obj)
        {
            if (!_m_curGroupItem.getCanRemove())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_default_group_cannot_remove));
                return;
            }
            //二次确认弹窗，点击确认移除
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_del_tip_desc)
                ,TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                ,null
                ,TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                , () =>
                {
                    NPPlayer.instance.friendsComp.reqDeleteFriendGroup(_m_curGroupItem.dbId, (_info) =>
                    {
                        QueueMgr.instance.DoUIRollBackByEsc();
                    });
                });
        }

        private void _clickChgGroupName(GameObject obj)
        {
            if(null == wnd)
                return;
            if(null == wnd.groupNameInputField)
                return;
            if (wnd.groupNameInputField.text == _m_curGroupItem.getGroupName()) //名字没变化
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_name_noChg));
                return;
            }
            if (!_m_curGroupItem.getCanChgName())
            {
                ALUGUICommon.setInputTxt(wnd.groupNameInputField, _m_curGroupItem.getGroupName());
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_default_group_cannot_chg_name));
                return;
            }
            if (CharacterDetermineMgr.instance.getUnicodeStringLength(wnd.groupNameInputField.text) > GRefdataCoreMgr.instance.npGeneral.friend_group_name_char_limit)
            {
                //字符长度超过上限
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_name_char_count_max));
                return;
            }

            //发消息换名字
            NPPlayer.instance.friendsComp.reqChgFriendGroupName(_m_curGroupItem.dbId, wnd.groupNameInputField.text, (_info) =>
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friend_editgroup_rename_tip));
                _refreshGroupName();
            });
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_group"></param>
        public void setInfo(PlayerFriendGroup _group)
        {
            _m_curGroupItem = _group;
            _refreshWnd();
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(null == _m_curGroupItem)
                return;
            if (null != _m_friendListGrid)
            {
                List<PlayerFriendItemData> itemList = new List<PlayerFriendItemData>();
                
                NPPlayer.instance.friendsComp.getFriendsDataList((_itemList) =>
                {
                    if (wnd == null || _itemList == null || _m_curGroupItem == null || _m_curGroupItem.playerList == null)
                        return;

                    foreach (PlayerFriendItemData itemData in _itemList)
                    {
                        if (_m_curGroupItem.playerList.Contains(itemData.cid))//分组中的
                        {
                            itemList.Add(itemData);
                        }
                    }
                    _m_friendListGrid?.showWnd();
                    _m_friendListGrid?.showItemList(itemList);
                },false);
            }

            //默认需要显隐的GO
            ALUGUICommon.setGameObjEnable(wnd.goDefaultShowList, _m_curGroupItem.isDefault);
            ALUGUICommon.setGameObjEnable(wnd.goDefaultHideList, !_m_curGroupItem.isDefault);

            _refreshGroupContainer(false);
            _refreshGroupName();
            _refreshSelectedCount(0);
        }
        
        /// <summary>
        /// 刷新选中的数量
        /// </summary>
        /// <param name="_count"></param>
        private void _refreshSelectedCount(int _count)
        {
            //从配置获取
            int maxSelectedFriendCount = 10;
            ALUGUICommon.setLabelTxt(wnd.txtSelectedCount, TextTranslate.instance.getLanguage( TransKeyConst.friends_group_edi_friend_count_num, _count, maxSelectedFriendCount));
        }

        /// <summary>
        /// 刷新分组名字
        /// </summary>
        private void _refreshGroupName()
        {
            if (null == wnd)
                return;
            if (null == _m_curGroupItem)
                return;

            //设置文本框默认分组不可编辑
            if(wnd.groupNameInputField != null)
                wnd.groupNameInputField.interactable = !_m_curGroupItem.isDefault;
            ALUGUICommon.setInputTxt(wnd.groupNameInputField, _m_curGroupItem.getGroupName());
            _showNameCharCount(_m_curGroupItem.getGroupName());
        }
        
        private void _showNameCharCount(string _groupName)
        {
            int charLength = CharacterDetermineMgr.instance.getUnicodeStringLength(_groupName);
            ALUGUICommon.setLabelTxt(wnd.txtGroupNameCharCount,
                TextTranslate.instance.getLanguage(TransKeyConst.friends_group_name_char_count,  charLength, GRefdataCoreMgr.instance.npGeneral.friend_group_name_char_limit));

            if(charLength <= GRefdataCoreMgr.instance.npGeneral.friend_group_name_char_limit)
            {
                ALUGUICommon.setUIObjColor(wnd.txtGroupNameCharCount, wnd.charEmoughColor);
            }
            else
            {
                ALUGUICommon.setUIObjColor(wnd.txtGroupNameCharCount, wnd.charMaxColor);
            }

        }
    }
}