using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class MyBitmap
    {

        public static byte[] convertImageToByteArray(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat fmt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                image.Save(ms, fmt);
                // or whatever output format you like
                return ms.ToArray();
            }
        }

        static public string  WriteXML(Image image,  System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);

            string left = "<html>" +
"<body>" +
"<h1>Base64 String to Image </h1>" +
"<img alt=\"my img\" src=\"data:image/png;base64,";
            //
            string right = "\"/&gt;" +
"</body>" +
"</html>";
            string sss = string.Format("{0}{1}{2}", left, base64String, right);
            return sss;
            }
         
        }





        static public string ImageToBase64(Image image,  System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        static public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

    }
}