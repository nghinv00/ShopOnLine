using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.UI.Browser
{
  public static  class PdfView
    {
      public static string View(string filePdf, string width, string height)
      {
          string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"{1}\" height=\"{2}\">";
          embed += "Nếu bạn không thể xem tập tin, bạn có thể tải về từ <a href = \"{0}\">Tải về</a>";
          embed += " hoặc cài ứng dụng <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> để xem tập tin.";
          embed += "</object>";
          return string.Format(embed, filePdf, width, height); ;
      }
    }
}
