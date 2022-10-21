namespace e_shop_api.Config
{
    public class JwtConfig
    {
        public string SignKey { get; set; }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        /// <summary>
        /// 有效期間 (分鐘)
        /// </summary>
        public int Expires { get; set; } 
    }
}