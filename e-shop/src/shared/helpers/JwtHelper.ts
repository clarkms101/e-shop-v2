export class JwtHelper {
  static parseJwt(): AdminInfo {
    let token = localStorage.getItem("adminJWT") as string;
    let base64Url = token.split(".")[1];
    let base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    let jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map(function (c) {
          return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join("")
    );

    return JSON.parse(jsonPayload) as AdminInfo;
  }
}

export class AdminInfo {
  JwtKeyApiAccessKey: string | undefined;
  JwtKeyAdminPermission: string | undefined;
  JwtKeyAdminAccount: string | undefined;
  JwtKeyAdminSystemUserId: number | undefined;
}
