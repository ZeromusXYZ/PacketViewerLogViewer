using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SevenZip;
using System.Reflection;

namespace PacketViewerLogViewer
{
    public partial class CompressForm : Form
    {
        public Dictionary<string, string> FilesToAdd = new Dictionary<string, string>();
        public string ArchiveFileName = "logs.7z";
        SevenZipCompressor sevenZipCompressor;

        public int BuildArchieveFilesList(string ProjectFolder)
        {
            if (!Directory.Exists(ProjectFolder))
                return 0;
            int res = 0;

            var startdir = new DirectoryInfo(ProjectFolder);
            var allfiles = startdir.GetFiles("*", SearchOption.AllDirectories);

            foreach(var fn in allfiles)
            {
                bool skipThis = false;
                var checkExt = Path.GetExtension(fn.FullName).ToLower();

                // skip project files
                // if (checkExt == ".pvlv") skipThis = true;
                // skip movies
                if (checkExt == ".avi") skipThis = true;
                if (checkExt == ".mp4") skipThis = true;
                if (checkExt == ".mpg") skipThis = true;
                if (checkExt == ".mpeg") skipThis = true;
                if (checkExt == ".ts") skipThis = true;
                if (checkExt == ".mkv") skipThis = true;
                if (checkExt == ".mov") skipThis = true;
                // skip self
                if (fn.FullName.ToLower() == ArchiveFileName.ToLower()) skipThis = true;

                if (!skipThis)
                {
                    var addFile = fn.FullName.Substring(ProjectFolder.Length);
                    FilesToAdd.Add(addFile, fn.FullName);
                    res++;
                }
            }

            return res;
        }

        public CompressForm()
        {
            InitializeComponent();
        }

        private void CompressForm_Shown(object sender, EventArgs e)
        {
            // Start here
            // Toggle between the x86 and x64 bit dll
            var dllpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
            try
            {
                if (File.Exists(dllpath))
                    SevenZipCompressor.SetLibraryPath(dllpath);
                else
                {
                    MessageBox.Show("7zip library not found:\r\n" + dllpath, "Load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                    return;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Couldn't load 7zip library Exception:\r\n" + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                return;
            }

            lZipFile.Text = ArchiveFileName;
            try
            {
                sevenZipCompressor = new SevenZipCompressor();
                sevenZipCompressor.CompressionLevel = SevenZip.CompressionLevel.Ultra;
                sevenZipCompressor.CompressionMethod = CompressionMethod.Lzma;
                sevenZipCompressor.CompressionMode = CompressionMode.Create;
                sevenZipCompressor.TempFolderPath = Path.GetTempPath();
                sevenZipCompressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                pb.Minimum = 0;
                pb.Maximum = FilesToAdd.Count;
                pb.Step = 1;
                bgwZipper.RunWorkerAsync();
            }
            catch (Exception x)
            {
                MessageBox.Show("Exception:\r\n" + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
            }
        }

        private void CompressForm_Load(object sender, EventArgs e)
        {
            // Init
        }

        private void BgwZipper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            if (bgwZipper.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            this.Invoke(new MethodInvoker(delegate
            {
                lInfo.Text = e.FileName;
            }));
        }

        private void FileCompressionFinished(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                pb.PerformStep();
            }));
            // System.Threading.Thread.Sleep(200);
        }

        private void BgwZipper_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                File.Delete(ArchiveFileName);
            }
            catch { }
            sevenZipCompressor.FileCompressionStarted += new EventHandler<FileNameEventArgs>(FileCompressionStarted);
            sevenZipCompressor.FileCompressionFinished += new EventHandler<EventArgs>(FileCompressionFinished);
            sevenZipCompressor.CompressFileDictionary(FilesToAdd, ArchiveFileName);
        }

        private void CompressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bgwZipper.CancelAsync();
        }
    }
}
