namespace PdfBenchmarks.PdfCreator
{
    public static class IPdfCreatorExtensions
    {
        public static async Task CreateSampleOutput(this IPdfCreator pdfCreator, string filename, string content)
        {
            var output = await pdfCreator.FromHtml(content);
            Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}output");
            await File.WriteAllBytesAsync($"{AppDomain.CurrentDomain.BaseDirectory}output/{filename}.pdf", output);
        }
    }
}
