#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if PCSOFT_ADS_ADMOB
    [DisallowMultipleComponent]
    public sealed partial class RewardedAd : AdBase<AdsRewardedItem>
    {
        #region Propertries

        public override bool SupportHide { get; } = false;
        public override bool IsReady => _rewardedAd != null && _rewardedAd.CanShowAd();

        #endregion

        #region Events

        public event EventHandler<AdErrorEventArgs> OnShowingFailed;

        #endregion

        private GoogleMobileAds.Api.RewardedAd _rewardedAd;
        private Action<RewardInfo> _finishAction;
    }

    public sealed class RewardInfo
    {
        public Reward Reward { get; }
        public RewardResult Result { get; }

        internal RewardInfo(Reward reward, RewardResult result)
        {
            Reward = reward;
            Result = result;
        }
    }

    public enum RewardResult
    {
        Success,
        NoAdToShow,
        RewardCanceled,
    }
#else
    public sealed class RewardInfo
    {
    }
#endif
}