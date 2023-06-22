using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyGames;
public class CrazyAdsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public CrazyBanner up;
    public CrazyBanner DOWN;

    private void Awake()
    {
        up.gameObject.SetActive(true);
        DOWN.gameObject.SetActive(true);
    }
    void Start()
    {
        up.MarkVisible(true);
        DOWN.MarkVisible(true);
        CrazyAds.Instance.updateBannersDisplay();
    }
    public void DisableAds()
    {
        up.MarkVisible(false);
        DOWN.MarkVisible(false);
        CrazyAds.Instance.updateBannersDisplay();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
