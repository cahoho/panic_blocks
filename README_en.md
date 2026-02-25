# Panic Blocks Documentation

[![Unity Version](https://img.shields.io/badge/Tuanjie-1.6.6-blue)](https://unity.cn)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**This is an award-winning project. It is now open-sourced for the convenience of technology enthusiasts to study and research.**

> [中文文档](https://github.com/cahoho/panic_blocks/blob/main/README.md) | [TRY NOW!](https://cahoho.github.io/panic_blocks/)  

## 🏆 About This Project

*Note: This project was developed using Unity China's Tuanjie Engine, version **Tuanjie1.6.6**. The project can be opened using **Unity 2022 LTS**.*<br>

This project contains the source code for the game **Panic Blocks**. This work received the **Best Gameplay Creativity Award** in the Mini-Game Track of the Game Development category at the **Unity China 2025 Developer Challenge**.You can see the report here. [2025 Unity China Development Competition](https://mp.weixin.qq.com/s/GvalmJ70C7Z1Ps1GMt7LBg) <br>

Panic Blocks is a unique parkour puzzle game that blends various interesting game mechanics. You control different characters simultaneously using the same set of keys, with each character responding differently to your commands. Coordinate all the blocks amidst the frantic operation to capture all the flags on the field and achieve victory. The recommended age for Panic Blocks is 8+. The target audience for this game is players seeking small-sized, fast-loading, highly satisfying, and fun games. It's designed for players who enjoy solving problems with their minds, building thinking skills, and engaging in unique and interesting puzzles.<br>

#### Game Assets:

|     Category     | Asset Contents                                               |
| :--------------: | :----------------------------------------------------------- |
|  **Art Assets**  | [Pixel Adventure 1](https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360) <a href="https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360" target="_blank"><img src="https://assetstorev1-prd-cdn.unity3d.com/key-image/ad020d03-97f5-4835-9559-ffd4426b729b.webp" alt="Pixel Adventure 1 Preview" width="120" style="vertical-align: middle; border-radius: 4px;" /></a> |
| **Music Assets** | [Music by Eric Matyas](https://soundimage.org/ "Sound Image") |

## ✨ Core Technical Implementation

- **Multi-Player Input System:** This system consists of a central input monitoring module ([PlayerManager.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/PlayerManager.cs)) and a character behaviour interpreter ([Player.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/Player.cs)). When the player presses any control key ([LP_Controller.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/LP_Controller) or [JP_Controller.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/JP_Controller)), the input system immediately captures the command. The system then does not search for a specific protagonist. Instead, it broadcasts this command as a global event to all controllable characters in the current level. Each character has a built-in independent behaviour interpreter. After receiving the same command, they execute actions according to their own preset logic rules.<br>

- **Flag Capture Victory Mechanism:** There is more than one flag. Therefore, the flag capture mechanism similarly uses the technical combination of central monitoring ([FlagManager.cs](https://github.com/cahoho/panic_blocks/blob/main/Assets/Scripts/FlagManager.cs)) + distributed parsing, similar to the multi-player input system.<br>
- **CDN Resource Loading:** The resource loading mechanism implements features like AssetBundle dynamic loading, support for local and remote resources, asynchronous loading, and CDN loading.<br>

## 🎮 Core Gameplay Creativity

- **Multi-Player Input System:** Abandons the traditional "one-to-one" control model of platformer games, adopting a highly unified "one-to-many command distribution system." This system is the cornerstone of this game's innovative gameplay, transforming simple player input into complex in-game behaviours, thereby creating a unique and challenging gaming experience.
- **Layer Switching System:** This system allows players to instantly change the terrain structure of the entire scene in specific levels via specific keys. The puzzle solution shifts from "finding the only path in a static environment" to "creating a path by altering the environment itself." Players must simultaneously consider the passable routes in both the "current map" and the "switched map" states, and plan the optimal timing and sequence of switches to allow all characters to safely reach the destination.

- **Rich High-Difficulty Maps:** Map design is the core of this game's gameplay. Excellent maps elevate the gameplay to a higher level.<br>

## 😊 About the Author

I am cahoho, a 2025 undergraduate student from a Chinese university, with a strong interest in game development, cybersecurity, AI, and their convergence. I welcome all developers to leave comments and stars on my open-source projects, and I hope this project can become a model teaching case.

**Contact**

You can email me directly at: cahoho@163.com<br>

**Copyright and Usage Notes**

> - This project is licensed under the **MIT Open Source License**. For details, please see the [LICENSE](https://github.com/cahoho/panic_blocks/blob/main/LICENSE) file.
> - **When creating works based on this project, you must credit the original author (Game by cahoho) and the Github open-source page**.
> - The game's startup screen contains copyright information for the non-original assets used. We strongly recommend that secondary creators also **retain or mention the sources of these assets in an appropriate manner in your works, as a sign of respect for the original creators**.

I am also striving to contribute to the fields of cybersecurity + security visualization + AI empowerment. I welcome developers from all fields to exchange and collaborate with me! 🥳

- **💻 Tech Portfolio:** [GitHub](https://github.com/cahoho) - This is my main collection of projects.
- **📝 Tech Articles:** [Juejin](https://juejin.cn/user/1470480625172570) - I share detailed technical articles here.
- **🤝 Communication & Collaboration:** If you are interested in **Cyberspace Security, Security Gamification, Advanced Unity Architecture, or Open Source Collaboration**, feel free to contact me via GitHub Issues or the blog mentioned above! I look forward to interesting discussions.

My Email: cahoho@163.com 
