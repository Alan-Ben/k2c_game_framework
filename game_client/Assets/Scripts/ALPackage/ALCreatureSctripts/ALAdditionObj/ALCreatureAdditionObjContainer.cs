using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**********************
     * 用于作为某一对象下的附加物的容器管理对象
     **/
    public class ALCreatureAdditionObjContainer
    {
        protected _AALBasicCreatureControl _m_creatureControl;
        /** 存放添加的子物体队列 */
        protected List<ALCreatureAdditionObj> _m_lAdditionObjList;

        public ALCreatureAdditionObjContainer(_AALBasicCreatureControl _creature)
        {
            _m_creatureControl = _creature;
            _m_lAdditionObjList = new List<ALCreatureAdditionObj>();
        }

        public _AALBasicCreatureControl creatureControl { get { return _m_creatureControl; } }

        /***************
         * 添加本对象，根据本对象属性进行添加
         **/
        public ALCreatureAdditionObj addAdditionObj(_AALSOBasicAdditionObjInfo _additionObj)
        {
            ALCreatureAdditionObj addtionObj = creatureControl.additionObjMgr.addAdditionObj(_additionObj);
            //生命周期为独立生命周期时不进入container进行控制，由外部控制对象进行控制
            if (null == addtionObj)
                return null;

            //仅在对象不为独立对象时才注册到本集合中
            if(AdditionLifeType.INSTANCE != _additionObj.additionLifeType)
                _m_lAdditionObjList.Add(addtionObj);

            return addtionObj;
        }

        /********************
         * 清除所有添加的物件
         **/
        public virtual void removeAllAdditionObj()
        {
            //删除所有添加物件
            for (int i = 0; i < _m_lAdditionObjList.Count; i++)
            {
                creatureControl.additionObjMgr.removeChildAdditionObj(_m_lAdditionObjList[i]);
            }
            _m_lAdditionObjList.Clear();
        }

        /********************
         * 将指定的添加物件删除
         **/
        public void removeChildAdditionObj(_AALSOBasicAdditionObjInfo _objInfo)
        {
            if (null == _objInfo)
                return;

            for (int i = 0; i < _m_lAdditionObjList.Count; )
            {
                if (null != _m_lAdditionObjList[i].tag && _m_lAdditionObjList[i].tag.CompareTo(_objInfo.objName) == 0)
                {
                    //名称一致
                    //删除物件
                    creatureControl.additionObjMgr.removeChildAdditionObj(_m_lAdditionObjList[i]);

                    //从队列删除
                    _m_lAdditionObjList.RemoveAt(i);
                }
                else
                {
                    //不删除时才增加下标
                    i++;
                }
            }
        }
        public void removeChildAdditionObj(ALCreatureAdditionObj _objInfo)
        {
            if (null == _objInfo)
                return;

            //删除物件
            creatureControl.additionObjMgr.removeChildAdditionObj(_objInfo);
        }
    }
}
#endif
