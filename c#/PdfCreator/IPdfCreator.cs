namespace PdfBenchmarks.PdfCreator
{
    public interface IPdfCreator
    {
        public Task<byte[]> FromHtml(string data);
    }
}