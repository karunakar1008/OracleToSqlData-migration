using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CFWDatamigrationApp
{
    public partial class Form1 : Form
    {
      
        DataTable dtSourceColumns = new DataTable();
        DataTable dtDestColumns = new DataTable();
        DataSet sourceData = new DataSet();

        TableColumn[] sourceSolumns;
        TableColumn[] destSolumns;
        string destinationTable;
        SqlOperations sqlOperations;
        OracleOperations oracleOperations;
        public Form1()
        {
            oracleOperations = new OracleOperations();
            sqlOperations = new SqlOperations();
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            dtSourceColumns.Columns.AddRange(
                                                new DataColumn[4] {
                                                    new DataColumn("Source Table Name",typeof(string)),
                                                new DataColumn("Source Column Name", typeof(string)),
                                                 new DataColumn("Dest Table Name", typeof(string)),
                                                  new DataColumn("Dest Column Name", typeof(string))
                                                });
            dtDestColumns.Columns.AddRange(
                                            new DataColumn[2] { new DataColumn("Table Name",typeof(string)),
                                            new DataColumn("Column Name", typeof(string))});

            dataGridSourceColumns.DataSource = dtSourceColumns;
            dataGridDestinationColumns.DataSource = dtDestColumns;

            foreach (string table in oracleOperations.GetAllTables())
            {
                comboSourcetable.Items.Add(table);
            }
            foreach (var table in sqlOperations.GetAllTables())
            {
                comboDestTables.Items.Add(table);
            }
        }

        private void comboSourcetable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataTable dt = new DataTable();
            string selectedTable = (string)comboBox.SelectedItem;
            var names = oracleOperations.GetAllColumnsByTable(selectedTable);
            sourceSolumns = names;
            comboSourceColumns.Items.Clear();

            foreach (var item in names)
            {
                comboSourceColumns.Items.Add(item.ColumnName);
            }

            foreach (var item in names)
            {
                dt.Columns.Add(new DataColumn(item.ColumnName, item.GetType()));
            }

            comboDestcolumns.Items.Clear();
            comboDestcolumns.Items.AddRange(names);
            dtSourceColumns.Rows.Clear();
        }

        private void comboDestTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedTable = (string)comboBox.SelectedItem;
            destSolumns = sqlOperations.GetAllColumnsByTable(selectedTable); ;
            comboDestcolumns.Items.Clear();
            foreach (var item in destSolumns)
            {
                comboDestcolumns.Items.Add(item.ColumnName);
            }
            DataTable dt = new DataTable();
            foreach (var item in destSolumns)
            {
                dt.Columns.Add(new DataColumn(item.ColumnName, item.GetType()));
            }
        }

        private void btmAddSourceColumn_Click(object sender, EventArgs e)
        {
            if (comboSourcetable.SelectedIndex == -1 || comboSourceColumns.SelectedIndex == -1 || comboDestTables.SelectedIndex == -1 || comboDestcolumns.SelectedIndex == -1)
            {
                MessageBox.Show("All fileds are mandatory!");
                return;
            }
            var sourceTableName = comboSourcetable.SelectedItem;
            var sourceColumnName = comboSourceColumns.SelectedItem;

            var destTableName = comboDestTables.SelectedItem;
            var destColumnName = comboDestcolumns.SelectedItem;

            dtSourceColumns.Rows.Add(
              sourceTableName,
               sourceColumnName,
               destTableName,
               destColumnName
               );

            dtDestColumns.Rows.Add(
              destTableName,
               destColumnName
               );

            comboSourceColumns.Items.Remove(sourceColumnName);
            comboSourceColumns.SelectedText = "";
            comboSourceColumns.SelectedIndex = -1;


            comboDestcolumns.Items.Remove(destColumnName);
            comboDestcolumns.SelectedText = "";
            comboDestcolumns.SelectedIndex = -1;
        }

        private void btnAddDestColumn_Click(object sender, EventArgs e)
        {
            if (comboDestTables.SelectedIndex == -1 || comboDestcolumns.SelectedIndex == -1)
            {
                MessageBox.Show("Both fileds are mandatory!");
                return;
            }
            var destTableName = comboDestTables.SelectedItem;
            var destColumnName = comboDestcolumns.SelectedItem;

            dtDestColumns.Rows.Add(
              destTableName,
               destColumnName
               );
            comboDestcolumns.Items.Remove(destColumnName);
            comboDestcolumns.SelectedText = "";
            comboDestcolumns.SelectedIndex = -1;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (dtSourceColumns.Rows.Count == 0)
            {
                MessageBox.Show("Atleast one column should be there in select statement!");
                return;
            }
            StringBuilder query = new StringBuilder("select ");
            string tableNmae = "";
            foreach (DataRow row in dtSourceColumns.Rows)
            {
                tableNmae = row.ItemArray[0].ToString();
                dt.Columns.Add(new DataColumn(row.ItemArray[3].ToString(), sourceSolumns.FirstOrDefault(c => c.ColumnName == row.ItemArray[1].ToString()).columnType.FullName.GetType()));
                query.Append(row.ItemArray[0] + "." + row.ItemArray[1] + " as " + row.ItemArray[3].ToString() + ",");
            }

            query.Remove(query.Length - 1, 1);
            query.Append(" FROM " + tableNmae);

            try
            {
                sourceData = oracleOperations.GetSourceData(query.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

            dataGridView1.DataSource = sourceData.Tables[0];
        }

        private void btnInsertData_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow row in dtSourceColumns.Rows)
                {
                    destinationTable = row.ItemArray[2].ToString();
                }
                sqlOperations.InsertDataToDestination(sourceData, destinationTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }
    }

    public class TableColumn
    {
        public string ColumnName { get; set; }

        public Type columnType { get; set; }
    }

    public class OracleOperations
    {
        string sourceConnetionString = null;
        DataSet sourceData = new DataSet();

        public OracleOperations()
        {
            this.sourceConnetionString = "Provider=OraOLEDB.Oracle;dbq=vc;Database=testconnection;User Id=system;Password=123;";
        }

        Dictionary<string, string> dictDTmaapings = new Dictionary<string, string>()
        {
                {"CHAR","System.String"},
                {"NCHAR","System.String"},
                {"VARCHAR2","System.String"},
                {"NVARCHAR2","System.String"},
                {"DATE","System.DateTime"},
                {"NUMBER","System.Decimal"},
                {"FLOAT","System.Decimal"},
                {"INTEGER","System.Decimal"},
                {"UNSIGNED INTEGER","System.Decimal"},
                {"LONG","System.String"},
        };

        public string[] GetAllTables()
        {
            List<string> tables = new List<string>();
            try
            {
                using (var connection = new OleDbConnection(sourceConnetionString))
                {
                    connection.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT TABLE_NAME, OWNER FROM all_tables ORDER BY TABLE_NAME";
                    //cmd.CommandText = "SELECT TABLE_NAME, OWNER FROM all_tables where TABLE_NAME='EMPLOYEETEST' ORDER BY TABLE_NAME";

                    OleDbDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tables.Add(dr[0].ToString());
                            //GetAllColumnsByTable(dr[0].ToString());
                        }
                    }
                    else
                    {
                        Console.Write("No Data In DataBase");
                    }
                    connection.Close();
                    return tables.ToArray<string>();
                }
            }
            catch (Exception ex)
            {
                //  return ex.Message.ToString();
                return tables.ToArray<string>();
            }

        }

        public TableColumn[] GetAllColumnsByTable(string tableName)
        {
            List<TableColumn> tableColumns = new List<TableColumn>();
            try
            {
                using (var connection = new OleDbConnection(sourceConnetionString))
                {
                    connection.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select COLUMN_NAME,DATA_TYPE from all_tab_columns where table_name = \'" + tableName + "\'";
                    OleDbDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tableColumns.Add(new TableColumn { ColumnName = dr[0].ToString(), columnType = Type.GetType(new OracleOperations().OracleCsharpTypeMapping(dr[1].ToString())) });
                        }
                    }
                    else
                    {
                        Console.Write("No Data In DataBase");
                    }
                    connection.Close();
                    return tableColumns.ToArray<TableColumn>();
                }
            }
            catch (Exception ex)
            {
                //  return ex.Message.ToString();
                return tableColumns.ToArray<TableColumn>();
            }
        }

        public string OracleCsharpTypeMapping(string oracleDataType)
        {
            return dictDTmaapings[oracleDataType];
        }

        public DataSet GetSourceData(string query)
        {
            using (var connection = new OleDbConnection(sourceConnetionString))
            {
                connection.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmd;
                sourceData.Clear();
                adapter.Fill(sourceData);
                connection.Close();
                return sourceData;
            }


        }
    }

    public class SqlOperations
    {
        string destConnetionString = null;
        DataSet sourceData = new DataSet();
        Dictionary<string, string> dicSqlToCsharpTypes = new Dictionary<string, string>()
        {

                {"char","System.String"},
                {"varchar","System.String"},
                {"nchar","System.String"},
                 {"nvarchar","System.String"},
                {"date","System.DateTime"},
                {"int","System.Int32"},
                {"decimal","System.Decimal"}
        };

        public SqlOperations()
        {
            this.destConnetionString = "Data Source=E3480-KBHOGYARI\\SQL2014;Initial Catalog=mydb;User ID = sa; Password = admin@123;";
        }

        public string[] GetAllTables()
        {
            List<string> tables = new List<string>();
            try
            {
                var connection = new SqlConnection(destConnetionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM sys.Tables order by name", connection);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        tables.Add(dr[0].ToString());
                    }
                }
                else
                {
                    Console.Write("No Data In DataBase");
                }
                connection.Close();
                return tables.ToArray<string>();
            }
            catch (Exception ex)
            {
                return tables.ToArray<string>();
            }

        }

        public TableColumn[] GetAllColumnsByTable(string tableName)
        {
            List<TableColumn> tableColumns = new List<TableColumn>();
            try
            {
                //using (var connection = new sql(destConnetionString))
                //{
                var connection = new SqlConnection(destConnetionString);
                connection.Open();
                string q = "select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = \'" + tableName + "\'";
                SqlCommand cmd = new SqlCommand(q, connection);
                SqlDataReader dr = cmd.ExecuteReader();


                //connection.Open();
                //OleDbCommand cmd = new OleDbCommand();
                //cmd.Connection = connection;
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = \'" + tableName + "\'";
                //OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        tableColumns.Add(new TableColumn { ColumnName = dr[0].ToString(), columnType = Type.GetType(this.OracleCsharpTypeMapping(dr[1].ToString())) });
                    }
                }
                else
                {
                    Console.Write("No Data In DataBase");
                }
                connection.Close();
                return tableColumns.ToArray<TableColumn>();
                //}
            }
            catch (Exception ex)
            {
                return tableColumns.ToArray<TableColumn>();
            }
        }

        public string OracleCsharpTypeMapping(string oracleDataType)
        {
            return dicSqlToCsharpTypes[oracleDataType];
        }

        public bool InsertDataToDestination(DataSet ds,string destinationTable)
        {
            using (SqlConnection con = new SqlConnection(destConnetionString))
            {
                con.Open();

                using (SqlBulkCopy sqlbc = new SqlBulkCopy(con))
                {
                    DataTable newProducts = ds.Tables[0];
                    sqlbc.DestinationTableName = destinationTable;
                    for (int i = 0; i < newProducts.Columns.Count; i++)
                    {
                        sqlbc.ColumnMappings.Add(i, i);
                    }

                    try
                    {
                        sqlbc.WriteToServer(newProducts);
                        MessageBox.Show("Bulk data stored successfully");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bulk data storimg failed!");
                        return false;
                    }

                }
              
            }
        }
    }
}
