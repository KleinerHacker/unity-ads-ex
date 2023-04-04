#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    [DisallowMultipleComponent]
    public sealed partial class InterstitialAd : AdBase<AdsInterstitialItem>
    {
        #region Properties

        public override bool SupportHide { get; } = false;
        public override bool IsReady => _interstitialAd != null && _interstitialAd.CanShowAd();

        #endregion

        #region Events

        public event EventHandler<AdErrorEventArgs> OnShowingFailed;

        #endregion

        private GoogleMobileAds.Api.InterstitialAd _interstitialAd;
        private Action _finishAction;
    }
}
#endif