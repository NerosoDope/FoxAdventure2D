using UnityEngine;
using System.IO;

public class URPTilemapScreenshot : MonoBehaviour
{
    [Header("References")]
    public Camera tilemapCamera;
    public RenderTexture renderTexture;

    [Header("Image Settings")]
    public int width = 1920;
    public int height = 1080;
    public string outputFileName = "TilemapCaptured.png";

    void Start()
    {
        // Gán RenderTexture vào camera
        tilemapCamera.targetTexture = renderTexture;

        // Lưu render texture hiện tại
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Vẽ camera lên RenderTexture
        tilemapCamera.Render();

        // Đọc pixels từ RenderTexture
        Texture2D image = new Texture2D(width, height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        // Gamma correction nếu đang dùng Linear Color Space
        if (QualitySettings.activeColorSpace == ColorSpace.Linear)
        {
            Color[] pixels = image.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].r = Mathf.Pow(pixels[i].r, 1f / 2.2f);
                pixels[i].g = Mathf.Pow(pixels[i].g, 1f / 2.2f);
                pixels[i].b = Mathf.Pow(pixels[i].b, 1f / 2.2f);
            }
            image.SetPixels(pixels);
            image.Apply();
        }

        // Encode ảnh thành PNG
        byte[] bytes = image.EncodeToPNG();

        // Đường dẫn lưu ảnh
        string filePath = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("✅ Tilemap saved to: " + filePath);

        // Dọn dẹp
        RenderTexture.active = currentRT;
        tilemapCamera.targetTexture = null;

        // Tuỳ chọn: tự hủy sau khi lưu
        Destroy(image);
        Destroy(gameObject);
    }
}
