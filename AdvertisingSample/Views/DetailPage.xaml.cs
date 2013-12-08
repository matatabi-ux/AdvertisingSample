using AdvertisingSample.Extentions;
using AdvertisingSample.Framework;
using AdvertisingSample.Presenters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdvertisingSample.Views
{
    /// <summary>
    /// アイテム詳細画面
    /// </summary>
    public sealed partial class DetailPage : PageBase, IPageBase<DetailPagePresenter>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// この画面の Presenter
        /// </summary>
        public DetailPagePresenter Presenter
        {
            get { return this.PresenterBase as DetailPagePresenter; }
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
