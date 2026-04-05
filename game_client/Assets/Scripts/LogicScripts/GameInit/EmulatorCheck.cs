using System;
using System.Linq;
using UnityEngine;

public static class EmulatorCheck {
#if UNITY_ANDROID && !UNITY_EDITOR
    /// <summary>
    /// 安卓环境下检测设备是否为模拟器
    /// </summary>
    /// <returns></returns>
    public static bool IsProbablyEmulator() {
        if (Application.platform != RuntimePlatform.Android) return false;

        try {
            // ---- Build.* 线索（官方公开字段）
            using (var Build = new AndroidJavaClass("android.os.Build")) {
                string fingerprint   = Build.GetStatic<string>("FINGERPRINT")   ?? "";
                string model         = Build.GetStatic<string>("MODEL")         ?? "";
                string brand         = Build.GetStatic<string>("BRAND")         ?? "";
                string device        = Build.GetStatic<string>("DEVICE")        ?? "";
                string product       = Build.GetStatic<string>("PRODUCT")       ?? "";
                string manufacturer  = Build.GetStatic<string>("MANUFACTURER")  ?? "";
                string hardware      = Build.GetStatic<string>("HARDWARE")      ?? "";
                string[] abis        = Build.GetStatic<string[]>("SUPPORTED_ABIS");

                bool suspectBuild =
                    fingerprint.StartsWith("generic", StringComparison.OrdinalIgnoreCase) ||
                    fingerprint.StartsWith("unknown", StringComparison.OrdinalIgnoreCase) ||
                    model.Contains("google_sdk", StringComparison.OrdinalIgnoreCase) ||
                    model.Contains("Emulator", StringComparison.OrdinalIgnoreCase) ||
                    model.Contains("Android SDK built for", StringComparison.OrdinalIgnoreCase) ||
                    (brand.StartsWith("generic", StringComparison.OrdinalIgnoreCase) &&
                     device.StartsWith("generic", StringComparison.OrdinalIgnoreCase)) ||
                    product.Contains("sdk_gphone", StringComparison.OrdinalIgnoreCase) ||
                    product.Contains("google_sdk", StringComparison.OrdinalIgnoreCase) ||
                    product.Contains("emulator", StringComparison.OrdinalIgnoreCase) ||
                    product.Contains("simulator", StringComparison.OrdinalIgnoreCase) ||
                    hardware.Contains("goldfish", StringComparison.OrdinalIgnoreCase) ||   // 经典模拟器内核代号
                    hardware.Contains("ranchu",   StringComparison.OrdinalIgnoreCase) ||   // 新一代模拟器内核代号
                    hardware.Contains("vbox",     StringComparison.OrdinalIgnoreCase);     // 基于VirtualBox系
                if (suspectBuild) return true;

                // x86/x86_64 ABI（真机极少；命中可提高“可疑度”）
                if (abis != null && abis.Any(a => !string.IsNullOrEmpty(a) && a.Contains("x86", StringComparison.OrdinalIgnoreCase)))
                    return true;
            }

            // ---- ro.kernel.qemu=1（QEMU标志）
            using (var SysProp = new AndroidJavaClass("android.os.SystemProperties")) {
                string qemu = SysProp.CallStatic<string>("get", "ro.kernel.qemu", "0");
                if (qemu == "1") return true;
            }

            // ---- ANDROID_ID 为空（常见于早期/部分模拟器镜像）
            using (var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var resolver = activity.Call<AndroidJavaObject>("getContentResolver"))
            using (var Secure = new AndroidJavaClass("android.provider.Settings$Secure")) {
                string androidId = Secure.CallStatic<string>("getString", resolver, "android_id");
                if (string.IsNullOrEmpty(androidId)) return true;
            }

            // ---- 网络运营商名为 "Android"（历史上常见于官方模拟器）
            using (var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                var telephony = activity.Call<AndroidJavaObject>("getSystemService", "phone");
                if (telephony != null) {
                    string op = telephony.Call<string>("getNetworkOperatorName");
                    if ("Android".Equals(op, StringComparison.OrdinalIgnoreCase)) return true;
                }
            }
        } catch { /* 保守返回 false，避免误伤真机 */ }

        return false;
    }
#else
    /// <summary>
    /// 安卓环境下检测设备是否为模拟器
    /// </summary>
    /// <returns></returns>
    public static bool IsProbablyEmulator() => false;
#endif
}
