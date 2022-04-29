
namespace NewTCPServer
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ATCPServerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ATCPServerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ATCPServerProcessInstaller
            // 
            this.ATCPServerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ATCPServerProcessInstaller.Password = null;
            this.ATCPServerProcessInstaller.Username = null;
            // 
            // ATCPServerInstaller
            // 
            this.ATCPServerInstaller.ServiceName = "ATCPService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ATCPServerProcessInstaller,
            this.ATCPServerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ATCPServerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ATCPServerInstaller;
    }
}