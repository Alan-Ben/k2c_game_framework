using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /****************************
     * 对象Mesh网格合并操作的控制对象
     **/
    public class ALCreatureMeshCombineMgr
    {
        private static ALCreatureMeshCombineMgr _g_instance = new ALCreatureMeshCombineMgr();
        public static ALCreatureMeshCombineMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALCreatureMeshCombineMgr();

                return _g_instance;
            }
        }

        /** 在最近一帧处理中需要合并的网格管理对象 */
        private HashSet<ALCreatureAdditionObjMgr> _m_hsCombineMeshMgr;
        /** 在最近一帧处理中需要连接骨骼关系的网格管理对象 */
        private HashSet<ALCreatureAdditionObjMgr> _m_hsLinkBoneMeshMgr;

        protected ALCreatureMeshCombineMgr()
        {
            _m_hsCombineMeshMgr = new HashSet<ALCreatureAdditionObjMgr>();
            _m_hsLinkBoneMeshMgr = new HashSet<ALCreatureAdditionObjMgr>();
        }

        /*********************
         * 添加一个需要合并的网格管理对象
         **/
        public void addNeedCombineMgr(ALCreatureAdditionObjMgr _mgr)
        {
            if (null == _mgr)
                return;

            _m_hsCombineMeshMgr.Add(_mgr);
        }

        /*********************
         * 添加一个需要连接骨骼关系的网格管理对象
         **/
        public void addNeedLinkBoneMgr(ALCreatureAdditionObjMgr _mgr)
        {
            if (null == _mgr)
                return;

            _m_hsLinkBoneMeshMgr.Add(_mgr);
        }

        /********************
         * 取出所有需要合并的网格管理对象
         * 并进行对应的合并网格操作
         **/
        public void combineAllMgr()
        {
            //进行网格合并操作
            _combineAllMgr();

            //进行连接操作
            _linkAllMgr();
        }

        protected void _combineAllMgr()
        {
            if (_m_hsCombineMeshMgr.Count <= 0)
                return;

            foreach (ALCreatureAdditionObjMgr mgr in _m_hsCombineMeshMgr)
            {
                if (null == mgr)
                    continue;

                //处理合并操作
                mgr.combine();
            }

            //清除需要合并网格的对象
            _m_hsCombineMeshMgr.Clear();
        }
        protected void _linkAllMgr()
        {
            if (_m_hsLinkBoneMeshMgr.Count <= 0)
                return;

            foreach (ALCreatureAdditionObjMgr mgr in _m_hsLinkBoneMeshMgr)
            {
                if (null == mgr)
                    continue;

                //处理合并操作
                mgr.linkBones();
            }

            //清除需要合并网格的对象
            _m_hsLinkBoneMeshMgr.Clear();
        }
    }
}
#endif
