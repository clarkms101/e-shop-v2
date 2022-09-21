export class DateHelper {

  // input : yyyy-MM-dd
  static parseDate(date: any) {
    var parts = date.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[0], parts[1] - 1, parts[2]); // months are 0-based
  }

  static getTimestampSeconds(date: Date) {
    let time = this.parseDate(date).getTime();
    let timestampSeconds = Math.floor(time / 1000);
    return timestampSeconds;
  }

  static getDate(timestampSeconds: number) {
    return new Date(timestampSeconds * 1000);
  }
}
