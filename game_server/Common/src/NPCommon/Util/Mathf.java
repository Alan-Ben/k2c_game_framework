package NPCommon.Util;

public class Mathf
{
    //
    // Summary:
    //     ///
    //     Clamps a value between a minimum float and maximum float value.
    //     ///
    //
    // Parameters:
    //   value:
    //
    //   min:
    //
    //   max:
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    //
    // Summary:
    //     ///
    //     Clamps value between 0 and 1 and returns value.
    //     ///
    //
    // Parameters:
    //   value:
    public static float Clamp01(float value)
    {
        if (value < 0) return 0;
        if (value > 1) return 1;
        return value;

    }

    //
    // Summary:
    //     ///
    //     Returns the largest of two or more values.
    //     ///
    //
    // Parameters:
    //   a:
    //
    //   b:
    //
    //   values:
    public static int Max(int a, int b)
    {
        return Math.max(a, b);
    }

    //
    // Summary:
    //     ///
    //     Returns largest of two or more values.
    //     ///
    //
    // Parameters:
    //   a:
    //
    //   b:
    //
    //   values:
    public static float Max(float a, float b)
    {
        return Math.max(a, b);
    }

    // Summary:
    //     ///
    //     Returns the smallest of two or more values.
    //     ///
    //
    // Parameters:
    //   a:
    //
    //   b:
    //
    //   values:
    public static int Min(int a, int b)
    {
        return Math.min(a, b);
    }

    //
    // Summary:
    //     ///
    //     Returns the smallest of two or more values.
    //     ///
    //
    // Parameters:
    //   a:
    //
    //   b:
    //
    //   values:
    public static float Min(float a, float b)
    {
        return Math.min(a, b);
    }

    //
    // Summary:
    //     ///
    //     Returns the absolute value of value.
    //     ///
    //
    // Parameters:
    //   value:
    public static int Abs(int value)
    {
        return Math.abs(value);
    }

    //
    // Summary:
    //     ///
    //     Returns the absolute value of f.
    //     ///
    //
    // Parameters:
    //   f:
    public static float Abs(float f)
    {
        return Math.abs(f);
    }

    //
    // Summary:
    //     ///
    //     Returns square root of f.
    //     ///
    //
    // Parameters:
    //   f:
    public static float Sqrt(float f)
    {
        return (float) Math.sqrt(f);
    }

    //
    // Summary:
    //     ///
    //     Returns the sine of angle f in radians.
    //     ///
    //
    // Parameters:
    //   f:
    public static float Sin(float f)
    {
        return (float) Math.sin(f);
    }

    public static float Cos(float f)
    {
        return (float) Math.cos(f);
    }

    //
    // Summary:
    //     ///
    //     Returns the tangent of angle f in radians.
    //     ///
    //
    // Parameters:
    //   f:
    public static float Tan(float f)
    {
        return (float) Math.tan(f);
    }

    public static float Pow(float arg0, float arg1)
    {
        return (float) Math.pow(arg0, arg1);
    }

    public static int CeilToInt(float radius)
    {

        return (int) Math.ceil(radius);
    }

    public static boolean Approximately(float compareValue, float targetValue)
    {
        return Math.abs(compareValue - targetValue) < 0.00001;
    }

    public static int RoundToInt(float _f)
    {
        return Math.round(_f);
    }
}
