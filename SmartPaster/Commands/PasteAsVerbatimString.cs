using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteAsVerbatimString)]
    internal sealed class PasteAsVerbatimString : BaseCommand<PasteAsVerbatimString>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            string text = Helpers.ClipboardText;
            //for VB we no longer do the CDATA trick.
            if (Helpers.IsVb(fileName))
                text = SmartFormatter.StringinizeInVb(text);
            else
                text = SmartFormatter.StringinizeInCs(text);

            var position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}