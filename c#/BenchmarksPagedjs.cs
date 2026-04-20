using BenchmarkDotNet.Attributes;
using PdfBenchmarks.PdfCreator;

namespace PdfBenchmarks
{
    [MemoryDiagnoser]
    public class BenchmarksPagedjs
    {
        public enum ContentValue
        {
            Normal = 0,
            Paged = 1
        }

        [ParamsAllValues]
        public ContentValue ContentIndex { get; set; }

        private static List<string> ContentValues => [Data.GetContent(), Data.GetPagedContent()];

        private readonly PlaywrightPdfCreator _playwrightPdfCreator = new();
        private readonly PuppeteerSharpPdfCreator _puppeteerPdfCreator = new("chrome-headless-shell");

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await _playwrightPdfCreator.Initialize();
            await _puppeteerPdfCreator.Initialize();
        }

        [Benchmark]
        public async Task PlaywrightNewPage()
        {
            await _playwrightPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task Playwright()
        {
            await _playwrightPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PuppeteerNewPage()
        {
            await _puppeteerPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task Puppeteer()
        {
            await _puppeteerPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }
    }
}