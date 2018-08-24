using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Windows.Forms;
using V1000_ModbusRTU;
using ModbusRTU;
using System.Runtime.InteropServices;
using XL = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data.OleDb;


namespace V1000_Drive_Programmer
{
    partial class frmMain
    {
        public bool dB_MachAddChart(string p_Mach, string p_ChrtNum)
        {
            bool ret_val = false;

            string sql = "INSERT INTO [Sheet1$] (CHRT_NUM) VALUES ('" + p_ChrtNum + "');";
            string db = p_Mach + dBChartExt;
            ret_val = dB_Update(db, sql);

            // create the actual chart file
            dB_CreateDB(p_ChrtNum, "IDX, PARAM_NUM, PARAM_VAL");

            return ret_val;
        }

        public bool dB_Insert(string p_dB, string p_Cols, string p_Vals)
        {
            bool ret_val = false;
            string sql = "INSERT INTO [Sheet1$] (" + p_Cols + ") VALUES (" + p_Vals + ");";
            ret_val = dB_Update(p_dB, sql);
            return ret_val;
        }

        public void dB_Drop(string p_dB)
        {
            string filename = DataDir + p_dB + dbFileExt;
            System.IO.File.Delete(filename);
        }

        //public bool dB_Delete(string p_dB, string p_Col, string p_Cond)
        //{
        //    bool ret_val = false;

        //    string sql = "DELETE FROM [Sheet1$] WHERE [" + p_Col + "] LIKE '" + p_Cond + "%';";
        //    ret_val = dB_Update(p_dB, sql);
        //    return ret_val;
        //}

        
        public bool dB_Delete(string p_dB, string p_Col, string p_Cond)
        {
            bool ret_val = false;
            int col_idx = 0, row_idx = 0;
            string filename = DataDir + p_dB + dbFileExt;

            XL.Application xlApp = new XL.Application();
            XL._Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            XL._Worksheet xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
            XL.Range xlRange = xlWorksheet.UsedRange;

            int row_cnt = xlRange.Rows.Count;
            int col_cnt = xlRange.Columns.Count;
            
            // First find the column number
            for(int i=1;i<col_cnt;i++)
            {
                string val = xlRange.Cells[1, i].Value2.ToString();
                if(val == p_Col)
                {
                    col_idx = i;
                    break;
                }
            }

            if(col_idx > 0)
            {
                // Next find the row number to delete
                for(int i=2;i<=row_cnt;i++)
                {
                    string val = xlRange.Cells[i, col_idx].Value2.ToString();
                    if(val == p_Cond)
                    {
                        row_idx = i;
                        break;
                    }
                }
                
                if(row_idx > 0)
                {
                    (xlWorksheet.Rows[row_idx, System.Reflection.Missing.Value] as XL.Range).Delete(XL.XlDeleteShiftDirection.xlShiftUp);
                    ret_val = true;
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlApp.DisplayAlerts = false;
            xlWorkbook.Save();
            xlWorkbook.Close();
            xlApp.Quit();

            // release COM objects
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlApp);

            return ret_val;
        }
        


        void dB_CreateDB(string p_Name, string p_Fields)
        {
            XL.Application xlApp = new XL.Application();
            XL.Workbook xlWorkbook = xlApp.Workbooks.Add();
            XL._Worksheet xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
            List<string> col_list = new List<string>();

            string filename = DataDir + p_Name + dbFileExt;

            while(p_Fields.IndexOf(',') >= 0)
            {
                int idx = p_Fields.IndexOf(',');
                string col = "";
                if(idx > 0)
                {
                    col = p_Fields.Substring(0, idx);
                    col_list.Add(col);
                }
                p_Fields = p_Fields.Remove(0, col.Length+1);
                p_Fields = p_Fields.TrimStart();
            } 

            col_list.Add(p_Fields);

            for(int i=0;i<col_list.Count;i++)
                xlWorksheet.Cells[1, i+1].Value2 = col_list[i];


            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlApp.DisplayAlerts = false;
            xlWorkbook.SaveAs(filename, ConflictResolution: XL.XlSaveConflictResolution.xlLocalSessionChanges);
            xlWorkbook.Close();
            xlApp.Quit();

            // release COM objects
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlApp);
        }

        private bool GetParamList(DataRow p_Row, string p_Col, ref DataTable p_Tbl, ref List<V1000_Param_Data> p_List)
        {
            bool ret_val = false;

            string file = p_Row[p_Col].ToString() + dbFileExt;
            string conn_str = OLEBaseStr + DataDir + file + OLEEndStr;

            // Get the table containing the list of parameters automatically modified by a 
            // heavy-duty setting and fill  the the Param_HD_Mods list with all the values.
            if(SQLGetTable(conn_str, ref p_Tbl))
            {
                ret_val = true;

                p_List.Clear();
                foreach(DataRow dr in p_Tbl.Rows)
                {
                    V1000_Param_Data param = new V1000_Param_Data();
                    V1000SQLtoParam(dr, ref param);
                    p_List.Add(param);
                }
            }

            return ret_val;
        }

        private bool GetParamGrpList(DataRow p_Row, string p_Col, ref DataTable p_Tbl)
        {
            bool ret_val = false;

            cmbParamGroup.Items.Clear();

            // Get the list of parameter groupings available and fill the Parameter group combobox
            string file = p_Row[p_Col].ToString() + dbFileExt;
            string conn_str = OLEBaseStr + DataDir + file + OLEEndStr;

            if(SQLGetTable(conn_str, ref p_Tbl))
            {
                ret_val = true;
                foreach(DataRow dr in p_Tbl.Rows)
                {
                    string str = dr["PARAM_GRP"].ToString() + " - " + dr["GRP_DESC"].ToString();
                    cmbParamGroup.Items.Add(str);
                }
            }

            return ret_val;
        }

