using Pasteur.Core.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Pasteur.Laboratory.P2
{
    /// <summary>
    /// Summary description for UploadImages
    /// </summary>
    public class UploadImages : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            var httpPostedFile2 = context.Request.Files["UploadedExcelFile"];
            var uploadType = context.Request.Form["UploadType"];
            {

                var httpPostedFile = context.Request.Files;
                if (httpPostedFile.Count > 0)
                {
                    for (int i = 0; i < httpPostedFile.Count; i++)
                    {
                        try
                        {
                            int u = 1;
                            var file = context.Request.Files["file_" + i];
                            string ext = System.IO.Path.GetExtension(file.FileName);
                            string name = string.Format("{0}", UrlRewriteHelper.ConvertToSEOFriendly(file.FileName.Replace("." + ext, ""), 100, true)) + "." + ext;
                            string path = string.Format(@"\Uploads\Images\ChungChi\{0}", name);



                            string serverPath = HttpContext.Current.Server.MapPath(path);
                            while (File.Exists(serverPath))
                            {
                                name = string.Format("{0}", UrlRewriteHelper.ConvertToSEOFriendly(file.FileName.Replace("." + ext, ""), 100, true)) + "(" + u + ")" + "." + ext;
                                path = string.Format(@"\Uploads\Images\ChungChi\{0}", name);
                                serverPath = HttpContext.Current.Server.MapPath(path);
                                u++;
                            }

                            file.SaveAs(serverPath);
                            context.Response.Write(path);
                            HttpContext.Current.Session["ChungChiImage"] = path;
                        }
                        catch (Exception ex) { }

                    }

                    
                }
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}