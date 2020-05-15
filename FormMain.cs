using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.InteropServices;

namespace ZiZhuJi
{
    public partial class FormMain : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public FormMain()
        {
            InitializeComponent();
            InitializeChromium();

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            chromeBrowser.JavascriptObjectRepository.Register("cefCustomObject", new CefCustomObject(chromeBrowser, this), isAsync: true, options: BindingOptions.DefaultBinder);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            string page = string.Format(@"{0}\www\index.html", Application.StartupPath);
            if (!File.Exists(page))
            {
                MessageBox.Show("Error The html file doesn't exists : " + page);
            }
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser(page);
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            chromeBrowser.BrowserSettings = browserSettings;

        }

        public void FormMain_Closing(object sender, FormClosingEventArgs e)
        {
            // TODO
            Cef.Shutdown();
        }

        public void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                CefCustomObject cefCustomObject = new CefCustomObject(chromeBrowser, this);
                cefCustomObject.showDevTools();

            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F12)
            {
                CefCustomObject cefCustomObject = new CefCustomObject(chromeBrowser, this);
                cefCustomObject.showDevTools();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
