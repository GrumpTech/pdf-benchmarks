using BenchmarkDotNet.Attributes;
using PdfBenchmarks.PdfCreator;

namespace PdfBenchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        public enum ContentValue
        {
            Short = 0,
            Long = 1,
        }

        [ParamsAllValues]
        public ContentValue ContentIndex { get; set; }

        private static List<string> ContentValues => [Data.ShortContent, Data.GetContent()];

        private readonly PlaywrightPdfCreator _playwrightPdfCreator = new();
        private readonly PlaywrightPdfCreator _playwrightChromiumPdfCreator = new("chromium");
        private readonly PuppeteerSharpPdfCreator _puppeteerSharpPdfCreator = new();
        private readonly PuppeteerSharpPdfCreator _puppeteerSharpShellPdfCreator = new("chrome-headless-shell");

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await _playwrightPdfCreator.Initialize();
            await _playwrightChromiumPdfCreator.Initialize();
            await _puppeteerSharpPdfCreator.Initialize();
            await _puppeteerSharpShellPdfCreator.Initialize();
        }

        [Benchmark]
        public async Task PlaywrightNewPage()
        {
            await _playwrightPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PlaywrightChromiumNewPage()
        {
            await _playwrightChromiumPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PuppeteerNewPage()
        {
            await _puppeteerSharpPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PuppeteerShellNewPage()
        {
            await _puppeteerSharpShellPdfCreator.FromHtmlNewPage(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task Playwright()
        {
            await _playwrightPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PlaywrightChromium()
        {
            await _playwrightChromiumPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task Puppeteer()
        {
            await _puppeteerSharpPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }

        [Benchmark]
        public async Task PuppeteerShell()
        {
            await _puppeteerSharpShellPdfCreator.FromHtml(ContentValues[(int)ContentIndex]);
        }
    }
}