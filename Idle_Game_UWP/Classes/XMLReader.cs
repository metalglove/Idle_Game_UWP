using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Idle_Game_UWP.Classes
{
    class XMLReader
    {
        private XmlDocument XmlDocument = new XmlDocument();
        public async Task<XmlDocument> ReadXMLAsync(Uri Url)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(Url);
            using (Stream Stream = await file.OpenStreamForReadAsync())
            {
                XmlDocument.Load(Stream);
            }            
            return XmlDocument;
        }
    }
}
