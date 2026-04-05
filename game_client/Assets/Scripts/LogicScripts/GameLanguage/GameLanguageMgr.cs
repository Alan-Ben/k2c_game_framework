using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace GOE
{
    public class GameLanguageMgr
    {
        private static GameLanguageMgr _g_instance;
        public static GameLanguageMgr instance { get { return _g_instance ??= new GameLanguageMgr(); } }

        
        #region 第一版TextMeshPro处理方案(修改FallBack, 弃用)
        // [NotNull] private Dictionary<TMP_FontAsset, TMP_FontAssetInfo> _m_dTextMeshProUseFontAssetDic = new Dictionary<TMP_FontAsset, TMP_FontAssetInfo>();//已经加载出来的TextMeshPro使用中的TMP_FontAsset字典
        // [NotNull] private List<TMP_FontAsset> _m_lNowLanguageFallbackTMPFontAssetList = new List<TMP_FontAsset>();//当前语言TMP_FontAsset中需要添加的fallback TMP_FontAsset
        #endregion
        [NotNull] private Dictionary<EFontType, NPCommonAssetPathInfo> _m_dFontTypeTMPFontAssetPathDic = new Dictionary<EFontType, NPCommonAssetPathInfo>();//字体对应的TMP_FontAsset路径字典
        [NotNull] private TMP_FontAssetInstantiateObjCore _m_oTMPFontAssetInstantiateObjCore = new TMP_FontAssetInstantiateObjCore();//TMP_FontAsset实例化对象管理器
        [NotNull] private HashSet<_ITextMeshProMono> _m_dFontTypeTMPTextMonoSet = new HashSet<_ITextMeshProMono>();//字体对应的TextMeshPro脚本集合
        
        [NotNull] private Dictionary<EFontType, NPCommonAssetPathInfo> _m_dFontTypeTextFontAssetPathDic = new Dictionary<EFontType, NPCommonAssetPathInfo>();//字体对应的Text Font路径字典
        [NotNull] private TextFontInstantiateObjCore _m_TextFontInstantiateObjCore = new TextFontInstantiateObjCore();//字体对应的FontAsset实例对象管理器
        [NotNull] private HashSet<_ITextMono> _m_dFontTypeTextMonoSet = new HashSet<_ITextMono>();//字体对应的Text脚本集合

        private NPGSOGameCommonInfo _m_localGameCommonInfo; // 本地通用信息SO
        public GameLanguageMgr()
        {
            _m_localGameCommonInfo = NPResUtil.loadLocalRes<NPGSOGameCommonInfo>(NPGSOGameCommonInfo.assetPath, NPGSOGameCommonInfo.objName,
                ".asset");
        }

#if UNITY_EDITOR
        ~GameLanguageMgr()
        {
            StringBuilder sb = new StringBuilder();
            HashSet<Type> typeList = new HashSet<Type>();

            if (_m_dFontTypeTMPTextMonoSet.Count > 0)
            {
                foreach (_ITextMeshProMono mono in _m_dFontTypeTMPTextMonoSet)
                {
                    if(mono == null)
                        continue;
                    
                    Type type = mono.GetType();
                    if (typeList.Add(type))
                    {
                        sb.Append(mono.GetType());
                        sb.Append(", ");       
                    }
                }
                
                Debug.LogError($"当GameLanguageMgr被回收时, 内部有未移除的TextMeshPro脚本, 请检查脚本:[{sb}], 是否没有在Destroy中调用GameLanguageMgr.onTextMeshProDestroy方法");
                typeList.Clear();
                _m_dFontTypeTMPTextMonoSet.Clear();
            }

            sb.Clear();
            typeList.Clear();
            if (_m_dFontTypeTextMonoSet.Count > 0)
            {
                foreach (_ITextMono mono in _m_dFontTypeTextMonoSet)
                {
                    if(mono == null)
                        continue;
                    
                    Type type = mono.GetType();
                    if (typeList.Add(type))
                    {
                        sb.Append(mono.GetType());
                        sb.Append(", ");       
                    }
                }
                
                Debug.LogError($"当GameLanguageMgr被回收时, 内部有未移除的Text脚本, 请检查脚本:[{sb}], 是否没有在Destroy中调用GameLanguageMgr.onTextDestroy方法");
                typeList.Clear();
                _m_dFontTypeTextMonoSet.Clear();
            }
        }
#endif
        
        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            resetFontAsset();
            // _m_dFontTypeTMPTextMonoSet.Clear();//这里不对_m_dFontTypeTMPTextMonoSet列表进行清除, 等物体自己调用Destroy时, 从列表中删除
            // _m_dFontTypeTextMonoSet.Clear();//这里不对_m_dFontTypeTextMonoSet列表进行清除, 等物体自己调用Destroy时, 从列表中删除
        }

        public void resetFontAsset()
        {
            _resetTMPFontAsset();
            _resetFontAsset();
        }
        
        /// <summary>
        /// 重置TMP_FontAsset
        /// </summary>
        private void _resetTMPFontAsset()
        {
            foreach (var textMeshProMono in _m_dFontTypeTMPTextMonoSet)
            {
                if(textMeshProMono != null)
                    textMeshProMono.setTMP_FontAsset(null, null);
            }
            
            _m_oTMPFontAssetInstantiateObjCore.discard();
            _m_dFontTypeTMPFontAssetPathDic.Clear();
            
            Resources.UnloadUnusedAssets();
            
            #region 第一版TextMeshPro处理方案(修改FallBack, 弃用)

            // resetNowLanguageFallbackTMPFontAssetList();//重置当前语言需要添加的FallbackTMPFontAssetList
            // _m_dTextMeshProUseFontAssetDic.Clear();

            #endregion
        }

        private void _resetFontAsset()
        {
            foreach (var texMono in _m_dFontTypeTextMonoSet)
            {
                if(texMono != null)
                    texMono.setFontAsset(null);
            }
            
            _m_TextFontInstantiateObjCore.discard();
            _m_dFontTypeTextFontAssetPathDic.Clear();
            
            Resources.UnloadUnusedAssets();
        }

        #region TextMeshPro 相关
        
        /// <summary>
        /// 设置字体类型对应的TMP_FontAsset路径
        /// </summary>
        /// <param name="_fontType"></param>
        /// <param name="_assetPathInfo"></param>
        public void setFontTypeTMPFontAsset(EFontType _fontType, NPCommonAssetPathInfo _assetPathInfo, Action _complete)
        {
            if (_assetPathInfo == null || !_assetPathInfo.enable)
            {
                Debug.LogError($"[GameLanguageMgr setFontTypeTMPFontAsset({_fontType}, {_assetPathInfo})] _assetPathInfo == null || !_assetPathInfo.enable, 请检查");
                return;
            }

            _m_dFontTypeTMPFontAssetPathDic[_fontType] = _assetPathInfo;

            _m_oTMPFontAssetInstantiateObjCore.getInstantiateObj(_assetPathInfo, (TMP_FontAssetInstantiateObj _fontInstantiateObj) =>
            {
                foreach (var TMPMono in _m_dFontTypeTMPTextMonoSet)
                {
                    if (TMPMono == null)
                        continue;

                    if (TMPMono.fontType == _fontType) //若字体类型相同, 直接设置TMP_FontAsset
                    {
                        _setTMPMonoFontAsset(TMPMono, _fontInstantiateObj?.obj, (_materialTag, _getMaterialAction) =>
                        {
                            _fontInstantiateObj?.getMaterialInstantiate(_materialTag, _getMaterialAction);
                        });
                    }
                }

                _complete?.Invoke();
            });
        }

        /// <summary>
        /// 获取字体类型对应的TMP_FontAsset
        /// </summary>
        public void getTMPFontAsset(EFontType _fontType, Action<TMP_FontAsset> _complete)
        {
            if (_m_dFontTypeTMPFontAssetPathDic.TryGetValue(_fontType, out NPCommonAssetPathInfo tmpFontAssetPath) && tmpFontAssetPath != null && tmpFontAssetPath.enable)
            {
                _m_oTMPFontAssetInstantiateObjCore.getInstantiateObj(tmpFontAssetPath, (TMP_FontAssetInstantiateObj _fontInstantiateObj) =>
                {
                    _complete?.Invoke(_fontInstantiateObj?.obj);
                });
            }
            else
            {
                Debug.LogWarning_EditorOnly($"[GameLanguageMgr getTMPFontAsset] 在_m_dFontTypeTMPFontAssetPathDic字典中没有找到fontType:{_fontType} 对应的TMP_FontAsset加载路径, 或者加载路径:[{tmpFontAssetPath}]配置错误, 请检查game_common_info配置");
                _complete?.Invoke(TMP_Settings.defaultFontAsset);//设置为默认的TMP_FontAsset
            }
        }
        
        /// <summary>
        /// 刷新所有TMP脚本对应的TMP_FontAsset
        /// </summary>
        private void _refreshAllTMPMonoFontAsset()
        {
            foreach (var TMPMono in _m_dFontTypeTMPTextMonoSet)
            {
                if(TMPMono == null)
                    continue;
                
                _refreshTMPMonoFontAsset(TMPMono);
            }
        }
        
        /// <summary>
        /// 刷新TMP脚本对应的TMP_FontAsset
        /// </summary>
        /// <param name="_tmpText"></param>
        private void _refreshTMPMonoFontAsset(_ITextMeshProMono _tmpText)
        {
            if(_tmpText == null)
                return;

            EFontType fontType = _tmpText.fontType;
            if (_m_dFontTypeTMPFontAssetPathDic.TryGetValue(fontType, out NPCommonAssetPathInfo tmpFontAssetPath) && tmpFontAssetPath != null && tmpFontAssetPath.enable)
            {
                _m_oTMPFontAssetInstantiateObjCore.getInstantiateObj(tmpFontAssetPath, (TMP_FontAssetInstantiateObj _fontInstantiateObj) =>
                {
                    _setTMPMonoFontAsset(_tmpText, _fontInstantiateObj?.obj, (_materialTag, _getMaterialAction) =>
                    {
                        _fontInstantiateObj?.getMaterialInstantiate(_materialTag, _getMaterialAction);
                    });
                });
            }
            else
            {
                Debug.LogWarning_EditorOnly($"[GameLanguageMgr _refreshTMPMonoFontAsset] 在_m_dFontTypeTMPFontAssetPathDic字典中没有找到fontType:{fontType} 对应的TMP_FontAsset加载路径, 或者加载路径:[{tmpFontAssetPath}]配置错误, 请检查game_common_info配置", _tmpText.mono);
                _setTMPMonoFontAsset(_tmpText, TMP_Settings.defaultFontAsset, null);//设置为默认的TMP_FontAsset
            }
        }

        /// <summary>
        /// 直接设置TMP_Text的TMP_FontAsset
        /// </summary>
        /// <param name="_tmpText"></param>
        /// <param name="_tmpFontAsset"></param>
        private void _setTMPMonoFontAsset(_ITextMeshProMono _tmpText, TMP_FontAsset _tmpFontAsset, Action<string, Action<Material>> _getFontMaterial)
        {
            if(_tmpText == null || _tmpFontAsset == _tmpText.TMPFontAsset)
                return;
            
            _tmpText.setTMP_FontAsset(_tmpFontAsset, _getFontMaterial);
        }
        
        /// <summary>
        /// 当有TextMeshPro 被Awake时
        /// </summary>
        /// <param name="_tmpTextMono"></param>
        public void onTextMeshProAwake(_ITextMeshProMono _tmpTextMono)
        {
            if(_tmpTextMono == null || !Application.isPlaying)//当游戏还未开始运行时, 也不处理
                return;

            //加入TMPMono脚本集合
            if (!_m_dFontTypeTMPTextMonoSet.Add(_tmpTextMono))
            {
                Debug.LogError($"[GameLanguageMgr onTextMeshProAwake] 重复添加了相同的_ITextMeshProMono, 应该只需要在组件awake时调用一次, 请检查", _tmpTextMono.mono);
                return;
            }

            _refreshTMPMonoFontAsset(_tmpTextMono);//刷新TMP脚本对应的TMP_FontAsset
        }
        
        /// <summary>
        /// 当有TextMeshPro 被Destroy时
        /// </summary>
        /// <param name="_tmpTextMono"></param>
        public void onTextMeshProDestroy(_ITextMeshProMono _tmpTextMono)
        {
            if(_tmpTextMono == null || !Application.isPlaying)//当游戏还未开始运行时, 也不处理
                return;
        
            if(!_m_dFontTypeTMPTextMonoSet.Remove(_tmpTextMono))
            {
                string objName = _tmpTextMono.mono == null || _tmpTextMono.mono.gameObject == null ? "" : _tmpTextMono.mono.gameObject.name;
                Debug.LogError($"[GameLanguageMgr onTextMeshProDestroy] 未在_m_dFontTypeTMPTextMonoSet集合中找到需要销毁的对象：{objName}", _tmpTextMono.mono);
                return;
            }
            
            _tmpTextMono.setTMP_FontAsset(null, null);
        }
        
        #region 第一版TextMeshPro处理方案(修改FallBack, 弃用)
        // /// <summary>
        // /// 设置当前语言的FallbackTMPFontAssetList
        // /// </summary>
        // /// <param name="_fallbackTMPFontAsset"></param>
        // public void setNowLanguageFallbackTMPFontAssetList(TMP_FontAsset _fallbackTMPFontAsset)
        // {
        //     resetNowLanguageFallbackTMPFontAssetList();//先重置之前语言的FallbackTMPFontAssetList
        //     _m_lNowLanguageFallbackTMPFontAssetList.Add(_fallbackTMPFontAsset);
        //
        //     // 给_m_dTextMeshProUseFontAssetDic中的FontAsset添加FallbackTMPFontAssetList
        //     foreach (var kv in _m_dTextMeshProUseFontAssetDic)
        //     {
        //         _addToTargetFontAssetFallbackFontAssetTable(kv.Key, _m_lNowLanguageFallbackTMPFontAssetList);
        //     }
        // }
        //
        // /// <summary>
        // /// 设置当前语言的FallbackTMPFontAssetList
        // /// </summary>
        // /// <param name="_fallbackTMPFontAssetList"></param>
        // public void setNowLanguageFallbackTMPFontAssetList(List<TMP_FontAsset> _fallbackTMPFontAssetList)
        // {
        //     resetNowLanguageFallbackTMPFontAssetList();//先重置之前语言的FallbackTMPFontAssetList
        //     _m_lNowLanguageFallbackTMPFontAssetList.AddRange(_fallbackTMPFontAssetList);
        //
        //     // 给_m_dTextMeshProUseFontAssetDic中的FontAsset添加FallbackTMPFontAssetList
        //     foreach (var kv in _m_dTextMeshProUseFontAssetDic)
        //     {
        //         _addToTargetFontAssetFallbackFontAssetTable(kv.Key, _m_lNowLanguageFallbackTMPFontAssetList);
        //     }
        // }
        //
        // /// <summary>
        // /// 重置所有TMP_FontAsset中的FallbackFontAssetTable列表
        // /// </summary>
        // public void resetAllFontAssetFallbackFontAssetTable()
        // {
        //     foreach (var kv in _m_dTextMeshProUseFontAssetDic)
        //     {
        //         _setTargetFontAssetFallbackFontAssetTable(kv.Key, kv.Value.initialFallbackTMPFontAssetList);
        //     }
        // }
        //
        // /// <summary>
        // /// 重置当前语言需要添加的FallbackTMPFontAssetList
        // /// </summary>
        // public void resetNowLanguageFallbackTMPFontAssetList()
        // {
        //     // 重置所有TMP_FontAsset中的FallbackFontAssetTable列表
        //     resetAllFontAssetFallbackFontAssetTable();
        //     
        //     _m_lNowLanguageFallbackTMPFontAssetList.Clear();
        // }
        //
        // /// <summary>
        // /// 添加_fallbackTMPFontAssetList到_targetAsset的fallbackFontAssetTable中
        // /// </summary>
        // /// <param name="_targetAsset"></param>
        // private void _addToTargetFontAssetFallbackFontAssetTable(TMP_FontAsset _targetAsset, List<TMP_FontAsset> _fallbackTMPFontAssetList)
        // {
        //     if(_targetAsset == null || _fallbackTMPFontAssetList == null)
        //         return;
        //
        //     if (_targetAsset.fallbackFontAssetTable == null)
        //     {
        //         _targetAsset.fallbackFontAssetTable = new List<TMP_FontAsset>(_fallbackTMPFontAssetList);
        //         return;
        //     }
        //     
        //     foreach (var fallbackTMPFontAsset in _fallbackTMPFontAssetList)
        //     {
        //         if(fallbackTMPFontAsset == null || fallbackTMPFontAsset.name == _targetAsset.name)
        //             continue;
        //
        //         if (_targetAsset.fallbackFontAssetTable.Find((_item) =>
        //             {
        //                 return _item != null && _item.name == fallbackTMPFontAsset.name;
        //             }) == null)
        //         {
        //             _targetAsset.fallbackFontAssetTable.Add(fallbackTMPFontAsset);
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// 直接设置_targetAsset的fallbackFontAssetTable为_fallbackTMPFontAssetList
        // /// </summary>
        // /// <param name="_targetAsset"></param>
        // /// <param name="_fallbackTMPFontAssetList"></param>
        // private void _setTargetFontAssetFallbackFontAssetTable(TMP_FontAsset _targetAsset, List<TMP_FontAsset> _fallbackTMPFontAssetList)
        // {
        //     if(_targetAsset == null || _fallbackTMPFontAssetList == null)
        //         return;
        //
        //     if (_targetAsset.fallbackFontAssetTable == null)
        //         _targetAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
        //     
        //     _targetAsset.fallbackFontAssetTable.Clear();
        //     _targetAsset.fallbackFontAssetTable.AddRange(_fallbackTMPFontAssetList);
        // }
        //
        // /// <summary>
        // /// 重置_asset的Fallback列表
        // /// </summary>
        // private void _resetFontAssetFallbackFontAssetTable(TMP_FontAsset _asset)
        // {
        //     if(_asset == null)
        //         return;
        //
        //     if (_m_dTextMeshProUseFontAssetDic.TryGetValue(_asset, out TMP_FontAssetInfo _TMPFontAssetInfo))
        //     {
        //         _setTargetFontAssetFallbackFontAssetTable(_asset, _TMPFontAssetInfo.initialFallbackTMPFontAssetList);
        //     }
        // }
        #endregion

        #endregion

        #region Text Font 相关
        
        /// <summary>
        /// 设置字体类型对应的FontAsset加载路径
        /// </summary>
        /// <param name="_fontType"></param>
        /// <param name="_assetPathInfo"></param>
        public void setFontTypeTextFontAsset(EFontType _fontType, NPCommonAssetPathInfo _assetPathInfo, Action _complete)
        {
            if (_assetPathInfo == null || !_assetPathInfo.enable)
            {
                Debug.LogError($"[GameLanguageMgr setFontTypeTextFontAsset({_fontType}, {_assetPathInfo})] _assetPathInfo == null || !_assetPathInfo.enable, 请检查");
                return;
            }

            _m_dFontTypeTextFontAssetPathDic[_fontType] = _assetPathInfo;
            
            _m_TextFontInstantiateObjCore.getInstantiateObj(_assetPathInfo, (_fontObj) =>
            {
                foreach (var TMPMono in _m_dFontTypeTextMonoSet)
                {
                    if(TMPMono == null)
                        continue;

                    if (TMPMono.fontType == _fontType)//若字体类型相同, 直接设置Font
                    {
                        _setTextMonoFontAsset(TMPMono, _fontObj);
                    }
                }
                
                _complete?.Invoke();
            });
        }

        /// <summary>
        /// 获取字体类型对应的FontAsset
        /// </summary>
        public void getTextFontAsset(EFontType _fontType, Action<Font> _complete)
        {
            if (_m_dFontTypeTextFontAssetPathDic.TryGetValue(_fontType, out NPCommonAssetPathInfo tmpFontAssetPath) && tmpFontAssetPath != null && tmpFontAssetPath.enable)
            {
                _m_TextFontInstantiateObjCore.getInstantiateObj(tmpFontAssetPath, (_fontAssetObj) =>
                {
                    _complete?.Invoke(_fontAssetObj);
                });
            }
            else
            {
                Debug.LogWarning_EditorOnly($"[GameLanguageMgr getTextFontAsset] 在_m_dFontTypeTextFontAssetPathDic字典中没有找到fontType:{_fontType} 对应的Font资源加载路径, 或者加载路径:[{tmpFontAssetPath}]配置错误, 请检查game_common_info配置");
                _complete?.Invoke(PLoginCommonInfo.instance.obj?.defaultTextExFont);//设置为平台资源配置的默认Font
            }
        }
        
        /// <summary>
        /// 刷新所有Text脚本对应的FontAsset
        /// </summary>
        private void _refreshAllTextMonoFontAsset()
        {
            foreach (var mono in _m_dFontTypeTextMonoSet)
            {
                if(mono == null)
                    continue;
                
                _refreshTextMonoFontAsset(mono);
            }
        }
        
        /// <summary>
        /// 刷新Text脚本对应的FontAsset
        /// </summary>
        /// <param name="_textMono"></param>
        private void _refreshTextMonoFontAsset(_ITextMono _textMono)
        {
            if(_textMono == null)
                return;

            EFontType fontType = _textMono.fontType;
            if (_m_dFontTypeTextFontAssetPathDic.TryGetValue(fontType, out NPCommonAssetPathInfo tmpFontAssetPath) && tmpFontAssetPath != null && tmpFontAssetPath.enable)
            {
                _m_TextFontInstantiateObjCore.getInstantiateObj(tmpFontAssetPath, (_fontAssetObj) =>
                {
                    _setTextMonoFontAsset(_textMono, _fontAssetObj);
                });
            }
            else
            {
                Debug.LogWarning_EditorOnly($"[GameLanguageMgr _refreshTextMonoFontAsset] 在_m_dFontTypeTextFontAssetPathDic字典中没有找到fontType:{fontType} 对应的Font资源加载路径, 或者加载路径:[{tmpFontAssetPath}]配置错误, 请检查game_common_info配置", _textMono.mono);
                _setTextMonoFontAsset(_textMono, PLoginCommonInfo.instance.obj?.defaultTextExFont);//设置为平台资源配置的默认Font
            }
        }

        /// <summary>
        /// 直接设置Text的FontAsset
        /// </summary>
        /// <param name="_textMono"></param>
        /// <param name="_fontAsset"></param>
        private void _setTextMonoFontAsset(_ITextMono _textMono, Font _fontAsset)
        {
            if(_textMono == null || _fontAsset == _textMono.fontAsset)
                return;

            // 如果是预制体预览场景的脚本, 不处理
#if UNITY_EDITOR
            if(_textMono.mono != null && UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(_textMono.mono.gameObject))
                return;
#endif
            _textMono.setFontAsset(_fontAsset);
        }

        /// <summary>
        /// 当有Text 被Awake时
        /// </summary>
        /// <param name="_textMono"></param>
        public void onTextMonoAwake(_ITextMono _textMono)
        {
            if(_textMono == null) return;
            // 如果是预制体预览场景的脚本, 不处理
#if UNITY_EDITOR
            if (_textMono.mono != null && UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(_textMono.mono.gameObject))
            {
                if (_m_localGameCommonInfo != null && _m_localGameCommonInfo.tryGetNoneSetLocalFontByType(_textMono.fontType, out Font fontAsset))
                {
                    if (fontAsset != _textMono.fontAsset)
                    {
                        _textMono.setFontAsset(fontAsset);
                    }
                }
                return;
            }
#endif
            
            if (!Application.isPlaying) //当游戏还未开始运行时, 也不处理
                return;

            //加入TextMono脚本集合
            if (!_m_dFontTypeTextMonoSet.Add(_textMono))
            {
                Debug.LogError($"[GameLanguageMgr onTextMonoAwake] 重复添加了相同的_ITextMono, 应该只需要在组件awake时调用一次, 请检查", _textMono.mono);
                return;
            }

            _refreshTextMonoFontAsset(_textMono);//刷新Text脚本对应的fontAsset
        }
        
        /// <summary>
        /// 当有Text 被Destroy时
        /// </summary>
        /// <param name="_tmpTextMono"></param>
        public void onTextMonoDestroy(_ITextMono _textMono)
        {
            if(_textMono == null || !Application.isPlaying)//当游戏还未开始运行时, 也不处理
                return;
        
            if(!_m_dFontTypeTextMonoSet.Remove(_textMono))
            {
                string objName = _textMono.mono == null || _textMono.mono.gameObject == null ? "" : _textMono.mono.gameObject.name;
                Debug.LogError($"[GameLanguageMgr onTextMeshProDestroy] 未在_m_dFontTypeTextMonoSet集合中找到需要销毁的对象：{objName}", _textMono.mono);
                return;
            }
            // 如果是预制体预览场景的脚本, 不处理
#if UNITY_EDITOR
            if(_textMono.mono != null && UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(_textMono.mono.gameObject))
                return;
#endif
            _textMono.setFontAsset(null);
        }
        
        #endregion
        
        public void setGameLanguage(ENPLanguage _language, bool _forceChg = false)
        {
            ENPLanguage oldLanguage = GameSetting.instance.getCurrentLanguage();
            if(oldLanguage == _language && !_forceChg)//若旧语言和新语言相同, 且不强制修改的话, 直接返回
                return;
            
            //设置保存选择的语言
            GameSetting.instance.setCurrentLanguage(_language);
            onLanguageChanged();
        }
        
        //语言修改时需要做的处理
        public void onLanguageChanged()
        {
            ENPLanguage curLanguage = GameSetting.instance.getCurrentLanguage();

            //预先加载语言bundle
            RefdataResCore.instance.loadAsset(LanuageAsset.getLanguageAssetPath(curLanguage)
                , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                {
                    //判断是否加载成功
                    if (!_isSuc || null == _assetObj)
                    {
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, LanuageAsset.getLanguageAssetPath(curLanguage)), TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                        return;
                    }

                    NPGGUILanguageSqlite.instance.clear();
                    NPPGUILanguageSqlite.instance.clear();
                    NPGGUILanguageSqlite.instance.discard();
                    NPPGUILanguageSqlite.instance.discard();

                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(3);
                    stepCounter.regAllDoneDelegate(() =>
                    {
                        // 设置改成自动登入
                        Game.instance.setIsAutoLogin(true);
                        //重新登录
                        Game.instance.reloginByDefault();

                        _refreshAllTMPMonoFontAsset();//刷新所有TMP脚本对应的TMP_FontAsset
                        _refreshAllTextMonoFontAsset();//刷新所有Text脚本对应的FontAsset
                    });

                    NPGGUILanguageSqlite.instance.initDB(stepCounter.addDoneStepCount, curLanguage);
                    GRefdataCoreMgr.instance.updateRefLanguage(curLanguage, stepCounter.addDoneStepCount);
                    
                    resetFontAsset();
                    GGameCommonInfo.instance.refreshFontAsset(stepCounter.addDoneStepCount);

                }, null);

            //设置AIHelp语言
            GCommon.setAIHelpLanguage();
        }
    }
}