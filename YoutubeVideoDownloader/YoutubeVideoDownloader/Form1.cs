using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeVideoDownloader
{
    public partial class Form1 : Form
    {
        private readonly YoutubeClient client;
        public Form1()
        {
            InitializeComponent();
            client = new YoutubeClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string videoUrl = textBox1.Text;

            var video = await client.Videos.GetAsync(videoUrl);
            label2.Text = "Downloading: "+video.Title;
            var streamInfoSet = await client.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamInfoSet.GetMuxedStreams().GetWithHighestVideoQuality();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), $"{video.Title}.{streamInfo.Container}");

            progressBar1.Value = 0;
            progressBar1.Visible = true;

            try
            {
                await client.Videos.Streams.DownloadAsync(streamInfo, filePath, new Progress<double>(value =>
                {
                    progressBar1.Value = (int)(value * 100);
                }));
                label2.Text = "Download complete";
                MessageBox.Show($"Video downloaded to: {filePath}", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the download: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by ?0\ncoded in C#", "About");
        }
    }
}
