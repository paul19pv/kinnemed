using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public class barcode
    {
        
        public Byte[] getCodigoBarrasImagem(string barcode, string file)
        {
            try
            {
                BarCode39 _barcode = new BarCode39();
                int barSize = 16;
                string fontFile = HttpContext.Current.Server.MapPath("~/fonts/FREE3OF9.ttf");
                return (_barcode.Code39(barcode, barSize, true, file, fontFile));
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
                val_error = ex.Message;
            }
            return null;
        }

        public Byte[] GenerarCodigo(string code)
        {
            var resultado = new byte[] { };
            int width = 600;
            int height = 72;
            int size = 72;
            if (!string.IsNullOrEmpty(code))
            {
                using (var stream = new MemoryStream())
                {
                    var bitmap = new Bitmap(width, height);
                    bitmap.SetResolution(144.0F, 144.0F);
                    var grafic = Graphics.FromImage(bitmap);
                    var fuente = CargarFuente(size);
                    var point = new Point();
                    var brush = new SolidBrush(Color.Black);
                    grafic.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
                    grafic.DrawString(FormatBarCode(code.ToUpper()), fuente, brush, point);
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    stream.Seek(0, SeekOrigin.Begin);
                    resultado = stream.ToArray();
                }
            }
            return resultado;
        }


        private string FormatBarCode(string code)
        {
            return string.Format("*{0}*", code);
            //return code;
        }

        private Font CargarFuente(int size)
        {
            var pfc = new PrivateFontCollection();
            string path = HttpContext.Current.Server.MapPath("~/fonts/c39hrp24dhtt.ttf");
            pfc.AddFontFile(path);
            return new Font(pfc.Families[0], (float)size);
        }
        public string val_error{get; set;}
    }
}