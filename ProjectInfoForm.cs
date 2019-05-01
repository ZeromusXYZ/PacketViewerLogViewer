using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketViewerLogViewer.Packets;

namespace PacketViewerLogViewer
{
    public partial class ProjectInfoForm : Form
    {
        PacketTabPage tp ;

        public ProjectInfoForm()
        {
            InitializeComponent();
        }

        public void LoadFromPacketTapPage(PacketTabPage sourceTP)
        {
            tp = sourceTP;

            if (tp != null)
            {
                tProjectFolder.Text = tp.ProjectFolder;
                tOpenedLog.Text = tp.LoadedLogFile;
                tSourceVideo.Text = tp.LinkVideoFileName;
                tYoutubeURL.Text = tp.LinkYoutubeURL;
                tPackedLogsURL.Text = "";
            }
        }

        private void ProjectInfoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
