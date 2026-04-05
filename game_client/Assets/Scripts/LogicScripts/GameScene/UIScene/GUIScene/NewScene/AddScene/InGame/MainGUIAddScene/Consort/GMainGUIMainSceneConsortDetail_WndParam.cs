namespace GOE
{
    public partial class GMainGUIMainSceneConsortDetail : _IGGUIWndUnLockConsortMainDetailWndParam
        , _IGGUIWndUnLockConsortDetailBusinessPageParam
        , _IGGUIWndUnlockConsortDetailInteractionPageParam
        , _IGGUIWndUnLockConsortDetailHaloPageParam
    {
        #region _IGGUIWndUnLockConsortMainDetailWndParam
        
        public EUnLockConsortDetailWndTabType consortDetailWndSelectTabType { get; set; }

        public int consortDetailWndShowConsortIndex { get; set; }
        
        public _IGGUIWndUnLockConsortDetailBusinessPageParam consortDetailWndBusinessPageParam { get { return this; } }
        public _IGGUIWndUnlockConsortDetailInteractionPageParam consortDetailWndInteractionPageParam { get { return this; } }
        public _IGGUIWndUnLockConsortDetailHaloPageParam consortDetailWndHaloPageParam { get { return this; } }

        #endregion

        #region _IGGUIWndUnLockConsortDetailBusinessPageParam

        public long consortDetailBusinessPageSelectSkillId { get; set; }

        #endregion

        #region _IGGUIWndUnlockConsortDetailInteractionPageParam

        public EUnlockConsortDetailWndInteractionPageTabType interactionPageTabType { get; set; }

        public int interactionSendGiftPageSelectIndex { get; set; }

        #endregion

        #region _IGGUIWndUnLockConsortDetailHaloPageParam

        public int selectHaloLvl { get; set; }

        #endregion
    }
}