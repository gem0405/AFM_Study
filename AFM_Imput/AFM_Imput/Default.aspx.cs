using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using NPOI.SS.Formula.Functions;
using NPOI.OpenXmlFormats.Dml;
using System.Collections;

namespace AFM_Imput
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string Serverpath = @"D:\一零八年國防部文物清冊";
            //int count = 1;
            //DirectoryInfo di = new DirectoryInfo(Serverpath);
            //foreach (FileInfo fi in di.GetFiles())
            //{
            //    string Path = fi.FullName;
            //    Label1.Text += fi.Name+"</br>";
            //    UpdateOrInsertSheet(Path);
            //    count += 1;
            //}
            string path = "";
            
            if (FileUpload1.HasFile)
            {
                path = Server.MapPath(@"Upload/"+FileUpload1.FileName);
                FileUpload1.SaveAs(path);
                UpdateOrInsertSheet1(path);
            }
        }
        private void UpdateOrInsertSheet1(string path)
        {
            int count = 0;

            IWorkbook workbook = null;  //新建IWorkbook物件
                                        //string fileName = "D:\\sample.xlsx";
            string fileName = path;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //ADO sql = new ADO("APM_Test");
            //DataTable DtColumn = sql.toDataSet("select TOP(1)*from TB_DCS5_TEST").Tables[0];
            //List<int> Colnum = new List<int>();
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx資料讀入workbook
            }
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook(fileStream);  //xls資料讀入workbook
            }
            for (int k = 0; k < workbook.NumberOfSheets; k++)
            {
                ISheet sheet = workbook.GetSheetAt(k);  //獲取工作表
                IRow row;// = sheet.GetRow(0);            //新建當前工作表行資料
                DataTable dt = new DataTable();
                //int count = 0;
                //Colnum.Clear();
                for (int i = 0; i < sheet.LastRowNum + 1; i++)  //對工作表每一行// i=1 表示從第2行開始(根據表格)
                {
                    System.Data.DataRow dr = dt.NewRow(); // 新增表格內的資料行
                    row = sheet.GetRow(i);   //row讀入第i行資料


                    if (row != null)
                    {
                        for (int j = 0; j < row.LastCellNum; j++)  //對工作表每一列          
                                                                   // for (int j = 0; j < row.LastCellNum; j++)  //對工作表每一列              
                        {
                            if (i == 0)
                            {
                                //for (int x = 0; x < DtColumn.Columns.Count; x++)
                                //{
                                //    if (DtColumn.Columns[x].ColumnName == row.GetCell(j).ToString())
                                //    {
                                        dt.Columns.Add(row.GetCell(j).ToString());
                                        //Colnum.Add(j);
                                //    }
                                //}
                               
                               
                                continue;
                            }
                            //if (j > Colnum.Count-1)
                            //    break;
                            if (row.GetCell(j) != null)
                                    //row.Cells[j].SetCellValue = "";
                                    dr[j] = row.GetCell(j).ToString(); //獲取i行j列資料 
                            //dr[j] = row.GetCell(Colnum[j]).ToString();
                             else
                                    dr[j] = ""; //獲取i行j列資料
                               

                           

                        }
                    }
                    //表格
                    if (i != 0)
                    {
                        dt.Rows.Add(dr);
                        //寫入資料庫
                        
                        count += DrInsertDB1(dr, dt);

                    }

                }
                //count += FileCheck(dt, sheet.SheetName);
                Label1.Text += string.Format("第{0}張工作表新增/更新完成{1}", k+1, "<br/>");
                Label1.Text += string.Format("已新增{0}筆資料{1}", count,"<br/>");
               
                //FileImput(dt, sheet.SheetName);
                // Console.ReadLine();
                fileStream.Close();
                workbook.Close();
            }
        //Label3.Text += string.Format("{1}此次總共找到{0}筆資料!", count,"<br/>");
        }
        private void UpdateOrInsertSheet(string path)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            //// 自訂資料欄位
            //資料庫第四格資料 @Item , 項次
            dt.Columns.Add("項次");
            /// 資料庫第一格資料 @ProductID","典藏碼"
            dt.Columns.Add("典藏碼");
            // 資料庫第二格資料 @ProductName","名稱(版本)"
            dt.Columns.Add("名稱(版本)");
          
       
            //資料庫第五格資料 @Amount , 數量
            dt.Columns.Add("數量");
            //資料庫第六格資料 @Size , 尺寸規格
            dt.Columns.Add("尺寸規格");
            //資料庫第七格資料 @Kind , 
            dt.Columns.Add("性質");
            //資料庫第八格資料 @OriginalHolder , 原持有者
            dt.Columns.Add("原持有者");
            // 資料庫第九格資料@Worth , 價值(元)
            dt.Columns.Add("價值(元)");
            //資料庫第十格資料@Material , 材質
            dt.Columns.Add("材質");
            //資料庫第十一格資料@ObjectTime , 年代
            dt.Columns.Add("年代");
            //資料庫第十二格資料@Maker , 製造者(地)
            dt.Columns.Add("製造者(地)");
            //資料庫第十三格資料@Status , 
            dt.Columns.Add("保存狀況");
            //資料庫第十四格資料@Place, 存儲(陳展地點)
            dt.Columns.Add("存儲(陳展地點)");
            //資料庫第十五格資料@ImageUrl , 影像(要加路徑處理)
            dt.Columns.Add("影像");
            //圖片完整路徑           
            //資料庫第十六格資料@Remark , 備考(解說)
            dt.Columns.Add("備考(解說)");
            //資料庫第十七格資料@Remark2 , 備註
            dt.Columns.Add("備註");
            //資料庫第十八格資料@CreateDate , 建立日期
            dt.Columns.Add("建立日期");
            //資料庫第十九格資料@CreateUserId, 建立者ID
            dt.Columns.Add("建立者ID");
            //資料庫第二十格資料@UpdateDate , 更新日期
            dt.Columns.Add("更新日期");
            //資料庫第二十一格資料@UpdateUserId , 更新者ID
            dt.Columns.Add("更新者ID");
            //資料庫第二十二格資料@Cancel , 註銷
            dt.Columns.Add("註銷");
            // 資料庫第三格資料 @UnitPathId","帳號編碼"
            dt.Columns.Add("帳號編碼");

            IWorkbook workbook = null;  //新建IWorkbook物件
                                        //string fileName = "D:\\sample.xlsx";
            string fileName = path;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx資料讀入workbook
            }
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook(fileStream);  //xls資料讀入workbook
            }
            ISheet sheet = workbook.GetSheetAt(0);  //獲取第一個工作表
            IRow row;// = sheet.GetRow(0);            //新建當前工作表行資料
            for (int i = 3; i < sheet.LastRowNum + 1; i++)  //對工作表每一行// i=1 表示從第2行開始(根據表格)
            {
                // 新增表格內的資料行
                System.Data.DataRow dr = dt.NewRow();

                row = sheet.GetRow(i);   //row讀入第i行資料

                if (row != null)
                {
                    for (int j = 0; j < 15; j++)  //對工作表每一列          
                                                  // for (int j = 0; j < row.LastCellNum; j++)  //對工作表每一列              
                    {
                        if (row.GetCell(j) != null)
                            //row.Cells[j].SetCellValue = "";
                            dr[j] = row.GetCell(j).ToString(); //獲取i行j列資料   
                        else
                            dr[j] = ""; //獲取i行j列資料   



                    }
                }
                //表格
                dt.Rows.Add(dr);
                //寫入資料庫
                DrInsertDB(dr, dt);
            }
            // Console.ReadLine();
            fileStream.Close();
            workbook.Close();
        }
        private int DrInsertDB1(DataRow dr, DataTable dt)
        {
            ADO ado = new ADO("APM_Test");
            string sql_select = @"Select FI_YEAR,FI_CAT,FI_SID,FI_GRP,FI_IDXNO from TB_DCS5 where FI_YEAR = @FI_YEAR and FI_CAT = @FI_CAT and FI_SID = @FI_SID 
            and FI_GRP = @FI_GRP and FI_IDXNO = @FI_IDXNO";
            ado.cmd.CommandText = sql_select;
            ado.cmd.Parameters.Clear();
            ado.cmd.Parameters.AddWithValue("@FI_YEAR", dr["FI_YEAR"]);
            ado.cmd.Parameters.AddWithValue("@FI_CAT", dr["FI_CAT"]);
            ado.cmd.Parameters.AddWithValue("@FI_SID", dr["FI_SID"]);
            ado.cmd.Parameters.AddWithValue("@FI_GRP", dr["FI_GRP"]);
            ado.cmd.Parameters.AddWithValue("@FI_IDXNO", dr["FI_IDXNO"]);
            ado.conn.Open();
            SqlDataReader DR = ado.cmd.ExecuteReader();
            StringBuilder cmtxt = new StringBuilder();
            string sql_string = "";
            if (DR.HasRows)
            {
                DR.Close();
                ado.conn.Close();
                ado.cmd.Parameters.Clear();
                sql_string = "Update [APM_Test].[dbo].TB_DCS5 SET ";
                cmtxt.Append(sql_string);
                try
                {  
                    for (int i = 0; i < dr.ItemArray.Length; i++)
                    {
                        
                        //sql_string = dt.Columns[i].ColumnName + " = " + dr[i] + ",";
                        cmtxt.AppendFormat(" {0} = '{1}'{2}",dt.Columns[i].ColumnName,dr[i],',');

                    }
                    //sql_string = cmtxt.ToString().Substring(0, cmtxt.ToString().Length - 1);
                    cmtxt.AppendFormat("FI_PAGESTR = '{0:0000}-{1:0000 }',",1,Convert.ToInt32(dr["FI_PGNU"]));
                    cmtxt.AppendFormat("FI_SEQNO = '{0:00000000}',", Convert.ToInt32(dr["FI_CVOL"].ToString().Substring(0,dr["FI_CVOL"].ToString().Length-1)));
                    cmtxt.AppendFormat("FI_INDEX = '{0}_{1}_{2}_{3}_{4}_{5}'", dr["FI_YEAR"], dr["FI_CAT"], dr["FI_SID"], dr["FI_GRP"], dr["FI_IDXNO"], dr["FI_CVOL"]);
                    cmtxt.Append(@"where FI_YEAR = @FI_YEAR and FI_CAT = @FI_CAT and FI_SID = @FI_SID 
                                 and FI_GRP = @FI_GRP and FI_IDXNO = @FI_IDXNO");
                   
                    ado.cmd.Parameters.AddWithValue("@FI_YEAR", dr["FI_YEAR"]);
                    ado.cmd.Parameters.AddWithValue("@FI_CAT", dr["FI_CAT"]);
                    ado.cmd.Parameters.AddWithValue("@FI_SID", dr["FI_SID"]);
                    ado.cmd.Parameters.AddWithValue("@FI_GRP", dr["FI_GRP"]);
                    ado.cmd.Parameters.AddWithValue("@FI_IDXNO", dr["FI_IDXNO"]);
                    return ado.DbNonQuery(cmtxt.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                DR.Close();
                ado.conn.Close();
                ado.cmd.Parameters.Clear();
                sql_string = "INSERT INTO TB_DCS5_TEST (";
                cmtxt.Append(sql_string);
                string sql_insert = ")Values(";
                try
                {
                    for (int i = 0; i < dr.ItemArray.Length; i++)
                    {

                        sql_string = dt.Columns[i].ColumnName + ",";
                        cmtxt.Append(sql_string);
                        sql_insert += "@" + dt.Columns[i].ColumnName + ",";
                        //ado.cmd.Parameters.AddWithValue("@" + sql_string.Replace(",",""), dr[i]);
                        ado.cmd.Parameters.AddWithValue("@" + dt.Columns[i].ColumnName, dr[i]);
                    }
                    sql_string = cmtxt.ToString().Substring(0, cmtxt.ToString().Length - 1) + sql_insert.Substring(0, sql_insert.Length - 1) + ")";
                    return ado.DbNonQuery(sql_string);
                }
                catch(Exception ex) 
                {
                    throw ex;
                }
                
            }
            
        }
        private void DrInsertDB(DataRow dr, DataTable dt)
        {
            //寫入資料庫
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["AFM"].ConnectionString;
            conn.Open();
            cmd.Connection = conn;

            string sql_select = "select ProductID from AFM_Product where ProductID=@ProductID";
            cmd.CommandText = sql_select;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ProductID", dr[1].ToString());
            SqlDataReader DR = cmd.ExecuteReader();

            string str_name = "";
            if (DR.HasRows)
            {
                DR.Read();
                str_name = DR["ProductID"].ToString();
               Label1.Text += "匯入完成後,典藏號:" + str_name + "==>該筆已經有資料,並更新資料" +
                      "" + "<br />";

                //更新資料
                DR.Close();
                //更新資料
                try
                {
                    string sql = "UPDATE AFM_Product SET ProductID = @ProductID,ProductName = @ProductName,UnitPathId = @UnitPathId,";
                    sql += "Item=@Item,Amount=@Amount,Size=@Size,Kind =@Kind,OriginalHolder=@OriginalHolder,";
                    sql += "Worth=@Worth,Material=@Material,ObjectTime=@ObjectTime,Maker=@Maker,Status=@Status,";
                    sql += "Place=@Place,ImageUrl=@ImageUrl,Remark=@Remark,Remark2=@Remark2,CreateDate=@CreateDate,";
                    sql += "CreateUserId=@CreateUserId,UpdateDate=@UpdateDate,UpdateUserId=@UpdateUserId,Cancel=@Cancel ";
                    sql += "  WHERE ProductID = @ProductID ";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Item", dr[0].ToString());
                    cmd.Parameters.AddWithValue("@ProductID", dr[1].ToString());
                    cmd.Parameters.AddWithValue("@ProductName", dr[2].ToString());
                    cmd.Parameters.AddWithValue("@UnitPathId", "00202A");                   
                    cmd.Parameters.AddWithValue("@Amount", dr[3].ToString());
                    cmd.Parameters.AddWithValue("@Size", dr[4].ToString());
                    cmd.Parameters.AddWithValue("@Kind", dr[5].ToString());
                    cmd.Parameters.AddWithValue("@OriginalHolder", dr[6].ToString());
                    cmd.Parameters.AddWithValue("@Worth", dr[7].ToString());
                    cmd.Parameters.AddWithValue("@Material", dr[8].ToString());
                    cmd.Parameters.AddWithValue("@ObjectTime", dr[9].ToString());
                    cmd.Parameters.AddWithValue("@Maker", dr[10].ToString());
                    cmd.Parameters.AddWithValue("@Status", dr[11].ToString());
                    cmd.Parameters.AddWithValue("@Place", dr[12].ToString());
                    cmd.Parameters.AddWithValue("@ImageUrl", "00202A" + "\\" + dr[1].ToString()+".JPG");
                    cmd.Parameters.AddWithValue("@Remark", dr[14].ToString());
                    ///////////////////////////////////////////////////////////////////////////
                    cmd.Parameters.AddWithValue("@Remark2", "");
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreateUserId","tony");
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateUserId","tony");
                    cmd.Parameters.AddWithValue("@Cancel", 1);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //string str_Err = ex.Message;
                }
            }
            else
            {

                DR.Close();
                //插入資料
                string sql = "INSERT INTO AFM_Product(ProductID,ProductName,UnitPathId,Item,Amount,Size,Kind,OriginalHolder,Worth,Material,ObjectTime,Maker,Status,Place,ImageUrl,Remark,Remark2,CreateDate,CreateUserId,UpdateDate,UpdateUserId,Cancel) ";
                sql += "  VALUES(@ProductID,@ProductName,@UnitPathId,@Item,@Amount,@Size,@Kind,@OriginalHolder,@Worth,@Material,@ObjectTime,@Maker,@Status,@Place,@ImageUrl,@Remark,@Remark2,@CreateDate,@CreateUserId,@UpdateDate,@UpdateUserId,@Cancel)";

                cmd.CommandText = sql;
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@Item", dr[0].ToString());
                    cmd.Parameters.AddWithValue("@ProductID", dr[1].ToString());
                    cmd.Parameters.AddWithValue("@ProductName", dr[2].ToString());
                    cmd.Parameters.AddWithValue("@UnitPathId", "00202A");
                    cmd.Parameters.AddWithValue("@Amount", dr[3].ToString());
                    cmd.Parameters.AddWithValue("@Size", dr[4].ToString());
                    cmd.Parameters.AddWithValue("@Kind", dr[5].ToString());
                    cmd.Parameters.AddWithValue("@OriginalHolder", dr[6].ToString());
                    cmd.Parameters.AddWithValue("@Worth", dr[7].ToString());
                    cmd.Parameters.AddWithValue("@Material", dr[8].ToString());
                    cmd.Parameters.AddWithValue("@ObjectTime", dr[9].ToString());
                    cmd.Parameters.AddWithValue("@Maker", dr[10].ToString());
                    cmd.Parameters.AddWithValue("@Status", dr[11].ToString());
                    cmd.Parameters.AddWithValue("@Place", dr[12].ToString());
                    cmd.Parameters.AddWithValue("@ImageUrl", "00202A" + "\\" + dr[1].ToString());
                    cmd.Parameters.AddWithValue("@Remark", dr[14].ToString());
                    ///////////////////////////////////////////////////////////////////////////
                    cmd.Parameters.AddWithValue("@Remark2", "");
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreateUserId", "tony");
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateUserId", "tony");
                    cmd.Parameters.AddWithValue("@Cancel", 1);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                    //string str_Err = ex.Message;
                }

            }
            conn.Close();
            Label1.Text = "匯入成功<br />";
            // 連結表格的資料到 GridView內
            //GridView1.DataSource = dt;
            //// 並繫結資料
            //GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string LoadPath = @"D:\一零八年國防部文物照片";
            DirectoryInfo di = new DirectoryInfo(LoadPath);
         
            foreach (DirectoryInfo dic in di.GetDirectories())
            {
                LoadPath = dic.FullName + "\\JPG";
                DirectoryInfo dicc = new DirectoryInfo(LoadPath);
                string Cfilename = "";
                foreach (FileInfo fi in dicc.GetFiles())
                {
                    int index = fi.Name.ToString().IndexOf("_");
                    string Ofilename = System.IO.Path.GetFileName(fi.FullName);
                    if(index <0)
                        index = Ofilename.ToString().IndexOf("-");
                    if (index > 0)
                    {
                        string filenum = Ofilename.Substring(index + 1, 1);
                        string NfileName = Ofilename.Substring(0, Ofilename.Length - (Ofilename.Length - index)) + ".JPG";
                        if (filenum == "0")
                        {
                            if (Cfilename != NfileName)
                                File.Copy(fi.FullName, @"C:\軍史\slnAFM\prjAFM\Upload\00202A\" + NfileName);
                        }
                        Cfilename = NfileName;
                    }
                }

                Label2.Text += dic.Name + "</br>";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string LoadPath = @"E:\一零八年國防部文物照片\034照片(1-5018)";
            DirectoryInfo di = new DirectoryInfo(LoadPath);
            foreach (DirectoryInfo dic in di.GetDirectories())
            {
                foreach (FileInfo fi in dic.GetFiles())
                {
                    string Oldfilename = fi.Name;
                   
                    string Newfilename = fi.Name.Replace("tif", "JPG");
                    File.Copy(fi.FullName, @"D:\軍史\slnAFM\prjAFM\Upload\00203A\" + Newfilename);
                }
                Label2.Text = dic.Name;
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string filepath = @"D:\+IIS\AFMIMGS\";
            DirectoryInfo di = new DirectoryInfo(filepath);
            StringBuilder sb = new StringBuilder();
            foreach (DirectoryInfo dif in di.GetDirectories())
            {
                foreach (DirectoryInfo diff in dif.GetDirectories())
                {
                    foreach (FileInfo fi in diff.GetFiles())
                    {
                        //sb.Append(fi.Name);
                        //if (fi.Name.Substring(sb.ToString().IndexOf(".") + 1).Length < 3)
                        //{
                        //    sb.Append(1);
                        //    break;
                        //}
                        if (fi.Name.Substring(0, 4) == "0000")
                        {
                            sb.Append(string.Format("{0}{1}", diff.FullName, '\\'));
                            sb.AppendFormat("{0:00000000}", Convert.ToInt32(dif.Name.Substring(0, dif.Name.Length-1)));
                            //sb.AppendFormat("{0:00000000}", Convert.ToInt32(fi.Name.Substring(0, fi.Name.IndexOf('.') - 1)));
                            sb.Append(fi.Name.Substring(fi.Name.IndexOf('.')));
                            fi.MoveTo(sb.ToString());
                        }

                        sb.Clear();
                    }
                    //if (sb.ToString() != "")
                    //    Label3.Text += string.Format("{0}{1}{0}", "<br/>", dif.Name);
                    //sb.Clear();
                    //if (Convert.ToInt32(dif.Name.Substring(0, dif.Name.Length - 1)) > 61157)
                    //    break;
                    // diff.MoveTo(string.Format("{0}{1}\\{2:00}", filepath, dif.Name,Convert.ToInt16(diff.Name)));
                }
            }
            Label3.Text = "修改完成!";
            //string filepath = @"E:\轉檔匯入_17";
            //DirectoryInfo di = new DirectoryInfo(filepath);
            //string numname_path = @"E:\錯誤\";
            //Directory.CreateDirectory(numname_path);
            //foreach (DirectoryInfo dif in di.GetDirectories())
            //{
            //    foreach (FileInfo fi in dif.GetFiles())
            //    {
            //        if (fi.Name.Substring(fi.Name.LastIndexOf(".") + 1, 3) != "jpg")
            //        {
            //            File.Move(fi.FullName, numname_path + fi.Name);

            //        }
            //    }
            //}
            //string Serverpath = @"D:\影像測試資料匣";

            //Label4.Text = "檔名更改完成";
            //FileImput();
            //string filepath = @"D:\work";
            //DirectoryInfo di = new DirectoryInfo(filepath);
            //foreach (DirectoryInfo dif in di.GetDirectories())
            //{
            //    if (dif.Name.Substring(0, 4) == "轉檔匯入")
            //    {
            //        int count = 0;
            //        foreach (DirectoryInfo diff in dif.GetDirectories())
            //        {
            //            //if (diff.Name == "048_1740.5_5000_0002_25")
            //            StringBuilder sb = new StringBuilder();
            //            sb.Append(diff.Name);
            //            string[] Dname = sb.ToString().Split('_');

            //            //if (diff.Name.Substring(diff.Name.LastIndexOf("_") + 1, 2) != "00")
            //            //{     

            //            sb.Clear();
            //                sb.Append(string.Format("{0}{1}", dif.FullName, "\\"));
            //                sb.Append(string.Format("{0:000}{1}", Convert.ToInt16(Dname[0]), "_"));
            //                sb.Append(string.Format("{0}{1}", Dname[1], "_"));
            //                sb.Append(string.Format("{0}{1}",Dname[2], "_"));
            //                sb.Append(string.Format("{0:0000}{1}", Convert.ToInt32(Dname[3]), "_"));
            //                sb.Append(string.Format("{0:00}", Convert.ToInt16(Dname[4])));

            //            if (!Directory.Exists(sb.ToString()))
            //            {
            //                diff.MoveTo(sb.ToString());
            //                count++;
            //            }


            //        }
            //        Label3.Text += string.Format("{0}修改成功{1}共修改{2}個檔案!{1}", dif.Name, "<br/>", count);
            //    }
            //}


        }
        public int FileCheck(DataTable dt,string sheetname)
        {
            StringBuilder sb = new StringBuilder();
           
            //string filepath = sb.ToString() ;
                                /*@"D:\\+IIS\\AFMIMGS";*/
            //DirectoryInfo di = new DirectoryInfo(filepath);
            //foreach (DirectoryInfo dif in di.GetDirectories())
            //{
            int count = 0;
            int excount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
                {

                //string numname = dt.Rows[i]["FI_CVOL"].ToString();
                //sb.Append(string.Format("{0}{1}", dif.FullName, "\\"));
                    sb.Append(@"D:\work\轉檔匯入_");
                    sb.Append(string.Format("{0}{1}", sheetname.Substring(0, 2), "\\"));
                    sb.Append(string.Format("{0:000}{1}", Convert.ToInt16(dt.Rows[i]["FI_YEAR"]), "_"));
                    sb.Append(string.Format("{0}{1}", dt.Rows[i]["FI_CAT"], "_"));
                    sb.Append(string.Format("{0}{1}", dt.Rows[i]["FI_SID"], "_"));
                    sb.Append(string.Format("{0:0000}{1}", Convert.ToInt32(dt.Rows[i]["FI_GRP"]), "_"));
                    sb.Append(string.Format("{0:00}", Convert.ToInt16(dt.Rows[i]["FI_IDXNO"])));
                if (!Directory.Exists(sb.ToString()))
                {
                    count++;
                    Label3.Text += string.Format("{0}{1}{1}", sb, "<br/>");

                }
                else
                {
                    excount++;
                }
                sb.Clear();
            }
            Label3.Text += string.Format("{1}{0}以上檔案無圖片!{0}檢查完成!{0}共找到{2}筆{0}少了{3}筆!{0}", "<br/>",sheetname, excount , count);
            return excount;
        //}
        }
        public void FileImput(DataTable dt, string sheetname)
        {
            string filepath = @"D:\work";

            //string numname = "0057544A";

           
            //string numname = "";

            DirectoryInfo di = new DirectoryInfo(filepath);
            foreach (DirectoryInfo dif in di.GetDirectories())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"D:\work\轉檔匯入_");
                sb.Append(sheetname.Substring(0, 2));
                if (dif.FullName  == sb.ToString())
                {
                    sb.Clear();  
                    int count = 0;
                  
                    //foreach (DirectoryInfo diff in dif.GetDirectories())
                    //{
                    //if (diff.Name == "048_1740.5_5000_0002_25")


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (diff.Name.Substring(diff.Name.LastIndexOf("_") + 1, 2) != "00")
                        //{                      
                        
                        string numname = dt.Rows[i]["FI_CVOL"].ToString();
                       
                        //if (Convert.ToInt32(numname.Substring(0,numname.Length-1)) > 61157)
                        //    break;
                        sb.AppendFormat("{0}{1}", dif.FullName, "\\");
                        sb.AppendFormat("{0:000}{1}",Convert.ToInt16(dt.Rows[i]["FI_YEAR"]),"_");
                        sb.AppendFormat("{0}{1}", dt.Rows[i]["FI_CAT"], "_");
                        sb.AppendFormat("{0}{1}", dt.Rows[i]["FI_SID"], "_");
                        sb.AppendFormat("{0:0000}{1}",Convert.ToInt32(dt.Rows[i]["FI_GRP"]), "_");
                        sb.AppendFormat("{0:00}", Convert.ToInt16(dt.Rows[i]["FI_IDXNO"]));

                        if (Directory.Exists(sb.ToString()))
                        {
                            // DirectoryInfo diff = new DirectoryInfo(sb.ToString());

                            //foreach (FileInfo fi in diff.GetFiles())
                            //{

                            ////取得檔名,排除副檔名
                            //string name = System.IO.Path.GetFileNameWithoutExtension(fi.FullName);
                            ////去除檔名最後的2個字元
                            ////若用DocuFreezer轉則此步驟可省略
                            ////name = name.Substring(0, name.Length - 2);
                            ////取出檔名最後三個字

                            ////string idxno = name.Split(_);
                            //string fi_Extension = name.Substring(name.LastIndexOf("_") + 1);
                            ////轉換為10進位數字
                            //int aValue = Convert.ToInt16(fi_Extension);
                            ////將10進位轉為16進位,不足3位數將前面補零,並將字母轉為大寫
                            ////string aStr = Convert.ToString(aValue,16).PadLeft(3, '0').ToUpper();

                            ////將10進位轉為36進位,不足3位數將前面補零
                            //string aStr = ConvertTo36(aValue).PadLeft(3, '0');
                            ////將aStr當作副檔名處理            
                            //string chgExtension = System.IO.Path.ChangeExtension(fi.FullName, aStr);
                            ////直接更改檔名
                            //fi.CopyTo(chgExtension);
                            ////MoveTo
                            ////加入總檔案號後,重新組建新的檔案完整路徑

                            //string numpath = numname.Substring(numname.Length - 2, 1);
                            ////自訂儲存路徑 diff.FullName.Replace("匯入", "完成");
                            //string SavePath = "D:\\+IIS\\AFMIMGS";
                            //string numname_path = SavePath + "\\" + numname + "\\" + numpath;
                            ////string newname_path = Path.GetDirectoryName(fi.FullName) + "\\" + numname + "." + aStr;
                            //string newname_path = numname_path + "\\" + numname + "." + aStr;
                            //// Move the file,最後變成的檔案放在後面.
                            //if (!Directory.Exists(numname_path))
                            //    Directory.CreateDirectory(numname_path);
                            //File.Move(chgExtension, newname_path);
                            ////紀錄


                            //}
                           
                            StringBuilder sb2 = new StringBuilder();
                           
                            sb2.Append(@"D:\+IIS\AFMIMGS\");
                            DirectoryInfo di2 = new DirectoryInfo(sb2.ToString());
                            sb2.Clear();
                            //sb2.AppendFormat(@"D:\+IIS\AFMIMGS_0427\{0}", numname);
                            sb2.Append(@"D:\+IIS\AFMIMGS_0427\");
                            if (!Directory.Exists(sb2.ToString()))
                            {
                                Directory.CreateDirectory(sb2.ToString());
                            }
                            sb2.Append(numname);
                            if(!Directory.Exists(sb2.ToString()))

                                di2.GetDirectories()[0].MoveTo(sb2.ToString());
                            
                            ChangeFilename(numname,sb2);
                            sb2.Clear();
                        

                        }
                        else
                        { 
                         
                          Label3.Text += string.Format("{1}{1}{0}{1}",sb,"<br/>");
                          count++;
                        }
                        sb.Clear();
                        //numname = string.Format("{0:0000000}{1}", (Convert.ToInt32(numname.Substring(0, numname.Length - 1)) + 1), "A");
                        //}
                        //}
                    }

                   

                    Label3.Text += string.Format("共{0}筆檔案無圖片{1}轉檔成功{2}{1}",count,"<br/>", dif.Name);
                }
                sb.Clear();

            }
        }
        public void ChangeFilename(string numname,StringBuilder sb)
        {
            //string filepath = @"D:\+IIS\AFMIMGS\";
            //DirectoryInfo di = new DirectoryInfo(filepath);
              
            DirectoryInfo dif = new DirectoryInfo(sb.ToString());
            
            foreach (DirectoryInfo diff in dif.GetDirectories())
                {
               
                sb.AppendFormat(@"\{0:00}", Convert.ToInt16(numname.Substring(numname.Length - 2,1)));
                if (!Directory.Exists(sb.ToString())) 
                    diff.MoveTo(sb.ToString());
                sb.Clear();
                    foreach (FileInfo fi in diff.GetFiles())
                    {
                       
                        sb.AppendFormat("{0}{1}", diff.FullName, '\\');
                        sb.AppendFormat("{0:00000000}", Convert.ToInt32(dif.Name.Substring(0, dif.Name.Length - 1)));
                        //sb.AppendFormat("{0:00000000}", Convert.ToInt32(fi.Name.Substring(0, fi.Name.IndexOf('.') - 1)));
                        sb.Append(fi.Name.Substring(fi.Name.IndexOf('.')));
                        if (!Directory.Exists(sb.ToString())) 
                            fi.MoveTo(sb.ToString());
                        sb.Clear();
                        //string aStr = fi.Name.Substring(fi.Name.IndexOf('.'));
                        //string numpath = string.Format("{0:00}", numname.Substring(numname.Length - 2, 1));
                        //自訂儲存路徑 diff.FullName.Replace("匯入", "完成");
                        //string SavePath = "D:\\+IIS\\AFMIMGS";
                        //string numname_path = SavePath + "\\" + numname + "\\" + numpath;
                        //string newname_path = Path.GetDirectoryName(fi.FullName) + "\\" + numname + "." + aStr;
                        //string newname_path = numname_path + "\\" + numname + aStr;
                        //Move the file,最後變成的檔案放在後面.
                        //if (!Directory.Exists(numname_path))
                        //    Directory.CreateDirectory(numname_path);
                        //fi.MoveTo(newname_path);

                    }
                  
                }
               
            
          
        }
        public static string ConvertTo36(int i)
        {
            string s = "";
            int j = 0;
            while (i >= 36)
            {
                j = i % 36;
                if (j < 10)
                    s += j.ToString();
                else
                    s += Convert.ToChar(j + 87);
                i = i / 36;
            }
            if (i < 10)
                s += i.ToString();
            else
                s += Convert.ToChar(i + 87);
            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return Convert.ToString(new string(c)).ToUpper();
        }
    }
}