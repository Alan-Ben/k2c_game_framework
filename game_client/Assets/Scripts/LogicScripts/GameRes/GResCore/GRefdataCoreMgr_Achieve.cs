
namespace GOE
{
    //成就相关
    public partial class GRefdataCoreMgr
    {
        private void _initAchieveRefCore()
        {
            AchieveStepRefObj stepRef = null;
            for (int i = 0; i < achieveStepList.refList.Count; i++)
            {
                stepRef = achieveStepList.refList[i];
                if (null == stepRef)
                    continue;

                AchieveRefObj achieveRefObj = achieveMap.getRef(stepRef.achieve_id);
                if (null == achieveRefObj)
                    continue;

                achieveRefObj.addStepRef(stepRef);
            }
        }
    }
}