using Autodesk.Windows;
using System.Windows.Media.Imaging;
using System.Reflection;

public class Icon
{
    public bool _added = false;

    public void IconButton()

    {
        //タブに同じものがない場合は実行されるようにする
        if (!_added)
        {
            RibbonControl ribbonControl = ComponentManager.Ribbon;

            RibbonTab Tab = null;

            foreach (RibbonTab tab in ribbonControl.Tabs)
            {
                if (tab.AutomationName == "Extra")
                {
                    Tab = tab;
                    break;
                }
            }
            if (Tab == null)
            {
                //タブの作成
                Tab = new RibbonTab();
                Tab.Title = "Extra";
                Tab.Id = "EXTRA_TAB_ID";
                ribbonControl.Tabs.Add(Tab);

                //パネルの作成
                RibbonPanelSource srcPanel = new RibbonPanelSource();
                srcPanel.Title = "CreateLinefromCSV";
                srcPanel.Id = "CREATELINEFROMCSV_ID";

                RibbonPanel Panel = new RibbonPanel();
                Panel.Source = srcPanel;
                Tab.Panels.Add(Panel);

                //ボタンの作成、コマンドハンドラー実装
                RibbonButton button = new RibbonButton();
                button.CommandHandler = new CommandHandler();
                button.CommandParameter = "._HelloWorld ";


                // 埋め込みソースの画像を使う場合には名前空間.画像名とします
                button.Text = "CreateLinefromCSV";
                button.Size = RibbonItemSize.Large;
                button.Image = LoadImage("AMDsample.Logo100.jpg", 32, 32);
                button.LargeImage = LoadImage("AMDsample.Logo100.jpg", 64, 64);
                button.ShowText = true;

                RibbonRowPanel ribRowPanel = new RibbonRowPanel();
                ribRowPanel.Items.Add(button);
                ribRowPanel.Items.Add(new RibbonRowBreak());

                srcPanel.Items.Add(ribRowPanel);

                RibbonSeparator rsP = new RibbonSeparator();
                rsP.SeparatorStyle = RibbonSeparatorStyle.Invisible;
                srcPanel.Items.Add(rsP);


                Tab.IsActive = true;
                _added = true;
            }
        }
    }

    //画像のBitmap化
    public BitmapImage LoadImage(string imageName, int Height, int Width)
    {
        BitmapImage image = new BitmapImage();

        image.BeginInit();
        image.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(imageName);
        image.DecodePixelHeight = Height;
        image.DecodePixelWidth = Width;
        image.EndInit();
        return image;
    }

}
