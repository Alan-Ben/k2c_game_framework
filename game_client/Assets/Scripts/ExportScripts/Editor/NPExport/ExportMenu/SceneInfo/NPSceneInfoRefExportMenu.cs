using System;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;



namespace GOE
{
	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public class NPSceneInfoRefExportMenu : NPBasicExportMenuItemEX
	{
	    private string assetPath = "";
	    private string objName = "";

	    public NPSceneInfoRefExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_tag, ENPExportSettingEnum.SCENE_INFO, _judgeCanShowFunc)
	    {
	        assetPath = NPGSOSceneInfoRefSet.assetPath;
	        objName = NPGSOSceneInfoRefSet.objName;

	        _regSubItem(new ALTextItem("主  场  景  信  息。", 15));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", exportEnum.ToString()));
	        _regSubItem(new NPComfirmItem("导 出", exportSOSceneInfoRefSet));

	        NPExportSettingMgr.instance.regSubExportSetting(exportEnum, exportSOSceneInfoRefSet, isSelect, _tag, _judgeCanShowFunc);
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "主场景信息(scene info)"; } }

	    protected override void _exExport()
	    {
	        exportSOSceneInfoRefSet();
	    }

	    //导出函数
	    private void exportSOSceneInfoRefSet()
	    {
	        if (string.IsNullOrEmpty(objName) || string.IsNullOrEmpty(assetPath))
	        {
	            Debug.LogError($"主场景信息:Ref Set 导出失败，包名为空，objName:{objName} assetPath:{assetPath}");
	            return;
	        }

	        //读取对应的excel文件
	        string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if(string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError("ExcelPath is null or empty!!");
	            return;
	        }

	        //构建处理对象
	        NPExportWnd.NPExcelReadingDealObj dealObj = new NPExportWnd.NPExcelReadingDealObj();

	        string tabName = "scene_info";
	        NPExportWnd.NPExcelReadingFuncInfo<SceneInfoRefObj> sceneInfoRefFunc =
	            new NPExportWnd.NPExcelReadingFuncInfo<SceneInfoRefObj>(
	                tabName
	                , exportEnum
	                , () => { return new SceneInfoRefObj(); }
	               , (_obj, _line, _lineData) =>
	               {
	                   lineValue = _lineData;
	                   line = _line;

	                   _obj.id = GetLong("id");
	                   _obj.name = GetString("name");
	                   _obj.scene_icon = new NPGTextureIndex();
	                   _obj.scene_icon.readIndex(GetString("scene_icon"));

	                   _obj.td_scene_idx = new NPGSceneIndex();
	                   _obj.td_scene_idx.readIndex(GetString("td_scene_idx"));

	                   _obj.corner_pos = ExportEditorUtility.readXlsVector3String(GetString("corner_pos"), "corner_pos");
	                   _obj.terr_idx = new NPGTerrainIndex();
	                   _obj.terr_idx.readIndex(GetString("terr_idx"));

	                   //摄像头配置
	                   _obj.camera_setting = new WCGCameraSettingInfo();
	                   _obj.camera_setting.cameraPosition = ExportEditorUtility.readXlsVector3String(GetString("cameraPosition"), "cameraPosition");
	                   _obj.camera_setting.cameraFocusPosition = ExportEditorUtility.readXlsVector3String(GetString("cameraFocusPosition"), "cameraFocusPosition");
	                   _obj.camera_setting.cameraFieldOfView = GetFloat("cameraFieldOfView");
	                   _obj.camera_setting.isOrthographic = GetBool("isOrthographic");
	                   _obj.camera_setting.orthographicSize = GetFloat("orthographicSize");
	                   _obj.camera_setting.originSize = GetFloat("originSize");
	                   _obj.camera_setting.clippingNear = GetFloat("clippingNear");
	                   _obj.camera_setting.clippingFar = GetFloat("clippingFar");

	                   _obj.min_org_scale = GetFloat("min_org_scale");
	                   _obj.max_org_scale = GetFloat("max_org_scale");

	                   _obj.map_drag_rect = GetRect("map_drag_rect");
	                   _obj.map_border_scale_size = GetVector2("map_border_scale_size");
	                   _obj.move_type = (ESceneMoveType) GetEnum("move_type", typeof(ESceneMoveType));
	                   _obj.use_drag_tip_wnd = GetBool("use_drag_tip_wnd");
	                   _obj.drag_tip_show_distance = GetVector2("drag_tip_show_distance");
	                   _obj.air_friction = GetFloat("air_friction");
	                   _obj.slide_friction = GetFloat("slide_friction");

	                   //场景效果配置
	                   _obj.dir_lights_so_index = new GLightGoIndex();
	                   _obj.dir_lights_so_index.readIndex(GetString("dir_lights_so_index"));
	                   _obj.urp_render_index = GetInt("urp_render_index");

	               });
	        dealObj.addDealObj(sceneInfoRefFunc);

	        //开始处理
	        NPExportWnd.readXls(excelPath, dealObj);

	        NPGSOSceneInfoRefSet refSet = ScriptableObject.CreateInstance<NPGSOSceneInfoRefSet>();
	        refSet.refList = new List<SceneInfoRefObj>(sceneInfoRefFunc.dataList);
	        ALBasicExportFunction.exportAsset(refSet, objName, assetPath, "unity3d");
	        //输出导出完成
	        Debug.LogWarning(string.Format("主  场  景  信  息: Ref Set  导出完成! {0} : {1}", assetPath, objName));
	    }
	}
      
}