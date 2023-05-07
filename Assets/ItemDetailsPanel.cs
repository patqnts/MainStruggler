using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsPanel : MonoBehaviour
{
    public Item item;
    public Image image;
    public Text nameText;
    public Text descriptionText;
    public Image[] materialImages; // an array of Image components for each material
    public Text[] materialAmountTexts; // an array of Text components for the amount of each material
    public Image priceImage;
    public Text priceAmountText;
    public Sprite coinSprite;


    private void Cycle()
    {
        foreach (var materialImage in materialImages)
        {
            materialImage.gameObject.SetActive(false);
        }
        foreach (var materialAmountText in materialAmountTexts)
        {
            materialAmountText.gameObject.SetActive(false);
        }
    }
    public void DisplayItemDetails()
    {
        // Set the image, name, description, materials required, and price text
        image.sprite = item.image;
        nameText.text = item.name;
        descriptionText.text = item.description;
        priceImage.sprite = coinSprite; // Assumes that you have a coin sprite to represent the price
        priceAmountText.text = item.priceAmount.ToString();

        // Hide material images and reset material amount texts if item does not have any material requirements

        Cycle();



        for (int i = 0; i < item.materialRequirements.Length; i++)
            {
                if (i < materialImages.Length && i < materialAmountTexts.Length) // Check if the index is within bounds
                {
                    materialImages[i].sprite = item.materialRequirements[i].materialSprite;
                    materialAmountTexts[i].text = item.materialRequirements[i].requiredAmount.ToString();

                
                materialImages[i].gameObject.SetActive(true); // Show the material image
                materialAmountTexts[i].gameObject.SetActive(true);
                if (item.materialRequirements[i].requiredAmount == 0)
                {
                    materialAmountTexts[i].gameObject.SetActive(false);
                }
                
            }
              

            }
        if (item.materialRequirements.Length == 0 || item.materialRequirements.Length == null)
        {
            foreach (var materialImage in materialImages)
            {
               // materialImage.sprite = null;
                materialImage.gameObject.SetActive(false);
            }
            foreach (var materialAmountText in materialAmountTexts)
            {
                // materialAmountText.text = "";
                materialAmountText.gameObject.SetActive(false);
            }
        }



    }


}
