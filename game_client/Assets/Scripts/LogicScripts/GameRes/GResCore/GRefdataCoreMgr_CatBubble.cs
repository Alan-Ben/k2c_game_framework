using System.Collections.Generic;
using UnityEngine.Pool;

namespace GOE
{
    //猫咪气泡相关
    public partial class GRefdataCoreMgr
    {
        public string getCatBubble()
        {
            List<string> bubbleList = ListPool<string>.Get();//使用对象池，避免频繁new
            for (int i = 0; i < catBubbleRefCore.refList.Count; i++)
            {
                CatBubbleRefObj refObj = catBubbleRefCore.refList[i];
                if (refObj == null)
                    continue;

                //展示条件是否达成
                if(refObj.show_condition != null && !refObj.show_condition.IsEnable(null))
                    continue;

                //无效条件是否达成
                if(refObj.invalid_condition != null && refObj.invalid_condition.IsEnable(null))
                    continue;

                if(refObj.bubble_list != null)
                    bubbleList.AddRange(refObj.bubble_list);
            }

            //如果没有满足条件的气泡文本，取默认气泡文本
            if(bubbleList.Count == 0 && GRefdataCoreMgr.instance.npGeneral.cat_bubble_default_list != null)
                bubbleList.AddRange(GRefdataCoreMgr.instance.npGeneral.cat_bubble_default_list);

            //获取随机气泡文本
            string targetBubble = null;
            if (bubbleList.Count > 0)
                targetBubble = bubbleList.GetRandomItem();

            //回收列表
            ListPool<string>.Release(bubbleList);

            return TextTranslate.instance.getLanguage(targetBubble);
        }
    }
}