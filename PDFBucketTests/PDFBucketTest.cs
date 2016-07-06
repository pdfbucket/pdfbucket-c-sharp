using System;
using PdfBucket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFBucketTests
{
    [TestClass]
    public class PDFBucketTest
    {
        PDFBucket pdfBucket;
        [TestInitialize()]
        public void Startup()
        {
            pdfBucket = new PDFBucket.Builder()
                    .SetApiKey("PIQ7T3GOM7D36R0O67Q97UM3F0I6CPB5")
                    .SetApiSecret("HieMN8dvi5zfSbKvqxKccxDo3LozqOIrY59U/jrZY54=")
                    .Build();
        }

        [TestMethod]
        public void ShouldReturnAPDFBucketObject()
        {
            Assert.IsInstanceOfType(pdfBucket, typeof(PDFBucket));
        }

        [TestMethod]
        public void ShouldGenerateEncryptedUrlPassingValidParameters()
        {
            var EncryptedUrl = pdfBucket.GenerateUrl("https://www.joyent.com/", "landscape", "A4", "2px", "0.7");
            Assert.IsInstanceOfType(EncryptedUrl, typeof(String));
        }

        [TestMethod]
        public void ThrowsInvalidUriPassedInWhenUriIsBlank()
        {
            try
            {
                var encryptedUrl = pdfBucket.GenerateUrl(null, "landscape", "A4", "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid uri value, must be not blank"));
            }

            try
            {
                var encryptedUrl = pdfBucket.GenerateUrl("    ", "landscape", "A4", "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid uri value, must be not blank"));
            }
        }

        [TestMethod]
        public void ThrowsWhenOrientationInvalid()
        {
            try
            {
                pdfBucket.GenerateUrl("https://www.joyent.com/", null, "A4", "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid orientation value, must be portrait or landscape"));
            }

            try
            {
                pdfBucket.GenerateUrl("https://www.joyent.com/", "something", "A4", "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid orientation value, must be portrait or landscape"));
            }
        }

        [TestMethod]
        public void ThrowsWhenPageSizeInvalid()
        {
            try
            {
                pdfBucket.GenerateUrl("https://www.joyent.com/", "landscape", null, "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid pageSize value, must be A4 or Letter"));
            }

            try
            {
                pdfBucket.GenerateUrl("https://www.joyent.com/", "landscape", "something", "2px", "0.7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Invalid pageSize value, must be A4 or Letter"));
            }
        }

        [TestMethod]
        public void ShouldCreatePDFBucketThroughEnvVars()
        {
            Environment.SetEnvironmentVariable("PDF_BUCKET_API_KEY", "PIQ7T3GOM7D36R0O67Q97UM3F0I6CPB5");
            Environment.SetEnvironmentVariable("PDF_BUCKET_API_SECRET", "HieMN8dvi5zfSbKvqxKccxDo3LozqOIrY59U/jrZY54=");
            var pdfBucket = new PDFBucket.Builder().Build();
            Environment.SetEnvironmentVariable("PDF_BUCKET_API_KEY", "");
            Environment.SetEnvironmentVariable("PDF_BUCKET_API_SECRET", "");
            Assert.IsInstanceOfType(pdfBucket, typeof(PDFBucket));
        }

        [TestMethod]
        public void CreatePDFBucketWithInvalidParams()
        {
            try
            {
                new PDFBucket.Builder()
                        .SetApiKey("PIQ7T3GOM7D36R0O67Q97UM3F0I6CPB5")
                        .Build();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("bucket ApiSecret is required"));
            }

            try
            {
                new PDFBucket.Builder()
                        .SetApiSecret("HieMN8dvi5zfSbKvqxKccxDo3LozqOIrY59U/jrZY54=")
                        .Build();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("bucket ApiKey is required"));
            }
        }
    }
}
