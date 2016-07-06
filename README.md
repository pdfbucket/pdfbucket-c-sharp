# PDFBucket   [![Build Status](https://ci.appveyor.com/api/projects/status/github/pdfbucket/pdfbucket-c-sharp)](https://ci.appveyor.com/project/galopezb/pdfbucket-c-sharp/build/tests)

PDFBucket package allows you to integrate easily with the PDFBucket service. Tested against 4.5, 4.5.1, 4.5.2, 4.6 and 4.6.1 .NET Framework versions.

## Installation

To install [PDFBucket](https://www.nuget.org/packages/PDFBucket) nuget package, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console).

```
Install-Package PDFBucket
```

## Usage

To encrypt a URL in your code instantiate a PDFBucket object and use its `GenerateUrl` method.
The new pdfBucket will use `PDF_BUCKET_API_KEY`, `PDF_BUCKET_API_SECRET`, `PDF_BUCKET_API_HOST` (default is `api.pdfbucket.io`) ENV vars:

```c#
using PdfBucket;
...

PDFBucket pdfBucket = new PDFBucket.Builder().Build();
```

You can also set any the api params, overwriting then ENV VARS like this:

```c#
PDFBucket otherPDFBucket = new PDFBucket.Builder()
	.SetApiKey("ABCDEFGHIJKLMNO")
	.SetApiSecret("1234567890ABCDE")
	.SetApiHost("api.example.com")
	.Build();
```

And you get the encryptedUrl using the `GenerateUrl` method:

```c#
var EncryptedUrl = pdfBucket.GenerateUrl("http://example.com", "portrait", "A4", "0", "1");
```

* Possible values for orientation: "landscape", "portrait"
* Possible values for page size: "Letter", "A4"
* Possible values for margin: https://developer.mozilla.org/en-US/docs/Web/CSS/margin#Formal_syntax
* Possible values for zoom: https://developer.mozilla.org/en-US/docs/Web/CSS/@viewport/zoom#Formal_syntax
