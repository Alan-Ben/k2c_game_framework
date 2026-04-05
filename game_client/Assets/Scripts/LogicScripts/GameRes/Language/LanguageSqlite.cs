using UnityEngine;
using System.IO;
using ALPackage;
using System;
using System.Text;
using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class LanguageSqliteBase : _ATALBasicSingleRefObj<TextAsset>, _ILanguageSqliteInterface
    {
        private ENPLanguage _m_language = ENPLanguage.EN_US;
        protected _ILanuageAsset _lanuageAsset { get; set; }

        protected SQLiteConnection _connection;

        //是否使用的本地数据库文件
        private bool _m_isUseLocalDb;
        protected string dbPath;

        protected Action InitCall;
        protected Dictionary<string, NPLanguageObj> languageMap = new Dictionary<string, NPLanguageObj>();

        protected override string _objName { get { return _lanuageAsset.LanguageAssetObjName(_m_language); } }

        protected override _AALResourceCore _resCore { get { return RefdataResCore.instance; } }

        /************
     * 资源加载路径
     **/
        protected override string _resPath { get { return _lanuageAsset.LanguageAssetPath(_m_language); } }

        protected void _onInit()
        {
            _m_isUseLocalDb = false;
            //获取数据路径
            dbPath = string.Format("{0}{1}", _resCore.getPatchAssetPath(""), Guid.NewGuid().ToString());
            if(File.Exists(dbPath))
                File.Delete(dbPath);

            if(obj != null)
            {
                File.WriteAllBytes(dbPath, obj.bytes);
                _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
#if UNITY_EDITOR
                Debug.Log("Language DB Final PATH: " + dbPath);
#endif
            }
            else
            {
                Debug.LogError($"Language Sqlite obj is null: {_m_language}");
            }

            TextTranslate.instance.regTranslateObj(this);
            //释放asset
            _discardAsset();

            if(InitCall != null)
                InitCall();
        }

        public void initDB(Action _completeCall, ENPLanguage _language)
        {
            _m_language = _language;

#if UNITY_EDITOR
            if(ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal && _lanuageAsset.useLocalResource)
            {
                _m_isUseLocalDb = true;
                //获取数据路径
                dbPath = string.Format(@"{0}/" + ALLocalResLoaderMgr.instance.localResRootPath + "/Refdata/__DLExport/{1}.txt", Application.dataPath, _objName);

                if(File.Exists(dbPath))
                {
                    _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
                    Debug.Log("Language DB Final PATH: " + dbPath);
                }
                else
                {
                    Debug.LogError("Can not find Language sqlite DB: " + dbPath);
                }

                TextTranslate.instance.regTranslateObj(this);

                if(_completeCall != null)
                    _completeCall();
            }
            else
#endif
            {
                InitCall = _completeCall;
                init(_onInit);
            }
        }

        public void clear()
        {
            clearLanguageMap();

            if(_connection != null)
                _connection.Dispose();
            
            //路径不为空 且不是用本地的数据库文件，需要删除临时生成的文件
            if (!string.IsNullOrEmpty(dbPath) && !_m_isUseLocalDb)
            {
                if(File.Exists(dbPath))
                    File.Delete(dbPath);
                dbPath = String.Empty;
            }
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        public string GetLanguage(string _key)
        {
            if(null == _key)
                return string.Empty;

            //非运行中做个容错，不走数据据，直接返回key
            if (!Application.isPlaying)
                return _key;
            
            //判断是否需要翻译
            if(!CharacterDetermineMgr.instance.needTranslation(_key))
            {
#if UNITY_EDITOR
                //这个提示有点多，还是只在编辑器时输出
                if(Game.instance.mainCamera.gameSetting.printLanguageKeyError)
                {
                    Debug.Log("do not need translate , key =  [" + _key + "]" + "\t\tkey.Length:  " + _key.Length);
                }
#endif

                //首先需要判断是否已经登录，避免提前进行数据构造，引发错误
                if (!GRefdataCoreMgr.instance.isInitDone)
                {
                    return _key.Replace("\\n", "\n"); //支持"\n" 
                }

                //使用NPPlayer.__getInstanceObj，避免调用new处理
                if (null != NPPlayer.__getInstanceObj() && NPPlayer.instance.playerInfo != null && NPPlayer.instance.playerInfo != null)
                {
                    _key = _key
                        .Replace("\\n", "\n")
                        .Replace("{#self}", NPPlayer.instance.playerInfo.PlayerName) //支持"\n" {#Self}替换为玩家名称
                        .Replace("{#self_blue}", $"<color=#86EFFF>{NPPlayer.instance.playerInfo.PlayerName}</color>"); //支持"\n" {#Self}替换为玩家名称
                }
                else
                {
                    _key = _key.Replace("\\n", "\n"); //支持"\n" 
                }
                
                //判断执行特殊替换
                return _specReplaceValue(_key);
            }

            NPLanguageObj lang = null;
            if(!languageMap.TryGetValue(_key, out lang))
            {
                try
                {
                    //放到里面来是为了减少每次查询时的ToLower转化
                    //都转为小写，因为LanguageExportMenu.cs的addLanguageMapByTabName函数里导出到数据库时，都转成小写和去掉头尾空格了
                    //而且必须要用ToLowerInvariant来转化，不然不同语言的小写是不一样的，比如土耳其的小写i是ı，加了这个才能转成英文的i
                    string lowerTrimKey = _key.ToLowerInvariant().Trim();
                    
                    string sql = string.Format("select * from \"{0}\" where Id = \'{1}\'", GameSetting.instance.getCurrentLanguage().ToString(), lowerTrimKey);
                    List<NPLanguageObj> resultList = null;
                    if (_connection != null)
                    {
                        resultList = _connection.Query<NPLanguageObj>(sql);
                    }
                    if(resultList != null && resultList.Count > 0)
                    {
                        lang = resultList[0];
                        if(lang != null && lang.Langugae != null)
                            lang.Langugae = lang.Langugae.Replace("\\n", "\n");
                    }
                    else
                    {
                        lang = new NPLanguageObj(_key, "");
                    }

                    try
                    {
                        //这里添加原始的，没有经过转化的key，即使外面填了两个不同大小写的key，也就是这里多一分冗余的key
                        languageMap.Add(_key, lang);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"GetLanguage Error1:{e}");//预防万一，之前上面逻辑错误导致这里会重复添加同样的key
                    }
                }
                catch(Exception ex)
                {
                    UnityEngine.Debug.LogError($"GetLanguage Error2:{ex}");
                    lang = new NPLanguageObj(_key, "err");//这里添加key是因为一次查不到后面应该也会查不到，就不用每次查询都报错了
                    languageMap.Add(_key, lang);
                }
            }

            if(string.IsNullOrEmpty(lang.Langugae))
            {
#if UNITY_EDITOR
                //这个提示有点多，还是只在编辑器时输出
                if(Game.instance.mainCamera.gameSetting.printLanguageKeyError)
                {
                    Debug.LogError(string.Format("翻译表找不到KEY: {0}", _key));
                }
#endif
                return _key;
            }

            //使用NPPlayer.__getInstanceObj，避免调用new处理
            if (null != NPPlayer.__getInstanceObj() && NPPlayer.instance.playerInfo != null)
            {
                return lang.Langugae
                    .Replace("{#self}", NPPlayer.instance.playerInfo.PlayerName) //支持"\n" {#Self}替换为玩家名称
                    .Replace("{#self_blue}", $"<color=#86EFFF>{NPPlayer.instance.playerInfo.PlayerName}</color>"); //支持"\n" {#Self}替换为玩家名称
            }
            else
            {
                return lang.Langugae; //支持"\n" 
            }
        }

        //NOTICE: 带参数的获取语言表字符串,对于可接受可变数量参数的方法的调用会对性能造成一定的损失,不带参数的请调用GetLanguage接口
        //public string getLanguage(string _langKey, params object[] _args)
        public string oldGetLanguage(string _langKey, params object[] _args)
        {
            string lang = GetLanguage(_langKey);

            //args为空直接返回翻译结果，不做Format处理，如果args不为空且长度不为0，那么不管取没取到翻译，都应该尝试Format一次
            if(_args == null || _args.Length == 0)
                return lang;

            try
            {
                //俄语特殊处理
                if(GameSetting.instance.getCurrentLanguage() == ENPLanguage.RU_RU && lang.Contains("#replace_N["))
                {
                    lang = replaceRU_RU(lang, _args);
                }
                lang = string.Format(lang, _args);
            }
            catch(Exception)
            {
                Debug.LogError("字符串格式化错误, 参数数量: " + _args.Length + "\t_langKey:  " + _langKey + "\t字符串： " + lang);
                return lang;
            }
            return lang;
        }

        /////////////////////////////////////////////
        ///     复数统一模式
        ///     |mul[0, #xxxx]|
        ///     |mul[0, 1]|
        /////////////////////////////////////////////

        //内部变量避免队列重复生成,参数列表缓存
        [JetBrains.Annotations.NotNull]private LanguageArgListCache tempArgListCache = new LanguageArgListCache();
        
        /// <summary>
        /// 先在获取前先使用关键词匹配，从关键词中检索到对应的数据
        /// </summary>
        /// <param name="_langKey"></param>
        /// <param name="_args"></param>
        /// <returns></returns>
        public string getLanguage(string _langKey, params object[] _args)
        //public string newGetLanguage(string _langKey, params object[] _args)
        {
            //获取翻译结果
            string lang = GetLanguage(_langKey);

#if UNITY_EDITOR
            //编辑器下对于没有翻译的key增加参数显示，方便自测
            if (CharacterDetermineMgr.instance.needTranslation(_langKey) && lang ==_langKey )
            {
                var errorKeyBuilder = new StringBuilder();
                for (var i = 0; i < _args.Length; i++)
                {
                    errorKeyBuilder.Append("{").Append(i).Append("},");
                }
                lang = errorKeyBuilder.ToString() + lang;
            }
#endif
            //args为空直接返回翻译结果，不做Format处理，如果args不为空且长度不为0，那么不管取没取到翻译，都应该尝试Format一次
            if(_args == null || _args.Length == 0)
                return lang;

            bool useNewList = false;

            List<object> tmpArgList = tempArgListCache.popItem();
            if (null == tmpArgList)
            {
                Debug.LogError("Pop出来的参数列表缓存是null，注意检查");
                return lang;
            }
            //保险下
            tmpArgList.Clear();
            
            try
            {
                //如果没有特殊自定义字段，则不会自动翻译带入key
                if(_langKey.IndexOf(_c_mulK) < 0)
                {
                    //如果有参数
                    if(_args != null && _args.Length > 0)
                    {
                        //参数需要判断是否需要翻译     
                        for(int i = 0; i < _args.Length; ++i)
                        {
                            //装箱操作，是可以把任意类型进行装箱操作的，但是我们是不是可以拆箱成我们需要的类型呢???
                            //不可以使用强制转化，会报异常System.InvalidCastException: Specified cast is not valid.
                            //tmpArg = (string)_args[i]; 
                            //tmpArg = Convert.ToString(_args[i]);
                            if(_args[i] is string)
                            {
                                string tmpArg = (string)_args[i];
                                if(tmpArg.Length >= 2 && (tmpArg[0].Equals('#') || tmpArg.StartsWith("$$")))
                                {
                                    tmpArgList.Add(TextTranslate.instance.getLanguage(tmpArg));
                                    useNewList = true;
                                }
                                else
                                {
                                    tmpArgList.Add(tmpArg);
                                }
                            }
                            else
                            {
                                tmpArgList.Add(_args[i]);
                            }
                        }
                    }
                }
            }
            catch(Exception _ex)
            {
                Debug.LogError($"getLanguage出现错误  _langKey：{_langKey}， Exception: {_ex}");
                tempArgListCache.pushBackCacheItem(tmpArgList);
                return _langKey;
            }

            try
            {
                // //根据复数统一模式进行匹配
                // if (GameSetting.instance.getCurrentLanguage() == ENPLanguage.RU_RU
                //     || GameSetting.instance.getCurrentLanguage() == ENPLanguage.EN_US)
                // {
                //     lang = _mulLang(lang, _args);
                // }

                //【客户端单复数支持】默认所有小语种都支持英语的单复数功能
                lang = _mulLang(lang, _args);
                
                //俄语特殊处理
                if (GameSetting.instance.getCurrentLanguage() == ENPLanguage.RU_RU && lang.Contains("#replace_N["))
                {
                    lang = replaceRU_RU(lang, _args);
                }

                if (useNewList)
                    lang = string.Format(lang, tmpArgList.ToArray());
                else
                    lang = string.Format(lang, _args);
            }
            catch (Exception)
            {
                Debug.LogError("字符串格式化错误, 参数数量: " + _args.Length + "\t_langKey:  " + _langKey + "\t字符串： " + lang);
            }
            finally
            {
                if (tmpArgList != null)
                {
                    tmpArgList.Clear();
                    tempArgListCache.pushBackCacheItem(tmpArgList);
                }
            }
            return lang;
        }

        /// <summary>
        /// 根据对应的数据，判断是否有特殊复数格式处理的内容，并进行处理
        /// </summary>
        /// <param name="_lang"></param>
        /// <param name="_args"></param>
        /// <returns></returns>
        private const string _c_mulK = "|mul[";
        private const int _c_KLen = 5;
        private const string _c_mulK_end = "]|";
        private const int _c_K_endLen = 2;
        private string _mulLang(string _lang, params object[] _args)
        {
            int keyIdx = _lang.IndexOf(_c_mulK);
            if(keyIdx < 0)
                return _lang;

            int startIdx = 0;
            StringBuilder builder = new StringBuilder();

            try
            {
                //有数据则使用数据进行处理
                while(keyIdx >= 0)
                {
                    //添加左半部分
                    builder.Append(_lang, startIdx, keyIdx - startIdx);
                    //获取结尾参数
                    int endIdx = _lang.IndexOf(_c_mulK_end, startIdx);
                    if(endIdx < 0)
                        return builder.ToString();

                    //截取中间的内容
                    string content = _lang.Substring(keyIdx + _c_KLen, endIdx - keyIdx - _c_KLen);
                    //默认参数数量只有一位，因此只取第一个字符
                    int argIdx = 0;
                    int sepIdx = content.IndexOf(',');
                    //解析参数下标
                    if(!int.TryParse(content.Substring(0, sepIdx), out argIdx))
                    {
                        //计算下一个开启位置
                        startIdx = endIdx + _c_K_endLen;
                        //获取下一个参数
                        keyIdx = _lang.IndexOf(_c_mulK, startIdx);

                        continue;
                    }

                    //解析对应参考值
                    int value = 0;
                    //获取参数并去除颜色大小的标签
                    string curArg = _args[argIdx].ToString();
                    if (curArg.Contains("</color>", StringComparison.InvariantCultureIgnoreCase))
                        curArg = curArg.removeRichText("color");
                    if (curArg.Contains("</size>", StringComparison.InvariantCultureIgnoreCase))
                        curArg = curArg.removeRichText("size");

                    if (!int.TryParse(curArg, out value))
                    {
                        //计算下一个开启位置
                        startIdx = endIdx + _c_K_endLen;
                        //获取下一个参数
                        keyIdx = _lang.IndexOf(_c_mulK, startIdx);

                        continue;
                    }

                    //判断第2个参数的key是否数值，如是则获取参数，如不是则直接获取
                    string secStr = content.Substring(sepIdx + 1);
                    int keyV = 0;
                    if(int.TryParse(secStr, out keyV))
                    {
                        //获取成功获取key
                        builder.Append(getMulLan(_args[keyV].ToString(), value));
                    }
                    else
                    {
                        //直接使用第2个作为Key处理
                        builder.Append(getMulLan(secStr.Trim(), value));
                    }

                    //计算下一个开启位置
                    startIdx = endIdx + _c_K_endLen;
                    //获取下一个参数
                    keyIdx = _lang.IndexOf(_c_mulK, startIdx);
                }

                //将最后的数据加入
                builder.Append(_lang.Substring(startIdx));

                //返回字符串
                return builder.ToString();
            }
            finally
            {
                builder.Clear();
            }
        }


        /// <summary>
        /// 根据带入的key和数量，检索对应的单数还是负数模式
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_count"></param>
        /// <returns></returns>
        public string getMulLan(string _key, int _count)
        {
            //判断是否需要翻译，如不需要翻译则直接返回
            if(!CharacterDetermineMgr.instance.needTranslation(_key))
                return _key;

            if(GameSetting.instance.getCurrentLanguage() == ENPLanguage.RU_RU)
            {
                return getLanguage(_key);
            }
            else//【客户端单复数支持】默认所有小语种都支持英语的单复数功能
            {
                //单数或复数
                //（按照martin说的：0属于many）
                if(_count <= 1 && _count != 0)
                {
                    return getLanguage(_key);
                }
                else
                {
                    string multS = getLanguage($"{_key}#many");
                    if(string.IsNullOrEmpty(multS))
                        return getLanguage(_key);
                    else
                        return multS;
                }
            }
        }

        /**
         *
         * 比如  
        中文: 全服累计击杀{0}个海盗团 

        俄文:Уничтожить {0}    #replace_N["Пирата","Пиратов","Пиратов"]#
                       i1 i2  n1                                      n2

        如果参数{0}的值 小于等于1,用 Пирата
        如果参数{0}的值 介于2到4之间,用 Пиратов
        如果参数{0}的值 大于等于5,用 Пиратов

        假设现在{0}=2

        那么 俄文翻译为 Уничтожить {0} Пиратов
         
         
         n1 == 
         */

        /// <summary>
        ///俄语替换 
        /// </summary>
        /// <param name="_lang"></param>
        /// <param name="_args"></param>
        /// <returns></returns>
        private string replaceRU_RU(string _lang, params object[] _args)
        {
            string finalStr = _lang;
            try
            {
                int n1, n2, i1, i2;
                n1 = _lang.IndexOf("#replace_N[", 0); //开始位置
                n2 = _lang.IndexOf("]#", n1); //结束位置
                string replaceValue = _lang.Substring(n1, n2 - n1 + 2);

                i1 = _lang.LastIndexOf("}", n1);//{的位置
                i2 = _lang.LastIndexOf("{", i1);//}的位置
                int replaceCount = int.Parse(_lang.Substring(i2 + 1, i1 - i2 - 1));//{N} 
                
                int value = 0;
                string arg = _args[replaceCount].ToString();//取得参数值的string
                // 需要考虑参数带颜色富文本的情况eg:    <color=#00E734FF>57</color>
                // 字符串是个特色的引用类型，修改其中一个字符串，就会创建一个全新的string对象，而另一个字符串不会发生任何变化，所以直接改字符串
                // 其他富文本格式暂不支持...
                if (arg.Contains("</color>") || arg.Contains("</Color>"))
                    arg = arg.removeColor();

                if(!int.TryParse(arg, out value))//上面一行N对应参数的值
                {
                    // 当通过tryParse解析失败时，判断数字后面有没有TBMK...，(ToLargeString函数得到值后面添加的单位), 若有使用格式3
                    if (arg.EndsWith("T", true, null) || arg.EndsWith("B", true, null) || arg.EndsWith("M", true, null) || arg.EndsWith("K", true, null))
                    {
                        // 因为数值为0时是使用格式3, 所以这里直接赋值为0
                        value = 0;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"俄语解析失败：lang[{_lang}],  args[{replaceCount}] : {arg} is not int or largeString");
                        return finalStr;
                    }
                }
                
                // <=0	格式3
                // 1	格式1
                // 2-4	格式2
                // 5-20	格式3
                // 21	格式1
                // 22-24	格式2
                // 25-30	格式3
                // 31	格式1
                // 32-34	格式2
                // 35-40	格式3
                // ...	后面的逢1、2、5循环

                int format = 1;//使用格式
                if (value <= 0)//小于0, 使用格式3
                {
                    format = 3;
                }
                else if (value == 1)// 1, 使用格式1
                {
                    format = 1;
                }
                else if(value >= 2 && value <= 4)// 2-4, 使用格式2
                {
                    format = 2;
                }
                else if(value >= 5 && value <= 20)// 5-20, 使用格式3
                {
                    format = 3;
                }
                else// 大于20的情况, 根据个位数判断
                {
                    int valueSingleDigits = value % 10;//获取数值的个位数
                    if (valueSingleDigits == 1)//个位数1, 使用格式1
                    {
                        format = 1;
                    }
                    else if(valueSingleDigits >= 2 && valueSingleDigits <= 4)// 个位数2-4, 使用格式2
                    {
                        format = 2;
                    }
                    else// 个位数0, 5-9, 使用格式3
                    {
                        format = 3;
                    }
                }

                string tag = replaceValue.Replace("#replace_N[", string.Empty).
                    Replace("]#", string.Empty).
                    Replace("\"", string.Empty).Split(',')[format - 1];//暂定默认规则, 格式1->下标0, 格式2->下标1, 格式3->下标2

                finalStr = _lang.Remove(n1, n2 - n1 + 2).Insert(n1, tag);
                if(finalStr.Contains("#replace_N["))
                {
                    return replaceRU_RU(finalStr, _args);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"俄语解析失败：key：{_lang}===args：{_args.ToStringList()}：{e}");
                return finalStr;
            }

            return finalStr;
        }


        //替换为ItemType的名字 $$-item_type_name-ENPItemType-sub_id
        private const string specKeyItemTypeName = "item_type_name";
        //替换为玩家的等级全名，阶重层
        private const string specKeyPlayerLvlName = "player_lvl_name";
        //替换为关卡组名字
        private const string specKeyMissionGroupName = "mission_group_name";
        //替换为客户端宠物参数的名字
        private const string specKeyClientPetParamName = "client_pet_param_name";
        // 替换为关卡名称(【关卡】取关卡名称$$-chapter_stage_name-chapter_stage_id)
        private const string specKeyChapterStageName = "chapter_stage_name";
        //替换为主线任务step名称 $$-quest_step_name-quest_step_id
        private const string questStepName = "quest_step_name";
        //替换为当前游历事件的npc或者妃子名称 $$-travel_npc_name
        private const string travelNPCName    = "travel_npc_name";
        //替换为当前拜访的好友名称 $$-visit_friend_name
        private const string visitFriendName = "visit_friend_name";
        // 替换为高级公式值 $$-variable_value-高级公式:EValueFormatType(EValueFormatType类型, 数值显示使用什么格式)
        private const string specKeyVariableValue = "variable_value";
        // 替换为翻译值 $$-translate-翻译key-参数1:参数2:...
        private const string specKeyTranslate = "translate";
        // 替换为任务步骤排序id $$-quest_step_sort_id-quest_step_id
        private const string specKeyQuestStepSortId = "quest_step_sort_id";
        // 家人未获得替换名称 $$-consort_not_get_replace_name-consort_id-not_get_name_key
        private const string consortNotGetReplaceName = "consort_not_get_replace_name";
        // 根据本地货币转换价格 $$-local_price-pay_id  比如:USD 9.9
        private const string localPriceReplaceName = "local_price";
        // 根据格式化类型进行格式化 $$-format_value-数值-EValueFormatType(EValueFormatType类型, 数值显示使用什么格式)
        // 调用GCommon.getValueFormatStr方法处理
        private const string specKeyFormatValue = "format_value";

        /// <summary>
        /// 特殊替换逻辑
        /// </summary>
        /// <returns></returns>
        private string _specReplaceValue(string _str)
        {
            //空字符就不用翻译了
            if(_str == null || string.IsNullOrEmpty(_str) )
                return _str;

            //不是$$大头的不替换
            if (!_str.StartsWith("$$"))
                return _str;

            //用-分割
            string[] strList = _str.Split('-');
            if (strList.Length < 2)
            {
                Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                return _str;
            }

            string keyName = strList[1];
            switch (keyName)
            {
                //替换为ItemType的名字
                case specKeyItemTypeName:
                    if (strList.Length != 4)
                        return _str;
                    ENPItemType itemType = ENPItemType.NONE;
                    long subId = 0;
                    
                    if (!ENPItemType.TryParse(strList[2], true, out itemType))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }
                    if (!long.TryParse(strList[3], out subId))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }

                    _str = GCommon.getItemName(itemType, subId);
                    break;
                //替换为关卡组名字
                case specKeyMissionGroupName:
                    // if (strList.Length != 3)
                    //     return _str;
                    // long missionGroupID = 0;
                    // if (!long.TryParse(strList[2], out missionGroupID))
                    // {
                    //     Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                    //     return _str;
                    // }
                    // NPChapterMissionRefObj chapterMissionRefObj = GRefdataCoreMgr.instance.chapterMissionRefCore.getRef(missionGroupID);
                    // if (null == chapterMissionRefObj)
                    // {
                    //     Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                    //     return _str;
                    // }
                    //
                    // _str = chapterMissionRefObj.getFullName();
                    break;
                //替换为宠物参数名字
                case specKeyClientPetParamName:
                    if (strList.Length != 3)
                        return _str;

                    ENPPlayerParam clientPetParam;
                    if (!Enum.TryParse(strList[2], out clientPetParam))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }

                    //暂时只支持心愿任务宠物跟跟随的宠物
                    switch (clientPetParam)
                    {
                        case ENPPlayerParam.VIP_LVL:
                            break;
                        default:
                            Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                            break;
                    }
                    break;
                case specKeyChapterStageName:
                    if (strList.Length != 3)
                        return _str;
                    long chapterStageID = ALCommon.ParseLong(strList[2]);
                    ChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(chapterStageID);
                    if (chapterRefObj == null)
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }
                    _str = getLanguage(chapterRefObj.name, chapterRefObj.name_args?.ToArray());
                    break;
                case questStepName:
                    if (strList.Length != 3)
                        return _str;
                    long questStepId = ALCommon.ParseLong(strList[2]);
                    QuestStepRefObj questStepRef = GRefdataCoreMgr.instance.questStepMap.getRef(questStepId);
                    if (questStepRef == null)
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }
                    _str = questStepRef.quest_step_name_str;
                    break;
                
                case travelNPCName:
                    _ATravelEventInfo eventInfo = NPPlayer.instance.travelComp.curDealEvent;
                    return eventInfo?.travelEventRefObj?.eventTarget?.roleInfo?.name ?? _str;
                    break;
                case visitFriendName:
                    NPCommonSimplePlayerInfo friendInfo = NPPlayer.instance.friendsComp.visitFriendInfo;
                    if (friendInfo == null)
                        return _str;

                    _str = getLanguage(friendInfo.name);
                    break;
                case specKeyVariableValue:
                    string[] splitLimitStrArray = _str.Split('-', 3);//只需要进行三段拆解, 第三个就是需要用到的参数, 不直接用‘-’全分割因为高级公式会用到
                    if (splitLimitStrArray == null || splitLimitStrArray.Length < 3 || string.IsNullOrEmpty(splitLimitStrArray[2]))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 specKeyVariableValue类型配置方式为 : $$-variable_value-高级公式:EValueFormatType(EValueFormatType类型, 数值显示使用什么格式), 当前配置为:{_str}");
                        return string.Empty;
                    }
                    
                    string[] paramArray = splitLimitStrArray[2].Split(':');//使用':'进行参数分割, 因为高级公式中不会存在:
                    if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 specKeyVariableValue类型配置方式为 : $$-variable_value-高级公式:EValueFormatType(EValueFormatType类型, 数值显示使用什么格式), 当前配置为:{_str}");
                        return string.Empty;
                    }
                    
                    long value = NPPlayerVariableGroupObj.readVariableGroup(paramArray[0], $"特殊替换逻辑 出错 specKeyVariableValue类型配置方式为 : $$-variable_value-高级公式:EValueFormatType(EValueFormatType类型, 数值显示使用什么格式), 当前配置为:{_str}")
                        .CalculateVariableResult(null);//计算高级公式值

                    if (paramArray.Length > 1 && !string.IsNullOrEmpty(paramArray[1]))
                    {
                        Enum.TryParse(paramArray[1], out EValueFormatType _valueFormatType);
                        return GCommon.getValueFormatStr(_valueFormatType, value);
                    }

                    _str = value.ToString();
                    break;
                
                case specKeyTranslate:
                    if (strList.Length < 3)
                        return _str;

                    string key = strList[2];
                    if (strList.Length < 4 || string.IsNullOrEmpty(strList[3]))
                    {
                        _str = key;
                    }
                    else
                    {
                        string[] specKeyTranslateParamArray = strList[3].Split(':');
                        _str = getLanguage(key, specKeyTranslateParamArray);
                    }
                    
                    break;
                case specKeyQuestStepSortId:
                    if (strList.Length != 3)
                        return _str;
                    long questStepSortId = ALCommon.ParseLong(strList[2]);
                    QuestStepRefObj questStepSortRef = GRefdataCoreMgr.instance.questStepMap.getRef(questStepSortId);
                    if (questStepSortRef == null)
                    {
                        Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                        return _str;
                    }
                    _str = questStepSortRef.sort_id.ToString();
                    break;
                case consortNotGetReplaceName:
                    if (strList.Length != 4)
                        return _str;
                    long replaceNameConsortId = ALCommon.ParseLong(strList[2]);
                    //已获得使用对应家人名称，未获得使用传入的key当做名称
                    if(NPPlayer.instance.consortComp.getConsortInfo(replaceNameConsortId) != null)
                        _str = GCommon.getItemName(ENPItemType.CONSORT, replaceNameConsortId);
                    else
                        _str = getLanguage(strList[3]);
                    break;
                case localPriceReplaceName:
                    if (strList.Length != 3)
                        return _str;
                    long payId = ALCommon.ParseLong(strList[2]);
                    _str = SDKMgr.instance.getLocalShowPrice(payId);
                    break;
                case specKeyFormatValue:
                    if (strList.Length != 4)
                    {
                        Debug.LogError($"特殊替换逻辑 出错 specKeyFormatValue类型配置方式为 : $$-format_value-数值-EValueFormatType(EValueFormatType类型, 数值显示使用什么格式), 当前配置为:{_str}");
                        return _str;
                    }
                    
                    long formatValue = 0;
                    if (!long.TryParse(strList[2], out formatValue))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 specKeyFormatValue 数值解析失败, 当前配置为:{_str}");
                        return _str;
                    }
                    
                    EValueFormatType valueFormatType;
                    if (!Enum.TryParse(strList[3], out valueFormatType))
                    {
                        Debug.LogError($"特殊替换逻辑 出错 specKeyFormatValue EValueFormatType解析失败, 当前配置为:{_str}");
                        return _str;
                    }
                    
                    _str = GCommon.getValueFormatStr(valueFormatType, formatValue);
                    break;
                default:
                    Debug.LogError($"特殊替换逻辑 出错 _str：{_str}");
                    break;
            }
            
            return _str;
        }

        //清空缓存
        public void clearLanguageMap()
        {
            languageMap.Clear();
        }
        
    }

    /// <summary>
    /// 游戏内翻译
    /// </summary>
    public class NPGGUILanguageSqlite : LanguageSqliteBase
    {
        private static NPGGUILanguageSqlite _g_instance = new NPGGUILanguageSqlite();
        [JetBrains.Annotations.NotNull]
        public static NPGGUILanguageSqlite instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUILanguageSqlite();
                return _g_instance;
            }
        }

        public NPGGUILanguageSqlite()
        {
            _lanuageAsset = new LanuageAsset();
        }

        public void discard()
        {
            _g_instance = null;
        }
    }

    /// <summary>
    /// 平台翻译
    /// </summary>
    public class NPPGUILanguageSqlite : LanguageSqliteBase
    {
        private static NPPGUILanguageSqlite _g_instance = new NPPGUILanguageSqlite();
        [JetBrains.Annotations.NotNull]
        public static NPPGUILanguageSqlite instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUILanguageSqlite();
                return _g_instance;
            }
        }

        public NPPGUILanguageSqlite()
        {
            _lanuageAsset = new PlatLanuageAsset();
        }

        public void discard()
        {
            _g_instance = null;
        }

        protected override _AALResourceCore _resCore { get { return PlatResCore.instance; } }

    }
}

