using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PdfBenchmarks.PdfCreator
{
    public class PuppeteerSharpPdfCreator : IPdfCreator
    {
        private bool initialized = false;
        private readonly string browserType;
        private IBrowser _browser = null!;
        private IPage _page = null!;

        public PuppeteerSharpPdfCreator(string browserType = "Chrome for Testing")
        {
            var browserTypes = new HashSet<string>(["Chrome for Testing", "chrome-headless-shell"]);
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
            await new BrowserFetcher().DownloadAsync();
            _browser = browserType switch
            {
                "chrome-headless-shell" => await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    HeadlessMode = HeadlessMode.Shell,
                }),
                _ => await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                }),
            };
            _page = await _browser.NewPageAsync();
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
            var page = await _browser.NewPageAsync();
            var result = await FromHtmlInternal(page, data);
            await page.CloseAsync();
            return result;
        }

        public static async Task<byte[]> FromHtml(IPage page, string data) {
            var res = await FromHtmlInternal(page, data);
            await page.ReloadAsync();
            return res;
        }


        private static async Task<byte[]> FromHtmlInternal(IPage page, string data)
        {
            await page.SetContentAsync(data);

            // allow page to run javascript before creating pdf
            await page.WaitForExpressionAsync("window.readyForPdf !== false");

            var pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PreferCSSPageSize = true,
                PrintBackground = true,
                MarginOptions = new MarginOptions
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
