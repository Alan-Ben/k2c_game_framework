using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ALPackage;
using NPEnum;


namespace GOE
{
	public class NPAudioExportMenu : NPBasicExportMenuItemEX
	{
		private string assetPath = "";
	    private string objName = "";

	    public NPAudioExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_tag, ENPExportSettingEnum.AUDIO, _judgeCanShowFunc)
	    {
	        assetPath = NPGSOAudioRefSet.assetPath;
	        objName = NPGSOAudioRefSet.objName;

	        _regSubItem(new ALTextItem("音 频 配 音 信 息 。", 15));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", exportEnum.ToString()));
	        _regSubItem(new NPComfirmItem("导 出", exportSOAudioRefSet));

	        NPExportSettingMgr.instance.regSubExportSetting(exportEnum, exportSOAudioRefSet, isSelect, _tag, _judgeCanShowFunc);
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "音频配音信息(Audio)"; } }

	    protected override void _exExport()
	    {
		    exportSOAudioRefSet();
	    }

	    //导出函数
	    private void exportSOAudioRefSet()
	    {
	        //读取对应的excel文件
	        string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if (string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError("ExcelPath is null or empty!!");
	            return;
	        }

	        //构建处理对象
	        NPExportWnd.NPExcelReadingDealObj dealObj = new NPExportWnd.NPExcelReadingDealObj();

	        if (string.IsNullOrEmpty(objName) || string.IsNullOrEmpty(assetPath))
	        {
	            Debug.LogError($"音频配音信息(Audio):Ref Set 导出失败，包名为空，objName:{objName} assetPath:{assetPath}");
	            return;
	        }
	        
	        NPExportWnd.NPExcelReadingFuncInfo<NPAudioRefObj> audioRefFunc =
	            new NPExportWnd.NPExcelReadingFuncInfo<NPAudioRefObj>(
		            "audio_ref"
	                , exportEnum
	                , () => { return new NPAudioRefObj(); }
	               , (_obj, _line, _lineData) =>
	               {
	                   lineValue = _lineData;
	                   line = _line;

	                   _obj.id = GetLong("id");
	                   _obj.audio_index = new NPGAudioIndex();
	                   _obj.audio_index.readIndex(GetString("audio_index"));
	                   _obj.audio_path = GetString("audio_path");
	                   _obj.duration = GetLong("duration");
	                   _obj.min_cache_count = GetInt("min_cache_count");
	                   _obj.max_cache_count = GetInt("max_cache_count");
	                   _obj.fade_in_time = GetFloat("fade_in_time");
	                   _obj.fade_out_time = GetFloat("fade_out_time");
	                   _obj.fade_in_tar_value = GetFloat("fade_in_tar_value");
	                   _obj.audio_group_id = GetLong("audio_group_id");
	                   _obj.is_ignore_screen_distance = GetBool("is_ignore_screen_distance");

	                   _obj.is_language_audio = false;//audio_ref表中所有数据都不是语言音频
	                   _obj.is_async_audio = GetBool("is_async_audio");
	               });
	        dealObj.addDealObj(audioRefFunc);

	        NPExportWnd.NPExcelReadingFuncInfo<NPAudioRefObj> voiceRefFunc =
	            new NPExportWnd.NPExcelReadingFuncInfo<NPAudioRefObj>(
	                "voice"
	                , exportEnum
	                , () => { return new NPAudioRefObj(); }
	               , (_obj, _line, _lineData) =>
	               {
		               lineValue = _lineData;
		               line = _line;

		               _obj.id = GetLong("id");
		               _obj.audio_index = new NPGAudioIndex();
		               _obj.audio_index.readIndex(GetString("audio_index"));
		               _obj.audio_path = GetString("audio_path");
		               _obj.duration = GetLong("duration");
		               _obj.min_cache_count = GetInt("min_cache_count");
		               _obj.max_cache_count = GetInt("max_cache_count");
		               _obj.fade_in_time = GetFloat("fade_in_time");
		               _obj.fade_out_time = GetFloat("fade_out_time");
		               _obj.fade_in_tar_value = GetFloat("fade_in_tar_value");
		               _obj.audio_group_id = GetLong("audio_group_id");
		               _obj.is_ignore_screen_distance = GetBool("is_ignore_screen_distance");

		               _obj.is_language_audio = true;//voice表中所有数据都是语言音频
		               _obj.is_async_audio = GetBool("is_async_audio");
	               });
	        dealObj.addDealObj(voiceRefFunc);

	        //开始处理
	        NPExportWnd.readXls(excelPath, dealObj);

	        if (audioRefFunc.dataList == null || audioRefFunc.dataList.Count == 0)
	        {
		        Debug.LogError(string.Format("音效excel导出工作表sheetName({0}), 没有数据!", audioRefFunc.sheetName));
	        }

	        if (voiceRefFunc.dataList == null || voiceRefFunc.dataList.Count == 0)
	        {
		        Debug.LogError(string.Format("音效excel导出工作表sheetName({0}), 没有数据!", voiceRefFunc.sheetName));
	        }
	        
	        Dictionary<long, NPAudioRefObj> audioMap = new Dictionary<long, NPAudioRefObj>();
	        NPAudioRefObj tempAudioRef = null;
	        if (audioRefFunc.dataList != null)
	        {
		        for (int i = 0; i < audioRefFunc.dataList.Count; ++i)
		        {
			        tempAudioRef = audioRefFunc.dataList[i];
			        if (!audioMap.ContainsKey(tempAudioRef.id))
				        audioMap.Add(tempAudioRef.id, tempAudioRef);
		        }
	        }
	        if (voiceRefFunc.dataList != null)
	        {
		        for (int i = 0; i < voiceRefFunc.dataList.Count; ++i)
		        {
			        tempAudioRef = voiceRefFunc.dataList[i];
			        if (!audioMap.ContainsKey(tempAudioRef.id))
				        audioMap.Add(tempAudioRef.id, tempAudioRef);
		        }
	        }
	        
	        NPGSOAudioRefSet refSet = ScriptableObject.CreateInstance<NPGSOAudioRefSet>();
	        refSet.refList = new List<NPAudioRefObj>(audioMap.Values);
	        ALBasicExportFunction.exportAsset(refSet, objName, assetPath, "unity3d");

	        //输出导出完成
	        Debug.LogWarning(string.Format("音 频 配 音 信 息: Ref Set  导出完成! {0} : {1}", assetPath, objName));
	    }
	}
}