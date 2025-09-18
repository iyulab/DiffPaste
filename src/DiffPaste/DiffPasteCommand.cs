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

            if (!(vsTextView is IVsUserData userData)) return null;

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
                    await UpdateStatusBarAsync("활성 텍스트 에디터를 찾을 수 없습니다.");
                    return;
                }

                var clipboardText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    await UpdateStatusBarAsync("클립보드가 비어있습니다.");
                    return;
                }

                var snapshot = textView.TextSnapshot;
                var documentText = snapshot.GetText();

                var differ = new TextDiffer();
                var result = differ.Process(documentText, clipboardText);

                using (var edit = textView.TextBuffer.CreateEdit())
                {
                    // Replace the entire document with the updated text
                    edit.Replace(new Span(0, snapshot.Length), result.Text);
                    edit.Apply();
                }

                // Construct status message based on changes
                var statusMessages = new List<string>();
                if (result.Changes.DeletedLines > 0) statusMessages.Add($"{result.Changes.DeletedLines}줄 삭제");
                if (result.Changes.ChangedLines > 0) statusMessages.Add($"{result.Changes.ChangedLines}줄 변경");
                if (result.Changes.AddedLines > 0) statusMessages.Add($"{result.Changes.AddedLines}줄 추가");

                string statusMessage = statusMessages.Count > 0
                    ? $"Diff 적용 완료: {string.Join(", ", statusMessages)}"
                    : "변경사항이 없습니다.";

                await UpdateStatusBarAsync(statusMessage);
            }
            catch (Exception ex)
            {
                await UpdateStatusBarAsync($"오류: {ex.Message}");
            }
        }

        private async Task UpdateStatusBarAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (await ServiceProvider.GetServiceAsync(typeof(SVsStatusbar)) is IVsStatusbar statusBar)
            {
                statusBar.SetText(message);
            }
        }
    }
}
