using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class MultiLangBasicListRefCore<T> : _ATALBasicRefListCore<T> where T : _IALBasicRefObj
    {
        private _AALResourceCore _m_rcResCore;
        private string _m_sObjName;
        private ENPLanguage _m_language = ENPLanguage.EN_US;
        private List<ENPLanguage> _m_lEnableLangList;

        public MultiLangBasicListRefCore(_AALResourceCore _resCore, string _objName, List<ENPLanguage> _enableList)
        {
            _m_rcResCore = _resCore;
            _m_sObjName = _objName;
            _m_lEnableLangList = _enableList;
        }

        protected override void _getRefFailed(long _id)
        {
#if UNITY_EDITOR
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                Debug.LogError($"Refdata list读取失败: {typeof(T).Name}, 表名：{_objName}, id:{_id}");
            }
#endif
        }

        /** 获取资源加载的对象 */
        protected override _AALResourceCore _resCore { get { return _m_rcResCore; } }
        /** 获取加载资源对象的路径 */
        protected override string _assetPath
        {
            get
            {
                return MultiLanguageAsset.getLanguageAssetPath(_m_language, _m_sObjName);
            }
        }

        protected override string _objName
        {
            get { return MultiLanguageAsset.getLanguageAssetObjName(_m_language, _m_sObjName); }
        }

        /// <summary>
        /// 设置语言初始化
        /// </summary>
        public void InitRefLanguage(ENPLanguage _language, Action _doneAction)
        {
            if (_m_lEnableLangList == null || !_m_lEnableLangList.Contains(_language))
                _language = ENPLanguage.EN_US;

            if (isInit)
            {
                Debug.Log_EditorOnly($"重复初始化语言相关数据配表，{_m_sObjName}");
                _doneAction?.Invoke();
                return;
            }
            
            //设置语言进行初始化
            _m_language = _language;
            init(_doneAction, (_type, _initRefObj) => { _doneAction?.Invoke(); });
        }
        
        /// <summary>
        /// 更新语言配表
        /// </summary>
        public void updateRefLanguage(ENPLanguage _language, Action _doneAction)
        {
            if (_m_lEnableLangList == null || !_m_lEnableLangList.Contains(_language))
                _language = ENPLanguage.EN_US;

            //切换语言不需要判断是否初始化，如果目标语言跟当前语言不一致，直接切换
            if (_m_language != _language)
            {
                _m_language = _language;
                
                //销毁旧的开始重新初始化
                discard();
                init(_doneAction, (_type, _initRefObj) => { _doneAction?.Invoke(); });
            }
            else
            {
                _doneAction?.Invoke();
            }
        }
    }
}