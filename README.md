# TIA-Automatic-saving-tool
 做博途项目的都知道，TIA Portal   崩溃从来不打招呼。写了两个小时的程序，一个未响应直接回到解放前，崩溃日志看不懂，保存按钮永远在最深的菜单里。    被崩了几次之后，我决定自己写一个自动保存工具。    核心功能：   - 定时自动保存（默认每10分钟）   - 空闲自动保存（离开电脑自动触发）   - 支持 V16~V20 全版本   - 后台静默运行，不打断工作流   - 自动识别博途启动，自动连接    适用场景：   - 项目做到一半，不想手动保存又怕崩   - 长时间调试，离开时忘了保存- 同时开多个项目，手动保存太繁琐，软件没有银弹，但至少可以让崩溃的代价小一点。

家人们，完整程序包.zip这个才是完整的一个文件，可以直接下载，下载安装就能用
<img width="793" height="679" alt="logo123" src="https://github.com/user-attachments/assets/07d7ac11-c650-41f4-814e-fee46fc6930e" />

==============================
  TIA Portal自动保存工具 使用说明
==============================

【软件功能】
  支持 TIA Portal V16~V20 的链式启动 + 自动保存工具，支持：
  - 工作时间定时自动保存（默认每10分钟）
  - 空闲时间自动保存（默认空闲3分钟触发）
  - 最小化到系统托盘后台运行
  - 自动检测并添加 Siemens TIA Openness 用户组权限


【安装步骤】
  1. 双击运行「博途自动保存工具_安装程序.exe」
     （如弹出权限提示，点"是"即可）
  2. 等待安装完成，自动安装到 C:\Program Files\TIA Portal AutoSave
  3. 桌面会出现快捷方式「TIA Portal自动保存工具」


【日常使用】
  1. 双击桌面快捷方式「TIA Portal自动保存工具」
  2. 程序会自动连接已运行的 TIA Portal（V16~V20）
  3. 首次运行需手动选择 Siemens.Engineering.dll（之后自动记忆）
  4. 连接成功后可点击「托盘」按钮最小化到后台


【参数设置】（在自动保存工具窗口中调整）
  - 间隔(分)：定时保存的时间间隔，默认10分钟
  - 空闲(分)：触发空闲保存的空闲时长，默认3分钟


【保存逻辑】
  - 工作时间：到了间隔时间就自动保存，不中断你的操作
  - 空闲时间：你离开电脑空闲超过设定值后自动保存


【注意事项】
  1. 需要电脑已安装 TIA Portal（V16~V20 任一版本）
  2. 首次运行会提示选择 Siemens.Engineering.dll
     路径参考：
     - V16: D:\Program Files\Siemens\Automation\Portal V16\PublicAPI\V16
     - V17: D:\Program Files\Siemens\Automation\Portal V17\PublicAPI\V17
     - V18~V20: 类似路径
  3. 博途关闭后自动保存工具会自动退出
  4. 请勿手动删除安装目录下的文件
  5. 若已打开博途，请不要再打开其他博途


【卸载方法】
  方法一：使用卸载程序（推荐）
  1. 运行「Uninstall_TIA_Portal_AutoSave.exe」
  2. 点击「是」确认卸载
  3. 自动清理所有文件和桌面快捷方式

  方法二：手动卸载
  1. 删除安装目录（C:\Program Files\TIA Portal AutoSave）
  2. 删除桌面快捷方式「TIA Portal自动保存工具」


【常见问题】
  问题：提示 Security error
  解决：需要将当前用户添加到 "Siemens TIA Openness" 用户组
       运行 lusrmgr.msc → 组 → 添加当前用户 → 注销重新登录

  问题：找不到 Siemens.Engineering.dll
  解决：博途安装时需勾选 Openness 组件，或在安装目录 PublicAPI 文件夹中查找


【更新记录】
  2026-05-25:
  - 软件名称改为「TIA Portal自动保存工具」
  - 支持 V16~V20 多版本
  - 更新应用图标
  - 自动权限管理（安装/启动时自动检测并添加用户组）
  - 新增一键卸载程序
