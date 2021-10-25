using System;
using System.Windows.Forms;

namespace SmartPaster
{
    public static class Helpers
    {
        /// <summary>
        ///  Convient property to retrieve the clipboard text from the clipboard
        /// </summary>
        public static string ClipboardText
        {
            get
            {
                var iData = Clipboard.GetDataObject();
                if (iData == null) return string.Empty;
                //is it Unicode? Then we use that
                if (iData.GetDataPresent(DataFormats.UnicodeText))
                    return Convert.ToString(iData.GetData(DataFormats.UnicodeText));
                //otherwise ANSI
                if (iData.GetDataPresent(DataFormats.Text))
                    return Convert.ToString(iData.GetData(DataFormats.Text));
                return string.Empty;
            }
        }

        public static bool IsXml(string fileName)
        {
            foreach (var ext in new[] { ".xml", ".xsd", ".config", ".xaml" })
            {
                if (fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool IsVb(string fileName)
        {
            return fileName.EndsWith(".vb", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCs(string fileName)
        {
            return fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCxx(string fileName)
        {
            foreach (var ext in new[] { ".cpp", ".cp", ".cc", ".cxx" })
            {
                if (fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}