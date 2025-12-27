using UnityEditor;
using System.IO;
using System.IO.Pipes;

public class ABBuild
{
    [MenuItem("Tools/AssetBuild")]
    public static void BuildPrefab()
    {
        string output = "Assets/StreamingAssets/";
        Directory.CreateDirectory(output);
        BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
}
