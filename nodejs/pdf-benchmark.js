import os from "node:os";
import readline from "node:readline";
import { getShortContent, getContent } from "./benchmark-utils.js";
import { runBenchmarks } from "./benchmarks.js";
import { runBenchmarksPagedjs } from "./benchmarks-pagedjs.js";
import { runBenchmarksConcurrent } from "./benchmarks-concurrent.js";

async function main() {
  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
  });
  console.log("Number of logical cores", os.cpus().length);
  const answer = await new Promise((resolve) => {
    rl.question(
      "0 - short content\n1 - long content\n2 - pagedjs\n3 - concurrent\n",
      resolve,
    );
  });
  rl.close();

  switch (answer) {
    case "0":
      await runBenchmarks(getShortContent());
      break;
    case "1":
      await runBenchmarks(await getContent());
      break;
    case "2":
      await runBenchmarksPagedjs();
      break;
    case "3":
      await runBenchmarksConcurrent();
      break;
    default:
      console.log("Unkown selection");
  }
}
main();
