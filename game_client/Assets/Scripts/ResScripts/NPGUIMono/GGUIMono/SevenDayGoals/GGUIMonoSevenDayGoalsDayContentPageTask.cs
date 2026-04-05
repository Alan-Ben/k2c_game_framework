using ALPackage;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsDayContentPageTask : _AALBasicUIWndMono
    {
        [ALHeader("任务列表")]
        public GGUIMonoSevenDayGoalsDayContentTaskContainer monoTaskContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5801); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5801); } }
    }
}