export class DateHelper {

  // input : yyyy-MM-dd
  static parseDate(date: any) {
    var parts = date.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[0], parts[1] - 1, parts[2]); // months are 0-based
  }

  // static getTimestampSeconds(date: Date) {
  //   let time = this.parseDate(date).getTime();
  //   let timestampSeconds = Math.floor(time / 1000);
  //   return timestampSeconds;
  // }

  static getTimestampSeconds(dateTime: Date) {
    let timestampSeconds = Math.floor(dateTime.getTime() / 1000);
    return timestampSeconds;
  }

  static getDate(timestampSeconds: number) {
    return new Date(timestampSeconds);
  }

  // yyyy-MM-dd
  static getDateString(dateTime: Date){
    let yyyy = dateTime.getFullYear();
    let MM = String(dateTime.getMonth() + 1).padStart(2, '0'); //January is 0!
    let dd = String(dateTime.getDate()).padStart(2, '0');
    let HH = String(dateTime.getHours()).padStart(2, '0');
    let mm = String(dateTime.getMinutes()).padStart(2, '0');
    let ss = String(dateTime.getSeconds()).padStart(2, '0');

    return `${yyyy}-${MM}-${dd}`;
  }
}
