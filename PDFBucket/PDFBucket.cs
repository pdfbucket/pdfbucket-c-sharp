using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

namespace PdfBucket
{
    public sealed class PDFBucket
    {
        public const string DefaultHost = "api.pdfbucket.io";
        private string ApiKey;
        private string ApiSecret;
        private string ApiHost;

        private PDFBucket(string ApiKey, string ApiSecret, string ApiHost)
        {
            this.ApiKey = ApiKey;
            this.ApiSecret = ApiSecret;
            this.ApiHost = ApiHost;
        }

        public string GenerateUrl(string Uri, string Orientation, string PageSize, string Margin, string Zoom)
        {
            if (Uri != null && !Uri.Trim().Equals(String.Empty) &&
                (Orientation == "portrait" || Orientation == "landscape") &&
                (PageSize == "A4" || PageSize == "Letter"))
            {
                var EncryptedUri = encrypt(ApiSecret, Uri);

                var Params = new Dictionary<string, string>
                {
                    { "orientation", Orientation },
                    { "page_size", PageSize },
                    { "margin", Margin },
                    { "zoom", Zoom },
                    { "api_key", ApiKey },
                    { "encrypted_uri", EncryptedUri }
                };

                var UrlEncoded = new FormUrlEncodedContent(Params);
                var Query = UrlEncoded.ReadAsStringAsync().Result;

                UriBuilder Builder = new UriBuilder();
                Builder.Host = ApiHost;
                Builder.Path = "/api/convert";
                Builder.Query = Query;
                Builder.Scheme = "https";

                return Builder.ToString();
            }
            else if (Uri == null || Uri.Trim().Equals(String.Empty))
            {
                throw new ArgumentException("Invalid Uri value, must be not blank");
            }
            else if (Orientation != "portrait" && Orientation != "landscape")
            {
                throw new ArgumentException("Invalid orientation value, must be portrait or landscape");
            }
            else if (PageSize != "A4" && PageSize != "Letter")
            {
                throw new ArgumentException("Invalid pageSize value, must be A4 or Letter");
            }
            return null;
        }

        private string encrypt(string Key, string Content)
        {
            byte[] BinaryKey = Convert.FromBase64String(Key);
            byte[] BinaryContent = ASCIIEncoding.UTF8.GetBytes(Content);

            byte[] RandomIV = new byte[16];

            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(RandomIV);

            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            cipher.Init(true, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", BinaryKey), RandomIV));

            byte[] encryptedBytes = cipher.DoFinal(BinaryContent);
            string base64EncryptedOutputString = Convert.ToBase64String(Concat(RandomIV, encryptedBytes));

            return base64EncryptedOutputString;
        }

        private byte[] Concat(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public sealed class Builder
        {
            private string ApiKey;
            private string ApiSecret;
            private string ApiHost;

            public Builder()
            {
                ApiKey = Environment.GetEnvironmentVariable("PDF_BUCKET_API_KEY");
                ApiSecret = Environment.GetEnvironmentVariable("PDF_BUCKET_API_SECRET");
                ApiHost = Environment.GetEnvironmentVariable("PDF_BUCKET_API_HOST");
                ApiHost = ApiHost == null ? DefaultHost : ApiHost;
            }

            public Builder SetApiKey(string ApiKey)
            {
                this.ApiKey = ApiKey;
                return this;
            }

            public Builder SetApiSecret(string ApiSecret)
            {
                this.ApiSecret = ApiSecret;
                return this;
            }

            public Builder SetApiHost(string ApiHost)
            {
                this.ApiHost = ApiHost;
                return this;
            }

            public PDFBucket Build()
            {
                if (ApiKey == null || ApiKey.Trim().Equals(String.Empty))
                {
                    throw new ArgumentException("bucket ApiKey is required");
                }

                if (ApiSecret == null || ApiSecret.Trim().Equals(String.Empty))
                {
                    throw new ArgumentException("bucket ApiSecret is required");
                }

                return new PDFBucket(ApiKey, ApiSecret, ApiHost);
            }
        }
    }
}
