# PDF generation benchmarks

Performance is evaluated to determine whether [Playwright](https://playwright.dev) and [Puppeteer](https://pptr.dev) are well-suited for PDF generation. Both Playwright and Puppeteer are browser automation APIs that enable PDF creation using browser engines. Performance differences are likely due to variations in the underlying engines.

A [2018 PDF generation benchmark](https://www.hardkoded.com/blogs/pdf-generators-benchmark) showed that Puppeteer outperformed [wkhtmltopdf](https://wkhtmltopdf.org/). This repository presents more recent benchmarks conducted in both Node.js and C#, comparing the performance of Playwright and Puppeteer. Based on the results, the following conclusions can be drawn:

- Generating a 40-page PDF takes less than 200 ms.
- The best performance was observed with Playwright using Headless Chromium, followed by Puppeteer with Chrome for Testing Headless Shell. For larger content, regular browsers were outperformed by up to a factor of five.
- Pooling browser pages can improve performance by up to five times for Headless Chromium, even with larger content. For real browser implementations, performance improves up to a factor of one and a half.
- Benchmark times increase by up to a factor of six when paging content using a JavaScript library called Paged.js. The absolute increase is similar whether pages are reused or newly created.
- The performance of Node.js and C# is similar, with differences possibly caused by different browser versions.

Caching images and JavaScript in the browser could probably improve performance further. This is not considered in the benchmarks.

_Visit <https://grumptech.github.io/templates> for more information and related projects on PDF generation with open source libraries._

## Background

Playwright and Puppeteer are browser automation tools that allow developers to control browsers for tasks such as testing, scraping, and PDF generation. This section provides some background information about Playwright and Puppeteer.

### Supported browsers

As of writing, PDF generation through browser automation is supported on the following browsers:

| Tool           | Browser                           | PDF generation |
| -------------- | --------------------------------- | -------------- |
| **Playwright** | **Headless Chromium**             | Yes            |
| Playwright     | Chromium                          | Yes            |
| Playwright     | Firefox                           | No             |
| Playwright     | WebKit                            | No             |
| **Puppeteer**  | **Chrome for Testing**            | Yes            |
| Puppeteer      | Chrome for Testing Headless Shell | Yes            |
| Puppeteer      | Firefox                           | Yes            |

**Note:** The default browsers for Playwright and Puppeteer are "Headless Chromium" and "Chrome for Testing", respectively.

**Note 2:** In Playwright, attempting PDF generation with Firefox or WebKit will result in an exception: _"PDF generation is only supported for Headless Chromium"_.

**Note 3:** "Chrome for Testing" is a special version of Chrome designed for test automation. It is free from automatic updates and offers a versioned binary closely matching the regular Chrome browser. For more information, visit this [Chrome for Testing blog](https://developer.chrome.com/blog/chrome-for-testing).

**Note 4:** Both Chrome and Chromium offer a headless version: a lightweight, standalone browser optimized for tasks like screenshotting, PDF generation, and web scraping. For further details, visit:

- [Headless Chromium](https://chromium.googlesource.com/chromium/src/+/lkgr/headless/README.md)
- [Chrome Headless Shell](https://developer.chrome.com/blog/chrome-headless-shell)

**Note 5:** For official browser support information, visit:

- [Playwright browser support](https://playwright.dev/docs/browsers)
- [Puppeteer browser support](https://pptr.dev/supported-browsers)

### Supported languages

Playwright [supports](https://playwright.dev/docs/languages) JavaScript and TypeScript, Python, Java, and .NET.\
Puppeteer is a [JavaScript API](https://github.com/puppeteer/puppeteer). [PuppeteerSharp](https://www.puppeteersharp.com/) is an unofficial .NET port.

## Benchmark method

### Installation

For Node.js benchmarks, the necessary packages and browsers can be installed with the following steps:

1. Navigate to the [nodejs directory](/nodejs).
2. Run `npm install` to install the required packages.
3. Run `npx playwright install --with-deps` to install all default browsers with dependencies.
4. Run `npx puppeteer browsers install firefox` to install Firefox for Puppeteer.
5. Start the Node.js benchmarks with `npm run start`.

For C#, install the browsers with these steps:

1. Build the solution in the [c# directory](/c#).
2. Navigate to the appropriate binary folder and run `pwsh playwright install` to install the required browsers.
   _Alternatively, browser versions can be installed with the npm playwright package: `npx -y playwright@1.49 install --with-deps`._

### Benchmarking libraries

The Node.js and C# benchmarks are created using open-source libraries: `tinybench` and `BenchmarkDotNet`, respectively. Benchmarks are executed with default settings.

### Data

The benchmarks generate PDFs with the following content:

- **Short content:** `<div>test</div>`
- **Long content:** W3C specifications of [CSS Selectors](https://www.w3.org/TR/2005/WD-css3-selectors-20051215/).  
  _Length: approximately 40 pages._
- **Content for concurrency benchmark:** Webpages collected from Wikipedia using this [script](/scripts/get-data.js).

### Versions

The following versions of Playwright, Puppeteer, and PuppeteerSharp were used in the benchmark:

| Tool           | Version on Node.js | Version on C# |
| -------------- | ------------------ | ------------- |
| Playwright     | 1.50.0             | 1.49.0        |
| Puppeteer      | 24.1.1             |               |
| PuppeteerSharp |                    | 20.0.5        |

### Browser version numbers

The following browser versions were used during the benchmark:

| Tool       | Browser                           | Browser version on Node.js | Browser version on C# |
| ---------- | --------------------------------- | -------------------------- | --------------------- |
| Playwright | Headless Chromium                 | 133.0.6943.16              | 131.0.6778.33         |
| Playwright | Chromium                          | 133.0.6943.16              | 131.0.6778.33         |
| Puppeteer  | Chrome for Testing                | 132.0.6834.110             | 130.0.6723.69         |
| Puppeteer  | Chrome for Testing Headless Shell | 132.0.6834.110             | 130.0.6723.69         |
| Puppeteer  | Firefox                           | 134.0.2                    |                       |

## Results

These benchmarks measure the time required to generate PDFs from both short and long content. Since browsers only needs to be launched once, startup time is excluded. The benchmarks compare performance with and without reusing browser pages.

| Tool       | Browser                           | New page | Content |   Node.js mean | Node.js std dev\* |      C# mean | C# std dev\* |
| ---------- | --------------------------------- | -------- | ------- | -------------: | ----------------: | -----------: | -----------: |
| Playwright | Headless Chromium                 | Yes      | Short   |      63.302 ms |          2.535 ms |     67.69 ms |     1.399 ms |
| Playwright | Chromium                          | Yes      | Short   |     135.275 ms |         12.858 ms |    134.50 ms |     2.438 ms |
| Puppeteer  | Chrome for Testing                | Yes      | Short   |      88.568 ms |         11.911 ms |     75.06 ms |     2.068 ms |
| Puppeteer  | Chrome for Testing Headless Shell | Yes      | Short   |      57.866 ms |         11.238 ms |     49.14 ms |     0.689 ms |
| Puppeteer  | Firefox                           | Yes      | Short   |     397.248 ms |         61.432 ms |              |              |
| Playwright | Headless Chromium                 | No       | Short   |      26.122 ms |          5.065 ms |     29.62 ms |     4.233 ms |
| Playwright | Chromium                          | No       | Short   |      42.419 ms |          4.904 ms |     42.93 ms |     1.064 ms |
| Puppeteer  | Chrome for Testing                | No       | Short   |      38.551 ms |          4.263 ms |     34.87 ms |     1.220 ms |
| Puppeteer  | Chrome for Testing Headless Shell | No       | Short   |      19.428 ms |          3.236 ms | **24.01 ms** |     6.382 ms |
| Puppeteer  | Firefox                           | No       | Short   |     270.161 ms |         17.537 ms |              |              |
| Playwright | Headless Chromium                 | Yes      | Long    |     246.115 ms |          8.145 ms |    277.94 ms |    13.677 ms |
| Playwright | Chromium                          | Yes      | Long    |     424.237 ms |         37.329 ms |    447.18 ms |     8.667 ms |
| Puppeteer  | Chrome for Testing                | Yes      | Long    |     591.202 ms |         12.633 ms |    542.88 ms |    11.036 ms |
| Puppeteer  | Chrome for Testing Headless Shell | Yes      | Long    |     396.007 ms |          9.378 ms |    357.43 ms |    11.403 ms |
| Puppeteer  | Firefox                           | Yes      | Long    |  13,738.198 ms |         34.017 ms |              |              |
| Playwright | Headless Chromium                 | No       | Long    | **184.697 ms** |         11.033 ms |    199.51 ms |    17.239 ms |
| Playwright | Chromium                          | No       | Long    |     320.053 ms |         18.578 ms |    349.76 ms |    22.001 ms |
| Puppeteer  | Chrome for Testing                | No       | Long    |     551.342 ms |         63.201 ms |    496.40 ms |    14.524 ms |
| Puppeteer  | Chrome for Testing Headless Shell | No       | Long    |     351.525 ms |         59.523 ms |    315.06 ms |    19.772 ms |
| Puppeteer  | Firefox                           | No       | Long    |  13,578.214 ms |         54.685 ms |              |              |

\* std dev: standard deviation

Headless browsers clearly show the best performance. Additionally, reusing the browser page significantly boosts performance. When reusing browser pages, ensure that the content is within your control and does not rely on session data.

### Pages.js benchmark

[Paged.js](https://pagedjs.org/) is an open-source JavaScript library that paginates HTML content for printing, designed for creating books using HTML and CSS. These benchmark compare PDF generation performance with and without the paging algorithm.

| Tool       | New page | Paged | Node.js mean | Node.js std dev\* |    C# mean | C# std dev\* |
| ---------- | -------- | ----- | -----------: | ----------------: | ---------: | -----------: |
| Playwright | Yes      | No    |     254.8 ms |              22.1 |   269.3 ms |      8.21 ms |
| Puppeteer  | Yes      | No    |     400.9 ms |              28.3 |   351.6 ms |      7.58 ms |
| Playwright | Yes      | Yes   |   1,184.6 ms |              14.3 | 1,257.4 ms |     16.31 ms |
| Puppeteer  | Yes      | Yes   |   1,423.1 ms |              33.7 | 1,400.2 ms |      9.60 ms |
| Playwright | No       | No    |     186.1 ms |              11.2 |   200.0 ms |      3.20 ms |
| Puppeteer  | No       | No    |     339.6 ms |              11.4 |   297.5 ms |      4.53 ms |
| Playwright | No       | Yes   |   1,121.1 ms |              36.2 | 1,197.1 ms |      9.66 ms |
| Puppeteer  | No       | Yes   |   1,335.8 ms |              49.7 | 1,325.4 ms |     12.56 ms |

\* std dev: standard deviation

Benchmarked times increase up to a factor 6 when paging content using Paged.js. The absolute increase is similar whether reusing or creating a new page.

### Concurrency benchmark

Node.js and C# handle concurrency differently. Node.js runs all JavaScript in a single-threaded event loop, while C# handles concurrency through task switching.

Since PDFs are generated via browsers, both implementations may leverage multi-threading. This benchmark investigates the impact of generating multiple PDFs concurrently.

The following benchmarks measure PDF generation for five different Wikipedia pages. In the second benchmark, PDFs were created concurrently across five separate browser pages.

| Tool       | New page | Concurrent | Node.js mean | Node.js std dev\* | C# mean | C# std dev\* |
| ---------- | -------- | ---------- | -----------: | ----------------: | ------: | -----------: |
| Playwright | No       | No         |      3.457 s |           0.206 s | 2.620 s |      0.041 s |
| Playwright | No       | Yes        |      2.030 s |           1,601 s | 1.920 s |      0.400 s |

\* std dev: standard deviation

The performance gain with Node.js from concurrent PDF generation seems to be at least as big as with C#.
