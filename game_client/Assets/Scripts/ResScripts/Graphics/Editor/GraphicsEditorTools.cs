
using UnityEditor;
using UnityEngine;

public class GraphicsEditorTools
{
    private static Color RGBMEncode(Color hdr, float MaxValue)
    {
        Color c = new Color();
        c.r = Mathf.Sqrt(hdr.r);
        c.g = Mathf.Sqrt(hdr.g);
        c.b = Mathf.Sqrt(hdr.b);
        c *= 1.0f / MaxValue;

        Color rgbm;
        rgbm.a = Mathf.Clamp01(c.maxColorComponent);
        rgbm.a = Mathf.Ceil(rgbm.a * 255.0f) / 255.0f;
        rgbm.r = c.r / rgbm.a;
        rgbm.g = c.g / rgbm.a;
        rgbm.b = c.b / rgbm.a;
        return rgbm;
    }

    // [MenuItem("Assets/ChangeHDRTexToRGBA")]
    public static void changeHDRTexToRGBA()
    {
        foreach (var obj in Selection.objects)
        {
            var tex = obj as Cubemap;
            if (tex != null)
            {
                Cubemap nmap = new Cubemap(tex.width, TextureFormat.RGBA32, tex.mipmapCount);

                for (int k = 0; k < tex.mipmapCount; k++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var colors = tex.GetPixels((CubemapFace)i, k);
                        for (int j = 0; j < colors.Length; j++)
                        {
                            colors[j] = RGBMEncode(colors[j], 16);//new Color(colors[j].r, colors[j].g, colors[j].b, colors[j].a);
                        }
                        
                        nmap.SetPixels(colors, (CubemapFace)i, k);
                    }
                }
              
                nmap.Apply(false);
                
                var path = AssetDatabase.GetAssetPath(tex) + "_Mono.cubemap";
                AssetDatabase.CreateAsset(nmap, path);
                AssetDatabase.Refresh();
            }
        }
    }
    
    [MenuItem("Tools/Graphics/ChangeHDRCubeMapToRGBM")]
    public static void changeHDRTexToRGBA2()
    {
        foreach (var obj in Selection.objects)
        {
            var tex = obj as Cubemap;
            if (tex != null)
            {
                SaveCubemapToPNG(tex, AssetDatabase.GetAssetPath(tex) + "_Mono.png");
            }
        }
    }
    
    [MenuItem("Tools/Graphics/ChangeHDRTexToRGBM")]
    public static void changeHDRTexToRGBM()
    {
        foreach (var obj in Selection.objects)
        {
            var tex = obj as Texture2D;
            if (tex != null)
            {
                string path = AssetDatabase.GetAssetPath(tex) + "_RGBM.png";
                Texture2D ntex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);

               
                Color[] pixels = tex.GetPixels();

                Color[] nPixels = new Color[pixels.Length];
                for (int i = 0; i < pixels.Length; i++)
                {
                    nPixels[i] = RGBMEncode(pixels[i], 16);
                }
                
                ntex.SetPixels(0, 0, tex.width, tex.height, nPixels);
                ntex.Apply();

                byte[] bytes = ntex.EncodeToPNG();
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log("Texture saved as PNG to: " + path);
            }
        }
    }
    
    static void SaveCubemapToPNG(Cubemap cubemap, string path)
    {
        Texture2D tex = new Texture2D(cubemap.width * 6, cubemap.height, TextureFormat.RGB24, false);

        // Loop through each face of the cubemap and copy pixels to the texture
        for (int faceIndex = 0; faceIndex < 6; faceIndex++)
        {
            Color[] pixels = cubemap.GetPixels((CubemapFace)faceIndex);

            // Create an array to store flipped pixels
            Color[] flippedPixels = new Color[pixels.Length];

            // Loop through each row of pixels in the original texture
            for (int y = 0; y < cubemap.height; y++)
            {
                // Loop through each pixel in the row
                for (int x = 0; x < cubemap.width; x++)
                {
                    // Calculate the index of the current pixel in the original array
                    int originalIndex = y * cubemap.width + x;

                    // Calculate the flipped y-coordinate
                    int flippedY = cubemap.height - y - 1;

                    // Calculate the index of the corresponding pixel in the flipped array
                    int flippedIndex = flippedY * cubemap.width + x;

                    // Store the pixel color in the flipped array
                    flippedPixels[flippedIndex] = RGBMEncode(pixels[originalIndex], 16);
                }
            }
            int startX = faceIndex % 6 * cubemap.width;
            tex.SetPixels(startX, 0, cubemap.width, cubemap.height, flippedPixels);
        }
        
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Cubemap saved as PNG to: " + path);
    }
}
