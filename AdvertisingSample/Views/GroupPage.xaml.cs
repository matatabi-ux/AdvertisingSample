using AdvertisingSample.Extentions;
using AdvertisingSample.Framework;
using AdvertisingSample.Presenters;
using AdvertisingSample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdvertisingSample.Views
{
    /// <summary>
    /// グループ詳細画面
    /// </summary>
    public sealed partial class GroupPage : PageBase, IPageBase<IGridPagePresenter>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroupPage()
        {
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
        /// トップアイテムクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnHeaderClick(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var viewModel = button.DataContext as GroupPageViewModel;

            this.Presenter.ItemClick(viewModel.TopItem);
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
