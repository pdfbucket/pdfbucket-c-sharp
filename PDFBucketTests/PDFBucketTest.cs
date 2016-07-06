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
                    .SetApiKey("LDMN2H2AD5OOL973DHTHDDSI5SDRL3VT")
                    .SetApiSecret("e5utyNlt1FgAdmFgDKjHjod5AsLK1mrSHSfR/85yGWY=")
                    .SetApiHost("staging.pdfbucket.io")
                    .Build();
            Assert.IsInstanceOfType(pdfBucket, typeof(PDFBucket));
        }
    }
}
