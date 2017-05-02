# ASP.NET Core File Browser

ASP.NET core app to browse directories and retrieve files using UseStaticFiles and UseDirectoryBrowser.

Additionally you can get a directory listing as JSON when you use `Accept: application/json`.  Check `MultiFormatDirectoryFormatter.cs` for source.

e.g.

```
GET http://localhost:54010/_browse/ HTTP/1.1
User-Agent: Fiddler
Accept: application/json; charset=utf-8
Host: localhost:54010
```

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Server: Kestrel
Date: Tue, 02 May 2017 05:48:04 GMT

{
  "directories": [
    {
      "Name": "example",
      "LastModified": "2016-02-07T09:32:50.2612344+00:00"
    }
  ],
  "files": [
    {
      "Name": "test.png",
      "Length": 2958,
      "LastModified": "2016-02-07T09:36:50+00:00"
    },
    {
      "Name": "test2.xml",
      "Length": 3819,
      "LastModified": "2016-04-18T01:58:31.3781711+00:00"
    },
    {
      "Name": "test3.pdf",
      "Length": 25757176,
      "LastModified": "2012-10-01T21:35:32+00:00"
    }
  ]
}
```

