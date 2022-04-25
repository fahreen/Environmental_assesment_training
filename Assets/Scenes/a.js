var lineReader = require('readline').createInterface({
  input: require('fs').createReadStream('TestOffice.unity')
});

let set = new Set();

lineReader.on('line', function (line) {
  if (line.includes('!u!')) {
    let id = line.split(' ')[2];
    if (set.has(id)) {
      console.log(id);
    } else {
      set.add(line.split(' ')[2]);
    }
  }
  // console.log('Line from file:', line);
});