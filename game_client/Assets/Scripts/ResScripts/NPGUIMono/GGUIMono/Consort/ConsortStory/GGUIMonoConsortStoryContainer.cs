using ALPackage;

namespace GOE
{
    public class GGUIMonoConsortStoryContainer : _TALUGUIMonoContainerWnd<GGUIMonoConsortStoryGridItem>
    {
        [ALHeader("有CG的item模板")]
        public GGUIMonoConsortStoryGridItem monoHasCGItemTemplate;
        
        [ALHeader("无CG的item模板")]
        public GGUIMonoConsortStoryGridItem monoNoCGItemTemplate;

        [ALHeader("bar模板")]
        public GGUIMonoConsortStoryBar monoBar;
    }
}