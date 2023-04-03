#if GOOGLE_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if GOOGLE_ADMOB
    [DisallowMultipleComponent]
    public sealed class InterstitialWithAwardAd : AdBase<AdsInterstitialWithAwardItem>
    {
        private RewardedAd _rewardedAd;
        private Action<RewardInfo> _finishAction;

        public override bool SupportHide { get; } = false;

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
            RewardedAd.Load(id, request, (ad, error) =>
            {
                if (error == null)
                {
                    OnAdLoaded();
                }
                else
                {
                    OnAdFailedToLoad(error);
                }

                DestroyAd(_rewardedAd, id);
                _rewardedAd = ad;
                InitAd(_rewardedAd, id);
            });
        }

        protected override void DoDispose() => DestroyAd(_rewardedAd, "");

        private void InitAd(RewardedAd ad, string id)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Initialize Interstitial Ad With Award: " + id);
#endif

            ad.OnAdFullScreenContentClosed += OnAdClosed;
            ad.OnAdFullScreenContentOpened += OnAdOpening;
            ad.OnAdFullScreenContentFailed += OnAdFailedToShow;
            ad.OnAdPaid += OnAdPaid;
        }

        private void DestroyAd(RewardedAd ad, string id)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Destroy Interstitial Ad With Award: " + id);
#endif

            ad.OnAdFullScreenContentClosed -= OnAdClosed;
            ad.OnAdFullScreenContentOpened -= OnAdOpening;
            ad.OnAdFullScreenContentFailed -= OnAdFailedToShow;
            ad.OnAdPaid -= OnAdPaid;

            _rewardedAd.Destroy();
        }

        protected override void DoShow()
        {
            if (_rewardedAd.CanShowAd())
            {
#if LOGGING_ADMOB
                Debug.Log("[ADVERTISEMENT] Show ad now");
#endif
                _rewardedAd.Show(RewardedAdOnUserEarnedReward);
            }
            else
            {
#if LOGGING_ADMOB
                Debug.Log("[ADVERTISEMENT] No ad found yet");
#endif
                _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
                _finishAction = null;
            }
        }

        protected override void DoHide()
        {
            throw new NotSupportedException();
        }

        private void OnAdFailedToShow(AdError error)
        {
            Debug.LogError("[ADVERTISEMENT] Ad show failure: " + error.GetMessage(), this);
            OnShowingFailed?.Invoke(this, new AdErrorEventArgs() { AdError = error });
            _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
            _finishAction = null;
        }

        private void RewardedAdOnUserEarnedReward(Reward e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad earned reward: " + e.Type + ", " + e.Amount);
#endif
            _finishAction?.Invoke(new RewardInfo(e, RewardResult.Success));
            _finishAction = null;
        }

        protected override void OnAdClosed()
        {
            base.OnAdClosed();
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
#else
    public sealed class RewardInfo
    {
    }
#endif
}