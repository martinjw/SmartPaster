using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteAsComment)]
    internal sealed class PasteAsComment : BaseCommand<PasteAsComment>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            string text = Helpers.ClipboardText;
            if (Helpers.IsVb(fileName))
                text = SmartFormatter.CommentizeInVb(text);
            else if (Helpers.IsCs(fileName))
                text = SmartFormatter.CommentizeInCs(text);
            else if (Helpers.IsXml(fileName))
                text = SmartFormatter.CommentizeInXml(text);

            var position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}