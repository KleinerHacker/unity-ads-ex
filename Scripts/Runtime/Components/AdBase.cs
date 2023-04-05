#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public abstract partial class AdBase : MonoBehaviour
    {
        private bool isShown;

        #region Properties

        public bool IsShown
        {
            get => isShown;
            protected set
            {
                isShown = value;
                Debug.Log("[ADVERTISEMENT] > Is Shown set to " + value);
            }
        }

        public abstract bool IsReady { get; }
        public abstract bool SupportHide { get; }

        protected abstract string AdId { get; }

        #endregion

        #region Events

        public event EventHandler OnLoaded;
        public event EventHandler OnOpened;
        public event EventHandler OnClosed;
        public event EventHandler<AdFailedToLoadEventArgs> OnFailed;
        public event EventHandler<AdValueEventArgs> OnPaid;
        public event EventHandler OnImpressionRecorded;
        public event EventHandler OnClicked;

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
}
#endif