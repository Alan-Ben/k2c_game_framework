using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 直接展示玩家名字取名弹窗
        /// </summary>
        public static void showPlayerCommonRename()
        {
            //玩家改名资源id
            long renamePathId = 1706;
            bool needShowBg = true;
            _showCommonRename(renamePathId, needShowBg, NPPlayer.instance.playerInfo.PlayerName,
                GRefdataCoreMgr.instance.npGeneral.player_info_rename_cost_list,
                GRefdataCoreMgr.instance.npGeneral.player_info_rename_cost_range,
                new Func<string>(() => GRefdataCoreMgr.instance.getRandomInitName()),
                (_afterName) =>
                {
                    NPPlayer.instance.playerInfoComp.reqChangeName(_afterName, () =>
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.playerInfo_renameSuc_str) ;
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE);
                    });
                }, () =>
                {
                    //发送消息，进入玩家取名界面
                    WinMsg.SendMsg(WinMsgType.ON_PLAYER_RENAME_WND_SHOW);
                });
        }

        /// <summary>
        /// 直接展示玩家创角设置名字
        /// </summary>
        public static void showPlayerCreatName()
        {
            //进入创角界面埋点
            GCommon.sendStepReport(TraceConst.ENTER_GAME_CREATE);

            //玩家改名资源id
            long renamePathId = 1736;
            bool needShowBg = false;
            _showCommonRename(renamePathId, needShowBg, GRefdataCoreMgr.instance.getRandomInitName(),
                null,
                GRefdataCoreMgr.instance.npGeneral.player_info_rename_cost_range,
                new Func<string>(() => GRefdataCoreMgr.instance.getRandomInitName()),
                (_afterName) =>
                {
                    NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_008_ReqSetDefault(_afterName)
                        , new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_008_RetSetDefault>(
                            (_res) =>
                            {
                                //发生成功取名trigger
                                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CREATE_PLAYER_NAME_SCU);

                                //发送创角完成埋点
                                GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.ROLE);
            
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE);
                            }
                        )
                    );
                },null,false);
        }
        
        /// <summary>
        /// 判断名字是否可用
        /// </summary>
        /// <returns></returns>
        public static bool checkNameCanUse(string _nameStr,WCGIntRange _nameRange,bool _isShowTip,string _customNotInRangeKey)
        {
            //如果名字为空格
            if (string.IsNullOrWhiteSpace(_nameStr))
            {
                if (_isShowTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.playerinfo_renameEmpty_str);
                return false;
            }

            //如果长度不符合规则            
            if (!CharacterDetermineMgr.instance.isSuitableLength(_nameStr, _nameRange.min, _nameRange.max))
            {
                if (_isShowTip)
                {
                    if(!string.IsNullOrEmpty(_customNotInRangeKey))
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_customNotInRangeKey));
                    else
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_renameRange_num_num, _nameRange.min, _nameRange.max));
                }
                return false;
            }

            //是否屏蔽字符
            if (CharacterDetermineMgr.instance.isPlayerNameIllegal(_nameStr))
            {
                if (_isShowTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_renameIllegal_none));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 展示改名弹窗
        /// </summary>
        /// <param name="_renameAccessPathId">资源id</param>
        /// <param name="_currentName">当前名称</param>
        /// <param name="_costItemList">消耗列表 满足其中一个即可</param>
        /// <param name="_nameRange">字数范围</param>
        /// <param name="_getRandomNameFunc">取随机名称方法</param>
        /// <param name="_confirmAction">确认改名回调</param>
        /// <param name="_onShow">展示回调</param>
        /// <param name="_canRollBack">是否可以按回退按钮关闭</param>
        private static void _showCommonRename(long _renameAccessPathId, bool _needShowBg, string _currentName, List<NPCommonCostItem> _costItemList, WCGIntRange _nameRange, Func<string> _getRandomNameFunc, Action<string> _confirmAction, Action _onShow = null,bool _canRollBack = true)
        {
            QueueMgr.instance.AddNode(new GNodeCommonRename(_renameAccessPathId, _needShowBg, _currentName, _costItemList, _nameRange, _getRandomNameFunc, _confirmAction, _onShow, _canRollBack));
        }
    }
}