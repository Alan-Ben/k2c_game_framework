using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /********************
     * 登录部分的通用结构体对象
     **/
    public class GGameCommonInfo : _ATALBasicSingleAssetObj<NPGSOGameCommonInfo>
    {
        private static GGameCommonInfo _g_instance = new GGameCommonInfo();
        [NotNull] public static GGameCommonInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GGameCommonInfo();
                return _g_instance;
            }
        }

        /** 获取资源管理对象 */
        protected override _AALResourceCore _resCore { get { return GameResCore.instance; } }
        /** 获取加载路径 */
        protected override string _resPath { get { return NPGSOGameCommonInfo.assetPath; } }
        /** 获取加载对象名 */
        protected override string _objName { get { return NPGSOGameCommonInfo.objName; } }

        //释放技能后的展示信息Cache对象
        private WCGTeamSkillPromptCache _m_cTeamSkillPromptCache;
        //范围显示对象cache对象
    
        //是否初始化
        private bool _m_bIsInit = false;

        public static void grayImage(Image _img)
        {
            grayImage(_img as MaskableGraphic);
        }
        public static void grayImage(RawImage _img)
        {
            grayImage(_img as MaskableGraphic);
        }
        public static void grayImage(MaskableGraphic _img)
        {
            if (null == _img)
                return;

            if(_img.material == instance.obj.guiGrayMat)
                return;

            if(_img is _IMatGrayBase imageGray)
            {
                imageGray.grayImage();
                return;
            }
            
            //设置为灰色材质
            _img.material = instance.obj.guiGrayMat;
        }
        public static void grayImage(Image[] _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0;i < _imgList.Length; ++i)
            {
                grayImage(_imgList[i]);
            }
        }
        public static void grayImage(List<Image> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0;i < _imgList.Count; ++i)
            {
                grayImage(_imgList[i]);
            }
        }
        public static void grayImage(RawImage[] _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0;i < _imgList.Length; ++i)
            {
                grayImage(_imgList[i]);
            }
        }
        public static void grayImage(List<RawImage> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Count; ++i)
            {
                grayImage(_imgList[i]);
            }
        }
        public static void grayImage(List<MaskableGraphic> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Count; ++i)
            {
                grayImage(_imgList[i]);
            }
        }

        public static void grayImage(List<MaskableGraphic> _imgList,bool isGray)
        {
            if (_imgList == null)
                return;
            for (int i = 0; i < _imgList.Count; ++i)
            {
                if (isGray)
                    grayImage(_imgList[i]);
                else
                    disgrayImage(_imgList[i]);
            }
        }

        public static void graySpriteRender(SpriteRenderer _spriteRender)
        {
            if (_spriteRender == null)
                return;

            _spriteRender.material = instance.obj.gtdGrayMat;
        }
        public static void graySpriteRender(List<SpriteRenderer> _spriteRenderList)
        {
            if (_spriteRenderList == null)
                return;

            for (int i = 0; i < _spriteRenderList.Count; i++)
            {
                if(_spriteRenderList[i] != null)
                {
                    _spriteRenderList[i].material = instance.obj.gtdGrayMat;
                }
            }
        }

        public static void disgrayImage(Image _img)
        {
            disgrayImage(_img as MaskableGraphic);
        }
        public static void disgrayImage(RawImage _img)
        {
            disgrayImage(_img as MaskableGraphic);
        }
        public static void disgrayImage(MaskableGraphic _img)
        {
            if (null == _img)
                return;
            
            if(_img is _IMatGrayBase grayImage)
            {
                grayImage.disgrayImage();
                return;
            }
            if(_img.material != instance.obj.guiGrayMat)
                return;
            
            if(_img.material == null)
                return;

            //设置为灰色材质
            _img.material = null;
        }
        public static void disgrayImage(Image[] _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Length; ++i)
            {
                disgrayImage(_imgList[i]);
            }
        }
        public static void disgrayImage(List<Image> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Count; ++i)
            {
                disgrayImage(_imgList[i]);
            }
        }
        public static void disgrayImage(RawImage[] _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Length; ++i)
            {
                disgrayImage(_imgList[i]);
            }
        }
        public static void disgrayImage(List<RawImage> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Count; ++i)
            {
                disgrayImage(_imgList[i]);
            }
        }
        public static void disgrayImage(List<MaskableGraphic> _imgList)
        {
            if(_imgList == null)
                return;
            for(int i = 0; i < _imgList.Count; ++i)
            {
                disgrayImage(_imgList[i]);
            }
        }
        public static void disgraySpriteRender(SpriteRenderer _spriteRender)
        {
            if (_spriteRender == null)
                return;

            _spriteRender.material = instance.obj.gtdDefaultMat;
        }
        public static void disgraySpriteRender(List<SpriteRenderer> _spriteRenderList)
        {
            if (_spriteRenderList == null)
                return;

            for (int i = 0; i < _spriteRenderList.Count; i++)
            {
                if(_spriteRenderList[i] != null)
                {
                    _spriteRenderList[i].material = instance.obj.gtdDefaultMat;
                }
            }
        }

        //从缓存池获取技能释放提示对象
        public WCGTeamSkillPromptMono popTeamSkillPrompt()
        {
            WCGTeamSkillPromptMono prompt = null;

            if(_m_cTeamSkillPromptCache != null)
            {
                prompt = _m_cTeamSkillPromptCache.popItem();
            }
            return prompt;
        }

        //技能释放提示对象放回缓存池
        public void pushbackTeamSkillPrompt(WCGTeamSkillPromptMono _teamSkillPrompt)
        {
            if(_teamSkillPrompt == null)
                return;
            if(_m_cTeamSkillPromptCache == null)
                return;
            _m_cTeamSkillPromptCache.pushBackCacheItem(_teamSkillPrompt);
        }

        //初始化提示对象缓存
        public void iniAllCache(Action _doneDelegate)
        {
            //初始化缓存池对象
            if(_m_bIsInit || null == obj)
            {
                if(_doneDelegate != null)
                    _doneDelegate();

                return;
            }

            //是否初始化
            _m_bIsInit = true;

            if(obj.team_skill_prompt != null)
            {
                _m_cTeamSkillPromptCache = new WCGTeamSkillPromptCache();
                _m_cTeamSkillPromptCache.init(obj.team_skill_prompt);
            }
            
            if(_doneDelegate != null)
                _doneDelegate();
        }

        /// <summary>
        /// 通过星级阶数获取当前星的图标
        /// 数组第一个图为1星，移次到5星，共5个图
        /// </summary>
        /// <returns></returns>
        public Sprite getStarSpriteByStarStep(int _step)
        {
            int index = _step - 1;
            
            if (null == instance || instance.obj == null) 
                return null;

            if (instance.obj.starSpriteList == null || instance.obj.starSpriteList.Count == 0)
                return null;
            
            if (index < 0 || index >= instance.obj.starSpriteList.Count)
                return null;
            return instance.obj.starSpriteList[index];
        }
        
        //清空缓存
        public void clearAllCache()
        {
            if(!_m_bIsInit)
                return;

            _m_bIsInit = false;

            //释放技能后的展示信息Cache对象
            if(_m_cTeamSkillPromptCache != null)
                _m_cTeamSkillPromptCache.discard();
            _m_cTeamSkillPromptCache = null;
        }

        #region 文字
        
        public void refreshFontAsset(Action _onComplete)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_onComplete);

            _refreshTextFontAsset(stepCounter.addDoneStepCount);
            _refreshTMPFontAsset(stepCounter.addDoneStepCount);
        }
        
        /// <summary>
        /// 更新TMP使用的字体资源
        /// </summary>
        private void _refreshTMPFontAsset(Action _onComplete)
        {
            if (obj == null)
            {
                if (_onComplete != null) _onComplete();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);//添加一步作为启动步骤
            stepCounter.regAllDoneDelegate(_onComplete);

            ENPLanguage _language = GameSetting.instance.getCurrentLanguage();//获取当前使用语言
            LanguageFontTypeAssetPath languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.TMPFontTypeAssetPathList, _language);//通过当前语言查找字体资源
            if (languageFontTypeAssetPath == null)//若通过当前语言查找到的字体资源为空，则使用ENPLanguage.NONE进行查找
            {
                languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.TMPFontTypeAssetPathList, ENPLanguage.NONE);
            }
            if (languageFontTypeAssetPath == null)//若通过当前语言查找到的字体资源为空，则默认使用英文查找字体资源
            {
                languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.TMPFontTypeAssetPathList, ENPLanguage.EN_US);
            }
            if (languageFontTypeAssetPath == null)//若默认使用英文也没有找到对应资源, 直接取列表第一个配置
            {
                languageFontTypeAssetPath = obj.TMPFontTypeAssetPathList?.SafeGet(0);
            }
            if (languageFontTypeAssetPath == null)//若通过以上三个都没有取到字体资源, 配置应该出了问题, 报错返回
            {
                Debug.LogError("[_refreshTMPFontAsset] 无法找到可用的TMP字体资源配置, 请检查game_common_info配置");
                stepCounter.addDoneStepCount();//完成ALStepCounter的启动步骤
                return;
            }

            foreach (EFontType enumFontType in Enum.GetValues(typeof(EFontType)))
            {
                NPCommonAssetPathInfo fontAssetPath = null;
                if (languageFontTypeAssetPath.fontAssetPathList != null)
                {
                    fontAssetPath = languageFontTypeAssetPath.fontAssetPathList.Find((_fontAssetPath) =>
                    {
                        return _fontAssetPath != null && _fontAssetPath.fontType == enumFontType;
                    })?.fontAssetPath;
                }
                
                if (fontAssetPath == null)
                    fontAssetPath = languageFontTypeAssetPath.defaultFontAssetPath;
                
                stepCounter.chgTotalStepCount(1);
                GameLanguageMgr.instance.setFontTypeTMPFontAsset(enumFontType, fontAssetPath, stepCounter.addDoneStepCount);
            }
            
            stepCounter.addDoneStepCount();//完成ALStepCounter的启动步骤
        }

        private void _refreshTextFontAsset(Action _onComplete)
        {
            if (obj == null)
            {
                if (_onComplete != null) _onComplete();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);//添加一步作为启动步骤
            stepCounter.regAllDoneDelegate(_onComplete);
            
            ENPLanguage _language = GameSetting.instance.getCurrentLanguage();//获取当前使用语言
            LanguageFontTypeAssetPath languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.textFontTypeAssetPath, _language);//通过当前语言查找字体资源
            if (languageFontTypeAssetPath == null)//若通过当前语言查找到的字体资源为空，则使用ENPLanguage.NONE进行查找
            {
                languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.textFontTypeAssetPath, ENPLanguage.NONE);
            }
            if (languageFontTypeAssetPath == null)//若通过当前语言查找到的字体资源为空，则默认使用英文查找字体资源
            {
                languageFontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(obj.textFontTypeAssetPath, ENPLanguage.EN_US);
            }
            if (languageFontTypeAssetPath == null)//若默认使用英文也没有找到对应资源, 直接取列表第一个配置
            {
                languageFontTypeAssetPath = obj.textFontTypeAssetPath?.SafeGet(0);
            }
            if (languageFontTypeAssetPath == null)//若通过以上三个都没有取到字体资源, 配置应该出了问题, 报错返回
            {
                Debug.LogError("[_refreshTextFontAsset] 无法找到可用的Font字体资源配置, 请检查game_common_info配置");
                stepCounter.addDoneStepCount();//完成ALStepCounter的启动步骤
                return;
            }

            foreach (EFontType enumFontType in Enum.GetValues(typeof(EFontType)))
            {
                NPCommonAssetPathInfo fontAssetPath = null;
                if (languageFontTypeAssetPath.fontAssetPathList != null)
                {
                    fontAssetPath = languageFontTypeAssetPath.fontAssetPathList.Find((_fontAssetPath) =>
                    {
                        return _fontAssetPath != null && _fontAssetPath.fontType == enumFontType;
                    })?.fontAssetPath;
                }
                
                if (fontAssetPath == null)
                    fontAssetPath = languageFontTypeAssetPath.defaultFontAssetPath;
             
                stepCounter.chgTotalStepCount(1);
                GameLanguageMgr.instance.setFontTypeTextFontAsset(enumFontType, fontAssetPath, stepCounter.addDoneStepCount);
            }
            
            stepCounter.addDoneStepCount();//完成ALStepCounter的启动步骤
        }

        #endregion
    }
}
