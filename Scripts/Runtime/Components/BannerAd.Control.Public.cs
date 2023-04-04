#if PCSOFT_ADS_ADMOB
namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class BannerAd
    {
        protected override void DoShow()
        {
            _banner.LoadAd(_request);
        }

        protected override void DoHide()
        {
            _banner.Hide();
            Dispose();
            Request();
        }
    }
}
#endif