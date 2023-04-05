#if PCSOFT_ADS_ADMOB
using System;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class RewardedAd
    {
        public void Show(Action<RewardInfo> onFinished)
        {
            if (IsShown)
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.LogWarning("[ADVERTISEMENT] Rewarded Ad already shown");
#endif

                onFinished?.Invoke(null);
                return;
            }

#if !UNITY_EDITOR && PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            _finishAction = onFinished;
            base.Show();
#else
            onFinished?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
#endif
        }

        protected override bool DoShow()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.Log("[ADVERTISEMENT] Show rewarded ad now");
#endif
                _rewardedAd.Show(RewardedAdOnUserEarnedReward);
                return true;
            }

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Unable to show rewarded ad, not ready");
#endif

            _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
            _finishAction = null;

            Request();
            return false;
        }

        protected override bool DoHide()
        {
            throw new NotSupportedException();
        }
    }
}
#endif