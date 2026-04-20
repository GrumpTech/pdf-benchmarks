using Microsoft.Playwright;

namespace PdfBenchmarks.PdfCreator
{
    public class PlaywrightPdfCreator : IPdfCreator
    {
        private bool initialized = false;
        private readonly string browserType;
        private IBrowser _browser = null!;
        private IPage _page = null!;

        public PlaywrightPdfCreator(string browserType = "default")
        {
            var browserTypes = new HashSet<string>(["default", "chromium", "webkit", "firefox"]);
            if (!browserTypes.Contains(browserType))
            {
                throw new ArgumentException("Unknown browser");
            }
            this.browserType = browserType;
        }


        public async Task Initialize()
        {
            if (initialized)
            {
                return;
            }
            var playwright = await Playwright.CreateAsync();
            _browser = browserType switch
            {
                "chromium" => await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions()
                {
                    Channel = "chromium"
                }),
                "firefox" => await playwright.Webkit.LaunchAsync(),
                "webkit" => await playwright.Webkit.LaunchAsync(),
                _ => await playwright.Chromium.LaunchAsync(),
            };
            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
            initialized = true;
        }

        public async Task<byte[]> FromHtml(string data)
        {
            await Initialize();
            return await FromHtml(_page, data);
        }

        public async Task<byte[]> FromHtmlNewPage(string data)
        {
            await Initialize();
            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();
            var result = await FromHtmlInternal(page, data);
            await page.CloseAsync();
            await context.CloseAsync();
            return result;
        }

        public static async Task<byte[]> FromHtml(IPage page, string data) {
            var res = await FromHtmlInternal(page, data);
            await page.ReloadAsync();
            return res;
        }

        public static async Task<byte[]> FromHtmlInternal(IPage page, string data)
        {
            await page.SetContentAsync(data);

            // allow page to run javascript before creating pdf
            await page.WaitForFunctionAsync("window.readyForPdf !== false");

            var pdf = await page.PdfAsync(new PagePdfOptions
            {
                Format = "a4",
                PreferCSSPageSize = true,
                PrintBackground = true,
                Margin = new Margin
                {
                    Left = "0",
                    Top = "0",
                    Right = "0",
                    Bottom = "0"
                },
            });
            return pdf;
        }
    }
}
