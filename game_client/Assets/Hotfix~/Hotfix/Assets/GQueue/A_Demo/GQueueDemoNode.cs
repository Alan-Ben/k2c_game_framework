using GOE;

namespace Hotfix
{
    /// <summary>
    /// 测试的Node
    /// </summary>
    public class GQueueDemoNode : UIQueueBaseNode
    {
        public GQueueDemoNode(string _nodeTag) 
            : base(EUIQueueStageType.MAIN, _nodeTag)
        {
        }

        public override bool NeedAutoRemove { get { return false; } }

        public override bool IsMainViewNode { get { return true; } }

        public override void EnterNode()
        {
            GTDSceneMain.instance.showMainScene(GTDHotfixScene_Demo.instance);//进入主界面3D场景

            
            // if (NPGGuiDemoWnd.instance.isLoaded)
            // {
            //     NPGGuiDemoWnd.instance.showWnd();
            // }
            // else
            // {
            //     NPGGuiDemoWnd.instance.load(() =>
            //     {
            //         NPGGuiDemoWnd.instance.showWnd();
            //     });
            // }
        }

        public override void QuitNode()
        {
            // NPGGuiDemoWnd.instance.discard();
        }
    }
}