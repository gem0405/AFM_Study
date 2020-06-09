using AFM_Imput;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Imput
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void UpdateOrInsertSheet1(string path)
        {
            int count = 0;

            IWorkbook workbook = null;  //新建IWorkbook物件
                                        //string fileName = "D:\\sample.xlsx";
            string fileName = path;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //ADO sql = new ADO("APM_Test");
            //DataTable DtColumn = sql.toDataSet("select TOP(1)*from TB_dcs5xxx_TEST").Tables[0];
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
                Label1.Text += string.Format("第{0}張工作表新增/更新完成{1}", k + 1, "<br/>");
                Label1.Text += string.Format("已新增{0}筆資料{1}", count, "<br/>");

                //FileImput(dt, sheet.SheetName);
                // Console.ReadLine();
                fileStream.Close();
                workbook.Close();
            }
            //Label3.Text += string.Format("{1}此次總共找到{0}筆資料!", count,"<br/>");

        }
        private int DrInsertDB1(DataRow dr, DataTable dt)
        {
            ADO ado = new ADO("APM_Test");
            string sql_select = @"Select FI_YEAR,FI_CAT,FI_SID,FI_GRP,FI_IDXNO from TB_dcs5xxx where FI_YEAR = @FI_YEAR and FI_CAT = @FI_CAT and FI_SID = @FI_SID 
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
                sql_string = "Update [APM_Test].[dbo].TB_dcs5xxx SET ";
                cmtxt.Append(sql_string);
                try
                {
                    for (int i = 0; i < dr.ItemArray.Length; i++)
                    {

                        //sql_string = dt.Columns[i].ColumnName + " = " + dr[i] + ",";
                        cmtxt.AppendFormat(" {0} = '{1}'{2}", dt.Columns[i].ColumnName, dr[i], ',');

                    }
                    //sql_string = cmtxt.ToString().Substring(0, cmtxt.ToString().Length - 1);
                    cmtxt.AppendFormat("FI_PAGESTR = '{0:0000}-{1:0000 }',", 1, Convert.ToInt32(dr["FI_PGNU"]));
                    cmtxt.AppendFormat("FI_SEQNO = '{0:00000000}',", Convert.ToInt32(dr["FI_CVOL"].ToString().Substring(0, dr["FI_CVOL"].ToString().Length - 1)));
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
                sql_string = "INSERT INTO TB_dcs5xxx (";
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
                catch (Exception ex)
                {
                    throw ex;
                }

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";


            if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.FileName != null)
            {
                path = openFileDialog1.FileName;
            }

            UpdateOrInsertSheet1(path);

        }
    }
   
}
