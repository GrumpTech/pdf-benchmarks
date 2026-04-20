import puppeteer from "puppeteer";

export class PuppeteerPdfCreator {
  #initialized = false;
  #browserType;
  #browser = null;
  #page = null;

  constructor(browserType = "Chrome for Testing") {
    const browserTypes = new Set([
      "Chrome for Testing",
      "chrome-headless-shell",
      "firefox",
    ]);
    if (!browserTypes.has(browserType)) {
      throw "Unknown browser";
    }
    this.#browserType = browserType;
  }

  async initialize() {
    if (this.#initialized) {
      return;
    }
    switch (this.#browserType) {
      case "Chrome for Testing":
        this.#browser = await puppeteer.launch();
        break;
      case "chrome-headless-shell":
        this.#browser = await puppeteer.launch({ headless: "shell" });
        break;
      default:
        this.#browser = await puppeteer.launch({ browser: this.#browserType });
    }
    this.#page = await this.#browser.newPage();
    this.#initialized = true;
  }

  async fromHtml(data) {
    await this.initialize();
    return await this.fromHtmlWithPage(this.#page, data);
  }

  async fromHtmlNewPage(data) {
    await this.initialize();
    const page = await this.#browser.newPage();
    const result = await this.#fromHtmlWithPage(page, data);
    await page.close();
    return result;
  }

  async fromHtmlWithPage(page, data) {
    const res = await this.#fromHtmlWithPage(page, data);
    await page.reload();
    return res;
  }

  async #fromHtmlWithPage(page, data) {
    await page.setContent(data);

    // allow page to run javascript before creating pdf
    await page.waitForFunction("window.readyForPdf !== false");

    const pdf = await page.pdf({
      format: "a4",
      printBackGround: true,
      preferCSSPageSize: true,
      margin: {
        left: 0,
        top: 0,
        right: 0,
        bottom: 0,
      },
    });
    return Buffer.from(pdf);
  }

  async destroy() {
    await this.#page.close();
    await this.#browser.close();
    this.#initialized = false;
  }
}
