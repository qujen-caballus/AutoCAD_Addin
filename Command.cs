using System.IO;
using System;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Collections.Generic;

namespace AMDsample
{


    public class Command
    {
        public List<double> OpenCSV()
        {
            //CSVファイルを読み込んでリストに収納していきます
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv file(*.csv)|*.csv";
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            List<double> csv_lists = new List<double>();
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイルを読み取り専用で開く
                Stream stream = ofd.OpenFile();
                if (stream != null)
                {
                    StreamReader sr = new StreamReader(ofd.FileName);
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] values = line.Split(',');
                            double[] doubleArray = Array.ConvertAll(values, double.Parse);
                            csv_lists.AddRange(doubleArray);
                        }
                    }
                }
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog("キャンセルされました");
            }
            return csv_lists;
        }
        //コマンドメソッドの指定
        [CommandMethod("CreateLinefromCSV")]
        public static void CreateRotatedDimension()
        {
            //アイコン追加
            Icon icon = new Icon();
            icon.IconButton();
            Document now_Doc = Application.DocumentManager.MdiActiveDocument;
            Database now_CurDb = now_Doc.Database;

            List<double> point_lists = new List<double>();

            Command command = new Command();
            point_lists = command.OpenCSV();

            //トランザクションの開始
            using (Transaction Trans = now_CurDb.TransactionManager.StartTransaction())
            {
                // ブロックテーブルの読み込み
                BlockTable BlkTbl;
                BlkTbl = Trans.GetObject(now_CurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord BlkTblRec;
                BlkTblRec = Trans.GetObject(BlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                //ポリラインを引く処理
                using (Polyline polyline = new Polyline())
                {
                    for (int i = 0; i < point_lists.Count / 2; i++)
                    {
                        polyline.AddVertexAt(i, new Point2d(point_lists[2 * i], point_lists[2 * i + 1]), 0, 0, 0);
                        polyline.Closed = true;
                    }
                    BlkTblRec.AppendEntity(polyline);
                    Trans.AddNewlyCreatedDBObject(polyline, true);
                }
                //トランザクションの終了
                Trans.Commit();
            }
        }
    }
}
