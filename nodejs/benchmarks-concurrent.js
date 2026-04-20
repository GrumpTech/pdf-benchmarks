import { Bench } from "tinybench";
import { chromium } from "playwright";
import { PlaywrightPdfCreator } from "./pdf-creator/playwright-pdf-creator.js";
import { runBench, getFiles } from "./benchmark-utils.js";

export async function runBenchmarksConcurrent() {
  const playwrightPdfCreator = new PlaywrightPdfCreator();
  const playwrightBrowser = await chromium.launch();
  const playwrightBrowserContext = await playwrightBrowser.newContext();

  const nPages = 5;
  const files = await getFiles(nPages);
  const playwrightPages = await createPlaywrightPages(
    playwrightBrowserContext,
    nPages,
  );
  let rotationOffset = 0;

  const bench = new Bench();
  bench
    .add("PlaywrightSerial", async () => {
      for (let i = 0, l = files.length; i < l; i++) {
        await playwrightPdfCreator.fromHtmlWithPage(
          playwrightPages[0],
          files[i],
        );
      }
    })
    .add("PlaywrightConcurrent", async () => {
      rotationOffset++;
      await Promise.all(
        files.map((f, idx) =>
          playwrightPdfCreator.fromHtmlWithPage(
            playwrightPages[(idx + rotationOffset) % nPages],
            f,
          ),
        ),
      );
    });
  await runBench(bench);

  await destroyPlaywrightPages(playwrightPages);
  await playwrightBrowserContext.close();
  await playwrightBrowser.close();
}

async function createPlaywrightPages(browserContext, nPages) {
  const promises = [];
  for (let i = 0; i < nPages; i++) {
    promises.push(browserContext.newPage());
  }
  return await Promise.all(promises);
}

async function destroyPlaywrightPages(pages) {
  await Promise.all(pages.map((p) => p.close()));
}
