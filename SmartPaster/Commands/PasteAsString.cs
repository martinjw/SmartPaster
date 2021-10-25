using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    [Command(PackageIds.cmdidPasteAsString)]
    internal sealed class PasteAsString : BaseCommand<PasteAsString>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            string text = Helpers.ClipboardText;
            if (Helpers.IsVb(fileName))
                text = SmartFormatter.LiterallyInVb(text);
            else if (Helpers.IsCs(fileName))
                text = SmartFormatter.LiterallyInCs(text);
            else if (Helpers.IsCxx(fileName))
                text = SmartFormatter.LiterallyInCxx(text);

            var position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}
