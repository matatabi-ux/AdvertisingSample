using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AdvertisingSample.Extentions
{
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
                    var child = VisualTreeHelperExtentions.FindChild<Canvas>(popup.Child);

                    if (child != null && "AdvertisingWebBrowser".Equals(child.GetType().Name))
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

        /// <summary>
        /// Visual Tree 内の親の指定クラスを検索して返す
        /// </summary>
        /// <typeparam name="T">対象のクラス</typeparam>
        /// <param name="dp">検索のルート要素</param>
        /// <returns>最初に見つかった要素、見つからなかった場合は null</returns>
        public static DependencyObject FindParent<T>(DependencyObject dp)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dp);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }
    }
}
