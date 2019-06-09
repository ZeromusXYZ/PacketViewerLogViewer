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
        public string ProjectName = string.Empty;
        private int thisFileSize = 0;
        SevenZipCompressor zipper;
        SevenZipExtractor unzipper;
        public enum ZipTaskType { doZip, doUnZip };
        public ZipTaskType task = ZipTaskType.doZip;

        public int BuildArchieveFilesList(string ProjectFolder)
        {
            if (!Directory.Exists(ProjectFolder))
                return 0;
            int res = 0;

            var startdir = new DirectoryInfo(ProjectFolder);
            var allfiles = startdir.GetFiles("*", SearchOption.AllDirectories);
            string ProjectFolderName = Path.GetFileName(ProjectFolder.TrimEnd(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar ;

            pb.Minimum = 0;
            pb.Maximum = 1;
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
                    var addFile = ProjectFolderName + fn.FullName.Substring(ProjectFolder.Length);
                    FilesToAdd.Add(addFile, fn.FullName);
                    res++;
                    pb.Maximum += (int)fn.Length;
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
            switch (task)
            {
                case ZipTaskType.doZip:
                    Text = "Add to archive";
                    break;
                case ZipTaskType.doUnZip:
                    Text = "Extract from archive";
                    break;
                default:
                    Text = "Unknown command";
                    DialogResult = DialogResult.Abort;
                    return;
            }
            // Start here
            // Toggle between the x86 and x64 bit dll
            var dllpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
            try
            {
                if (File.Exists(dllpath))
                {
                    SevenZipBase.SetLibraryPath(dllpath);
                }
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

            switch (task)
            {
                case ZipTaskType.doZip:
                    if (File.Exists(ArchiveFileName))
                    {
                        if (MessageBox.Show("Target 7z file already exists, do you want to overwrite it ?\r\n" + ArchiveFileName, "Overwrite File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            DialogResult = DialogResult.Cancel;
                            return;
                        }
                    }

                    try
                    {
                        zipper = new SevenZipCompressor();
                        zipper.CompressionLevel = SevenZip.CompressionLevel.Ultra;
                        zipper.CompressionMethod = CompressionMethod.Lzma;
                        zipper.CompressionMode = CompressionMode.Create;
                        zipper.TempFolderPath = Path.GetTempPath();
                        zipper.ArchiveFormat = OutArchiveFormat.SevenZip;
                        bgwZipper.RunWorkerAsync();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Exception:\r\n" + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Abort;
                    }
                    break;
                case ZipTaskType.doUnZip:
                    if (!File.Exists(ArchiveFileName))
                    {
                        MessageBox.Show("Source 7z file not found ?\r\n" + ArchiveFileName, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult = DialogResult.Cancel;
                        return;
                    }

                    try
                    {
                        unzipper = new SevenZipExtractor(ArchiveFileName);
                        if (unzipper.FilesCount <= 0)
                        {
                            MessageBox.Show("Nothing to extract", "No files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            DialogResult = DialogResult.Cancel;
                        }
                        else
                        {
                            pb.Minimum = 0;
                            pb.Maximum = (int)unzipper.FilesCount+1 ;
                            pb.Step = 1;
                            pb.Value = 0;
                            // unzipper.FileExtractionStarted += new EventHandler<FileInfoEventArgs>(FileExtractStarted);
                            unzipper.FileExtractionFinished += new EventHandler<FileInfoEventArgs>(FileExtractFinished);
                            unzipper.FileExists += new EventHandler<FileOverwriteEventArgs>(FileExtractFileExists);
                            bgwUnZipper.RunWorkerAsync();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Exception:\r\n" + x.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Abort;
                    }
                    break;
                default:
                    DialogResult = DialogResult.Abort;
                    return;
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
            if (FilesToAdd.TryGetValue(e.FileName,out string sourceFile))
            {
                var fi = new FileInfo(sourceFile);
                thisFileSize = (int)fi.Length;
            }
            else
            {
                thisFileSize = 0;
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
                pb.Value += thisFileSize;
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
            zipper.FileCompressionStarted += new EventHandler<FileNameEventArgs>(FileCompressionStarted);
            zipper.FileCompressionFinished += new EventHandler<EventArgs>(FileCompressionFinished);
            zipper.CompressFileDictionary(FilesToAdd, ArchiveFileName);
        }

        private void CompressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgwZipper.IsBusy)
                bgwZipper.CancelAsync();
            if (bgwUnZipper.IsBusy)
                bgwUnZipper.CancelAsync();
        }

        private void FileExtractStarted(object sender, FileInfoEventArgs e)
        {
            if (bgwUnZipper.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (e.PercentDone == 0)
            {
                //this.Invoke(new MethodInvoker(delegate
                //{
                lInfo.Text = e.FileInfo.FileName;
                lInfo.Refresh();
                //}));
            }
        }

        private void FileExtractFinished(object sender, FileInfoEventArgs e)
        {
            if (bgwUnZipper.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (e.PercentDone == 100)
            {
                //this.Invoke(new MethodInvoker(delegate
                //{
                pb.Value += (int)e.FileInfo.Size ;
                //}));
                //System.Threading.Thread.Sleep(200);
            }

        }

        private void FileExtractFileExists(object sender, FileOverwriteEventArgs e)
        {
            if (bgwUnZipper.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            e.Cancel = false;
        }

        private void DoUnZip()
        {
            string tDir = Path.GetDirectoryName(ArchiveFileName);
            string expectedRootDir = ProjectName ;
            expectedRootDir = expectedRootDir.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            tDir = tDir.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            var filelist = unzipper.ArchiveFileNames.ToArray();
            ulong totsize = 0;
            foreach (var fd in unzipper.ArchiveFileData)
            {
                totsize += fd.Size;
            }

            this.Invoke(new MethodInvoker(delegate
            {
                pb.Maximum = (int)totsize + 1;
                pb.Value = 0;
            }));
            foreach (var fn in filelist)
            {
                try
                {

                    var zippedName = string.Empty;
                    if (fn.StartsWith(expectedRootDir))
                    {
                        zippedName = fn.Substring(expectedRootDir.Length);
                    }
                    else
                    {
                        zippedName = fn;
                    }
                    var targetName = tDir + zippedName;
                    var targetFileDir = Path.GetDirectoryName(targetName);
                    if (!Directory.Exists(targetFileDir))
                        Directory.CreateDirectory(targetFileDir);
                    var fs = File.Create(targetName);
                    //this.Invoke(new MethodInvoker(delegate
                    //{
                        lInfo.Text = targetName;
                        lInfo.Refresh();
                        unzipper.ExtractFile(fn, fs);
                    //}));
                    fs.Close();
                }
                catch (Exception x)
                {
                    if (MessageBox.Show("Exception extracting file:\r\n"+x+"\r\n"+fn+"\r\nDo you want to continue ?","Exception",MessageBoxButtons.YesNo,MessageBoxIcon.Error) == DialogResult.No)
                    {
                        break;
                    }
                }
            }
            // unzipper.ExtractFiles(tDir, filelist);
            // unzipper.ExtractArchive(tDir);
        }

        private void BgwUnZipper_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                DoUnZip();
            }));
        }

        private void BgwUnZipper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CompressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (zipper != null)
                zipper = null;
            if (unzipper != null)
                unzipper = null;
        }
    }
}
