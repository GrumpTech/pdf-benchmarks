import { promises as fs } from "node:fs";

export async function runBench(bench) {
  bench.addEventListener("cycle", (event) => {
    console.log("Finished " + event.task.name);
  });
  await bench.run();
  console.table(
    bench.tasks.map(({ name, result }) => ({
      "Task name": name,
      "Average time (ms)": Math.round(result.period * 1000) / 1000,
      "Standard deviation (ms)": Math.round(result.sd * 1000) / 1000,
      Samples: result.throughput?.samples?.length,
    })),
  );
}

export async function getFiles(number) {
  const filenames = await fs.readdir("../data/wikipedia");
  if (number > filenames.length) {
    console.error(
      `Requesting ${number} files and only ${filenames.length} files available.`,
    );
    process.exit(1);
  }
  return await Promise.all(
    filenames
      .slice(0, number)
      .map((n) => fs.readFile(`../data/wikipedia/${n}`, "utf-8")),
  );
}

export function getShortContent() {
  return "<div>test</div>";
}

export async function getContent() {
  return await fs.readFile("../data/page.html", "utf8");
}

export async function getPagedContent() {
  const layout = await fs.readFile("../data/pagedjs.html", "utf8");
  const content = await getContent();
  return layout.replace("{{> template}}", content);
}
