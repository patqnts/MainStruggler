using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    public Image imageComponent;
    public string imagePath;
    public string folderId;

    private void Start()
    {
        LoadImage(folderId);
    }

    private void Update()
    {
        // Check for input or conditions to trigger the deletion of save data
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteSaveData(folderId);
        }
    }

    public void LoadImage(string profileId)
    {
        profileId = folderId;
        // Construct the full path to the image file
        string fullPath = Path.Combine(Application.persistentDataPath, profileId, "savepoint.png");

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            // Read the image data from the file
            byte[] imageData = File.ReadAllBytes(fullPath);

            // Create a texture and assign the image data to it
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            // Assign the texture to the image component
            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Set the alpha value of the image to 1.0f (fully opaque)
            Color imageColor = imageComponent.color;
            imageColor.a = 1.0f;
            imageComponent.color = imageColor;

            // Set the size of the image component to match the native size of the sprite
            imageComponent.SetNativeSize();
        }
        else
        {
            Debug.Log("Image file not found: " + fullPath);
        }
    }

    public void DeleteSaveData(string profileId)
    {
        profileId = folderId;
        // Construct the full paths to the save data files
        string saveDataPath = Path.Combine(Application.persistentDataPath, profileId, "PlayerDatas.json");
        string imagePath = Path.Combine(Application.persistentDataPath, profileId, "savepoint.png");

        // Check if the PlayerDatas.json file exists
        if (File.Exists(saveDataPath))
        {
            // Delete the PlayerDatas.json file
            File.Delete(saveDataPath);
            Color imageColor = imageComponent.color;
            imageColor.a = 0f;
            imageComponent.color = imageColor;
            imageComponent.sprite = null;


            Debug.Log("Player data deleted: " + saveDataPath);
        }
        else
        {
            Debug.Log("Player data file not found: " + saveDataPath);
        }

        // Check if the savepoint.png file exists
        if (File.Exists(imagePath))
        {
            // Delete the savepoint.png file
            File.Delete(imagePath);
            Debug.Log("Screenshot deleted: " + imagePath);
        }
        else
        {
            Debug.Log("Screenshot file not found: " + imagePath);
        }
    }

}
