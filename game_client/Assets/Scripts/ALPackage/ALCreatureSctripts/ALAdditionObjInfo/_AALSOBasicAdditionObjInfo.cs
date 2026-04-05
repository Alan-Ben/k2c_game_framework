using System;
using System.Collections.Generic;
using UnityEngine;


#if AL_CREATURE_SYS
namespace ALPackage
{
    /******************
     * 添加物件的归属模式
     **/
    public enum AdditionLifeType
    {
        INSTANCE,   //独立个体模式，此模式依赖自身处理生成消亡操作（此操作无法使用无限循环模式）
        CHILD,      //跟随父物体被删除，此模式可使用无限循环模式
    }

    /******************
     * 添加物件的坐标参考对象
     **/
    public enum AdditionPositionType
    {
        WORLD,      //参考世界坐标，不跟随父物体的移动而移动
        PARENT,     //跟随父物体移动而移动
    }

    /******************
     * 添加物件的生存时间模式
     **/
    public enum AdditionTimeType
    {
        INFINATE,   //无限时间模式
        TIMER,      //根据指定时间销毁
    }

    /*******************
     * 在动作中给动作主体附带上3D物件信息
     **/

    [System.Serializable]
    public abstract class _AALSOBasicAdditionObjInfo : ScriptableObject
    {
        /** 用于查询的添加物体名称 */
        public string objName;

        /** 添加的父物体路径 */
        public _AALSOBaseCreatureChildObjInfo parentObjInfo;
        /** 添加物体的存在模式 */
        public AdditionLifeType additionLifeType;
        /** 添加物体的坐标模式 */
        public AdditionPositionType additionPositionType;
        /** 添加物体的时间模式 */
        public AdditionTimeType additionTimeType;
        /** 播放时间 */
        public float playTime;

        /** 添加物偏离的位置 */
        public Vector3 relatePos;
        /** 添加物增加的旋转角度 */
        public Vector3 rotateEuler;

        public _AALSOBasicAdditionObjInfo()
            : base()
        {
            objName = "";

            additionLifeType = AdditionLifeType.INSTANCE;
            additionPositionType = AdditionPositionType.WORLD;
            additionTimeType = AdditionTimeType.TIMER;
            playTime = 0;
        }

        /*****************
         * 创建对应物件
         **/
        public GameObject createObj(_AALBasicCreatureControl _creatureControl, Transform _parentObj)
        {
            //获取模板资源对象
            GameObject resGo = _getResGameObject(_creatureControl);
            //获取资源对象
            if (null == resGo)
                return null;

            Vector3 pos = relatePos;
            if(null != _parentObj)
                pos = _parentObj.TransformPoint(relatePos);

            //创建物件
            GameObject newObj = GameObject.Instantiate(resGo, pos, _parentObj.rotation) as GameObject;
            //设置旋转角度
            newObj.transform.Rotate(rotateEuler);

            if (null == newObj)
                return null;

            Vector3 creatureScale = new Vector3(1f, 1f, 1f);
            if (null != _creatureControl)
                creatureScale = _creatureControl.scaleSize;

            //比例不为1的时候调整缩放和位置
            if (1 != creatureScale.x
                || 1 != creatureScale.y
                || 1 != creatureScale.z)
            {
                //根据父对象的缩放设置本对象的缩放大小
                Vector3 localScale = newObj.transform.localScale;
                Vector3 realScale = new Vector3(creatureScale.x * localScale.x
                                    , creatureScale.y * localScale.y
                                    , creatureScale.z * localScale.z);
                //设置物体的缩放
                newObj.transform.localScale = realScale;
            }

            //重置父节点
            newObj.transform.SetParent(null);

            //调用事件函数
            _onCreateObj();

            return newObj;
        }

        /***************
         * 尝试删除本对象，删除对象根据本对象的名称以及对应Session的相关信息进行删除
         **/
        public void removeObj(ALCreatureAdditionObjMgr _parentMgr)
        {
            //删除对象
            _parentMgr.removeChildAdditionObj(this);
        }

        /*******************
         * 带入本对象创建的数据对象，对资源进行释放
         **/
        public void discardObj(GameObject _go)
        {
            ALUnityCommon.releaseGameObj(_go);

            //调用事件函数
            _onDiscardObj();
        }

        /*****************
         * 获取需要创建的对象模板
         **/
        protected abstract GameObject _getResGameObject(_AALBasicCreatureControl _creatureControl);
        /*****************
         * 创建对象和删除对象时的事件函数，可用于对引用数据的引用次数统计
         **/
        protected abstract void _onCreateObj();
        /*****************
         * 获取需要创建的对象模板
         **/
        protected abstract void _onDiscardObj();
    }
}
#endif
