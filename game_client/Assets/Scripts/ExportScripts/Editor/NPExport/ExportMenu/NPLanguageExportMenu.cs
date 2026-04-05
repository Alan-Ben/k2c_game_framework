using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ALPackage;
using SQLite4Unity3d;
using UnityEditor;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using Excel;
using GOE;


namespace GOE
{
	//导出语言表
	public class LanguageExportMenuBase : NPBasicExportMenuItemEX
	{
	    protected _ILanuageAsset _lanuageAsset;
	    protected List<ENPExportSettingEnum> _m_expandTabList;

	    protected override string _menuText
	    {
	        get
	        {
	            return "语言表导出";
	        }
	    }

	    public LanguageExportMenuBase(string _tag, ENPExportSettingEnum _exportEnum, Func<string, string, bool> _judgeCanShowFunc, List<ENPExportSettingEnum> _expandList = null)
	        : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        //设置资源文件路径和文件名
	        init();

	        _regSubItem(new ALTextItem("language", 15));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathItem("语 言 信 息 Excel 文 件：", exportEnum.ToString()));

	        //页签名
	        _m_expandTabList = _expandList;

	        _regSubItem(new NPComfirmItem("导 出", exportMap));

	        NPExportSettingMgr.instance.regSubExportSetting(exportEnum, exportMap, isSelect, _tag, _judgeCanShowFunc);
	    }

	    //设置资源文件路径和文件名
	    protected virtual void init()
	    {

	    }

	    protected override void _exExport()
	    {
	        exportMap();
	    }

	    private List<NPLanguageObj> filterDuplicateObj(List<NPLanguageObj> _srcLangList)
	    {
	        Dictionary<string, NPLanguageObj> langMap = new Dictionary<string, NPLanguageObj>();
	        NPLanguageObj tempRef = null;
	        for(int i = 0; i < _srcLangList.Count; ++i)
	        {
	            tempRef = _srcLangList[i];
	            if(!langMap.ContainsKey(tempRef.Id))
	                langMap.Add(tempRef.Id, tempRef);
	            else
	                Debug.LogError("存在重复的KEY： " + tempRef.Id);
	        }
	        return new List<NPLanguageObj>(langMap.Values);
	    }

	    //判断语言表key的合法性，防止策划大爷忘记了规则又配置错
	    private bool _checkLangKeyIsLegal(string _key)
	    {
	        //空字符就不用翻译了
	        if(_key == null)
	            return false;

	        //小于两个字符，没法翻译
	        if(_key.Length < 2)
	            return false;

        if(!_key[0].Equals('#'))
	            return false;

	        return true;
	    }

	    private void exportMap()
	    {
	        if(null == _lanuageAsset)
	        {
	            Debug.LogError("语言导出错误");
	            return;
	        }

	        string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if(string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError("ExcelPath is null or empty!!");
	            return;
	        }

	        //改成导出全部页签的翻译
	        Dictionary<ENPLanguage, List<NPLanguageObj>> tempMap = new Dictionary<ENPLanguage, List<NPLanguageObj>>();
	        List<string> sheetNameList = NPExportWnd.getAllSheetNameList(excelPath);

	        foreach(string sheetName in sheetNameList)
	        {
	            addLanguageMapByTabName(excelPath, sheetName, tempMap);
	        }

	        //Lambda 表达式去重复id, 返回结果只保留users这个List中重复的元素的第一个(id相等认为重复)
	        //mapRefObjList = mapRefObjList.Where<WCGLanguageObj>((obj, bResule) => mapRefObjList.FindIndex(item => item.Id == obj.Id) == bResule).ToList();
	        foreach(ENPLanguage langType in tempMap.Keys)
	        {
	            string assetName = _lanuageAsset.LanguageAssetObjName(langType);
	            string assetPath = _lanuageAsset.LanguageAssetPath(langType);
	            string dbPath = string.Format(@"{0}/Resources/Refdata/__DLExport/{1}.txt", Application.dataPath, assetName);

	            if(File.Exists(dbPath))
	                File.Delete(dbPath);
	            SQLiteConnection ds = new SQLiteConnection(dbPath);
	            try
	            {
	                List<NPLanguageObj> refObjList = null;
	                if(!tempMap.TryGetValue(langType, out refObjList))
	                    continue;
	                //过滤重复的
	                refObjList = filterDuplicateObj(refObjList);


	                // 尝试丢弃模板表，保证不出错丢弃此表
	                ds.DropTable<NPLanguageObj>();
	                // 先直接删除这个语言表
	                string del = string.Format("DROP TABLE if EXISTS \"{0}\"", langType.ToString());
	                var cmdDel = ds.CreateCommand(del);
	                cmdDel.ExecuteNonQuery();

	                // 创建模板表
	                ds.CreateTable<NPLanguageObj>();

	                // 把数据插入模板表
	                ds.InsertAll(refObjList);

	                // 把模板表用语言类型的名字重命名，会废弃掉模板表
	                TableMapping map = ds.GetMapping(typeof(NPLanguageObj));
	                string query = string.Format("ALTER TABLE {0} RENAME TO {1}", map.TableName, langType.ToString());
	                var cmd = ds.CreateCommand(query);

	                cmd.ExecuteNonQuery();
	                //提交事务
	                ds.Commit();
	            }
	            catch(Exception ex)
	            {
	                ds.Rollback();
	                Debug.LogException(ex);
	            }
	            finally
	            {
	                ds.Execute("VACUUM");
	                ds.Close();

	                AssetDatabase.Refresh();
	                AssetImporter assetImporter = AssetImporter.GetAtPath("Assets/Resources/Refdata/__DLExport/" + assetName + ".txt");

	                if(null == assetImporter)
	                {
	                    UnityEngine.Debug.LogError("get AssetImporter is null: " + "Assets/Resources/Refdata/__DLExport/" + assetPath);
	                }
	                else
	                {
	                    assetImporter.assetBundleName = assetPath.Replace(".unity3d", "");
	                    assetImporter.assetBundleVariant = "unity3d";
	                }
	            }
	        }
	    }

	    private void addLanguageMapByTabName(string _excelPath, string _tabName, Dictionary<ENPLanguage, List<NPLanguageObj>> _tempMap)
	    {
	        List<TempLanguageData> mapStoryRefObjList = NPExportWnd.readXls<TempLanguageData>(
	            _excelPath
	            , _tabName
	            , exportEnum
	            , (_obj, _line, _lineData) =>
	            {
	                lineValue = _lineData;
	                line = _line;

					//{}括号数量
                    int lBracketCount = 0;
                    int RBracketCount = 0;

					//把语言表key都转为小写，去掉头尾空格，防止傻逼策划配置错
					_obj.Id = GetString("id").ToLowerInvariant().Trim();
	                //字符串合法性判断
	                if(!_checkLangKeyIsLegal(_obj.Id))
	                {
                        Debug.LogError("语言表[" + GetString("id") + "]不合法!,字符不能为空或者null,字符串长度>=2, 第一个或者第二个字符必须为#");
	                }
	                //1.简体中文
	                _obj.Zh_cn = GetString("zh_cn");
                    ENPLanguage langType = ENPLanguage.ZH_CN;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Zh_cn));
                    if (!string.IsNullOrEmpty(_obj.Zh_cn))
                    {
                        lBracketCount = _obj.Zh_cn.Count(f => (f == '{'));
                        RBracketCount = _obj.Zh_cn.Count(f => (f == '}'));
						if(lBracketCount != RBracketCount)
                            Debug.LogError($"{_obj.Id}    [zh_cn]括号错误，请检查！value:{_obj.Zh_cn}");
					}

					//2.英语(美国)
					_obj.En_us = GetString("en_us");
                    langType = ENPLanguage.EN_US;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.En_us));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.En_us, langType);

					//3.俄语(俄罗斯)
					_obj.Ru_ru = GetString("ru_ru");
                    langType = ENPLanguage.RU_RU;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Ru_ru));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Ru_ru, langType);

					//4.日文
					_obj.Ja_jp = GetString("ja_jp");
                    langType = ENPLanguage.JA_JP;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Ja_jp));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Ja_jp, langType);

					//5.土耳其语
					_obj.Tr_tr = GetString("tr_tr");
                    langType = ENPLanguage.TR_TR;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Tr_tr));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Tr_tr, langType);

					//6德语
					_obj.De_de = GetString("de_de");
                    langType = ENPLanguage.DE_DE;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.De_de));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.De_de, langType);

					//7.法语
					_obj.Fr_fr = GetString("fr_fr");
                    langType = ENPLanguage.FR_FR;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Fr_fr));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Fr_fr, langType);

					//8.西班牙语
					_obj.Es_es = GetString("es_es");
                    langType = ENPLanguage.ES_ES;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Es_es));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Es_es, langType);

					//9.葡萄牙语
					_obj.Pt_pt = GetString("pt_pt");
                    langType = ENPLanguage.PT_PT;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Pt_pt));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Pt_pt, langType);

					//10.意大利语（意大利）
					_obj.It_it = GetString("it_it");
                    langType = ENPLanguage.IT_IT;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.It_it));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.It_it, langType);

					//11.繁体中文
					_obj.Zh_tw = GetString("zh_tw");
                    langType = ENPLanguage.ZH_TW;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Zh_tw));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Zh_tw, langType);

					//12.阿拉伯语
					_obj.Ar_ar = GetString("ar_ar");
                    langType = ENPLanguage.AR_AR;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Ar_ar));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Ar_ar, langType);

					//13.韩语
					_obj.Ko_kr = GetString("ko_kr");
                    langType = ENPLanguage.KO_KR;
					if (!_tempMap.ContainsKey(langType))
	                    _tempMap[langType] = new List<NPLanguageObj>();
	                _tempMap[langType].Add(new NPLanguageObj(_obj.Id, _obj.Ko_kr));
                    _checkBracketCount(lBracketCount, RBracketCount, _obj.Id, _obj.Ko_kr, langType);

				}, true);


			//输出txt文件
            NPExportWnd.WriteToTxtFile(_excelPath, _tabName, exportEnum);
        }

		//检查括号数量及参数数量
        private void _checkBracketCount(int _targetLCount,int _targetRCount, string _key, string _value, ENPLanguage _tag)
        {
            if (string.IsNullOrEmpty(_value))
                return;

			int lBracketCount = _value.Count(f => (f == '{'));
            int RBracketCount = _value.Count(f => (f == '}'));
			if (lBracketCount != RBracketCount || (_targetLCount == _targetRCount && (_targetLCount != lBracketCount || _targetRCount != RBracketCount)))
				Debug.LogError($"{_key}    [{_tag}]参数数量或括号错误，请检查！value:{_value}");
        }


		public class TempLanguageData
	    {
	        private string id;

	        private string zh_cn;//中文

	        private string en_us;//英语(美国)

	        private string ja_jp;//日文

	        private string tr_tr;//土耳其语

	        private string ru_ru;//俄语(俄罗斯)

	        private string de_de;//德语

	        private string it_it;//意大利语

	        private string pt_pt;//葡萄牙语

	        private string fr_fr;//法语

	        private string es_es;//西班牙语

	        private string zh_tw;//繁体中文

	        private string ar_ar;//阿拉伯语

	        private string ko_kr;//韩语

	        public string Id { get { return id; } set { id = value; } }

	        public string Zh_cn
	        {
	            get { return zh_cn; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                zh_cn = value;
	            }
	        }

	        public string En_us
	        {
	            get { return en_us; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                en_us = value;
	            }
	        }

	        public string Ja_jp
	        {
	            get { return ja_jp; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                ja_jp = value;
	            }
	        }

	        public string Tr_tr
	        {
	            get { return tr_tr; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                tr_tr = value;
	            }
	        }

	        public string Ru_ru
	        {
	            get { return ru_ru; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                ru_ru = value;
	            }
	        }

	        public string De_de
	        {
	            get { return de_de; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                de_de = value;
	            }
	        }

	        public string It_it
	        {
	            get { return it_it; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                it_it = value;
	            }
	        }

	        public string Pt_pt
	        {
	            get { return pt_pt; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                pt_pt = value;
	            }
	        }

	        public string Fr_fr
	        {
	            get { return fr_fr; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                fr_fr = value;
	            }
	        }

	        public string Es_es
	        {
	            get { return es_es; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                es_es = value;
	            }
	        }

	        public string Zh_tw
	        {
	            get { return zh_tw; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                zh_tw = value;
	            }
	        }

	        public string Ar_ar
	        {
	            get { return ar_ar; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                ar_ar = value;
	            }
	        }

	        public string Ko_kr
	        {
	            get { return ko_kr; }
	            set
	            {
	                if(!string.IsNullOrEmpty(value))
	                    value = value.Replace("\n", "").Replace("\r", "");
	                ko_kr = value;
	            }
	        }
	    }

	}

	/// <summary>
	/// 游戏内翻译表导出
	/// </summary>
	public class LanguageExportMenu : LanguageExportMenuBase
	{
	    public LanguageExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_tag, ENPExportSettingEnum.G_LANGUAGE, _judgeCanShowFunc, new List<ENPExportSettingEnum>(){ENPExportSettingEnum.G_LANGUAGE_EXPAND_1,
	                ENPExportSettingEnum.G_LANGUAGE_EXPAND_2,ENPExportSettingEnum.G_LANGUAGE_EXPAND_3,ENPExportSettingEnum.G_LANGUAGE_EXPAND_4})
	    {
	    }
	    protected override string _menuText
	    {
	        get
	        {
	            return "语言表(language)";
	        }
	    }
	    protected override void init()
	    {
	        _lanuageAsset = new LanuageAsset();
	    }
	}

	/// <summary>
	/// 平台翻译表导出
	/// </summary>
	public class PlatLanguageExportMenu : LanguageExportMenuBase
	{

	    public PlatLanguageExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_tag, ENPExportSettingEnum.P_LANGUAGE, _judgeCanShowFunc)
	    {
	    }
	    protected override string _menuText
	    {
	        get
	        {
	            return "平台语言表导出";
	        }
	    }

	    protected override void init()
	    {
	        exportEnum = ENPExportSettingEnum.P_LANGUAGE;
	        _lanuageAsset = new PlatLanuageAsset();

	    }

	}

}