using UnityEngine;

namespace CrazyGames
{
    class CrazySDKInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            CrazySDK.ResetDomain();
            CrazyAds.ResetDomain();
            CrazyEvents.ResetDomain();
            CrazyUser.ResetDomain();

            CrazySDK.Instance.InitSDK();
        }
    }
}