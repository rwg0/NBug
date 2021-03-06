﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Http.cs" company="NBug Project">
//   Copyright (c) 2011 - 2013 Teoman Soygul. Licensed under MIT license.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;

namespace NBug.Core.Submission.Web
{
	using System.IO;

	using NBug.Core.Reporting.Info;
	using NBug.Core.Util.Logging;
	using NBug.Core.Util.Serialization;
	using NBug.Core.Util.Web;

	public class HttpFactory : IProtocolFactory
	{
		public string SupportedType
		{
			get
			{
				return "Http";
			}
		}

		public IProtocol FromConnectionString(string connectionString)
		{
			return new Http(connectionString);
		}
	}

	public class Http : ProtocolBase
	{
		public Http(string connectionString)
			: base(connectionString)
		{
		}

		public Http()
		{
		}

		// Connection string format (single line)
		// Warning: There should be no semicolon (;) or equals sign (=) used in any field.
		// Note: Url should be a full url with a trailing slash (/) or file extension (i.e. .php), like: http://....../ -or- http://....../upload.php

		/* Type=Http;
		 * Url=http://tracker.mydomain.com/myproject/upload.php;
		 */
		public string Url { get; set; }

		public override bool Send(string fileName, Stream file, Report report, SerializableException exception)
		{
			// Advanced method with ability to post variables along with file (do not forget to urlencode the query parameters)
			// http://www.codeproject.com/KB/cs/uploadfileex.aspx
			// http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
			// http://stackoverflow.com/questions/767790/how-do-i-upload-an-image-file-using-a-post-request-in-c
			// http://netomatix.com/HttpPostData.aspx

			/* upload.php file my look like the one below (note that uploaded files are not statically named in this case script may need modification)
			 *
			 * <?php
			 * $uploadDir = 'Upload/';
			 * $uploadFile = $uploadDir . basename($_FILES['file']['name']);
			 * if (is_uploaded_file($_FILES['file']['tmp_name']))
			 * {
			 *     echo "File ". $_FILES['file']['name'] ." is successfully uploaded!\r\n";
			 *     if (move_uploaded_file($_FILES['file']['tmp_name'], $uploadFile))
			 *     {
			 *         echo "File is successfully stored! ";
			 *     }
			 *     else print_r($_FILES);
			 * }
			 * else
			 * {
			 *     echo "Upload Failed!";
			 *     print_r($_FILES);
			 * }
			 * ?>
			 */

			using (var webClient = new WebClient())
			{
			    var fs = (FileStream) file ;
			    var name = fs.Name;
                file.Close();

			    var url = this.Url;

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                request.AllowAutoRedirect = false;

                using (var wr = request.GetResponse() as HttpWebResponse)
                {
                    if (wr.StatusCode == HttpStatusCode.Redirect || wr.StatusCode == HttpStatusCode.TemporaryRedirect || wr.StatusCode == HttpStatusCode.RedirectKeepVerb || wr.StatusCode == HttpStatusCode.Moved
                        || wr.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        url = wr.GetResponseHeader("Location");
                    }
                }

				var response = webClient.UploadFile(url, fs.Name);
				Logger.Info("Response from HTTP server: " + System.Text.Encoding.ASCII.GetString(response));
			}

			return true;
		}
	}
}