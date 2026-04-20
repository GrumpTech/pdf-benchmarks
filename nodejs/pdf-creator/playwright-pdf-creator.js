import { chromium, webkit, firefox } from "playwright";

export class PlaywrightPdfCreator {
  #initialized = false;
  #browserType;
  #browser;
  #browserContext;
  #page;

  constructor(browserType = "default") {
    const browserTypes = new Set(["default", "chromium", "webkit", "firefox"]);
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
      case "chromium":
        this.#browser = await chromium.launch({ channel: "chromium" });
        break;
      case "firefox":
        this.#browser = await firefox.launch();
        break;
      case "webkit":
        this.#browser = await webkit.launch();
        break;
      default:
        this.#browser = await chromium.launch();
    }
    this.#browserContext = await this.#browser.newContext();
    this.#page = await this.#browserContext.newPage();
    this.#initialized = true;
  }

  async fromHtml(data) {
    await this.initialize();
    return await this.fromHtmlWithPage(this.#page, data);
  }

  async fromHtmlNewPage(data) {
    await this.initialize();
    const context = await this.#browser.newContext();
    const page = await context.newPage();
    const result = await this.#fromHtmlWithPage(page, data);
    await page.close();
    await context.close();
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
    await this.#browserContext.close();
    await this.#browser.close();
    this.#initialized = false;
  }
}
