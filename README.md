# CQ机器人插件：武大助手（WhuHelper）
![GitHub top language](https://img.shields.io/github/languages/top/iSudocat/CSharpTermProject_CQApp?color=00a03e)
![GitHub](https://img.shields.io/github/license/iSudocat/CSharpTermProject_CQApp)
![GitHub repo size](https://img.shields.io/github/repo-size/iSudocat/CSharpTermProject_CQApp)
![GitHub contributors](https://img.shields.io/github/contributors/iSudocat/CSharpTermProject_CQApp?color=ffb549)
![GitHub last commit](https://img.shields.io/github/last-commit/iSudocat/CSharpTermProject_CQApp?color=ff585d)

## 系统环境
* Windows 7 / Windows Server 2012 或更高版本
* Visual Studio 2019
* .Net Framework 4.6.2
* 酷Q Air/Pro(推荐)

## 功能说明
* 教务系统登录与课程、成绩信息获取（多用户）
* 新出成绩提醒（机主本人）
* 课表查看，上课提醒（多用户）
* 成绩查看与GPA计算（多用户）
* 日程提醒（多用户）
* 群关键词关注提醒（多用户）
* Git监工（多用户）

## 生成帮助
* [点击查看](Build.md)

## 使用提示
1. 本插件使用SQLite数据库，在原始设计中，所有数据库文件将在启用插件进行初始化时自动下载（存储路径位于 CQ根目录\data\app\cc.wnapp.whuHelper）。
 * 出于安全考虑，下载链接已经移除。如果您需要此功能，需要自行配置下载链接。
 * 相关文件存放在仓库的[Important]Dependent Files文件夹中。
2. 在原始设计中，本插件所需的所有dll文件也将在启用插件进行初始化时自动下载（存储路径位于 CQ根目录）。
 * 出于安全考虑，下载链接已经移除。如果您需要此功能，需要自行配置下载链接。
 * 相关文件存放在仓库的[Important]Dependent Files文件夹中。
3. 当插件出现数据库相关错误时，除尝试修改数据库外，您也可尝试自行重置数据库（点击插件设置菜单 - 重置xx数据库）[**警告**：重置数据库将丢失现有数据库的所有数据]。
4. 教务系统登录所需的验证码识别，打码平台接入[超人代码](http://www.chaorendama.com/)，您需要自行注册该平台，并修改插件中的平台账户信息才可正常使用。
 * 出于安全考虑，代码中的这些原始信息已经移除。
5. 如需使用Git监工功能，您需要自行在GitHub创建第三方访问程序，并根据实际情况修改代码中的OAuth配置信息，并对Git数据库中的链接信息进行配置。
 * 出于安全考虑，代码中和数据库中的这些原始信息已经移除。
6. 课程表导出下载功能使用了腾讯云对象存储COS，如需使用，您必须自行注册腾讯云相关服务，并根据自身情况配置该部分信息。
 * 出于安全考虑，代码中和数据库中的这些原始信息已经移除。
7. 本插件为期末大作业，开发者们忙于赶DDL（危），难免仍存有部分bug，还请包涵。

## 注意事项
1. **禁止**在酷Q社区擅自发布本应用。
2. 如果您希望对代码进行个人性质的修改（即不准备提交至本开源项目的修改）并自行使用，请**务必**更新Appid及作者等应用信息，以免造成不必要的版权纠纷，影响您的开发者权益。  
   详细信息请参阅：
   - [Native.SDK官方指南 - 快速入门 - 设置Appid](https://native.run/articles/01.html#%E8%AE%BE%E7%BD%AE-appid)
   - [酷Q文库 - 开发 - Appid规范](https://docs.cqp.im/dev/v9/appid/)
3. 在开发完成使用本插件时，请将生成配置由Debug调整为Release。

## 合作开发
* [CJQ-CS-WHU](https://github.com/CJQ-CS-WHU)
* [Cookie Zhang](https://github.com/Stevetich)
* [Eric_Lian](https://github.com/ExerciseBook)
* [Lagueen](https://github.com/Lagueen)
* [Peter Sheng](https://github.com/PeterSH6)
* [Xixi](https://github.com/2426837192)
* [zzqzzqzzq0816](https://github.com/zzqzzqzzq0816)

## 更多文档与资源
1. [Native.SDK官方指南](https://native.run/articles/Home.html)
2. [酷Q文库 开发指南](https://docs.cqp.im/dev/)
3. [Native.SDK官方代码示例](https://github.com/Jie2GG/Native.Framework/tree/Example)

## 特别感谢
* 机器人框架：[CQ机器人](https://cqp.cc/)
* C# SDK：[Native.SDK](https://github.com/Jie2GG/Native.Framework/tree/Final)
