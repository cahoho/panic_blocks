# 方寸大乱文档

[![Unity Version](https://img.shields.io/badge/Tuanjie-1.6.6-blue)](https://unity.cn)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**此为获奖项目。为方便广大技术爱好者学习研究，现进行开源。**

> [English Document](https://github.com/cahoho/panic_blocks/blob/main/README_en.md)

## 🏆 关于此项目

*说明：该项目使用Unity中国团结引擎开发，引擎版本为**Tuanjie1.6.6**，可使用**Unity2022LTS**打开项目。*<br>

​		该项目是**方寸大乱**游戏的源代码。该作品获得了**Unity中国2025开发挑战赛**游戏开发赛区小游戏赛道的**最佳玩法创意奖**奖项。<br>

​		方块大乱跑酷是一款独具特色的跑酷解密游戏。它融合了多种有趣的游戏机制。你将用同一套按键同时操控不同角色，它们对指令的反应各不相同。在令人手忙脚乱的操作中协调所有方块来夺得场上的所有旗子获得胜利。 方寸大乱跑酷软件的适龄信息为8+。该游戏的目标用户是追求小体量、快加载、高爽感、有趣味的游戏玩家。为希望能够动脑解决问题、构建思维能力、进行特色有趣解密的游戏玩家准备。<br>

#### 游戏素材：

|     项目     |                           素材内容                           |
| :----------: | :----------------------------------------------------------: |
| **美术素材** | [Pixel Adventure 1](https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360) <a href="https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360" target="_blank"><img src="https://assetstorev1-prd-cdn.unity3d.com/key-image/ad020d03-97f5-4835-9559-ffd4426b729b.webp" alt="Pixel Adventure 1 预览" width="120" style="vertical-align: middle; border-radius: 4px;" /></a> |
| **音乐素材** | [Music by Eric Matyas](https://soundimage.org/ "Sound Image") |

## ✨ 核心技术实现

-  **多玩家输入系统：**该系统由一个中央输入监听模块[PlayerManager.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/PlayerManager.cs)和一个角色行为解析器[Player.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/Player.cs)共同构成。当玩家按下任何一个控制键[LP_Controller.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/LP_Controller)或者[JP_Controller.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/JP_Controller)时，输入系统会立刻捕获该指令。随后，系统不会去寻找某个特定的主角，而是将这个指令作为一个全局事件，向当前关卡中所有需要被控制的角色进行事件广播。每个角色都内置了一个独立的行为解析器。它们在接收到同一指令后，会依据自身预设的逻辑规则来执行动作。<br>

-  **夺旗胜利机制：**旗帜数量不仅有一个。因此夺旗机制同样采用了与多玩家输入系统类似的中央监听[FlagManager.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/FlagManager.cs)+分发解析的技术组合。<br>
-  **CDN资源加载：**资源加载机制实现了 AssetBundle 动态加载、本地与远程资源支持、异步加载、CDN 加载等功能。<br>

## 🎮 核心玩法创意

-  **多玩家输入系统：**摒弃了传统平台跳跃游戏中“一对一”的控制模式，转而采用了一套高度统一的“一对多指令分发系统”。这套系统是本作创新玩法的基石，它将简单的玩家输入转化为复杂的游戏内行为，从而创造出独特而富有挑战性的游戏体验。
-  **切换地层系统：**此系统允许玩家在特定关卡中，通过特定按键，瞬间改变整个场景的地层结构。谜题的解决方案从“在静态环境中寻找唯一路径”转变为“通过改变环境本身来创造路径”。玩家必须同时思考“当前地图”与“切换后地图”两种状态下的通关路径，并规划出能让所有角色安全抵达终点的最佳切换时机与顺序。

-  **丰富的高难度地图：**地图设计是该游戏玩法的核心。优秀的地图促使了游戏玩法上升了更高的高度。<br>

## 😊关于作者

我是cahoho，来自中国本科高校的一名2025级在校本科生，对游戏开发、网络安全、AI及其融合领域有着浓厚的兴趣。欢迎各位开发者在我的开源项目留下评论、Star，也期待该项目可以成为教学案例的典范。

**留言**

您可以直接向我发送邮件：cahoho@126.com<br> 

**版权与使用说明**

> - 本项目采用 **MIT 开源许可证**，详情请见 [LICENSE](https://github.com/cahoho/panic_blocks/blob/main/LICENSE) 文件。
> - **基于本项目创作时，必须注明原作者（Game by cahoho）及Github开源页面**。
> - 游戏启动封面中包含所使用的非原创素材的版权信息。我们强烈建议二次创作者在您的作品中也能以适当方式**保留或提及这些素材来源，以示对原创作者的尊重**。



我还在努力向网络安全+安全可视化+AI赋能领域努力。欢迎各路开发者与我交流合作！🥳

- **💻 技术主页**：[GitHub](https://github.com/cahoho) - 这里是我的主要项目集。
- **📝 技术思考**：[掘金](https://juejin.cn/user/1470480625172570) - 我会在这里分享详细的技术文章。
- **🤝 交流合作**：如果你对**网络空间安全、游戏化安全、Unity高级架构或开源协作**感兴趣，欢迎通过GitHub Issues或上述博客与我交流！期待有趣的讨论。

我的邮箱：cahoho@126.com
