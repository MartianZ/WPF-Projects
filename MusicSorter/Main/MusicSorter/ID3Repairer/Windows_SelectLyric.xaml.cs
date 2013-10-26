using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Net;
using System.Windows.Interop;
using QianQianLrc;
using ID3.ID3v2Frames;
using ID3;
using ID3.ID3v1Frame;
using System.Xml;


namespace MusicSorter
{
    /// <summary>
    /// Windows_SelectLyric.xaml 的交互逻辑
    /// </summary>
    public partial class Windows_SelectLyric : Window
    {
        public ObservableCollection<Lyric> LyricsList = new ObservableCollection<Lyric>();
        public string ReturnString = "";
        public Windows_SelectLyric()
        {
            InitializeComponent();
        }

        private void SelectLyrics_Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xml = SearchHelper.SearchLrc(SelectLyrics_TextBox_Artist.Text, SelectLyrics_TextBox_Song.Text);
            LyricsList.Clear();
            if (xml == null) return;


            XmlNodeList list = xml.SelectNodes("/result/lrc");
            if (list.Count > 0)
            {
                int sLrcId = -1;
                string xSinger = null, xTitle = null;
                foreach (XmlNode node in list)
                {
                    if (node != null && node.Attributes != null && node.Attributes["id"] != null)
                    {
                        int lrcId = -1;
                        if (int.TryParse(node.Attributes["id"].Value, out lrcId))
                        {
                            sLrcId = lrcId;
                            xSinger = node.Attributes["artist"].Value;
                            xTitle = node.Attributes["title"].Value;
                            LyricsList.Add(new Lyric(sLrcId.ToString(), xTitle, xSinger));
                        }
                    }
                }
            }
            LyricListview.ItemsSource = null;
            LyricListview.ItemsSource = LyricsList;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void LyricListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Lyric selecteditem = LyricListview.SelectedItem as Lyric;
            if (selecteditem == null) return;
            int lrcId = -1;
            int.TryParse(selecteditem.LyricID, out lrcId);
            string temp = QianQianLrc.SearchHelper.DownloadLRCLite(lrcId, selecteditem.LyricArtist, selecteditem.LyricTitle);

            Regex rr = new Regex(@"\[.*?\]", RegexOptions.IgnoreCase);
            temp = rr.Replace(temp, "").Trim();


            ReturnString = temp;
            this.Hide();

        }
    }

    public class Lyric : INotifyPropertyChanged
    {
        public string LyricID { get; set; }
        public string LyricTitle { get; set; }
        public string LyricArtist { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Lyric(string id, string title, string artist)
        {
            this.LyricID = id;
            this.LyricTitle = title;
            this.LyricArtist = artist;
        }
    }

}
