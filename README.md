# PreFix

启用「前缀」废案的 mod。

<img width="1180" height="597" alt="image" src="https://github.com/user-attachments/assets/f3d9b948-eb67-4a92-a88a-61c84a97c8b1" />


## 安装

1. 安装 [BepInEx 6 IL2CPP](https://github.com/BepInEx/BepInEx)
2. 将编译后的 `PreFix.dll` 放入 `BepInEx/plugins` 目录

## 构建

修改 `PreFix.csproj` 中的 `GamePath` 为你的游戏安装路径，然后：

```bash
dotnet build -c Release
```

## 许可证

[MIT](LICENSE)
