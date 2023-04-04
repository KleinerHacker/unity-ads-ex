﻿#if PCSOFT_ADS_ADMOB
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
                Debug.Log("Ad already shown");

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

        protected override void DoShow()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.Log("[ADVERTISEMENT] Show ad now");
#endif
                _rewardedAd.Show(RewardedAdOnUserEarnedReward);
            }
            else
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.Log("[ADVERTISEMENT] No ad found yet");
#endif
                _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
                _finishAction = null;

                Request();
            }
        }

        protected override void DoHide()
        {
            throw new NotSupportedException();
        }
    }
}
#endif