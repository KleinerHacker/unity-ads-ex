using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if GOOGLE_ADMOB
    [DisallowMultipleComponent]
    public sealed class InterstitialWithAwardAd : AdBase<AdsInterstitialWithAwardItem>
    {
        private RewardedAd _rewardedAd;
        private Action<RewardInfo> _finishAction;

        public override bool SupportHide { get; } = false;

        public event EventHandler<AdFailedToLoadEventArgs> OnLoadingFailed;
        public event EventHandler<AdErrorEventArgs> OnShowingFailed;

        public void Show(Action<RewardInfo> onFinished)
        {
            if (IsShown)
            {
                Debug.Log("Ad already shown");

                onFinished?.Invoke(null);
                return;
            }

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            _finishAction = onFinished;
            base.Show();
#else
            onFinished?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
#endif
        }

        protected override void DoRequest(string id, AdRequest request)
        {
            _rewardedAd = new RewardedAd(id);
            _rewardedAd.OnAdLoaded += BannerOnAdLoaded;
            _rewardedAd.OnAdClosed += BannerOnAdClosed;
            _rewardedAd.OnAdOpening += BannerOnAdOpening;
            _rewardedAd.OnAdFailedToLoad += RewardedAdOnAdFailedToLoad;
            _rewardedAd.OnAdFailedToShow += RewardedAdOnAdFailedToShow;
            _rewardedAd.OnPaidEvent += BannerOnAdPaid;
            _rewardedAd.OnUserEarnedReward += RewardedAdOnUserEarnedReward;

            _rewardedAd.LoadAd(request);
        }

        protected override void DoDispose()
        {
            _rewardedAd.OnAdLoaded -= BannerOnAdLoaded;
            _rewardedAd.OnAdClosed -= BannerOnAdClosed;
            _rewardedAd.OnAdOpening -= BannerOnAdOpening;
            _rewardedAd.OnAdFailedToLoad -= RewardedAdOnAdFailedToLoad;
            _rewardedAd.OnAdFailedToShow -= RewardedAdOnAdFailedToShow;
            _rewardedAd.OnPaidEvent -= BannerOnAdPaid;
            _rewardedAd.OnUserEarnedReward -= RewardedAdOnUserEarnedReward;

            _rewardedAd = null;
        }

        protected override void DoShow()
        {
            if (_rewardedAd.IsLoaded())
            {
#if LOGGING_ADMOB
                Debug.Log("Show ad now");
#endif
                _rewardedAd.Show();
            }
            else
            {
#if LOGGING_ADMOB
                Debug.Log("No ad found yet");
#endif
                _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
                _finishAction = null;
            }
        }

        protected override void DoHide()
        {
            throw new NotSupportedException();
        }

        private void RewardedAdOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.LogError("[ADVERTISEMENT] Ad load failure: " + e.LoadAdError, this);
            OnLoadingFailed?.Invoke(sender, e);
            _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
            _finishAction = null;
        }

        private void RewardedAdOnAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            Debug.LogError("[ADVERTISEMENT] Ad show failure: " + e.AdError.GetMessage(), this);
            OnShowingFailed?.Invoke(sender, e);
            _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
            _finishAction = null;
        }

        private void RewardedAdOnUserEarnedReward(object sender, Reward e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad earned reward: " + e.Type + ", " + e.Amount);
#endif
            _finishAction?.Invoke(new RewardInfo(e, RewardResult.Success));
            _finishAction = null;
        }

        protected override void BannerOnAdClosed(object sender, EventArgs e)
        {
            base.BannerOnAdClosed(sender, e);
            _finishAction?.Invoke(new RewardInfo(null, RewardResult.RewardCanceled));
            _finishAction = null;
        }
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
#endif
}