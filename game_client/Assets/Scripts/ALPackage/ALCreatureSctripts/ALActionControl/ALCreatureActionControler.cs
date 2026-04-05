using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureActionControler
    {
        private _AALBasicCreatureControl creatureControl;

        /** 计算操作序列号的存储对象 */
        private int actionSerialize;

        /** 存储action的队列 */
        private List<ALBaseCreatureActionObj> actionList;

        /** 本对象的所有动作的播放速度 */
        private float _m_fCreatureActionTimeScale;

        public ALCreatureActionControler(_AALBasicCreatureControl _creature)
        {
            creatureControl = _creature;

            actionSerialize = 1;

            actionList = new List<ALBaseCreatureActionObj>();

            _m_fCreatureActionTimeScale = 1f;
        }

        public float creatureActionTimeScale { get { return _m_fCreatureActionTimeScale; } }
        public bool isActionEmpty { get { return actionList.Count <= 0; } }

        /*********************
         * 设置本角色控制对象的行为播放速度
         **/
        public void setCreatureActionTimeScale(float _timeScale)
        {
            if (null == actionList)
                return;

            //设置播放速度
            _m_fCreatureActionTimeScale = _timeScale;

            //逐个刷新播放速度
            for (int i = 0; i < actionList.Count; i++)
            {
                actionList[i].refreshTimeScale();
            }
        }

        /**************************
         * 获取对应序列号的行为对象
         **/
        public ALBaseCreatureActionObj getActionObj(int _serialize)
        {
            if (null == actionList)
                return null;

            for (int i = 0; i < actionList.Count; i++)
            {
                if (actionList[i].getSerialize() == _serialize)
                    return actionList[i];
            }

            return null;
        }

        /*****************
         * 添加行为
         **/
        public ALBaseCreatureActionObj addAction(ALSOCreatureActionInfo _actionInfo, float _actionSpeed)
        {
            return _addAction(_actionInfo, _actionSpeed);
        }

        /*****************
         * 添加状态机的下属行为
         **/
        public ALCreatureStateActionObj addAction(_AALBaseCharacterActionState _parentState, ALSOCreatureActionInfo _actionInfo, float _actionSpeed)
        {
            return _addAction(_parentState, _actionInfo, _actionSpeed);
        }

        /****************
         * 移除指定的Action
         **/
        public void removeAction(ALBaseCreatureActionObj _actionObj)
        {
            if (null == actionList)
                return ;

            //if action object is not in the list then return
            if (!actionList.Remove(_actionObj))
                return;

            _actionObj.discard();
        }

        /****************
         * 清除所有动作
         **/
        public void clearAction()
        {
            if (null == actionList)
                return;

            List<ALBaseCreatureActionObj> tmpList = new List<ALBaseCreatureActionObj>();
            tmpList.AddRange(actionList);

            ALBaseCreatureActionObj action = null;
            for (int i = 0; i < tmpList.Count; i++)
            {
                action = tmpList[i];
                if (null == action)
                    continue;

                action.discard();
            }

            tmpList.Clear();
        }

        /******************
         * 释放所有的行为对象
         **/
        public void discard()
        {
            if (null == actionList)
                return ;

            List<ALBaseCreatureActionObj> tmpList = actionList;
            actionList = null;

            ALBaseCreatureActionObj action = null;
            for (int i = 0; i < tmpList.Count; i++)
            {
                action = tmpList[i];
                if (null == action)
                    continue;

                action.discard();
            }

            tmpList.Clear();
        }

        /*****************
         * 添加行为到行为控制对象中
         **/
        protected ALBaseCreatureActionObj _addAction(ALSOCreatureActionInfo _actionInfo, float _actionSpeed)
        {
            if (null == actionList)
                return null;

            if (null == _actionInfo)
                return null;

            //create a new object
            ALBaseCreatureActionObj actionObj = new ALBaseCreatureActionObj(creatureControl, __getNewActionSerialize(), _actionSpeed, _actionInfo);

            //init action
            actionObj.initAction();

            actionList.Add(actionObj);

            return actionObj;
        }

        /*****************
         * 添加行为到行为控制对象中
         **/
        protected ALCreatureStateActionObj _addAction(_AALBaseCharacterActionState _parentState, ALSOCreatureActionInfo _actionInfo, float _actionSpeed)
        {
            if (null == actionList)
                return null;

            if (null == _actionInfo)
                return null;

            //create a new object
            ALCreatureStateActionObj actionObj = new ALCreatureStateActionObj(_parentState, __getNewActionSerialize(), _actionSpeed, _actionInfo);

            //init action
            actionObj.initAction();

            actionList.Add(actionObj);

            return actionObj;
        }

        /******************
         * 获取新的操作序列号
         **/
        private int __getNewActionSerialize()
        {
            return actionSerialize++;
        }
    }
}
#endif
