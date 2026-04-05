namespace GOE
{
    public class GNodeAdultMain : MainUIMainSceneNode_Var_InGame
    {
        public GNodeAdultMain()
            : base(GMainGUIAddSceneAdultMain.instance, GUISceneMain.instance, 0, UINodeTagConst_Adult.C_MAIN_ADULT_NODE)
        {
        }
        
        
        public override ENoticeType enableNoticeType { get { return ENoticeType.ADULT; } }
    }
}