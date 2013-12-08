using AdvertisingSample.Extentions;
using AdvertisingSample.Framework;
using AdvertisingSample.Presenters;
using AdvertisingSample.ViewModels;
using MicrosoftAdvertising.Shared.WinRT;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AdvertisingSample.Views
{
    /// <summary>
    /// トップ画面
    /// </summary>
    public sealed partial class TopPage : PageBase, IPageBase<IGridPagePresenter>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPage()
            : base()
        {
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            this.InitializeComponent();
        }

        /// <summary>
        /// この画面の Presenter
        /// </summary>
        public IGridPagePresenter Presenter
        {
            get { return this.PresenterBase as IGridPagePresenter; }
        }

        /// <summary>
        /// GridView サイズ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.Presenter.ScrollIntoView(sender as ListViewBase);
        }

        /// <summary>
        /// タイルクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Presenter.ItemClick(e.ClickedItem as PhotoItemViewModel);
        }

        /// <summary>
        /// ヘッダクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnHeaderClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            this.Presenter.HeaderClick(button.DataContext as PhotoGroupViewModel);
        }

        /// <summary>
        /// AppBar が開いた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAppBarOpened(object sender, object e)
        {
            // 広告が表示されている場合は AppBar を閉じる
            if (VisualTreeHelperExtentions.IsShowAdPopup)
            {
                ((AppBar)sender).IsOpen = false;
            }
        }

        /// <summary>
        /// About ボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnAboutButtonClick(object sender, RoutedEventArgs e)
        {
            this.AppBar.IsOpen = false;
            PresenterLocator.Get<MainPresenter>().ShowAboutFlyout();
        }
    }
}
