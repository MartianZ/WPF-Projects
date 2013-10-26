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

namespace MusicSeeker
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
        
        [DllImport("KernelModule.dll")]
        public static extern void WriteITunesLyrics(string FileName, string Content);
        [DllImport("KernelModule.dll")]
        public static extern void SetCover(string FileName, string Path);
        [DllImport("KernelModule.dll")]
        public static extern void FixID3Tags(string FileName, string Title, string Artist, string Album);
        [DllImport("KernelModule.dll")]
        public static extern void GetLyrics(string Title, string Artist,int Server, StringBuilder resulta);



		public MainWindow()
		{
			this.InitializeComponent();


            DataListBoxDoubleClick = this.Resources["DataListBoxDoubleClick"] as Storyboard;
            HideGroupBox = this.Resources["HideGroupBox"] as Storyboard;
		}

        public ObservableCollection<Song> SongList = new ObservableCollection<Song>();
        public ObservableCollection<DownLoad> DownLoadList = new ObservableCollection<DownLoad>();

        Storyboard DataListBoxDoubleClick;
        Storyboard HideGroupBox;

        #region 界面相关
        private void minimize_MouseEnter(object sender, MouseEventArgs e)
        {
            minimize.Source = new BitmapImage(new Uri(@"\Skin\minimize2.bmp", UriKind.Relative));
        }

        private void minimize_MouseLeave(object sender, MouseEventArgs e)
        {
            minimize.Source = new BitmapImage(new Uri(@"\Skin\minimize1.bmp", UriKind.Relative));
        }

        private void minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            minimize.Source = new BitmapImage(new Uri(@"\Skin\minimize3.bmp", UriKind.Relative));
        }

        private void minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void exit_MouseEnter(object sender, MouseEventArgs e)
        {
            exit.Source = new BitmapImage(new Uri(@"\Skin\exit2.bmp", UriKind.Relative));
        }

        private void exit_MouseLeave(object sender, MouseEventArgs e)
        {
            exit.Source = new BitmapImage(new Uri(@"\Skin\exit1.bmp", UriKind.Relative));
        }

        private void exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            exit.Source = new BitmapImage(new Uri(@"\Skin\exit3.bmp", UriKind.Relative));
        }

        private void exit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void SearchKeyword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.SearchKeyword.Text == "音乐名、唱片名、表演者") this.SearchKeyword.Text = "";
        }
        private void SearchKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SearchButton_Click(null, null);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataListBox.ItemsSource = SongList;
            
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            swich.Visibility = Visibility.Visible;
        }
        #endregion

        private static string Keywords; //搜索关键字
        private static string AlbumName; //专辑名称
        private static int SourceTAR; //目标
        private static string DynamicLyrics, StaticLyrics; //歌词

        private Thread SearchThread;
        private Thread SingleAlbumThread/*, MultiAlbumThread*/;
        private Thread LRCThread;
        private Thread UpdateThread;

        #region 搜索主线程
        private string ClearHTML(string s)
        {
            string ans = s;
            ans = ans.Replace("&amp;", "&");
            ans = ans.Replace("&apos;", "'");
            ans = ans.Replace("&quot;", "\"");
            ans = ans.Replace("&lt;", "<");
            ans = ans.Replace("&gt;", ">");

            return ans;
        }

        private void CheckUpdate()
        {
            string s = WebHelper.Analytics("http://api.4321.la/analytics.php?ver=20110304", Keywords);
            if (s == "Update")
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                {
                    if (!(SearchThread == null)) if (SearchThread.IsAlive) SearchThread.Abort();
                    MessageBox.Show("系统检测到软件已经发布新版本，请立即更新，单击确定退出软件并引导您到官网下载最新版本！\n\n官方网站：http://www.4321.la", "更新", MessageBoxButton.OK, MessageBoxImage.Warning);
                    System.Diagnostics.Process.Start("http://www.4321.la");
                    this.Close();

                });
            }
        }

        private void StartSearch()
        {
            if (string.IsNullOrEmpty(Keywords)) return;
            
            for (int p = 1; p < 20; p++) //获得前20页的内容，后面的内容搜索关键度吻合度过低，已经没有意义
            {
                string SourceCode = WebHelper.GetPageHTMLSource("http://soso.music.qq.com/fcgi-bin/music.fcg?w=" + WebHelper.Encode(Keywords) + "&p=" + p.ToString() + "&t=0&catZhida=1");
                if (SourceCode.IndexOf("很抱歉，没有找到与") != -1)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        MessageBox.Show("很抱歉，没有找到与“" + Keywords + "”相关搜索结果！\n\n由于搜索算法限制，暂不支持搜索关键字中含有除简体中文、英文之外的字符，请尽量优化关键词！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        LabelTitle.Content = " 搜索发现音乐 - 4321.La";
                    });
                    return;
                }

                SourceCode = SourceCode.Replace("	", ""); //替换掉不该要的空格，防止影响正则匹配
                SourceCode = SourceCode.Replace("<td><input type=\"checkbox\"  checked name=\"checkbox\"/></td>", ""); //替换复选框 修复直接搜索歌手名影响正则匹配的BUG
                SourceCode = SourceCode.Replace("<td class=\"data\" >", "<td class=\"data\">");
                Regex regex = new Regex("<tr class=\"bor2\" songtype=\"11\"><td class=\"data\">(?<data>.*?)</td>",
                        RegexOptions.IgnoreCase);

                if (regex.IsMatch(SourceCode))
                {
                    MatchCollection matches = regex.Matches(SourceCode);
                    //648227|X|4286|林俊杰|54866|JJ林俊杰_100天|1981609|244|8|1|0|9768928|3911305|320000 14
                    //歌曲ID|歌曲名|歌手ID|歌手名|专辑ID|专辑名
                    //"mid", "msong", "msingerid", "msinger", "malbumid", "malbum", 
                    //"msize", "minterval", "mstream", "mdownload", "msingertype", "size320", "size128", "mrate", "sizeape", "sizeflac"
                    //462600|X|12690|薛晓枫|37630|雪雨风|1389819|170|10|1|1|0|2738930|0!!!|0|0
                    string[][] temp = new string[matches.Count][]; //声明一交错数组保存数据
                    for (int i = 0; i < matches.Count; i++)
                    {
                        string[] temp2 = new string[14];
                        temp2 = matches[i].Groups["data"].Value.Split('|');
                        temp[i] = temp2;
                    }

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate() //每载入完一页后重新绑定ItemsSource
                    {
                        LabelTitle.Content = "^_^ 已完成 " + ((p / 2).ToString()) + "0%";
                        for (int i = 0; i < matches.Count; i++)
                        {
                            int Stream_Server = int.Parse(temp[i][8]) + 10;
                            string DownLoadURL = temp[i][13] == "0" ? "http://stream" + Stream_Server.ToString() + ".qqmusic.qq.com/" + (30000000 + int.Parse(temp[i][0])).ToString() + ".mp3" : "http://download.music.qq.com/320k/" + temp[i][0] + ".mp3";
                            
                            SongList.Add(new Song
                                (
                                 ClearHTML(temp[i][1]), ClearHTML(temp[i][3]), ClearHTML(temp[i][5]), 
                                 temp[i][0], temp[i][2], temp[i][4], 
                                 "Skin\\music-default-medium.gif", "Skin\\music-default-medium.gif",
                                 DownLoadURL, temp[i][13] == "0" ? "128Kbps" : (int.Parse(temp[i][13]) / 1000).ToString() + "Kbps"
                                ));
                        }
                    });
                }

            }
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
            {
                LabelTitle.Content = " 搜索发现音乐 - 4321.La";
            });

        }
        #endregion

        #region 专辑封面部分
        private void GetSingleAlbumPicture()
        {
            if (SongList[SourceTAR].AlbumURL.IndexOf("music-default") <= 0)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    AlbumPicture.Source = new BitmapImage(new Uri(SongList[SourceTAR].AlbumURL2, UriKind.RelativeOrAbsolute));
                    CheckBox1.IsEnabled = true;
                    CheckBox1.IsChecked = INIClass.IniReadValue("CheckBox1");
                });
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                {
                    AlbumPicture.Source = null;
                    CheckBox1.IsEnabled = false;
                    CheckBox1.IsChecked = false;
                    
                });
            }
            string DouBanAPI = "http://api.douban.com/music/subjects?start-index=1&max-results=1&apikey=058a7fc77af5da75109f7f5670e18f5f&q="; //豆瓣API
            DouBanAPI += AlbumName;
            string code = WebHelper.GetPageHTMLSource(DouBanAPI);
            Regex regex = new Regex("rel=\"alternate\"/>(.*?)<link href=\"(?<data>.*?)\" rel=\"image\"/>", RegexOptions.IgnoreCase);
            if (regex.IsMatch(code))
            {
                try
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate() //低优先级 下载专辑图片并载入
                    {

                        string PicURL = regex.Match(code).Groups["data"].Value/*.Replace("/spic/", "/lpic/")*/;
                        if ((PicURL == null) || PicURL.IndexOf("music-default") > 0) return;
                        //new BitmapImage(new Uri(@regex.Match(code).Groups["data"].Value, UriKind.Absolute));
                        SongList[SourceTAR].AlbumURL = PicURL;
                        SongList[SourceTAR].AlbumURL2 = PicURL.Replace("/spic/", "/lpic/");
                        AlbumPicture.Source = new BitmapImage(new Uri(SongList[SourceTAR].AlbumURL2, UriKind.RelativeOrAbsolute));
                        CheckBox1.IsEnabled = true;
                        CheckBox1.IsChecked = INIClass.IniReadValue("CheckBox1");

                    });
                }
                catch
                {
                    //Do Nothing
                }

            }
            
        }


        private void GetMultiAlbumPicture()
        {

        }
        #endregion

        #region 歌词部分
        private void GetLyrics()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                CheckBox2.IsEnabled = false;
                CheckBox2.IsChecked = false;
                CheckBox3.IsEnabled = false;
                CheckBox3.IsChecked = false;
            });
            StringBuilder Resulta = new StringBuilder();
            Resulta.Capacity = 65536;
            string SongTitle = SongList[SourceTAR].SongTitle;
            string Artist = SongList[SourceTAR].Artist;
            


            //ふわふわ时间 (#23『放课后!』Mix)
            Regex r = new Regex(@"(\(|（).*?(\)|）)");
            SongTitle = r.Replace(SongTitle, "");
            Artist = r.Replace(Artist, "");
            GetLyrics(SongTitle, Artist, INIClass.IniReadValue("LyricsServer") ? 1 : 0 , Resulta);
            DynamicLyrics = Resulta.ToString();
            if (!string.IsNullOrEmpty(DynamicLyrics))
            {
                Regex regex = new Regex(@"\[.*?\]",
                        RegexOptions.IgnoreCase);

                StaticLyrics = regex.Replace(DynamicLyrics, "").Trim();

                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    CheckBox2.IsEnabled = true;
                    CheckBox2.IsChecked = INIClass.IniReadValue("CheckBox2");
                    CheckBox3.IsEnabled = true;
                    CheckBox3.IsChecked = INIClass.IniReadValue("CheckBox3");
                });
            }
            else
            {
                StaticLyrics = "";
            }

        }
        #endregion
        ///cgi.music.soso.com/fcgi-bin/fcg_smartbox.q?utf8=0&w=no&outputtype=xml
        /// <summary>
        /// 单击搜索按钮 - 开始搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((SearchKeyword.Text=="音乐名、唱片名、表演者")||(string.IsNullOrEmpty(SearchKeyword.Text))) return;
            LabelTitle.Content = "正在连接服务器……";
            Keywords = SearchKeyword.Text;
            if (!(SearchThread == null)) if (SearchThread.IsAlive) SearchThread.Abort();
            SongList.Clear();
            SearchThread = new Thread(StartSearch);
            SearchThread.Start();
            if (!(UpdateThread == null)) if (UpdateThread.IsAlive) UpdateThread.Abort();
            UpdateThread = new Thread(CheckUpdate);
            UpdateThread.Start();
        }

        /// <summary>
        /// DataListBox 选中搜索结果项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SourceTAR == DataListBox.SelectedIndex) return;
            if (DataListBox.SelectedIndex < 0) return;
            SourceTAR = DataListBox.SelectedIndex;
            if (SourceTAR < 0) return;
            AlbumPicture.Source = null;

            AlbumName = SongList[SourceTAR].Album; //设置豆瓣网的API搜索关键词

            if (!(SingleAlbumThread == null)) if (SingleAlbumThread.IsAlive) SingleAlbumThread.Abort();
            SingleAlbumThread = new Thread(GetSingleAlbumPicture);
            SingleAlbumThread.Start();

            if (!(LRCThread == null)) if (LRCThread.IsAlive) LRCThread.Abort();
            LRCThread = new Thread(GetLyrics);
            LRCThread.Start();

        }

        /// <summary>
        /// DataListbox 双击搜索结果项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataListBox.SelectedIndex < 0) return;
            if (DataListBox.ItemsSource == DownLoadList)
            {
                //打开目录并选中文件
                string FileName = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\" + DownLoadList[DataListBox.SelectedIndex].SongTitle + ".mp3";
                FileInfo f = new FileInfo(FileName);

                if (f.Exists)
                {
                    ShowSelectedInExplorer.FileOrFolder(FileName);
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
                }
                return;
            }
            //MessageBox.Show(SongList[DataListBox.SelectedIndex].SongTitle);
            //AlbumPicture.Source = new BitmapImage(new Uri(SongList[DataListBox.SelectedIndex].AlbumURL2, UriKind.RelativeOrAbsolute));
            LabelSongTitle.Content = "歌曲：" + SongList[DataListBox.SelectedIndex].SongTitle;
            LabelArtist.Content = "歌手：" + SongList[DataListBox.SelectedIndex].Artist;
            LabelAlbumName.Content = "专辑：" + SongList[DataListBox.SelectedIndex].Album;
            //TextBoxAlbumName.Text = SongList[DataListBox.SelectedIndex].Album;
            DataListBoxDoubleClick.Begin();   //StoryBoard

            //判断是否存在iTunes的目录
            string MyMusicDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            MyMusicDir += @"\iTunes\iTunes Media\自动添加到 iTunes";
            if (!Directory.Exists(MyMusicDir))
            {
                CheckBox4.IsEnabled = false;
                CheckBox4.IsChecked = false;
            }
            else
            {
                CheckBox4.IsEnabled = true;
                CheckBox4.IsChecked = /*true*/INIClass.IniReadValue("CheckBox4"); ;
            }

            swich.Visibility = Visibility.Hidden;
        }

        #region 下载核心
        /// <summary>
        /// 下载核心 - 主线程
        /// </summary>
        /// <param name="p">线程负责的下载数据源的ID</param>
        private void StartDownload(Object p)
        {
            int id = (int)p;
            string url = /*"http://download.music.qq.com/320k/" + DownLoadList[id].TitleID + ".mp3";*/ DownLoadList[id].DownLoadURL;

            string filename = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\" + DownLoadList[id].SongTitle + ".mp3.!";
            filename = filename.Replace("\"", "＂").Replace("/", "／").Replace("?", "？").Replace("*", "＊").Replace("|", "｜").Replace("<", "＜").Replace(">", "＞");
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                Myrq.Referer = "";

                if (url.IndexOf("stream") != -1)
                {
                    //128kbps
                    Myrq.Headers.Add("Cache-Control: no-cache");
                    Myrq.Headers.Add("Down-Param: uin=1234567&qqkey=&downkey=&&");
                    Myrq.Headers.Add("Cookie: qqmusic_uin=1234567; qqmusic_key=; qqmusic_privatekey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA; qqmusic_fromtag=11;  ");

                }
                else
                {
                    //32kbps、192kbps下载模式
                    Myrq.Headers.Add("Cache-Control: no-cache");
                    Myrq.Headers.Add("Pragma: getIfoFileURI.dlna.org");
                    Myrq.Headers.Add("Cookie: qqmusic_uin=10001; qqmusic_key=; qqmusic_privatekey=; qqmusic_fromtag=11;");
                    Myrq.Headers.Add("GetContentFeatures.DLNA.ORG: 1");
                }
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                int Maximum = (int)totalBytes;
                int starttime = System.DateTime.Now.Second;
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(@filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                long lasttotalDownloadedByte = 0;
                byte[] by = new byte[4096];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    int endtime = System.DateTime.Now.Second;

                    if ((endtime - starttime) >= 1)
                    {
                        //计算下载速度
                        int Speed = Convert.ToInt16(((totalDownloadedByte - lasttotalDownloadedByte) / 1024));
                        lasttotalDownloadedByte = totalDownloadedByte;
                        starttime = endtime;
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                        {
                            DownLoadList[id].Speed = Speed.ToString() + " KB/s";
                        });
                    }
                    so.Write(by, 0, osize);
                    if (Maximum != 0)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                        {
                            DownLoadList[id].DownloadProgress = Math.Floor((double)(int)totalDownloadedByte / Maximum * 100).ToString() + " %";
                        });
                    }
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                so.Dispose();
                st.Close();
                st.Dispose();
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                {
                    DownLoadList[id].DownloadProgress = "已完成";
                    DownLoadList[id].Speed = "";
                });
            }
            catch (Exception e)
            {
                #region 下载失败处理模块
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                {
                    DownLoadList[id].DownloadProgress = "下载失败";
                    DownLoadList[id].Speed = "";
                    MessageBox.Show("下载文件错误，请将下列内容反馈给我们，以方便进一步完善软件！\n（Ctrl+C 可复制 官方网站：www.4321.la）\n" + e.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                });
                try
                {
                    FileInfo file = new FileInfo(@filename);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                catch
                {
                    //Do Nothing
                }
                #endregion
            }
            finally
            {
                #region 下载完成处理模块
                try
                {
                    FileInfo file = new FileInfo(@filename);
                    if (file.Exists)
                    {
                        try
                        {
                            FileInfo fileo = new FileInfo(@filename.Replace(".!", ""));
                            if (fileo.Exists) fileo.Delete();
                            filename = filename.Replace(".!", "");
                            file.MoveTo(@filename);  //重命名文件
                        }
                        catch (Exception e)
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                            {
                                MessageBox.Show("下载文件错误，请将下列内容反馈给我们，以方便进一步完善软件！\n（Ctrl+C 可复制 官方网站：www.4321.la）\n" + e.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
                        }
                        finally
                        {

                            //歌词
                            if (!(string.IsNullOrEmpty(DownLoadList[id].DLrc)))
                            {
                                StreamWriter writer = new StreamWriter(filename.Replace(".mp3",".lrc"));
                                writer.Write(DynamicLyrics);
                                writer.Close();
                            }
                            if (!(string.IsNullOrEmpty(DownLoadList[id].SLrc)))
                            {
                                WriteITunesLyrics(filename, StaticLyrics);
                            }

                            //下载专辑图片
                            if (DownLoadList[id].AutoAlbum)
                            {
                                WebClient mywebclient = new WebClient();
                                string aurl = DownLoadList[id].AlbumURL2;
                                string newfilename = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".jpg";
                                try
                                {
                                    mywebclient.DownloadFile(aurl, newfilename);

                                }
                                catch (Exception ex)
                                {
                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                                    {
                                        MessageBox.Show("下载文件错误，请将下列内容反馈给我们，以方便进一步完善软件！\n（Ctrl+C 可复制 官方网站：www.4321.la）\n" + ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                                    });
                                }
                                finally
                                {
                                    mywebclient.Dispose();
                                    SetCover(filename, newfilename); //应用到MP3文件标签中
                                    FileInfo picfile = new FileInfo(@newfilename);
                                    picfile.Delete(); //删除专辑图片
                                    
                                }

                            }


                            //FixID3Tags
                            FixID3Tags(filename, DownLoadList[id].SongTitle, DownLoadList[id].Artist, DownLoadList[id].Album);

                            //自动添加到iTunes
                            if (DownLoadList[id].AutoiTunes)
                            {
                                FileInfo Fileo = new FileInfo(filename);
                                Fileo.CopyTo(System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\iTunes\iTunes Media\自动添加到 iTunes\" + System.IO.Path.GetFileName(filename), true);
                            }

                        }

                    }


                }
                catch (Exception e)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        MessageBox.Show("下载文件错误，请将下列内容反馈给我们，以方便进一步完善软件！\n（Ctrl+C 可复制 官方网站：www.4321.la）\n" + e.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
                #endregion
            }
        }


        #endregion


        /// <summary>
        /// 添加新的下载任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DownLoadList.Add(new DownLoad(
                SongList[SourceTAR].SongTitle,
                SongList[SourceTAR].Artist,
                SongList[SourceTAR].Album,
                SongList[SourceTAR].TitleID,
                SongList[SourceTAR].ArtistID,
                SongList[SourceTAR].AlbumID,
                SongList[SourceTAR].AlbumURL,
                SongList[SourceTAR].AlbumURL2,
                SongList[SourceTAR].DownLoadURL,
                (CheckBox3.IsChecked == true) ? DynamicLyrics : "",
                (CheckBox2.IsChecked == true) ? StaticLyrics : "",
                (CheckBox1.IsChecked == true),
                (CheckBox4.IsChecked == true)));
            HideGroupBox.Begin();
            DataListBox.ItemsSource = null;
            DataListBox.ItemsSource = DownLoadList;

            swich.Visibility = Visibility.Visible;

            Thread DThread = new Thread(StartDownload);
            int id = DownLoadList.Count - 1;
            DThread.Start(id);
        }

        /// <summary>
        /// 在下载列表数据源、搜索结果数据源之间切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void swich_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DataListBox.ItemsSource == SongList)
            {
                DataListBox.ItemsSource = DownLoadList;
            }
            else
            {
                DataListBox.ItemsSource = SongList;
            }
        }


        /// <summary>
        /// 编辑ID3标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditID3_Click(object sender, RoutedEventArgs e)
        {
            ID3EditorTitle.Text = SongList[SourceTAR].SongTitle;
            ID3EditorAlbum.Text = SongList[SourceTAR].Album;
            ID3EditorArtist.Text = SongList[SourceTAR].Artist;
            groupBox.IsEnabled = false;
        }

        private void ID3Apply_Click(object sender, RoutedEventArgs e)
        {
            groupBox.IsEnabled = true;
            
            SongList[SourceTAR].SongTitle = ID3EditorTitle.Text.Trim();
            SongList[SourceTAR].Album = ID3EditorAlbum.Text.Trim();
            SongList[SourceTAR].Artist = ID3EditorArtist.Text.Trim();
            AlbumPicture.Source = null;
            SongList[SourceTAR].AlbumURL = "Skin\\music-default-medium.gif";
            AlbumName = ID3EditorAlbum.Text.Trim();

            if (!(SingleAlbumThread == null)) if (SingleAlbumThread.IsAlive) SingleAlbumThread.Abort();
            SingleAlbumThread = new Thread(GetSingleAlbumPicture);
            SingleAlbumThread.Start();

            if (!(LRCThread == null)) if (LRCThread.IsAlive) LRCThread.Abort();
            LRCThread = new Thread(GetLyrics);
            LRCThread.Start();

            LabelSongTitle.Content = "歌曲：" + SongList[DataListBox.SelectedIndex].SongTitle;
            LabelArtist.Content = "歌手：" + SongList[DataListBox.SelectedIndex].Artist;
            LabelAlbumName.Content = "专辑：" + SongList[DataListBox.SelectedIndex].Album;
        }

        private void ID3Cancel_Click(object sender, RoutedEventArgs e)
        {
            groupBox.IsEnabled = true;
        }

        private void ID3EditorAlbum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ID3Apply_Click(null, null);
        }


        #region ID3Editor部分文本框焦点特效
        private TextBox tb;
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            tb = (TextBox)sender;

            DoubleAnimation ta = new DoubleAnimation();
            ta.From = 0.5;
            ta.To = 0;
            ta.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTargetName(ta, "Opacity");
            Storyboard.SetTargetProperty(ta, new PropertyPath(Border.OpacityProperty));
            Storyboard storyBoard = new Storyboard();
            storyBoard.Children.Add(ta);
            this.RegisterName("Opacity", this.ColorFulBorder);
            storyBoard.Begin(this, true);

        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            double fleft, ftop, fwidth;
            TextBox ntb = (TextBox)sender;
            if (tb == null)
            {
                fleft = ntb.Margin.Left;
                ftop = ntb.Margin.Top;
                fwidth = ntb.Width;
            }
            else
            {
                //获得失去焦点的文本框的位置
                fleft = tb.Margin.Left;
                ftop = tb.Margin.Top;
                fwidth = tb.Width;

            }
            //获得获得焦点的文本框的位置
            double nleft = ntb.Margin.Left;
            double ntop = ntb.Margin.Top;
            double nwidth = ntb.Width;
            //开启动画效果

            //ThicknessAnimation ta = new ThicknessAnimation();
            SplineThicknessKeyFrame stk1 = new SplineThicknessKeyFrame();
            SplineThicknessKeyFrame stk2 = new SplineThicknessKeyFrame();
            ThicknessAnimationUsingKeyFrames du = new ThicknessAnimationUsingKeyFrames();


            stk1.Value = new Thickness(fleft, ftop, 0, 0);
            stk2.Value = new Thickness(nleft, ntop, 0, 0);

            stk1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));
            stk2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400));
            stk2.KeySpline = new KeySpline(0, 0, 0, 1);

            du.KeyFrames.Add(stk1);
            du.KeyFrames.Add(stk2);

            //ta.From = new Thickness(fleft,ftop,0,0);
            //ta.To = new Thickness(nleft, ntop, 0, 0);
            //ta.Duration = new Duration(TimeSpan.FromMilliseconds(200));



            Storyboard.SetTargetName(du, "Margin");
            Storyboard.SetTargetProperty(du, new PropertyPath(Border.MarginProperty));

            Storyboard storyBoard = new Storyboard();

            storyBoard.Children.Add(du);

            this.RegisterName("Margin", this.ColorFulBorder);


            DoubleAnimation ta = new DoubleAnimation();
            ta.From = 0;
            ta.To = 0.5;
            ta.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTargetName(ta, "Opacity");
            Storyboard.SetTargetProperty(ta, new PropertyPath(Border.OpacityProperty));
            Storyboard storyBoard2 = new Storyboard();
            storyBoard2.Children.Add(ta);
            this.RegisterName("Opacity", this.ColorFulBorder);
            storyBoard2.Begin(this, true);


            DoubleAnimation tw = new DoubleAnimation();
            tw.From = fwidth;
            tw.To = nwidth;
            tw.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTargetName(tw, "Width");
            Storyboard.SetTargetProperty(tw, new PropertyPath(Border.WidthProperty));
            Storyboard storyBoard3 = new Storyboard();
            storyBoard3.Children.Add(tw);
            this.RegisterName("Width", this.ColorFulBorder);
            storyBoard3.Begin(this, true);

            storyBoard.Begin(this, true);


        }

        #endregion



        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.IsChecked == true)
            {
                INIClass.IniWriteValue(cb.Name, "true");
            }
            else
            {
                INIClass.IniWriteValue(cb.Name, "false");
            }
        }


    }

    #region 歌曲列表泛型类
    public class Song : INotifyPropertyChanged
    {
        public string SongTitle { get; set; }
        public string TitleAndArtist { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string ArtistAndAlbum { get; set; }
        public string TitleID { get; set; }
        public string ArtistID { get; set; }
        public string AlbumID { get; set; }
        public string DownLoadURL { get; set; }
        private string albumurl;
        private string albumurl2;
        public string AlbumURL 
        {
            get { return albumurl; }
            set
            {
                albumurl = value;
                NotifyPropertyChange("AlbumURL");
            }
 
        }

        public string AlbumURL2
        {
            get { return albumurl2; }
            set
            {
                albumurl2 = value;
                NotifyPropertyChange("AlbumURL2");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Song(string SongTitle, string Artist, string Album, string TitleID, string ArtistID, string AlbumID, string AlbumURL, string AlbumURL2, string DownLoadURL,string Bitrate)
        {
            
            this.SongTitle = SongTitle;
            this.Artist = Artist;
            this.Album = Album;
            this.TitleID = TitleID;
            this.ArtistID = ArtistID;
            this.AlbumID = AlbumID;
            this.AlbumURL = AlbumURL;
            this.AlbumURL2 = AlbumURL2;
            this.ArtistAndAlbum = "歌手：" + Artist + " 专辑：" + Album;
            this.TitleAndArtist = SongTitle + " - " + Artist + " (" + Bitrate + ")";
            this.DownLoadURL = DownLoadURL;
        }

    }
    #endregion

    #region 下载列表泛型类
    public class DownLoad : INotifyPropertyChanged
    {
        public string SongTitle {get; set;}
        public string TitleAndArtist
        {
            set
            {
                SongTitle = value;
            }
            get
            {

                return SongTitle + "  " + downloadprogress + "  " + speed;
                
            }
        }
        
        public string Artist { get; set; }
        public string ArtistAndAlbum
        {
            set
            {
                
            }
            get
            {
                return "歌手：" + Artist + " " + "专辑：" + Album;
            }
        }
        public string Album { get; set; }
        public string TitleID { get; set; }
        public string ArtistID { get; set; }
        public string AlbumID { get; set; }
        public string AlbumURL { get; set; }
        public string AlbumURL2 { get; set; }
        public string DownLoadURL { get; set; }
        public string DLrc { get; set; }
        public string SLrc { get; set; }
        private string downloadprogress;
        public bool AutoAlbum { get; set; }
        public bool AutoiTunes { get; set; }
        public string DownloadProgress
        {
            get
            {
                return downloadprogress;
            }
            set
            {
                downloadprogress = value;
                NotifyPropertyChange("TitleAndArtist");
            }
        }

        private string speed;
        public string Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                NotifyPropertyChange("TitleAndArtist");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public DownLoad(
            string SongTitle, 
            string Artist,
            string Album, 
            string TitleID, 
            string ArtistID, 
            string AlbumID, 
            string AlbumURL, 
            string AlbumURL2, 
            string DownLoadURL, 
            string DLrc, 
            string SLrc, 
            bool AutoAlbum, 
            bool AutoiTunes)
        {
            this.SongTitle = SongTitle;
            this.Artist = Artist;
            this.Album = Album;
            this.TitleID = TitleID;
            this.ArtistID = ArtistID;
            this.AlbumID = AlbumID;
            this.AlbumURL = AlbumURL;
            this.AlbumURL2 = AlbumURL2;
            this.downloadprogress = "准备下载";
            this.speed = "0 KB/S";
            this.DownLoadURL = DownLoadURL;
            this.DLrc = DLrc;
            this.SLrc = SLrc;
            this.AutoAlbum = AutoAlbum;
            this.AutoiTunes = AutoiTunes;
        }
    }
    #endregion
}