using UnityEditor;
using UnityEngine;

public class SpriteImportSetting : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {

        if (assetPath.StartsWith("Assets/Resources/mir"))
        {
            TextureImporter importer = (TextureImporter)assetImporter;
            TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(textureImporterSettings);
            textureImporterSettings.spriteAlignment = (int)SpriteAlignment.TopLeft;
            textureImporterSettings.spritePixelsPerUnit = 1;
            importer.SetTextureSettings(textureImporterSettings);
        }
    }

}
