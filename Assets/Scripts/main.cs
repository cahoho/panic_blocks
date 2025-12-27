using System.Collections;
using System.Collections.Generic; // 需要这个来使用 List
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// CDN设置。
/// 由于笔者能力有限，本代码采用AI生成
/// </summary>
public class Main : MonoBehaviour
{
    [Header("资源加载设置")]
    public string[] assetNames = { "blink", "flag", "player", "ui" }; // 每个资源包内部的预制体名称
    public string[] bundleNames = { "box_bundle", "player_bundle", "ui_bundle" }; // <-- 新增：资源包名称列表
    public string cdnRoot = ""; // 设置你的CDN地址。为保护隐私，在这里不做赋值

    [Header("加载选项")]
    public bool loadFromLocal = true;
    public bool loadFromCDN = false;

    // Unity 消息 - 10 个引用
    IEnumerator Start()
    {
        // 加载本地资源包
        if (loadFromLocal)
        {
            yield return LoadAllBundlesFromLocal();
        }

        // 加载CDN资源包
        if (loadFromCDN)
        {
            yield return LoadAllBundlesFromCDN(cdnRoot);
        }

        Debug.Log("所有资源加载流程完成。");
    }

    // Update 每帧调用 - 10 个引用
    void Update()
    {
    }

    // 从本地加载所有资源包
    IEnumerator LoadAllBundlesFromLocal()
    {
        Debug.Log("开始从本地加载所有资源包...");

        foreach (string currentBundleName in bundleNames) // 对每个资源包名称进行循环
        {
            // 资源包路径（假设在 StreamingAssets 中）
            string bundlePath = System.IO.Path.Combine(Application.streamingAssetsPath + "/", currentBundleName);
            Debug.Log($"尝试加载本地资源包: {currentBundleName} from {bundlePath}");

            // 加载资源包
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

            if (bundle == null)
            {
                Debug.LogError($"Failed to load AssetBundle '{currentBundleName}' from local!");
                continue; // 加载下一个资源包
            }

            Debug.Log($"本地资源包 '{currentBundleName}' 加载成功，开始加载预制体...");

            // 加载所有指定的预制体
            foreach (string assetName in assetNames) // 在当前资源包内查找预制体
            {
                GameObject prefab = bundle.LoadAsset<GameObject>(assetName);

                if (prefab != null)
                {
                    Instantiate(prefab);
                    Debug.Log($"成功从本地资源包 '{currentBundleName}' 加载并实例化预制体: {assetName}");
                }
                else
                {
                    Debug.LogWarning($"在本地资源包 '{currentBundleName}' 中找不到预制体: {assetName}");
                }

                yield return null; // 每帧加载一个，避免卡顿
            }

            // 卸载资源包（但保留已加载的资源在内存中）
            bundle.Unload(false);
            Debug.Log($"本地资源包 '{currentBundleName}' 加载完成");
        }
        Debug.Log("所有本地资源包加载完成");
    }

    // 从CDN加载所有资源包
    IEnumerator LoadAllBundlesFromCDN(string cdnroot)
    {
        Debug.Log("开始从CDN加载所有资源包...");

        foreach (string currentBundleName in bundleNames) // 对每个资源包名称进行循环
        {
            string remoteurl = cdnroot + "/" + currentBundleName;
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(remoteurl);
            Debug.Log($"CDN下载地址: {remoteurl}");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"CDN下载错误 for '{currentBundleName}': " + request.error);
                continue; // 下载下一个资源包
            }
            else
            {
                // 从下载处理器获取AssetBundle
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

                if (bundle == null)
                {
                    Debug.LogError($"无法从CDN响应中获取AssetBundle for '{currentBundleName}'");
                    continue; // 下载下一个资源包
                }

                Debug.Log($"CDN资源包 '{currentBundleName}' 下载成功，开始加载预制体...");

                // 加载所有指定的预制体
                foreach (string assetName in assetNames) // 在当前资源包内查找预制体
                {
                    GameObject prefab = bundle.LoadAsset<GameObject>(assetName);

                    if (prefab != null)
                    {
                        Instantiate(prefab);
                        Debug.Log($"成功从CDN资源包 '{currentBundleName}' 加载并实例化预制体: {assetName}");
                    }
                    else
                    {
                        Debug.LogWarning($"在CDN资源包 '{currentBundleName}' 中找不到预制体: {assetName}");
                    }

                    yield return null; // 每帧加载一个，避免卡顿
                }

                bundle.Unload(false);
                Debug.Log($"CDN资源包 '{currentBundleName}' 加载完成");
            }
        }
        Debug.Log("所有CDN资源包加载完成");
    }
}
