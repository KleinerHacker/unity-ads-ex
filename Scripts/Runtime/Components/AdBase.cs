using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if GOOGLE_ADMOB
    public abstract class AdBase : MonoBehaviour
    {
        #region Properties

        public bool IsShown { get; private set; }
        public bool IsReady { get; private set; }
        public abstract bool SupportHide { get; }

        protected abstract string AdId { get; }

        #endregion

        #region Events

        public event EventHandler OnLoaded;
        public event EventHandler OnOpened;
        public event EventHandler OnClosed;
        public event EventHandler<AdFailedToLoadEventArgs> OnFailed;
        public event EventHandler<AdValueEventArgs> OnPaid;

        #endregion

        #region Builtin Methods

        protected virtual void Start()
        {
            Request();
        }

        protected virtual void OnDisable()
        {
            Dispose();
        }

        #endregion

        protected void Request()
        {
            if (IsReady)
                return;

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Try to request ad", this);
#endif

            var request = new AdRequest.Builder().Build();

            DoRequest(AdId, request);
#endif
            IsReady = true;
        }

        protected void Dispose()
        {
            if (!IsReady)
                return;

#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Try top destroy ad");
#endif

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoDispose();
#endif
            IsReady = false;
        }

        public void Show()
        {
            if (!IsReady)
                throw new InvalidOperationException("Ad is not ready yet. please call 'Request' first");
            if (IsShown)
                return;

#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Try to show ad", this);
#endif

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoShow();
#endif
            IsShown = true;
        }

        public void Hide()
        {
            if (!SupportHide)
                throw new NotSupportedException("Ad do not support 'Hide'");
            if (!IsReady)
                throw new InvalidOperationException("Ad is not ready yet. please call 'Request' first");
            if (!IsShown)
                return;

#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Hide ad", this);
#endif

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoHide();
#endif
            IsShown = false;
        }

        protected abstract void DoRequest(string id, AdRequest request);
        protected abstract void DoDispose();
        protected abstract void DoShow();
        protected abstract void DoHide();

        protected virtual void BannerOnAdLoaded(object sender, EventArgs e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad loaded, show now", this);
#endif
            OnLoaded?.Invoke(sender, e);
        }

        protected virtual void BannerOnAdClosed(object sender, EventArgs e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad closed", this);
#endif
            OnClosed?.Invoke(sender, e);

            if (IsShown && SupportHide)
            {
                Hide();
            }

            IsShown = false;

            Dispose();
            Request();
        }

        protected virtual void BannerOnAdOpening(object sender, EventArgs e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad opened", this);
#endif
            OnOpened?.Invoke(sender, e);
        }

        protected virtual void BannerOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.LogError("[ADVERTISEMENT] Ad failure: " + e.LoadAdError, this);
            OnFailed?.Invoke(sender, e);

            if (IsShown && SupportHide)
            {
                Hide();
            }

            IsShown = false;
        }

        protected virtual void BannerOnAdPaid(object sender, AdValueEventArgs e)
        {
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Ad paid: " + e.AdValue.Precision + ", " + e.AdValue.Value + ", " + e.AdValue.CurrencyCode);
#endif
            OnPaid?.Invoke(sender, e);
        }
    }

    public abstract class AdBase<T> : AdBase where T : AdsItem
    {
        #region Properties

        public T Preset { get; set; }

        protected override string AdId
        {
            get
            {
#if UNITY_ANDROID
                var adUnitId = Preset.AndroidId;
#elif UNITY_IPHONE
                var adUnitId = Preset.IOSId;
#else
                var adUnitId = "unexpected_platform";
#endif

                return adUnitId;
            }
        }

        #endregion
    }
#endif
}