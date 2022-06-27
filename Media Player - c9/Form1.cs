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

namespace Media_Player___c9
{
    public partial class Form1 : Form
    {
        List<string> filteredFiles = new List<string>();
        FolderBrowserDialog browser = new FolderBrowserDialog();
        int currentFile = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadFolderEvent(object sender, EventArgs e)
        {
            VideoPlayer.Ctlcontrols.stop();

            if (filteredFiles.Count > 1)
            {
                filteredFiles.Clear();
                filteredFiles = null;
                Playlist.Items.Clear();
                currentFile = 0;
            }

            DialogResult result = browser.ShowDialog();

            if (result == DialogResult.OK)
            {
                filteredFiles = Directory.GetFiles(browser.SelectedPath, "*.*").Where(file => file.ToLower().EndsWith("webm") 
                || file.ToLower().EndsWith("mp4") || file.ToLower().EndsWith("wmv") || file.ToLower().EndsWith("mkv") ||
                file.ToLower().EndsWith("avi")).ToList();

                LoadPlaylist();
            }
        }

        private void LoadAboutEvent(object sender, EventArgs e)
        {
            MessageBox.Show("This App id made by andikscript" + Environment.NewLine
                + "Hope you enjoy the simple this media player" + Environment.NewLine
                + "Click on Open Folder Button to load the video folder and start playing" + Environment.NewLine
                + "Enjoy it");
        }

        private void MediaPlayerStateChangeEvent(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 0)
            {
                labelDuration.Text = "Media Player is ready to be loaded";
            }

            if (e.newState == 1)
            {
                labelDuration.Text = "Media Player is stopped";
            }

            if (e.newState == 3)
            {
                labelDuration.Text = "Duration : " + VideoPlayer.currentMedia.durationString;
            }

            if (e.newState == 8)
            {
                if (currentFile >= filteredFiles.Count - 1)
                {
                    currentFile = 0;
                } else
                {
                    currentFile += 1;
                }

                Playlist.SelectedIndex = currentFile;
                ShowFileName(fileName);
            }

            if (e.newState == 9)
            {
                labelDuration.Text = "Label new video";
            }

            if (e.newState == 10)
            {
                timer.Start();
            }
        }

        private void PlaylistChange(object sender, EventArgs e)
        {
            currentFile = Playlist.SelectedIndex;
            PlayFile(Playlist.SelectedItem.ToString());
            ShowFileName(fileName);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            VideoPlayer.Ctlcontrols.play();
            timer.Stop();
        }

        private void LoadPlaylist()
        {
            VideoPlayer.currentPlaylist = VideoPlayer.newPlaylist("Playlist", "");

            foreach(string videos in filteredFiles)
            {
                VideoPlayer.currentPlaylist.appendItem(VideoPlayer.newMedia(videos));
                Playlist.Items.Add(videos);
            }

            if (filteredFiles.Count > 0)
            {
                fileName.Text = "Files Found " + filteredFiles.Count;
                Playlist.SelectedIndex = currentFile;
                PlayFile(Playlist.SelectedItems.ToString());
            } else
            {
                MessageBox.Show("No Video Found this Folder");
            }
        }

        private void PlayFile(string url)
        {
            VideoPlayer.URL = url;
        }

        private void ShowFileName(Label label)
        {
            string file = Path.GetFileName(Playlist.SelectedItem.ToString());
            fileName.Text = "Current Playing " + file;
        }
    }
}
