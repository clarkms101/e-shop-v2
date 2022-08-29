// 會合併到service-proxies.ts
export class ApiBase {
  authToken = '';
  protected constructor() {
  }

  setAuthToken(token: string) {
    this.authToken = token;
  }

  protected transformOptions(options: any): Promise<any> {
    options.headers = options.headers.append('authorization', this.authToken);
    // options.headers = options.headers.append('authorization', `Bearer ${this.authToken}`);
    return Promise.resolve(options);
  }
}
