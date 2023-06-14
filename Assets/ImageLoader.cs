using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    public Image imageComponent;
    public string imagePath;
    public string folderId;
    public Text progressPercentage;
    public GameObject playUI;
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
            playUI.SetActive(true);
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
            playUI.SetActive(false);
            Debug.Log("Image file not found: " + fullPath);
        }

        // Read the player data from the PlayerDatas.json file
        string playerDataPath = Path.Combine(Application.persistentDataPath, profileId, "PlayerDatas.json");
        if (File.Exists(playerDataPath))
        {
            string jsonData = File.ReadAllText(playerDataPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            // Calculate the text value based on the boss booleans
            int bossCount = 0;
            if (playerData.isDeadGolem)
                bossCount++;
            if (playerData.isDeadSlime)
                bossCount++;
            if (playerData.isDeadBomber)
                bossCount++;
            if (playerData.isDeadDogo)
                bossCount++;

            int BossProgress = bossCount * 20;
            int totalProgress = BossProgress + Mathf.RoundToInt(playerData.maxHealth);
            progressPercentage.text = "["+ totalProgress.ToString() +"%" + "]";
            Debug.Log("Text Value: " + totalProgress);
        }
        else
        {
            Debug.Log("Player data file not found: " + playerDataPath);
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
            progressPercentage.text = null;
            playUI.SetActive(false);

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
