using System.IO;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.LinkLabel;
using System.Reflection;

namespace Laboratorio__2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView.NavigationStarting += EnsureHttps;
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            comboBox1.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            comboBox1.Width = goButton.Left - comboBox1.Left;
        }

        private void Guardar(string fileName, string texto)
        {
            FileStream flujo = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter escritor = new StreamWriter(flujo);
            escritor.WriteLine(texto);
            escritor.Close();
        }
        private void BotonIr_Click(object sender, EventArgs e)
        {
            string link = "";
            if (!(comboBox1.Text.Contains(".")))
            {
                link = "https://www.google.com/search?q=" + comboBox1.Text;
            }

            if(webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(link);
            }

            Guardar(@"C:\Users\Sir_d\Documents\Historial.txt", comboBox1.Text);
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void haciaDelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string line;
            int counter = 0;
            
            StreamReader lector = new StreamReader(@"C:\Users\Sir_d\Documents\Historial.txt");
            while ((line = lector.ReadLine()) != null)
            {
                comboBox1.Items.Add(line);
                counter++;
            }
            lector.Close();
        }

        private void webView_Click(object sender, EventArgs e)
        {

        }
    }
}
