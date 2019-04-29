using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BorislavIvanov.ClearTrackedChanges
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidClearTrackedChangesPkgString)]
    public sealed class ClearTrackedChangesPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ClearTrackedChangesPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            if (GetService(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidClearTrackedChangesCmdSet, (int)PkgCmdIDList.cmdidClearTrackedChanges);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager3 textManager = this.GetService(typeof(VsTextManagerClass)) as IVsTextManager3;

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