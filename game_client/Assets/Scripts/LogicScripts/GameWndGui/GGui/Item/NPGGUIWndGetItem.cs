using ALPackage;
using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用获得道具窗体
    /// </summary>
    public class NPGGUIWndGetItem : _ANPGGUIWndBaseGetItem<NPGGUIMonoGetItem>
    {
        private static NPGGUIWndGetItem _m_gInstance;
        public static NPGGUIWndGetItem instance
        {
            get
            {
                if (_m_gInstance == null)
                    _m_gInstance = new NPGGUIWndGetItem();
                return _m_gInstance;
            }
        }
        
        protected override string _monoAssetPath
        {
            get { return NPGGUIMonoGetItem.assetPath; }
        }

        protected override string _monoObjName
        {
            get { return NPGGUIMonoGetItem.objName; }
        }

    }
}
