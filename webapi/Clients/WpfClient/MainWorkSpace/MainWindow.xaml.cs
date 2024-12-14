using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using KQClient.MainWorkSpace;
using KQClient.UICommon;
using KQClient.Views;
using KQCommon;
using KQCommon.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace KQClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        FileLogger fileLogger = new FileLogger();
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = ViewModelSource.Create<MainViewModel>();
            this.DataContext = ViewModel;

            SwitchWindowState();
            ViewModel.ShowGlyph = true;
            ViewModel.MenuIsOpen = false;
            lastSender = null;
            
        }

        private object lastSender = null;

        public MainViewModel ViewModel { get; private set; }

        private void ListBoxItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.ControlMenuStatus(sender);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowGlyph = true;
            ViewModel.MenuIsOpen = false;
            lastSender = null;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowGlyph = false;
            popup.IsOpen = false;
            lastSender = null;
        }


        bool _isMaximized;
        Rect _lastNormalRect;

        /// <summary>
        /// 窗口切换最大化最小化功能
        /// </summary>
        private void SwitchWindowState()
        {
            var mainWin = Window.GetWindow(this);
            if (_isMaximized)
            {
                //mainWin.Left = _lastNormalRect.Left;
                //mainWin.Top = _lastNormalRect.Top;
                mainWin.Left = (mainWin.Width - _lastNormalRect.Width) / 2;
                mainWin.Top = (mainWin.Height - _lastNormalRect.Height) / 2;
                mainWin.Width = _lastNormalRect.Width;
                mainWin.Height = _lastNormalRect.Height;
                popup.Height = 633;
            }
            else
            {
                _lastNormalRect = new Rect(mainWin.Left, mainWin.Top, mainWin.Width, mainWin.Height);
                Rect workArea = SystemParameters.WorkArea;
                mainWin.Left = workArea.Left;
                mainWin.Top = workArea.Top;
                mainWin.Width = workArea.Width;
                mainWin.Height = workArea.Height;
                popup.Height = 964;
            }
            _isMaximized = !_isMaximized;

            

        }

        /// <summary>
        /// 自定义最大化、最小化、关闭按钮功能
        /// </summary>
        private void WindowButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                if (tag == "WindowMinimize")
                {
                    Window.GetWindow(this).WindowState = WindowState.Minimized;
                }
                else if (tag == "WindowMaximize")
                {
                    SwitchWindowState();
                }
                else if (tag == "WindowRestore")
                {
                    SwitchWindowState();
                }
                else if (tag == "WindowClose")
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void ThemedWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                SwitchWindowState();
            }
        }

        /// <summary>
        /// 点击空白处事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.IsVisibilty();
        }

        private void MsgClick(object sender, RoutedEventArgs e)
        {
            MsgPopup.IsOpen = !MsgPopup.IsOpen;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    [GenerateViewModel]
    public partial class MainViewModel : ViewModelBase
    {
        public virtual MainWorkspaceViewModel WorkSpace { get; set; } = new MainWorkspaceViewModel();
        private HisODataService.Default.Container context = HisService.HisContext();
        public virtual List<BSMenu> Modules { get; set; }
        public virtual BSMenu CurrentModule { get; set; }
        public virtual bool ShowGlyph { get; set; }
        public virtual bool MenuIsOpen { get; set; }

        [GenerateProperty]
        private Queue<string> msgQueue;

        //private GrowlHelper growlHelper { get; set; }

        [GenerateProperty]
        string mainMenuType;

        public MainViewModel()
        {
            Modules = BSMenuData.GetMenu();

            BSMenu mainMenu = new BSMenu();
            mainMenu.Target = typeof(MainHomePage);

            var Content = Activator.CreateInstance(mainMenu.Target as Type);
            WorkSpace.OpenItem(Content, "首页", "");

            GrowlHelper.SatrtInfo();
            GrowlHelper.SubEvent += () => GetMsg();
        }

        /// <summary>
        /// 第一次登录时读取历史消息
        /// </summary>
        private void SetMag()
        {
            context.SysPushRecords.Select(x => x.Message).ToList().ForEach(o =>MsgQueue.Enqueue(o));
        }

        /// <summary>
        /// 接收登陆状态时的实时消息
        /// </summary>
        private void GetMsg()
        {
            MsgQueue = GrowlHelper.GetMsgQueue();
        }

        /// <summary>
        /// 清除历史消息
        /// </summary>
        internal void ClearMsg()
        {
            GrowlHelper.ClearMsgQueue();
        }
        

        protected ISplashScreenManagerService SplashScreenManagerService { get { return this.GetService<ISplashScreenManagerService>(); } }
        public void OpenView(object sender)
        {
            SplashScreenManagerService.Show();
            if (sender is BSMenu menu && (menu.SubMenus == null || menu.SubMenus.Count == 0))
            {
                var title = menu.Title;

                switch (menu.ContentType)
                {
                    case BSMenuContentType.Catalog:
                        return;
                    case BSMenuContentType.Native:
                        var type = menu.Target as Type;
                        if (type == null) return;

                        var content = Activator.CreateInstance(menu.Target as Type);

                        if (WorkSpace.ActivateDocument(DocumentViewModel.GetIdentity(content, title)))
                        {
                            MenuIsOpen = false;
                            SplashScreenManagerService.Close();
                            return;
                        }
                        WorkSpace.OpenItem(content, title, menu.Icon);
                        MenuIsOpen = false;
                        break;
                    case BSMenuContentType.Web:
                        break;
                    default:
                        break;
                }
            }
            SplashScreenManagerService.Close();
        }  

        private object lastSender = null;
        internal void ControlMenuStatus(object sender)
        {
            var itemCount = (sender as ListBoxItem).Content;

            SplashScreenManagerService.Show();

            if (itemCount is BSMenu menu && (menu.SubMenus == null || menu.SubMenus.Count == 0))
            {
                MenuIsOpen = false;

                var title = menu.Title;

                switch (menu.ContentType)
                {
                    case BSMenuContentType.Catalog:
                        return;
                    case BSMenuContentType.Native:

                        var type = menu.Target as Type;
                        if (type == null) return;

                        var content = Activator.CreateInstance(menu.Target as Type);

                        if (WorkSpace.ActivateDocument(DocumentViewModel.GetIdentity(content, title)))
                        {
                            MenuIsOpen = false;
                            SplashScreenManagerService.Close();
                            return;
                        }
                        WorkSpace.OpenItem(content, title, menu.Icon);
                        MenuIsOpen = false;
                        break;
                    case BSMenuContentType.Web:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if(itemCount is BSMenu menu1)
                {
                    MainMenuType = menu1.Title + "列表";
                }

                if (lastSender == sender)
                {
                    MenuIsOpen = !MenuIsOpen;
                }
                else
                {
                    MenuIsOpen = true;
                }
                lastSender = sender;
            }
            SplashScreenManagerService.Close();
        }

        public void IsVisibilty()
        {
            MenuIsOpen = false;
        }
    }
}
