using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteAsBytes)]
    internal sealed class PasteAsBytes : BaseCommand<PasteAsBytes>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            if (Helpers.IsCxx(fileName) || Helpers.IsCs(fileName))
            {
                var sb = new StringBuilder();
                var count = 0;
                foreach (var ch in Helpers.ClipboardText)
                {
                    sb.AppendFormat("0x{0:x2}, ", (int)ch);
                    if (++count == 16)
                    {
                        count = 0;
                        sb.AppendLine();
                    }
                }
                var position = docView.TextView.Caret.Position.BufferPosition;
                docView.TextBuffer?.Insert(position, sb.ToString());
            }
        }
    }
}