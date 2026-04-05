using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class SDFGenerator : EditorWindow
{
	private struct Point
	{
		public int dx;
		public int dy;

		public Point(int x, int y)
		{
			dx = x;
			dy = y;
		}

		public int DistSq()
		{
			return dx * dx + dy * dy;
		}

		public static Point inside = new Point(0, 0);
		public static Point empty = new Point(9999, 9999);
	}


	private Texture2D m_SourceTexture = null;
	private int m_GridWidth = 0;
	private int m_GridHeight = 0;
	private SDFTextureType _m_eTextureType = SDFTextureType.ALPHA;
	private bool _m_SaveCover = false;
	private float m_ValueScale = 3;
	private bool m_IsBlur = true;

	enum BlurType
	{
		Gaussian,
		Simple
	}

	private BlurType m_BlurType;
	public int m_BlurSize = 2; // Size of the blur kernel (odd number)
	public int m_BlurIterations = 4; // Number of iterations for the blur effect

	private enum SDFTextureType
	{
		ALPHA,
		RED,
		COLOR_ALPHA,
	}

	[MenuItem("Tools/Outline/SDF Generator")]
	public static void OpenWindow()
	{
		EditorWindow.GetWindow<SDFGenerator>(true, "SDF Generator");
	}

	private void OnGUI()
	{
		m_SourceTexture = (Texture2D)EditorGUILayout.ObjectField("Source Texture", m_SourceTexture, typeof(Texture2D), false);

		var enabled = GUI.enabled;
		if (m_SourceTexture == null)
			GUI.enabled = false;

		string typeLabel = "导出贴图类型:";
		switch (_m_eTextureType)
		{
			case SDFTextureType.ALPHA:
				typeLabel += "使用Alpha 通道";
				break;
			case SDFTextureType.RED:
				typeLabel += "使用R通道";
				break;
			case SDFTextureType.COLOR_ALPHA:
				typeLabel += "使用Alpha 通道并保留颜色图";
				break;
		}
		_m_eTextureType = (SDFTextureType)EditorGUILayout.EnumPopup(typeLabel, _m_eTextureType);
		_m_SaveCover = EditorGUILayout.Toggle("是否覆盖原图", _m_SaveCover);
		m_ValueScale = EditorGUILayout.FloatField("渐变缩放，值越高，精度越高，边缘越小", m_ValueScale);
		m_IsBlur = EditorGUILayout.Toggle("是否模糊", m_IsBlur);
		if (m_IsBlur)
		{
			m_BlurType = (BlurType)EditorGUILayout.EnumPopup("模糊类型", m_BlurType);
			m_BlurSize = EditorGUILayout.IntSlider("模糊半径", m_BlurSize, 1, 10);
			m_BlurIterations = EditorGUILayout.IntSlider("模糊迭代次数", m_BlurIterations, 1, 10);
		}
		EditorGUILayout.Space();
		if (GUILayout.Button("Generate"))
			Generate();
		GUI.enabled = enabled;
	}

	private void Generate()
	{
		string texPath = AssetDatabase.GetAssetPath(m_SourceTexture);
		TextureImporter textureImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

		bool isReadable = textureImporter.isReadable;
		if (!isReadable)
		{
			textureImporter.isReadable = true;
			textureImporter.SaveAndReimport();
		}
		m_GridWidth = m_SourceTexture.width + 2;
		m_GridHeight = m_SourceTexture.height + 2;
		Point[] grid1 = new Point[m_GridWidth * m_GridHeight];
		Point[] grid2 = new Point[m_GridWidth * m_GridHeight];

		Color32[] pixels = m_SourceTexture.GetPixels32();

		for (int y = 0; y < m_GridWidth; y++)
			for (int x = 0; x < m_GridHeight; x++)
			{
				int gIdx = y * m_GridWidth + x;
				int sIdx = (y - 1) * m_SourceTexture.width + (x - 1);
				if (x == 0 || y == 0 || x == (m_GridWidth - 1) || y == (m_GridHeight - 1))
				{
					grid1[gIdx] = Point.empty;
					grid2[gIdx] = Point.empty;
				}
				else
				{
					int c = pixels[sIdx].a;
					// Debug.Log($"{y},{x}Alpha--({c})");
					switch (_m_eTextureType)
					{
						case SDFTextureType.ALPHA:
							c = pixels[sIdx].a;
							break;
						case SDFTextureType.RED:
							c = pixels[sIdx].r;
							break;
						case SDFTextureType.COLOR_ALPHA:
							c = pixels[sIdx].a;
							break;
					}
					if (c < 128)
					{
						grid1[gIdx] = Point.inside;
						grid2[gIdx] = Point.empty;
					}
					else
					{
						grid1[gIdx] = Point.empty;
						grid2[gIdx] = Point.inside;
					}
				}
			}
						
		GenerateSDF(grid1);
		GenerateSDF(grid2);


		if (_m_SaveCover)
		{
			string projectPath = Application.dataPath;
			projectPath = projectPath.Substring(0, projectPath.Length - 6);
			string filePath = Path.Combine(projectPath, texPath);
			SaveTex(grid1, grid2, textureImporter, filePath);
		}
		else
		{
			string filePath = EditorUtility.SaveFilePanel("Save Signed Distance Field", 
				new FileInfo(AssetDatabase.GetAssetPath(m_SourceTexture)).DirectoryName,
				m_SourceTexture.name + " SDF", 
				"png");
			if (!string.IsNullOrEmpty(filePath))
			{
				SaveTex(grid1, grid2, textureImporter, filePath);
			}
		}

		if (!isReadable)
		{
			textureImporter.isReadable = false;
			textureImporter.SaveAndReimport();
		}
	}

	private void SaveTex(Point[] grid1, Point[] grid2 ,TextureImporter textureImporter , string filePath)
	{
		// Generate final texture
		Color[] pixels = new Color[m_SourceTexture.width * m_SourceTexture.height];

		for (int y = 0; y < m_SourceTexture.height; y++)
			for (int x = 0; x < m_SourceTexture.width; x++)
			{
				int dist1 = (int)Mathf.Sqrt((float)Get(grid1, x + 1, y + 1).DistSq());
				int dist2 = (int)Mathf.Sqrt((float)Get(grid2, x + 1, y + 1).DistSq());
				int dist = Mathf.Clamp(128 + (int)Mathf.Round((dist1 - dist2) * m_ValueScale), 0, 255);
				switch (_m_eTextureType)
				{
					case SDFTextureType.ALPHA:
						pixels[y * m_SourceTexture.width + x] = new Color32((byte)dist, (byte)dist, (byte)dist, (byte)dist);
						break;
					case SDFTextureType.RED:
						dist = 255 - dist;
						pixels[y * m_SourceTexture.width + x] = new Color32((byte)dist, (byte)dist, (byte)dist, (byte)dist);
						break;
					case SDFTextureType.COLOR_ALPHA:
						var srcColor = m_SourceTexture.GetPixel(x, y);
						pixels[y * m_SourceTexture.width + x] = new Color32((byte)(255*srcColor.r), (byte)(255*srcColor.g), (byte)(255*srcColor.b), (byte)dist);
						break;
				}
			}

		Texture2D dest = new Texture2D(m_SourceTexture.width, m_SourceTexture.height);

		if (m_IsBlur)
		{
			Color[] blurPixels = new Color[m_SourceTexture.width * m_SourceTexture.height];
			switch (m_BlurType)
			{
				case BlurType.Gaussian:
					BlurUtil.GaussianBlur(pixels,blurPixels, m_SourceTexture.width, m_SourceTexture.height, m_BlurSize, m_BlurIterations, _m_eTextureType == SDFTextureType.COLOR_ALPHA);
					break;
				case BlurType.Simple:
					BlurUtil.SimpleBlur(pixels,blurPixels, m_SourceTexture.width, m_SourceTexture.height, m_BlurSize, m_BlurIterations, _m_eTextureType == SDFTextureType.COLOR_ALPHA);
					break;
			}
		
			dest.SetPixels(blurPixels);
		}
		else
		{
			dest.SetPixels(pixels);
		}
		
		dest.Apply();

		File.WriteAllBytes(filePath, dest.EncodeToPNG());
		AssetDatabase.Refresh();
		
		string unityPath = filePath.Remove(0, Application.dataPath.Length - 6);

		
		///注释格式设置代码，使用项目格式
		// var importer = AssetImporter.GetAtPath(unityPath) as TextureImporter;
		//
		// if (importer != null)
		// {
		// 	importer.textureType = TextureImporterType.Sprite;
		// 	importer.spritePivot = textureImporter.spritePivot;
		// 	TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();
		// 	switch (_m_eTextureType)
		// 	{
		// 		case SDFTextureType.ALPHA:
		// 			settings.format = TextureImporterFormat.Alpha8;
		// 			settings.overridden = true;
		// 			settings.name = "Standalone";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			settings.name = "iPhone";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			settings.name = "Android";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			break;
		// 		case SDFTextureType.RED:
		// 			settings.format = TextureImporterFormat.R8;
		// 			settings.overridden = true;
		// 			settings.name = "Standalone";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			settings.name = "iPhone";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			settings.name = "Android";
		// 			importer.SetPlatformTextureSettings(settings);
		// 			break;
		// 		case SDFTextureType.COLOR_ALPHA:
		// 			break;
		// 	}
		// 	importer.SaveAndReimport();
		// 	importer.mipmapEnabled = false;
		// }
		// importer.SetPlatformTextureSettings("Standard", 2048, TextureImporterFormat.Alpha8);
		
		
		DestroyImmediate(dest);
	}
	private void GenerateSDF(Point[] grid)
	{
		// Pass 0
		for (int y = 1; y < (m_GridHeight - 1); y++)
		{
			for (int x = 1; x < (m_GridWidth - 1); x++)
			{
				Point p = Get(grid, x, y);
				p = Compare(grid, p, x, y, -1,  0);
				p = Compare(grid, p, x, y,  0, -1);
				p = Compare(grid, p, x, y, -1, -1);
				p = Compare(grid, p, x, y,  1, -1);
				Put(grid, x, y, p);
			}

			for (int x = (m_GridWidth - 2); x >= 1; x--)
			{
				Point p = Get(grid, x, y);
				p = Compare(grid, p, x, y, 1, 0);
				Put(grid, x, y, p);
			}
		}

		// Pass 1
		for (int y = (m_GridHeight - 2); y >= 1; y--)
		{
			for (int x = (m_GridWidth - 2); x >= 0; x--)
			{
				Point p = Get(grid, x, y);
				p = Compare(grid, p, x, y,  1,  0);
				p = Compare(grid, p, x, y,  0,  1);
				p = Compare(grid, p, x, y, -1,  1);
				p = Compare(grid, p, x, y,  1,  1);
				Put(grid, x, y, p);
			}

			for (int x = 1; x < (m_GridWidth - 1); x++)
			{
				Point p = Get(grid, x, y);
				p = Compare(grid, p, x, y, -1, 0);
				Put(grid, x, y, p);
			}
		}
	}

	private Point Get(Point[] grid, int x, int y)
	{
		return grid[y * m_GridWidth + x];
	}

	private void Put(Point[] grid, int x, int y, Point point)
	{
		grid[y * m_GridWidth + x] = point;
	}

	private Point Compare(Point[] grid, Point point, int x, int y, int offsetx, int offsety)
	{
		Point other = Get(grid, x + offsetx, y + offsety);
		other.dx += offsetx;
		other.dy += offsety;
		
		if (other.DistSq() < point.DistSq())
			return other;
		return point;
	}
}
