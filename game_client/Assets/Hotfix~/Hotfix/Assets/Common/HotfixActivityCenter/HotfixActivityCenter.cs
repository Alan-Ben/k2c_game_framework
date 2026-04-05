using ALPackage;
using CommonEnum;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class HotfixActivityCenter
    {
        /// <summary>
        /// 主工程调用，处理活动中心加载热更活动页面
        /// </summary>
        public static _AALBasicLoadUIWndBasicClass dealLoadActivityCenterPage(long _id, Transform _pageParent)
        {
            ActivityCenterRefObj activityCenterRef = GRefdataCoreMgr.instance.activityCenterRefCore.getRef(_id);
            if (activityCenterRef == null)
                return null;

            //目标窗口
            _AALBasicLoadUIWndBasicClass targetPage = null;

            //如果是活动类型
            if (activityCenterRef.activity_id > 0)
            {
                GActivityMainRefObj activityMainRefObj = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityCenterRef.activity_id);
                if (activityMainRefObj == null)
                {
                    Debug.LogError($"热更活动中心加载页面失败，活动id：{activityCenterRef.activity_id}，请确认活动是否存在");
                    return null;
                }

                //加载热更活动页面
                switch (activityMainRefObj.type_id)
                {
                    //万能活动
                    case ECommonActivityType.REGULAR_EVENT:
                        targetPage = new GGuiWndRegularEventMain(activityCenterRef.activity_id, _pageParent);
                        targetPage.load(() =>
                        {
                            if (targetPage == null)
                                return;

                            targetPage.showWnd();
                        });
                        break;
                    //三消活动
                    case ECommonActivityType.TILE_MATCH:
                        targetPage = new GGUIWndTileMatchMain(activityCenterRef.activity_id, _pageParent);
                        targetPage.load(() =>
                        {
                            if (targetPage == null)
                                return;

                            targetPage.showWnd();
                        });
                        break;
                    case ECommonActivityType.NUM_MERGE:
                        targetPage = new GGUIWndNumMergeMain(activityCenterRef.activity_id, _pageParent);
                        targetPage.load(() =>
                        {
                            if (targetPage == null)
                                return;

                            targetPage.showWnd();
                        });
                        break;
                }
            }
           
            return targetPage;
        }
    }
}