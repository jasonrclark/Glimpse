﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseResponseFilter : Stream
    {
        internal Stream OutputStream { get; set; }
        internal HttpContextBase Context { get; set; }

        private Regex bodyEnd = new Regex("</body>", RegexOptions.Compiled | RegexOptions.Multiline);

        public GlimpseResponseFilter(Stream output, HttpContextBase context)
        {
            OutputStream = output;
            Context = context;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Convert the content in buffer to a string
            var encoding = Context.Response.ContentEncoding;
            string contentInBuffer = encoding.GetString(buffer);
            // Buffer content in responseContent until we reach the end of the page's markup

            if (bodyEnd.IsMatch(contentInBuffer) && Context.GetGlimpseMode() == GlimpseMode.On)
            {
                var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
                var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

                var html = string.Format(@"<script type='text/javascript' id='glimpseData' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>", dataPath, clientPath);
                
                // Add glimpse output script
                string bodyCloseWithScript = bodyEnd.Replace(contentInBuffer,html);

                // Write content to the outputStream
                byte[] outputBuffer = encoding.GetBytes(bodyCloseWithScript);

                OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
            }
            else
            {
                OutputStream.Write(buffer, offset, count);
            }
        }
        
        public override void Flush()
        {
            OutputStream.Flush();
        }

        public override bool CanRead
        {
            get { return OutputStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return OutputStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return OutputStream.CanWrite; }
        }

        public override long Length
        {
            get { return OutputStream.Length; }
        }

        public override long Position
        {
            get { return OutputStream.Position; }
            set { OutputStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return OutputStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return OutputStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            OutputStream.SetLength(value);
        }

        public override void Close()
        {
            OutputStream.Close();
        }
    }
}