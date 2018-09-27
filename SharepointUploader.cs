using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointLibraryFileUploader
{
    public class SharepointUploader
    {
        public bool Upload(string filePath, string siteUrl, string listTitle)
        {
            try
            {
                using (var clientContext = new ClientContext(siteUrl))
                {
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        var fi = new FileInfo(filePath);
                        var list = clientContext.Web.Lists.GetByTitle(listTitle);
                        clientContext.Load(list.RootFolder);
                        clientContext.ExecuteQuery();
                        var fileUrl = String.Format("{0}/{1}", list.RootFolder.ServerRelativeUrl, fi.Name);
                        Microsoft.SharePoint.Client.File.SaveBinaryDirect(clientContext, fileUrl, fs, true);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
