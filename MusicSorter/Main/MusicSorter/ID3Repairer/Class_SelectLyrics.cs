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


namespace MusicSorter
{
    public partial class MainWindow : Window
    {
        Windows_SelectLyric Window_SelectLyric = new Windows_SelectLyric();
        private void Button_Lyrics_GetOnline_Click(object sender, RoutedEventArgs e)
        {
            
            Window_SelectLyric.LyricsList.Clear();
            Window_SelectLyric.ReturnString = "";
            int SelectedIndex = DataListBox.SelectedIndex;
            if (SelectedIndex < 0 || SelectedIndex > SongList.Count - 1) return;
            Window_SelectLyric.SelectLyrics_TextBox_Artist.Text = SongList[SelectedIndex].SongArtist;
            Window_SelectLyric.SelectLyrics_TextBox_Song.Text = SongList[SelectedIndex].SongTitle;
            Window_SelectLyric.ShowDialog();
            if (Window_SelectLyric.ReturnString == "") return;
            Textbox_EditLyrics.Text = Window_SelectLyric.ReturnString;
        }

    }
}
