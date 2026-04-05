using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /*************************
     * 全局的添加物的管理对象，在场景需要释放所有对象时可直接调用
     **/
    public class ALStaticAdditionObjMgr
    {
        private static ALStaticAdditionObjMgr _g_instance = new ALStaticAdditionObjMgr();
        public static ALStaticAdditionObjMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALStaticAdditionObjMgr();

                return _g_instance;
            }
        }

        /** 存放添加的子物体队列 */
        protected HashSet<ALCreatureAdditionObj> _m_hsAdditionObjSet;

        public ALStaticAdditionObjMgr()
        {
            _m_hsAdditionObjSet = new HashSet<ALCreatureAdditionObj>();
        }

        /***************
         * 添加本对象，根据本对象属性进行添加
         **/
        public ALCreatureAdditionObj addAdditionObj(Transform _parentObj, _AALSOBasicAdditionObjInfo _additionObj)
        {
            if (null == _additionObj || null == _parentObj)
                return null;

            //创建物件
            GameObject newObj = _additionObj.createObj(null, _parentObj);

            if (null == newObj)
                return null;

            //根据不同的坐标参考放置对象
            if (AdditionPositionType.WORLD == _additionObj.additionPositionType)
            {
                newObj.transform.SetParent(null);
            }
            else if (AdditionPositionType.PARENT == _additionObj.additionPositionType)
            {
                newObj.transform.SetParent(_parentObj);
            }

            //在全局对象添加则不对生命周期属性进行判断，一致取INSTANCE
            //物体归属模式为独立个体时不允许永久存在的对象
            //此时当作普通添加物件处理
            ALCreatureAdditionObj additionObj = _regChildAdditionObj(_additionObj, newObj);

            if (_additionObj.playTime > 0)
            {
                //添加定时脚本
                ALAdditionObjTimer timerObj = newObj.AddComponent<ALAdditionObjTimer>();

                //设定脚本参数
                timerObj.lifeTime = _additionObj.playTime;
                timerObj.additionObj = additionObj;
            }

            return additionObj;
        }

        /********************
         * 清除所有添加的物件
         **/
        public void removeAllAdditionObj()
        {
            foreach (ALCreatureAdditionObj obj in _m_hsAdditionObjSet)
            {
                obj._removeGameObject();
            }

            _m_hsAdditionObjSet.Clear();
        }

        /********************
         * 向本物体中注册添加物件，并返回注册的节点对象
         **/
        protected internal ALCreatureAdditionObj _regChildAdditionObj(_AALSOBasicAdditionObjInfo _objInfo, GameObject _gameObj)
        {
            ALCreatureAdditionObj additionObj = new ALCreatureAdditionObj(_objInfo, _gameObj);

            _m_hsAdditionObjSet.Add(additionObj);

            return additionObj;
        }

        /********************
         * 向本物体中删除物件关联
         **/
        protected internal void _unregAdditionObj(ALCreatureAdditionObj _obj)
        {
            _m_hsAdditionObjSet.Remove(_obj);
        }
    }
}

#endif
