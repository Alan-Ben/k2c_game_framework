
namespace GOE
{
    public static partial class GCommon
    {
        //建筑建造过程中输入屏蔽序列化值
        private static int _m_iBuildingConstructionMaskSerialize;

        /// <summary>
        /// 建筑正在建造状态设置
        /// </summary>
        public static void setBuildingIsUnderConstruction(bool _isUnderConstruction, long _sfxId)
        {
            //根据是否开始建造，屏蔽或恢复输入，并设置UI显示状态
            if (_isUnderConstruction)
            {
                _m_iBuildingConstructionMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                GMainGUIAddSceneBuilding.instance.hideWndsGO();
                GGUIWndMain.instance.hideWnd();
                TipQueueMgr.instance.pauseShowType(ETipQueueType.NATION_POWER);
                NPPlayer.instance.buildingComp.addUnderConstructionBuildingSfxId(_sfxId);
            }
            else
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_iBuildingConstructionMaskSerialize);
                GMainGUIAddSceneBuilding.instance.showWndsGO();
                GGUIWndMain.instance.showWnd();
                TipQueueMgr.instance.resumeShowType(ETipQueueType.NATION_POWER);
                NPPlayer.instance.buildingComp.removeUnderConstructionBuildingSfxId(_sfxId);
            }
            WinMsg.SendMsg(WinMsgType.IS_BUILDING_UNDER_CONSTRUCTION, _isUnderConstruction);
        }
    }
}
