#if PCSOFT_ADS_ADMOB
namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class BannerAd
    {
        protected override bool DoShow()
        {
            _banner.LoadAd(_request);
            return true;
        }

        protected override bool DoHide()
        {
            _banner.Hide();
            Dispose();
            Request();

            return true;
        }
    }
}
#endif