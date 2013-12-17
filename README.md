
Microsoft Advertising SDK は Windows ストアアプリに対応しているため、アプリを無料にして
ダウンロード数を確保し、広告収入で稼ぐというビジネスモデルが個人の開発者でも簡単に実現できます。

広告の種類もさまざまあり、例えば広告領域をタップすると全画面で動画が再生される画面に遷移する
広告もあります。

![全画面広告](https://heypyw.dm1.livefilestore.com/y2pOZd3VlqNNj9forx1od--jmVrTrP33N6bmrK78nTijpd9UK1dyBx-t5rJ-SigyAypFo9FD-weH4cJ1jyTyV9g5r3adTDd1scUuPvmASD4_n0/ad.png?psid=1)

しかし、この全画面広告、Windows 8.0 用ストアアプリのときと仕様が変わっており、アプリバーや
設定チャームなどの Popup コントロールが広告の前面に表示できてしまいます！

![アプリバーが広告に重なる](https://heypyw.dm2302.livefilestore.com/y2pymqvJ2p891PVSDvKCSWc1Z3YWtDBgtG9VuZYjetob6XrfcaS5hcckPmlLACJw4tL4rtv5YF3ZCEzU7-0mfmDX3zQUBxSscBt5LXCkJz4T3I/adng.png?psid=1)

Advertising の全画面広告は内部的に WebView を利用しており、Wndows 8.0 までは Popup コントロール
よりも前面にレンダリングされていたためこのような現象は発生しませんでした。
しかし、Windows 8.1 からは Popup が最前面に表示されるようになったため、Popup コントロールが
広告表示を妨げるというあまりよろしくない状態が起こり得てしまいます・・・

今回はこの問題への対処コードの一例をご紹介したいと思います。

## サンプルコード 

下記の GitHub リポジトリにて公開しています。

[GitHub AdvertisingSample](https://github.com/tatsuji-kuroyanagi/AdvertisingSample)

※情報取得に Flickr API を利用していますが、API キーを指定するまではダミーのキャッシュデータを読み込むようになっています。
 実際に Flickr のデータを取得するにはアプリを Flickr に登録し、コード内の FlickrConstants.cs に API キーを指定してください。

## 実装のポイント

* VisualTreeHelper.GetOpenPopups で表示中の Popup コントロールを取得します
* VisualTreeHelper を利用して Popup コントロール子要素の AdvertisingWebBrowser を検索します
* AdvertisingWebBrowser が表示中の Popup コントロールに存在する場合、アプリバーなどの Popup 表示を抑止します

## 具体的なやり方

Windows 8.1 用のストアアプリからは VisualTreeHelper が強化され、GetOpenPopups のメソッドで
表示中の Popup コントロールを取得できるようになりました。
全画面広告は Popup コントロールとして表示されるため、このメソッドを利用して下記のような
判定処理を記述します。

```VisualTreeHelperExtentions.cs
/// <summary>
/// VisualTreeHelper 拡張クラス
/// </summary>
public class VisualTreeHelperExtentions
{
    /// <summary>
    /// 広告 Popup 表示フラグ
    /// </summary>
    public static bool IsShowAdPopup
    {
        get
        {
            foreach (var popup in VisualTreeHelper.GetOpenPopups(Window.Current))
            {
                // AdvertisingWebBrowser が参照できないので Canvas チェックで代用
                var child = VisualTreeHelperExtentions
                                     .FindChild<Canvas>(popup.Child);

                if (child != null
                    && "AdvertisingWebBrowser".Equals(child.GetType().Name))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Visual Tree 内の配下の指定クラスを検索して返す
    /// </summary>
    /// <typeparam name="T">対象のクラス</typeparam>
    /// <param name="dp">検索のルート要素</param>
    /// <returns>最初に見つかった要素、見つからなかった場合は null</returns>
    public static DependencyObject FindChild<T>(DependencyObject dp)
    {
        int childCount = VisualTreeHelper.GetChildrenCount(dp);
        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dp, i);
            if (child != null && child is T)
            {
                return child;
            }
            else
            {
                FindChild<T>(child);
            }
        }
        return null;
    }
}
```

アプリバー表示中に VisualTreeHelper.GetOpenPopups を実行した場合、アプリバーも Popup として
コレクションに含まれてしまうため、さらに子要素を検索し「AdvertisingWebBrowser」という
クラスが含まれているかどうかで全画面広告が表示されているかどうかを判定します。

```TopPage.xaml.cs
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
```

あとは AppBar の Opened イベントハンドラ内などでこの判定処理を行い、表示を抑止するだけで
全画面広告表示時のみアプリバーが表示されなくなります。

