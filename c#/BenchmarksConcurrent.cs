using BenchmarkDotNet.Attributes;
using Microsoft.Playwright;
using PdfBenchmarks.PdfCreator;

namespace PdfBenchmarks
{
    [MemoryDiagnoser]
    public class BenchmarksConcurrent
    {
        private static readonly int nPages = 5;
        private static List<string> ContentValues => Data.GetWikipediaContent(nPages);

        private readonly PlaywrightPdfCreator _playwrightPdfCreator = new();
        private readonly List<IPage> _playwrightPages = [];
        private int rotationOffset = 0;


        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await _playwrightPdfCreator.Initialize();
            await CreatePlaywrightPages();
        }

        [Benchmark]
        public async Task PlaywrightSerial()
        {
            for (int i = 0; i < nPages; i++)
            {
                await _playwrightPdfCreator.FromHtml(ContentValues[i]);
            }
        }

        [Benchmark]
        public async Task PlaywrightConcurrent()
        {
            rotationOffset++;
            var tasks = new List<Task>();
            for (int i = 0; i < nPages; i++)
            {
                tasks.Add(PlaywrightPdfCreator.FromHtml(_playwrightPages[(i + rotationOffset) % nPages], ContentValues[i]));
            }
            await Task.WhenAll(tasks);
        }

        private async Task CreatePlaywrightPages()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync();
            var context = await browser.NewContextAsync();
            for (int i = 0; i < nPages; i++)
            {
                _playwrightPages.Add(await context.NewPageAsync());
            }
        }
    }
}