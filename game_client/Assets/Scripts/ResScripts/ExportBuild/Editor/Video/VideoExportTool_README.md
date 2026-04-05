# 视频批量导出工具使用说明

## 功能概述
Unity批量视频导出工具，支持使用FFmpeg将原始视频转换为VP8（WebM）格式。通过文件名/文件夹命名约定控制导出参数，并记录导出历史以避免重复导出。

## 快速开始

### 1. 安装FFmpeg
确保系统已安装FFmpeg并添加到PATH环境变量。测试方法：
```bash
ffmpeg -version
```

### 2. 配置导出参数
- 在Unity菜单中选择 `GOE > 视频工具 > 打开视频导出配置`
- 配置以下参数：
  - **Bat Script Path**: bat脚本路径（默认：`Scripts/video_export.bat`）
  - **Original Video Folder**: 原始视频文件夹路径（默认：`video_clip_original`）
  - **Export Video Folder**: 导出视频文件夹路径（默认：`video_clip_vp8`）
  - **Export Record Path**: 导出记录文件路径（默认：`video_clip_vp8/export_record.json`）
  - **Param Configs**: 参数配置列表（可添加自定义参数）

### 3. 组织视频文件
将原始视频放入配置的原始视频文件夹中。

### 4. 执行批量导出
在Unity菜单中选择 `GOE > 视频工具 > 批量导出视频 (VP8)`

## 参数配置规则

### 参数缩写标记
在文件名或文件夹名中使用 `--<参数缩写>` 来指定导出参数：
- `--` 作为参数缩写开始标识
- 以 `\` 或 `.` 作为结束标识
- 使用最靠近文件的参数缩写

### 示例
```
video_clip_original/
  ├── video1.mp4                     # 使用默认参数
  ├── video2--hd.mp4                 # 使用hd参数（文件名中指定）
  ├── folder--sd/                    # 文件夹指定sd参数
  │   ├── video3.mp4                 # 使用sd参数（继承文件夹）
  │   └── video4--ld.mp4             # 使用ld参数（文件名优先级更高）
  └── game--hd/
      └── level1--sd/
          └── video5.mp4             # 使用sd参数（最近的参数）
```

### 预设参数
默认配置包含以下参数：
- **default**: `-c:v libvpx -b:v 1M -crf 10` （默认质量）
- **hd**: `-c:v libvpx -b:v 2M -crf 8` （高清）
- **sd**: `-c:v libvpx -b:v 1M -crf 12` （标清）
- **ld**: `-c:v libvpx -b:v 500K -crf 15` （低质量）

可以在配置中添加自定义参数。

## 导出记录机制

### 记录内容
每次导出会记录以下信息：
- 原始视频文件名（不带`--参数`部分）
- 使用的参数缩写
- 原始视频文件大小
- 原始视频最后修改时间
- 相对路径

### 跳过逻辑
如果导出路径已存在视频文件，且记录信息完全一致，则跳过导出。

### 触发重新导出的情况
- 导出文件不存在
- 没有导出记录
- 原始视频内容发生变化（文件大小或修改时间不同）
- 使用的参数缩写不同
- 文件名发生变化

## 菜单功能

### GOE > 视频工具 > 批量导出视频 (VP8)
执行批量导出，会显示：
- 总文件数
- 导出数量
- 跳过数量
- 失败数量

### GOE > 视频工具 > 打开视频导出配置
打开ScriptableObject配置文件进行编辑。

### GOE > 视频工具 > 清空导出记录
清空所有导出记录，下次导出时将重新导出所有视频。

## 导出流程

1. 扫描原始视频文件夹，查找所有视频文件
2. 对每个视频：
   - 提取参数缩写（从文件名和路径）
   - 生成清理后的文件名（移除`--参数`部分）
   - 检查是否需要导出（比对记录）
   - 如需导出：
     - 调用bat脚本执行FFmpeg转码
     - 记录导出信息
3. 显示导出统计

## 文件结构

```
Assets/Scripts/ResScripts/ExportBuild/Editor/
├── VideoExport.cs              # 核心导出逻辑
├── VideoExportConfig.cs        # 配置类
├── VideoExportRecord.cs        # 记录类
├── VideoExportEditor.cs        # 编辑器菜单
└── VideoExportConfig.asset     # 配置实例（自动创建）

Assets/Scripts/
└── video_export.bat            # FFmpeg调用脚本

video_clip_original/            # 原始视频文件夹（项目根目录）
video_clip_vp8/                 # 导出视频文件夹（项目根目录）
└── export_record.json          # 导出记录文件
```

## 注意事项

1. **FFmpeg依赖**: 必须安装FFmpeg并确保在PATH中
2. **磁盘空间**: 确保有足够的磁盘空间存储导出视频
3. **导出时间**: 批量导出可能需要较长时间，请耐心等待
4. **日志查看**: 导出过程中的详细信息会在Unity Console中显示
5. **参数命名**: 参数缩写只能包含字母、数字和下划线
6. **路径规则**: 
   - 原始视频和导出视频的相对路径是对应的
   - 文件名中的`--参数`部分会被移除
   - 扩展名会统一改为`.webm`

## 自定义参数示例

在配置中添加新参数：
```
Short Name: uhd
Detail Params: -c:v libvpx-vp9 -b:v 4M -crf 6 -row-mt 1
```

使用方法：
```
video--uhd.mp4  # 使用uhd参数导出
```

## 故障排查

### 问题：FFmpeg未找到
**解决**: 安装FFmpeg并添加到系统PATH

### 问题：bat脚本执行失败
**解决**: 检查bat脚本路径配置是否正确

### 问题：所有视频都被跳过
**解决**: 如需重新导出，使用"清空导出记录"功能

### 问题：导出视频质量不满意
**解决**: 调整参数配置中的FFmpeg参数（如调整码率-b:v或CRF值）
