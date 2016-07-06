using System;
using PdfBucket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFBucketTests
{
    [TestClass]
    public class PDFBucketTest
    {
        [TestMethod, TestCategory("ValidParameters")]
        public void ShouldReturnAPDFBucketObject()
        {
            PDFBucket pdfBucket = new PDFBucket.Builder()
                    .SetApiKey("PIQ7T3GOM7D36R0O67Q97UM3F0I6CPB5")
                    .SetApiSecret("HieMN8dvi5zfSbKvqxKccxDo3LozqOIrY59U/jrZY54=")
                    .Build();
            Assert.IsInstanceOfType(pdfBucket, typeof(PDFBucket));
        }
    }
}
