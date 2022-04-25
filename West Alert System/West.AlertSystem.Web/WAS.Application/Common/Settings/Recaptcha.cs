namespace WAS.Application.Common.Settings
{
    public class Recaptcha
    {
        /// <summary>
        /// Google Recaptcha data SiteKey
        /// </summary>
        public string RecaptchaSiteKey { get; set; }

        /// <summary>
        /// flag to enable/disable captcha
        /// </summary>
        public string EnableCaptcha { get; set; }
    }
}
