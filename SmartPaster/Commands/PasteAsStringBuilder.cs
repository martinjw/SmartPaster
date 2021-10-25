using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteAsStringBuilder)]
    internal sealed class PasteAsStringBuilder : BaseCommand<PasteAsStringBuilder>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            string text = Helpers.ClipboardText;
            const string stringbuilder = "sb";
            if (Helpers.IsVb(fileName))
                text = SmartFormatter.StringbuilderizeInVb(text, stringbuilder);
            else if (Helpers.IsCs(fileName))
                text = SmartFormatter.StringbuilderizeInCs(text, stringbuilder);

            var position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}