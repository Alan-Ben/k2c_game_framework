
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIMonoMain : _AALBasicUIWndMono
    {
        public List<GGUIMonoMainFunctionTab> functionTabList;
        
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_MAIN_FUNCTION_SELECT); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_MAIN_FUNCTION_SELECT);} }        
    }
}