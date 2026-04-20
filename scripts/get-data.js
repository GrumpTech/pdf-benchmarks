const fs = require("node:fs");

const subjects = [
  "Earth",
  "Sun",
  "Moon",
  "Black hole",
  "Mars",
  "Solar System",
  "Pluto",
  "Big Bang",
  "Milky Way",
  "Saturn",
  "Kepler's Supernova",
  "Universe",
  "Dark matter",
  "Galaxy",
  "Jupiter",
  "Venus",
  "Supernovae",
  "Andromeda Galaxy",
  "Mercury (planet)",
  "Neptune",
  "Uranus",
  "Solar eclipse",
  "Lunar eclipse",
  "Dark energy",
  "Halley's Comet",
  "Event horizon",
  "Titan (moon)",
  "Eclipse",
  "Sirius",
  "Winter solstice",
];

function downloadPage(subject) {
  const page = subject.replace(/ /g, "%20");
  fetch(`https://en.wikipedia.org/wiki/${page}?action=render`)
    .then((response) => response.text())
    .then((content) => {
      fs.writeFileSync(
        `../data/wikipedia/${subject}.html`,
        '<base href="https://en.wikipedia.org/" />\n' + content,
      );
    });
}

!fs.existsSync("../data/wikipedia") && fs.mkdirSync("../data/wikipedia");
subjects.forEach((s) => downloadPage(s));
