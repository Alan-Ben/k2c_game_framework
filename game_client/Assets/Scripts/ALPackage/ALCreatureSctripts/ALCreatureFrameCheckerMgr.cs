using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureFrameCheckerMgr
    {
        private _AALBasicCreatureControl creatureControl;

        /** 每帧需要检测的对象列表 */
        private List<_IALFrameChecker> checkerList;

        /** 每帧结束后需要触发的特殊事件，每次遍历执行后清除列表 */
        private List<_IALFrameLateChecker> laterCheckerList;

        public ALCreatureFrameCheckerMgr(_AALBasicCreatureControl _creature)
        {
            creatureControl = _creature;

            checkerList = new List<_IALFrameChecker>();

            laterCheckerList = new List<_IALFrameLateChecker>();
        }

        /**************
         * 注册一个处理对象
         **/
        public void regChecker(_IALFrameChecker _checker)
        {
            checkerList.Add(_checker);
        }

        /*****************
         * 进行处理
         **/
        public void dealCheck()
        {
            for (int i = 0; i < checkerList.Count; )
            {
                _IALFrameChecker checker = checkerList[i];

                if (null == checker)
                    continue;

                checker.update(creatureControl);

                //判断处理对象是否有效，无效则从列表删除
                if (checker.checkerEnable())
                {
                    i++;
                }
                else if (checkerList.Count > i && checkerList[i] == checker)
                {
                    checkerList.RemoveAt(i);
                }
            }
        }

        /**************
         * 注册一个处理对象
         **/
        public void regLaterChecker(_IALFrameLateChecker _checker)
        {
            laterCheckerList.Add(_checker);
        }

        /******************
         * 移除一个处理对象
         **/
        public void unregLaterChecker(_IALFrameLateChecker _checker)
        {
            laterCheckerList.Remove(_checker);
        }

        /*****************
         * 进行处理
         **/
        public void dealLaterCheck()
        {
            for (int i = 0; i < laterCheckerList.Count; )
            {
                _IALFrameLateChecker checker = laterCheckerList[i];

                if (null == checker)
                    continue;

                checker.lateUpdate(creatureControl);

                //判断处理对象是否有效，无效则从列表删除
                if (checker.laterCheckerEnable())
                {
                    i++;
                }
                else
                {
                    if (laterCheckerList.Count > i && laterCheckerList[i] == checker)
                        laterCheckerList.RemoveAt(i);
                }
            }
        }

        /*****************
         * 清空需要处理的队列
         **/
        public void discard()
        {
            checkerList.Clear();
            laterCheckerList.Clear();
        }
    }
}
#endif
