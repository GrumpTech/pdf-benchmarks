using BenchmarkDotNet.Running;
using PdfBenchmarks.PdfCreator;
using System.Text;

namespace PdfBenchmarks
{
    static class Program
    {
        static async Task Main()
        {
            await CreatePdfs();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        }

        static async Task CreatePdfs()
        {
            Console.WriteLine("Create benchmark PDFs: (y/n)");
            var createPdfs = Console.ReadLine();
            if (createPdfs?.ToLower() != "y")
            {
                return;
            }

            var playwrightPdfCreator = new PlaywrightPdfCreator();
            var puppeteerSharpPdfCreator = new PuppeteerSharpPdfCreator();

            await playwrightPdfCreator.CreateSampleOutput("playwright_short", Data.ShortContent);
            await puppeteerSharpPdfCreator.CreateSampleOutput("puppeteer_short", Data.ShortContent);

            await playwrightPdfCreator.CreateSampleOutput("playwright", Data.GetContent());
            await puppeteerSharpPdfCreator.CreateSampleOutput("puppeteer", Data.GetContent());

            await playwrightPdfCreator.CreateSampleOutput("playwright_paged", Data.GetPagedContent());
            await puppeteerSharpPdfCreator.CreateSampleOutput("puppeteer_paged", Data.GetPagedContent());

            Console.WriteLine($"Benchmark PDFs written to {AppDomain.CurrentDomain.BaseDirectory}output{Path.DirectorySeparatorChar}");
        }
    }
}