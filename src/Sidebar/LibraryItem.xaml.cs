﻿using System;
using System.Collections.Generic;
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
using System.Threading;
using System.IO;
using System.Net;

namespace Sidebar
{
    /// <summary>
    /// Interaction logic for LibraryItem.xaml
    /// </summary>
    public partial class LibraryItem : UserControl
    {
        private bool _selected = false;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (value == true)
                    Bg1.Opacity = 0.7;
                else
                    Bg1.Opacity = 0;
            }
        }

        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;

                if (_icon != null)
                {
                    ThreadStart threadStarter = delegate
                    {
                        DownloadIcon();
                    };
                    Thread thread = new Thread(threadStarter);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
            }
        }

        private string _header = "";

        public string Header
        {
            get { return _header; }
            set
            {
                ItemTitleTextBlock.Text = value;
                _header = value;
            }
        }

        public string Developer
        {
            get { return ItemDeveloperTextBlock.Text; }
            set { ItemDeveloperTextBlock.Text = value; }
        }

        public int Id;
        public string Link;
        public string Description;
        public string Version;

        public LibraryItem()
        {
            InitializeComponent();
        }

        private void DownloadIcon()
        {
            if (!Directory.Exists(SidebarWindow.sett.path + @"\Cache") || !File.Exists(SidebarWindow.sett.path + @"\Cache\" + Id))
            {
                Directory.CreateDirectory(SidebarWindow.sett.path + @"\Cache");

                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(_icon, SidebarWindow.sett.path + @"\Cache\" + Id);

                    ItemIconImage.Dispatcher.BeginInvoke((Action)delegate
                    {
                        ItemIconImage.Source = new BitmapImage(new Uri(SidebarWindow.sett.path + @"\Cache\" + Id));
                    }, null);
                }
                catch
                {
                    ItemIconImage.Dispatcher.Invoke((Action)delegate
                    {
                        ItemIconImage.Source = new BitmapImage(new Uri("/Sidebar;component/Resources/Tile_Icon.png", UriKind.Relative));
                    }, null);
                }
            }
            else
            {
                if (File.Exists(SidebarWindow.sett.path + @"\Cache\" + Id))
                {
                    FileInfo info = new FileInfo(SidebarWindow.sett.path + @"\Cache\" + Id);
                    if (info.Length > 0)
                    {
                        ItemIconImage.Dispatcher.Invoke((Action)delegate
                        {
                            ItemIconImage.Source = new BitmapImage(new Uri(SidebarWindow.sett.path + @"\Cache\" + Id));
                        }, null);
                    }
                    else
                    {
                        info.Delete();
                        DownloadIcon();
                    }
                }

                DirectoryInfo d = new DirectoryInfo(SidebarWindow.sett.path + @"\Cache");
                if (Math.Abs(DateTime.Now.Day - d.CreationTime.Day) > 7)
                    try
                    {
                        d.Delete(true);
                    }
                    catch { }
            }
        }
    }
}
