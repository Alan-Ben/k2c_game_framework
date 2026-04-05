
using UnityEngine;

public class BlurUtil
{

    #region GaussianBlur

    public static void GaussianBlur(Texture2D source, Texture2D destination, int blurSize, int iterations)
    {
        Color[] srcPixels = source.GetPixels();
        Color[] destPixels = new Color[srcPixels.Length];

        // Pre-calculate Gaussian kernel
        float[] kernel = CalculateGaussianKernel(blurSize);

        // Apply horizontal blur
        for (int i = 0; i < iterations; i++)
        {
            ApplyGaussianBlur(srcPixels, destPixels, source.width, source.height, kernel, blurSize);
            Swap(ref srcPixels, ref destPixels); // Swap arrays for next iteration
        }

        // Apply vertical blur
        for (int i = 0; i < iterations; i++)
        {
            ApplyGaussianBlur(srcPixels, destPixels, source.width, source.height, kernel, blurSize, true);
            Swap(ref srcPixels, ref destPixels); // Swap arrays for next iteration
        }

        destination.SetPixels(destPixels);
        destination.Apply();
    }
    
    public static void GaussianBlur(Color[] srcPixels, Color[] destPixels, int width, int height, int blurSize, int iterations, bool onlyAlpha)
    {

        // Pre-calculate Gaussian kernel
        float[] kernel = CalculateGaussianKernel(blurSize);

        // Apply horizontal blur
        for (int i = 0; i < iterations; i++)
        {
            ApplyGaussianBlur(srcPixels, destPixels, width, height, kernel, blurSize, false, onlyAlpha);
            Swap(ref srcPixels, ref destPixels); // Swap arrays for next iteration
        }

        // Apply vertical blur
        for (int i = 0; i < iterations; i++)
        {
            ApplyGaussianBlur(srcPixels, destPixels, width, height, kernel, blurSize, true, onlyAlpha);
            Swap(ref srcPixels, ref destPixels); // Swap arrays for next iteration
        }
    }

    private static void ApplyGaussianBlur(Color[] sourcePixels, Color[] destinationPixels, int width, int height, float[] kernel, int blurSize, bool vertical = false, bool onlyAlpha = false)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelOffset = blurSize;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = 0, g = 0, b = 0, a = 0;
                float weightSum = 0;

                for (int ky = -kernelOffset; ky <= kernelOffset; ky++)
                {
                    for (int kx = -kernelOffset; kx <= kernelOffset; kx++)
                    {
                        int pixelX = Mathf.Clamp(x + kx, 0, width - 1);
                        int pixelY = Mathf.Clamp(y + ky, 0, height - 1);
                        Color pixel = sourcePixels[pixelY * width + pixelX];

                        float weight = kernel[(ky + kernelOffset) * kernelSize + (kx + kernelOffset)];

                        r += pixel.r * weight;
                        g += pixel.g * weight;
                        b += pixel.b * weight;
                        a += pixel.a * weight;

                        weightSum += weight;
                    }
                }

                int index = y * width + x;
                if (onlyAlpha)
                {
                    Color s = sourcePixels[index];
                    destinationPixels[index] = new Color(s.r, s.g, s.b, a / weightSum);
                }
                else
                    destinationPixels[index] = new Color(r / weightSum, g / weightSum, b / weightSum, a / weightSum);
            }
        }
    }

    private static float[] CalculateGaussianKernel(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        float[] kernel = new float[kernelSize * kernelSize];
        float sigma = blurSize / 3f;
        float twoSigmaSquare = 2.0f * sigma * sigma;
        float constant = 1.0f / (Mathf.PI * twoSigmaSquare);
        int kernelIndex = 0;
        float totalWeight = 0;

        for (int i = -blurSize; i <= blurSize; ++i)
        {
            for (int j = -blurSize; j <= blurSize; ++j)
            {
                float distance = i * i + j * j;
                kernel[kernelIndex] = constant * Mathf.Exp(-distance / twoSigmaSquare);
                totalWeight += kernel[kernelIndex];
                ++kernelIndex;
            }
        }

        // Normalize the kernel
        for (int i = 0; i < kernel.Length; ++i)
        {
            kernel[i] /= totalWeight;
        }

        return kernel;
    }

    private static void Swap(ref Color[] array1, ref Color[] array2)
    {
        Color[] temp = array1;
        array1 = array2;
        array2 = temp;
    }

    #endregion
   

    #region SimpleBlur

    public static void SimpleBlur(Texture2D source, Texture2D destination, int blurSize, int iterations)
    {
        Color[] srcPixels = source.GetPixels();
        Color[] destPixels = new Color[srcPixels.Length];

        for (int i = 0; i < iterations; i++)
        {
            ApplyBlur(srcPixels, destPixels, source.width, source.height, blurSize, false);
            ApplyBlur(destPixels, srcPixels, source.width, source.height, blurSize, false);
        }

        destination.SetPixels(destPixels);
        destination.Apply();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="srcPixels"></param>
    /// <param name="destPixels"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="blurSize"></param>
    /// <param name="iterations"></param>
    /// <param name="onlyAlpha">是否只模糊A通道</param>
    public static void SimpleBlur(Color[] srcPixels, Color[] destPixels, int width, int height, int blurSize, int iterations, bool onlyAlpha)
    {
        for (int i = 0; i < iterations; i++)
        {
            ApplyBlur(srcPixels, destPixels, width, height, blurSize, onlyAlpha);
            ApplyBlur(destPixels, srcPixels, width, height, blurSize, onlyAlpha);
        }
    }

    static void ApplyBlur(Color[] sourcePixels, Color[] destinationPixels, int width, int height, int blurSize, bool onlyAlpha)
    {
        int kernelOffset = (blurSize - 1) / 2;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Apply blur kernel
                float r = 0, g = 0, b = 0, a = 0;
                int blurPixelCount = 0;

                for (int ky = -kernelOffset; ky <= kernelOffset; ky++)
                {
                    for (int kx = -kernelOffset; kx <= kernelOffset; kx++)
                    {
                        int pixelX = Mathf.Clamp(x + kx, 0, width - 1);
                        int pixelY = Mathf.Clamp(y + ky, 0, height - 1);
                        Color pixel = sourcePixels[pixelY * width + pixelX];

                        r += pixel.r;
                        g += pixel.g;
                        b += pixel.b;
                        a += pixel.a;
                        blurPixelCount++;
                    }
                }

                r /= blurPixelCount;
                g /= blurPixelCount;
                b /= blurPixelCount;
                a /= blurPixelCount;

                int index = y * width + x;
                if (onlyAlpha)
                {
                    r = sourcePixels[index].r;
                    g = sourcePixels[index].g;
                    b = sourcePixels[index].b;
                }
                destinationPixels[index] = new Color(r, g, b, a);
            }
        }
    }

    #endregion
}