        public bool SQLGetTable(string p_ConnStr, ref DataTable p_Tbl, string p_Query = "SELECT * FROM [SHEET1$]")
        {
            bool RetVal = false;

            using(OleDbConnection dbConn = new OleDbConnection(p_ConnStr))
            {
                if(dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                    if(dbConn.State == ConnectionState.Open)
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(p_Query, dbConn);
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds);
                            p_Tbl.Clear();
                            p_Tbl = ds.Tables[0];
                            if(p_Tbl.Rows.Count > 0)
                                RetVal = true;
                            else
                                RetVal = false;
                        }
                        catch
                        {
                            MessageBox.Show("Database Error!");
                            RetVal = false;
                        }

                        dbConn.Close();

                    }
                    else
                        RetVal = false;
                }
            }
            return RetVal;
        }

        public int dB_Query(string p_dB, ref DataTable p_Tbl, string p_Cols, string p_ID, string p_Cond = "%")
        {
            int row_cnt;
            StringBuilder query = new StringBuilder("SELECT " + p_Cols + " FROM [Sheet1$] ");

            if(p_ID != "")
                query.Append("WHERE " + p_ID + " LIKE '" + p_Cond + "'");
            
            row_cnt = dB_Query(p_dB, query.ToString(), ref p_Tbl);

            return row_cnt;
        }

        public int dB_Query(string p_dB, string p_SQL, ref DataTable p_FillTbl)
        {
            int row_cnt = 0;

            string conn_str = OLEBaseStr + DataDir + p_dB + dbFileExt + OLEEndStr;
            using(OleDbConnection db_conn = new OleDbConnection(conn_str))
            {
                if(db_conn.State == ConnectionState.Closed)
                {
                    db_conn.Open();
                    if(db_conn.State == ConnectionState.Open)
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(p_SQL, db_conn);
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds);
                            p_FillTbl.Clear();
                            p_FillTbl = ds.Tables[0];
                            row_cnt = p_FillTbl.Rows.Count;
                        }
                        catch
                        {
                            MessageBox.Show("Database Error!!");
                        }

                        db_conn.Close();
                    }
                }
            }

            return row_cnt;
        }

        public bool dB_Update_Mtr(string p_MtrNum, string p_Col, string p_Val)
        {
            bool ret_val = false;

            string sql = "UPDATE [Sheet1$] SET " + p_Col + " = '" + p_Val + "' WHERE MOTOR_PARTNUM = " + p_MtrNum;
            ret_val = dB_Update(dBMotor, sql);
            return ret_val;
        }

        public bool dB_Update(string p_dB, string p_ID, string p_IDVal, string p_Col, string p_Val)
        {
            bool ret_val = false;

            string sql = "UPDATE [Sheet1$] SET " + p_Col + " = '" + p_Val + "' WHERE " + p_ID + " = '" + p_IDVal + "'";
            ret_val = dB_Update(p_dB, sql);
            return ret_val;
        }

        public bool dB_Update(string p_dB, string p_SQL)
        {
            bool ret_val = false;

            string conn_str = OLEBaseStr + DataDir + p_dB + OLEEndStr;

            using(OleDbConnection db_conn = new OleDbConnection(conn_str))
            {
                if(db_conn.State == ConnectionState.Closed)
                {
                    db_conn.Open();
                    if(db_conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            OleDbCommand cmd = new OleDbCommand(p_SQL, db_conn);
                            if(cmd.ExecuteNonQuery() > 0)
                                ret_val = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Database Error!!\n" + ex.Message);
                        }
                        db_conn.Close();
                    }
                }
            }

            return ret_val;
        }

        public void V1000SQLtoParam(DataRow p_dr, ref V1000_Param_Data p_Data)
        {
            p_Data.RegAddress = Convert.ToUInt16(p_dr[1].ToString());
            p_Data.ParamNum = p_dr[2].ToString();
            p_Data.ParamName = p_dr[3].ToString();
            p_Data.Multiplier = Convert.ToUInt16(p_dr[5].ToString());
            p_Data.NumBase = Convert.ToByte(p_dr[6].ToString());
            p_Data.Units = p_dr[7].ToString();
            p_Data.DefVal = Convert.ToUInt16(p_dr[4].ToString());
        }

        public void GetMotorCurrent()
        {
            txtMtrFLC.Text = "";

            // First combine all strings to create the appropriate column header for the motor current data 
            string str_volt = cmbMtrVoltMax.SelectedItem.ToString();
            string str_freq = cmbMtrFreqBase.SelectedItem.ToString();
            string hdr = "FLC_" + str_volt.Substring(0, str_volt.IndexOf(' ')) + "_" + str_freq.Substring(0, str_freq.IndexOf(' '));

            if((hdr != "FLC_400_60") && (hdr != "FLC_415_60") && (hdr != "FLC_460_50"))
            {
                string mtr = cmbMtrPartNum.SelectedItem.ToString();
                DataTable tbl = new DataTable();
                string query = "SELECT " + hdr + " FROM [Sheet1$] WHERE [MOTOR_PARTNUM] LIKE '" + mtr + "'";
                if(dB_Query(dBMotor, query, ref tbl) > 0)
                {
                    foreach(DataRow dr in tbl.Rows)
                        txtMtrFLC.Text = dr[0].ToString();
                }
            }

        }

    }
}
