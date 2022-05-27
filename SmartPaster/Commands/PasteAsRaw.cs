using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteAsRaw)]
    internal sealed class PasteAsRaw : BaseCommand<PasteAsRaw>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            string fileName = docView.TextBuffer?.GetFileName();

            string text = SmartFormatter.RawInCs(Helpers.ClipboardText);

            var position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}