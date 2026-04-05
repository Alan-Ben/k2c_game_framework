---
name: git_merge_helper
description: 跨 `tp_server`、`ServerProtocol`、`ClientProtocol` 拉取并合并分支时使用。用户说“拉取 master”“合并 internal”“同步最新代码”时触发。兼容原 Claude agent `git-merge-helper`。
---

# Git 合并辅助 Skill

## 适用场景
- 拉取并合并 `master`、`internal`、`external`
- 同步主工程与协议仓库的对应分支
- 在多仓库环境下按固定顺序执行更新

## 分支映射
### master
- `tp_server` -> `master`
- `ServerProtocol` -> `master`
- `ClientProtocol` -> `server_master`

### internal
- `tp_server` -> `internal`
- `ServerProtocol` -> `internal`
- `ClientProtocol` -> `server_internal`

### external
- `tp_server` -> `external`
- `ServerProtocol` -> `external`
- `ClientProtocol` -> `server_external`

## 执行步骤
1. 先确认用户要合并的目标分支；如果未说明，必须先问清楚。
2. 执行前检查各仓库是否有未提交改动，并向用户报告风险。
3. 按顺序处理：
   - `ServerProtocol`
   - `ClientProtocol`
   - `tp_server`
4. 对每个仓库执行对应分支的拉取和合并，并记录结果。

## 安全要求
- 未确认目标分支前，不执行合并
- 发现未提交改动时，不要擅自覆盖或清理
- 遇到冲突时，明确说明冲突文件和下一步建议
- 涉及网络、凭据、权限问题时，如实反馈失败原因

## 输出要求
- 用中文简洁汇报每个仓库的结果
- 明确列出成功、冲突、失败三类状态
- 若失败，说明是在 `pull`、`merge` 还是分支不存在阶段出错
