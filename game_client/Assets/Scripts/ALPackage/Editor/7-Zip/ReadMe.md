# 工作流程
  1. 打包流程，资源用7z.exe压缩为7z格式文件，内容用lzma2算法压缩
  2. 游戏解包用7z插件中lzma模块解压 
    - 2.1 随包资源
       - Editor, IOS 直接文件->文件解压
       - Android  www读取，内存->文件解压 
    - 2.2 下载资源
       - 下载完成后，文件->文件解压   
  3. 当前工作流程支持 android, ios, editor(macos,windows)
  4. 目录：
      - 插件目录：plugins\LZMA, 里面包括Android,ios,mac,windows插件
      - 工具目录：ALPacake\Editor\7-zip
# 说明

  - 7z.exe一个控制台程序，用于7z格式的压缩和解压，是从www.7-zip.org下载的7z工具的一部分
 -  7z是一种文件格式，支持多种压缩算法，在wcg和盗梦slg中我们用7z文件格式，文件内容用lzma2算法压缩
 -  7z官网提供的sdk，csharp部分不支持生成7z文件格式，它只支持生成 lzma 算法产生的2进制格式文件，这是7z文件的子集，7z文件还包含一些文件头，所以用csharp sdk压缩的文件无法用7z插件解压。
 - lzma插件来源于https://www.assetstore.unity3d.com/cn/#!/content/12674，这个插件提供了包括lzma格式在内的多种解压原生api，如果不用它，我们必须自己编译for android和ios的原生动态链接库。
 - lzma格式 是著名的LZ77算法改进版本，lzma改进了压缩比，并保持高速解压和低内存消耗
 -  lzma2 是lzma的改进，它比lzma在压缩时提供了更好的多线程支持，还有其他一些改进
 -  7z.exe lzma格式的说明可以看工具目录的帮助文档