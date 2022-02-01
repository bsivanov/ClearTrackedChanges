using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Task = System.Threading.Tasks.Task;

namespace BorislavIvanov.ClearTrackedChanges
{
    /// <summary>
    /// A package providing the ClearTrackedChanges command.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.GuidClearTrackedChangesPkgString)]
    public sealed class ClearTrackedChangesPackage : AsyncPackage
    {
        /// <summary>Initializes package asynchronously.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, we *may* be on a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            // Otherwise, remove the switch to the UI thread if you don't need it.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            if (await this.GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.GuidClearTrackedChangesCmdSet, (int)PkgCmdIDList.CmdIdClearTrackedChanges);
                MenuCommand menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager3 textManager = this.GetService(typeof(VsTextManagerClass)) as IVsTextManager3;
            Assumes.Present(textManager);

            VIEWPREFERENCES3[] viewPreferences3Array = new VIEWPREFERENCES3[1];
            FONTCOLORPREFERENCES2[] fontColorPreferences2Array = new FONTCOLORPREFERENCES2[1];
            FRAMEPREFERENCES2[] framePreferences2Array = new FRAMEPREFERENCES2[1];
            LANGPREFERENCES2[] langPreferences2Array = new LANGPREFERENCES2[1];

            textManager.GetUserPreferences3(viewPreferences3Array, framePreferences2Array, langPreferences2Array, fontColorPreferences2Array);

            VIEWPREFERENCES3 viewPreferences3 = viewPreferences3Array[0];
            if (viewPreferences3.fTrackChanges == 1)
            {
                viewPreferences3.fTrackChanges = 0;
                textManager.SetUserPreferences3(new VIEWPREFERENCES3[] { viewPreferences3 }, framePreferences2Array, langPreferences2Array, fontColorPreferences2Array);

                viewPreferences3.fTrackChanges = 1;
                textManager.SetUserPreferences3(new VIEWPREFERENCES3[] { viewPreferences3 }, framePreferences2Array, langPreferences2Array, fontColorPreferences2Array);
            }
        }
    }
}