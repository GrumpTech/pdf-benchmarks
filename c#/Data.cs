namespace PdfBenchmarks
{
    public static class Data
    {
        public const string ShortContent = "<div>test</div>";
        private static string? content;
        private static string? pagedContent;
        private static List<string>? wikipediaContent;

        public static string GetContent()
        {
            content ??= File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}Data/page.html");
            return content;
        }

        public static string GetPagedContent()
        {
            pagedContent ??= File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}Data/pagedjs.html").Replace("{{> template}}", GetContent());
            return pagedContent;
        }

        public static List<string> GetWikipediaContent(int number)
        {
            if (wikipediaContent == null || wikipediaContent.Count < number)
            {
                var files = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}Data/wikipedia");
                if (number > files.Length)
                {
                    throw new ArgumentException($"Requesting {number} files and only {files.Length} files available.");
                }
                wikipediaContent = files.Take(number).Select(f => File.ReadAllText($"{f}")).ToList();
            }
            return wikipediaContent;
        }
    }
}