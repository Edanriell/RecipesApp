namespace RecipesApp.Mobile.Misc;

internal static class HttpClientHelper
{
    internal static HttpClient GetPlatformHttpClient(string baseAddress)
    {
        if (DeviceInfo.Platform == DevicePlatform.Android ||
            DeviceInfo.Platform == DevicePlatform.iOS)
        {
            var handler = new HttpsClientHandlerService();
            return new HttpClient(handler.GetPlatformMessageHandler())
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        return new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
    }
}