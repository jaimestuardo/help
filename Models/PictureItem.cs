using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
    public class PictureItem
    {
        public string Extension { get; set; }
        public Stream Stream { get; set; }

        public async Task<byte[]> GetBytesAsync()
        {
            if (Stream == null)
            {
                return null;
            }

            using MemoryStream ms = new();
            await Stream.CopyToAsync(ms);
            Stream.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}
