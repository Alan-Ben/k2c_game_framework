using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友添加分组
    /// </summary>
    public class GGUIWndFriendAddGruop :_ANPGGUIBasicWnd<GGUIMonoFriendAddGruop>
    {
        private static GGUIWndFriendAddGruop _g_instance;
        public static GGUIWndFriendAddGruop instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndFriendAddGruop();
                return _g_instance;
            }
        }
        
        private GGUIWndFriendGruopEditorFriendListGrid _m_friendListGrid;
        public GGUIWndFriendAddGruop() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoFriendAddGruop.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendAddGruop.objName; }
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
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnAddGroup, _onAddGroupClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseClick);
            if (null != wnd.groupNameInputField)
            {
                wnd.groupNameInputField.onValueChanged.AddListener(_onValueChanged);
            }
            if (null != wnd.friendListGrid)
            {
                _m_friendListGrid = new GGUIWndFriendGruopEditorFriendListGrid(wnd.friendListGrid);
            }
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onCloseClick(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        private void _onAddGroupClick(GameObject obj)
        {
            if(null == wnd)
                return;
            if (null == wnd.groupNameInputField)
                return;
            string groupName = wnd.groupNameInputField.text;
            if (string.IsNullOrEmpty(groupName))
            {
                //请输入分组名
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_name_empty_tip));
                return;
            }
            
            if (CharacterDetermineMgr.instance.getUnicodeStringLength(groupName) > GRefdataCoreMgr.instance.npGeneral.friend_group_name_char_limit)
            {
                //字符长度超过上限
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_name_char_count_max));
                return;
            }

            if (NPPlayer.instance.friendsComp.getCustomFriendGroupCount() >= NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.CUSTOM_FRIEND_GROUP_NUM))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_custom_group_count_max));
                return;
            }

            List<long> cidList = new List<long>();
            List<PlayerFriendItemData> selectedList = _m_friendListGrid?.getSelectedItemList();
            if (null != selectedList)
            {
                foreach (PlayerFriendItemData itemData in selectedList)
                {
                    cidList.Add(itemData.cid);
                }
            }
            NPPlayer.instance.friendsComp.reqCreateFriendGroup(groupName,cidList, (_info) =>
            {
                QueueMgr.instance.DoUIRollBackByEsc();
            });
        }

        private void _onValueChanged(string _text)
        {
            string curText = _text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref curText);
            ALUGUICommon.setInputTxt(wnd.groupNameInputField, curText);

            _showNameCharCount(curText);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if (null != _m_friendListGrid)
            {
                List<PlayerFriendItemData> itemList = new List<PlayerFriendItemData>();
                NPPlayer.instance.friendsComp.getFriendsDataList((_itemList) =>
                {
                    itemList.AddRange(_itemList);
                    _m_friendListGrid.showWnd();
                    _m_friendListGrid.showItemList(itemList);
                });
            }

            ALUGUICommon.setLabelTxt(wnd.txtGroupLimit,
                TextTranslate.instance.getLanguage(TransKeyConst.friends_group_count_limit, NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.CUSTOM_FRIEND_GROUP_NUM)));

            _showNameCharCount(wnd.groupNameInputField.text);
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