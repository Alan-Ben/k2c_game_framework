using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureAnimationControler
    {
        private _AALBasicCreatureControl parent;

        private int _m_iCreatureAnimationSerialize;

        /** 当前有效的动作片段对象集合 */
        private Dictionary<int, ALCreatureAnimationSession> _m_dAnimationSessionDic;

        /** 已经无效需要检测的动作片段集合 */
        private List<ALCreatureAnimationSession> _m_lDisableAnimationSessionList;

        public ALCreatureAnimationControler(_AALBasicCreatureControl _parent)
        {
            parent = _parent;

            _m_iCreatureAnimationSerialize = 1;

            _m_dAnimationSessionDic = new Dictionary<int, ALCreatureAnimationSession>();

            _m_lDisableAnimationSessionList = new List<ALCreatureAnimationSession>();
        }

        /**************
         * 添加动作组
         **/
        public ALCreatureAnimationSession addAnimation(ALSOBaseAnimationInfo _animationInfo, float _playSpeed)
        {
            if (null == _animationInfo)
                return null;

            //创建动作Session对象
            ALCreatureAnimationSession animationSession = new ALCreatureAnimationSession(parent, getAnimationSerialize(), _playSpeed, _animationInfo);

            animationSession.initAnimationList();

            //add into dictionary
            _m_dAnimationSessionDic.Add(animationSession.getSerialize(), animationSession);

            return animationSession;
        }
        public ALCreatureMoveStateAnimationSession addMoveStateAnimation(_AALBaseCharacterMoveState _moveState, ALSOBaseAnimationInfo _animationInfo, float _playSpeed)
        {
            if (null == _moveState || null == _animationInfo)
                return null;

            //创建动作Session对象
            ALCreatureMoveStateAnimationSession animationSession = new ALCreatureMoveStateAnimationSession(_moveState, getAnimationSerialize(), _playSpeed, _animationInfo);

            animationSession.initAnimationList();

            //add into dictionary
            _m_dAnimationSessionDic.Add(animationSession.getSerialize(), animationSession);

            return animationSession;
        }
        public ALCreatureActionAnimationSession addActionAnimation(ALSOBaseAnimationInfo _animationInfo, ALBaseCreatureActionObj _parentActionObj)
        {
            if (null == _animationInfo)
                return null;

            //创建动作Session对象
            ALCreatureActionAnimationSession animationSession = new ALCreatureActionAnimationSession(_parentActionObj, getAnimationSerialize(), _animationInfo);

            animationSession.initAnimationList();

            //add into dictionary
            _m_dAnimationSessionDic.Add(animationSession.getSerialize(), animationSession);

            return animationSession;
        }

        /**************
         * 删除动作组
         **/
        public void removeAnimation(ALCreatureAnimationSession _session)
        {
            if (null == _session)
                return;

            removeAnimation(_session.getSerialize());
        }
        public void removeAnimation(int _serialize)
        {
            if (!_m_dAnimationSessionDic.ContainsKey(_serialize))
                return;

            ALCreatureAnimationSession session = _m_dAnimationSessionDic[_serialize];

            //remove all the animations in the session
            session.disableAnimation();

            //remove from animation session
            _m_dAnimationSessionDic.Remove(_serialize);

            //add into disable list
            _m_lDisableAnimationSessionList.Add(session);
        }

        /**********************
         * 删除所有动作对象
         **/
        public void discard()
        {
            //释放仍然有效资源
            foreach (ALCreatureAnimationSession session in _m_dAnimationSessionDic.Values)
            {
                session.discard();
            }
            _m_dAnimationSessionDic.Clear();

            //释放所有无效资源
            for (int i = 0; i < _m_lDisableAnimationSessionList.Count; i++)
            {
                _m_lDisableAnimationSessionList[i].discard();
            }
            _m_lDisableAnimationSessionList.Clear();
        }

        /***************
         * 检查所有被无效化的动作组是否完全无效，无效则从队列删除
         **/
        public void checkAnimationDisableList()
        {
            for (int i = 0; i < _m_lDisableAnimationSessionList.Count; )
            {
                if (_m_lDisableAnimationSessionList[i].checkAnimationUseless())
                {
                    //确认无效后从队列删除
                    _m_lDisableAnimationSessionList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /*********************
         * 根据SessionID获取对应动作Session
         **/
        public ALCreatureAnimationSession getAnimationSession(int _sessionID)
        {
            if (!_m_dAnimationSessionDic.ContainsKey(_sessionID))
                return null;

            return _m_dAnimationSessionDic[_sessionID];
        }

        /****************
         * 获取动作信息的序列
         **/
        private int getAnimationSerialize()
        {
            return _m_iCreatureAnimationSerialize++;
        }
    }
}
#endif
