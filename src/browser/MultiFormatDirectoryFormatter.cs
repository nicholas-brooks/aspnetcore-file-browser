using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace browser
{
    // Code based on:
    // https://github.com/aspnet/StaticFiles/blob/91ad1c60f1558668627c04fbea99ea681ccae11a/src/Microsoft.AspNetCore.StaticFiles/HtmlDirectoryFormatter.cs
    public class MultiFormatDirectoryFormatter : HtmlDirectoryFormatter
    {
        private const string JsonUtf8 = "application/json; charset=utf-8";

        private static readonly Task CompletedTask = CreateCompletedTask();

        public MultiFormatDirectoryFormatter(HtmlEncoder encoder) : base(encoder)
        {

        }

        private static Task CreateCompletedTask()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        /// <summary>
        /// Generates an HTML view for a directory.
        /// </summary>
        public override Task GenerateContentAsync(HttpContext context, IEnumerable<IFileInfo> contents)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }

            if (IsHeadMethod(context.Request.Method))
            {
                // HEAD, no response body
                // todo: set ContentType
                return CompletedTask;
            }

            if (RequireJsonResponse(context.Request.Headers["Accept"].FirstOrDefault()))
            {
                string data = JsonConvert.SerializeObject(ToJson(contents));
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                context.Response.ContentType = JsonUtf8;
                context.Response.ContentLength = bytes.Length;
                return context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
            else
            {
                return base.GenerateContentAsync(context, contents);
            }
        }

        private bool RequireJsonResponse(string value)
        {
            return value.StartsWith("application/json", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsHeadMethod(string method)
        {
            return string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase);
        }

        private object ToJson(IEnumerable<IFileInfo> contents)
        {
            return new
            {
                directories = contents.Where(x => x.IsDirectory).Select(x => new
                {
                    x.Name,
                    x.LastModified,
                }),
                files = contents.Where(x => !x.IsDirectory).Select(x => new
                {
                    x.Name,
                    x.Length,
                    x.LastModified,
                })
            };
        }
    }
}
