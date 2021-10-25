using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit;
using Task = System.Threading.Tasks.Task;

namespace SmartPaster
{
    /// <summary>
    /// Command handler
    /// </summary>
    [Command(PackageIds.cmdidPasteWithReplace)]
    internal sealed class PasteWithReplace : BaseCommand<PasteWithReplace>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not text window

            using (var replaceForm = new ReplaceForm())
            {
                if (replaceForm.ShowDialog() == DialogResult.OK)
                {
                    var src = replaceForm.TextToReplace;
                    var dst = replaceForm.ReplaceText;
                    var txt = Helpers.ClipboardText.Replace(src, dst);
                    var position = docView.TextView.Caret.Position.BufferPosition;
                    docView.TextBuffer?.Insert(position, txt);
                }
            }
        }
    }
}