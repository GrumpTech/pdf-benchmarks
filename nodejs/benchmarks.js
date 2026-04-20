import { Bench } from "tinybench";
import { PlaywrightPdfCreator } from "./pdf-creator/playwright-pdf-creator.js";
import { PuppeteerPdfCreator } from "./pdf-creator/puppeteer-pdf-creator.js";
import { runBench } from "./benchmark-utils.js";

export async function runBenchmarks(content) {
  const playwrightPdfCreator = new PlaywrightPdfCreator();
  const playwrightChromiumPdfCreator = new PlaywrightPdfCreator("chromium");
  const puppeteerPdfCreator = new PuppeteerPdfCreator();
  const puppeteerShellPdfCreator = new PuppeteerPdfCreator(
    "chrome-headless-shell",
  );
  const puppeteerFirefoxPdfCreator = new PuppeteerPdfCreator("firefox");

  await playwrightPdfCreator.initialize();
  await playwrightChromiumPdfCreator.initialize();
  await puppeteerPdfCreator.initialize();
  await puppeteerShellPdfCreator.initialize();
  await puppeteerFirefoxPdfCreator.initialize();

  const bench = new Bench();
  bench
    .add(`Playwright New Page`, async () => {
      await playwrightPdfCreator.fromHtmlNewPage(content);
    })
    .add(`Playwright Chromium New Page`, async () => {
      await playwrightChromiumPdfCreator.fromHtmlNewPage(content);
    })
    .add(`Puppeteer New Page`, async () => {
      await puppeteerPdfCreator.fromHtmlNewPage(content);
    })
    .add(`Puppeteer Shell New Page`, async () => {
      await puppeteerShellPdfCreator.fromHtmlNewPage(content);
    })
    .add(`Puppeteer Firefox New Page`, async () => {
      await puppeteerFirefoxPdfCreator.fromHtmlNewPage(content);
    })
    .add(`Playwright`, async () => {
      await playwrightPdfCreator.fromHtml(content);
    })
    .add(`Playwright Chromium`, async () => {
      await playwrightChromiumPdfCreator.fromHtml(content);
    })
    .add(`Puppeteer`, async () => {
      await puppeteerPdfCreator.fromHtml(content);
    })
    .add(`Puppeteer Shell`, async () => {
      await puppeteerShellPdfCreator.fromHtml(content);
    })
    .add(`Puppeteer Firefox`, async () => {
      await puppeteerFirefoxPdfCreator.fromHtml(content);
    });
  await runBench(bench);

  await playwrightPdfCreator.destroy();
  await playwrightChromiumPdfCreator.destroy();
  await puppeteerPdfCreator.destroy();
  await puppeteerShellPdfCreator.destroy();
  await puppeteerFirefoxPdfCreator.destroy();
}
