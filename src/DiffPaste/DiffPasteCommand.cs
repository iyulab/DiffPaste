using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;
using TextDiff; // Ensure TextDiff.Sharp is referenced

namespace DiffPaste
{
    internal sealed class DiffPasteCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("a25c8e6c-1426-4fde-b566-855785737a15");
        private readonly AsyncPackage package;

        public static DiffPasteCommand Instance { get; private set; }

        private DiffPasteCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in DiffPasteCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new DiffPasteCommand(package, commandService);
        }

        private async Task<IWpfTextView> GetActiveTextViewAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var textManager = await ServiceProvider.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;
            textManager.GetActiveView(1, null, out IVsTextView vsTextView);

            if (vsTextView == null) return null;

            var userData = vsTextView as IVsUserData;
            if (userData == null) return null;

            Guid guidWpfViewHost = Microsoft.VisualStudio.Editor.DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidWpfViewHost, out object host);
            var viewHost = host as IWpfTextViewHost;

            return viewHost?.TextView;
        }

        private void Execute(object sender, EventArgs e)
        {
            _ = ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var textView = await GetActiveTextViewAsync();
                if (textView == null)
                {
                    await UpdateStatusBarAsync("No active text view found.");
                    return;
                }

                var clipboardText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    await UpdateStatusBarAsync("No valid text in clipboard.");
                    return;
                }

                var snapshot = textView.TextSnapshot;
                var documentText = snapshot.GetText();

                var processor = new DiffProcessor();
                var result = processor.Process(documentText, clipboardText);

                using (var edit = textView.TextBuffer.CreateEdit())
                {
                    // Replace the entire document with the updated text
                    edit.Replace(new Span(0, snapshot.Length), result.Text);
                    edit.Apply();
                }

                // Construct status message based on changes
                var statusMessages = new List<string>();
                if (result.Changes.DeletedLines > 0) statusMessages.Add($"{result.Changes.DeletedLines} deleted");
                if (result.Changes.ChangedLines > 0) statusMessages.Add($"{result.Changes.ChangedLines} changed");
                if (result.Changes.AddedLines > 0) statusMessages.Add($"{result.Changes.AddedLines} added");

                string statusMessage = statusMessages.Count > 0
                    ? $"Diff applied: {string.Join(", ", statusMessages)}"
                    : "No changes applied.";

                await UpdateStatusBarAsync(statusMessage);
            }
            catch (Exception ex)
            {
                await UpdateStatusBarAsync($"Error: {ex.Message}");
            }
        }

        private async Task UpdateStatusBarAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var statusBar = await ServiceProvider.GetServiceAsync(typeof(SVsStatusbar)) as IVsStatusbar;
            if (statusBar != null)
            {
                statusBar.SetText(message);
            }
        }
    }
}
