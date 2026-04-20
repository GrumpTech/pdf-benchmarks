import { Bench } from "tinybench";
import { PlaywrightPdfCreator } from "./pdf-creator/playwright-pdf-creator.js";
import { PuppeteerPdfCreator } from "./pdf-creator/puppeteer-pdf-creator.js";
import { runBench, getContent, getPagedContent } from "./benchmark-utils.js";

export async function runBenchmarksPagedjs() {
  const playwrightPdfCreator = new PlaywrightPdfCreator();
  await playwrightPdfCreator.initialize();
  const puppeteerPdfCreator = new PuppeteerPdfCreator("chrome-headless-shell");
  await puppeteerPdfCreator.initialize();

  const content = await getContent();
  const pagedContent = await getPagedContent();
  const bench = new Bench();
  bench
    .add("PlaywrightNewPage", async () => {
      await playwrightPdfCreator.fromHtmlNewPage(content);
    })
    .add("PagedPlaywrightNewPage", async () => {
      await playwrightPdfCreator.fromHtmlNewPage(pagedContent);
    })
    .add("Playwright", async () => {
      await playwrightPdfCreator.fromHtml(content);
    })
    .add("PagedPlaywright", async () => {
      await playwrightPdfCreator.fromHtml(pagedContent);
    })
    .add("PuppeteerNewPage", async () => {
      await puppeteerPdfCreator.fromHtmlNewPage(content);
    })
    .add("PagedPuppeteerNewPage", async () => {
      await puppeteerPdfCreator.fromHtmlNewPage(pagedContent);
    })
    .add("Puppeteer", async () => {
      await puppeteerPdfCreator.fromHtml(content);
    })
    .add("PagedPuppeteer", async () => {
      await puppeteerPdfCreator.fromHtml(pagedContent);
    });
  await runBench(bench);

  await playwrightPdfCreator.destroy();
  await puppeteerPdfCreator.destroy();
}
