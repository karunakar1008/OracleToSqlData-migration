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
        string connetionString = null;
        SqlConnection cnn;

        DataTable dtSourceColumns = new DataTable();
        DataTable dtDestColumns = new DataTable();
        DataSet sourceData = new DataSet();

        TableColumn[] sourceSolumns;
        TableColumn[] destSolumns;
        string destinationTable;

        public Form1()
        {
            connetionString = "Data Source=E3480-KBHOGYARI\\SQL2014;Initial Catalog=mydb;User ID=sa;Password=admin@123";
            InitializeComponent();
        }

        public void InsertStudent(string tableName, params object[] columnValues)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {

                DataTable dt = new DataTable();

                if (dtDestColumns.Rows.Count == 0)
                {
                    MessageBox.Show("Atleast one column should be there in select statement!");
                    return;
                }
                StringBuilder query = new StringBuilder("Insert into ");
                StringBuilder query2 = new StringBuilder("");

                string tableNmae = "";
                foreach (DataRow row in dtSourceColumns.Rows)
                {
                    tableNmae = row.ItemArray[2].ToString();
                    destinationTable = row.ItemArray[2].ToString();
                    //  dt.Columns.Add(new DataColumn(row.ItemArray[1].ToString(), sourceSolumns.FirstOrDefault(c => c.ColumnName == row.ItemArray[1].ToString()).GetType()));
                    query2.Append(row.ItemArray[0] + "." + row.ItemArray[1] + ",");
                }
                query2.Remove(query.Length - 1, 1);
                query.Append(tableNmae);
                query.Append(query2.ToString());

                cnn = new SqlConnection(connetionString);
                DataSet myDs = new DataSet();
                try
                {
                    cnn.Open();
                    SqlCommand myCmd = new SqlCommand(query.ToString(), cnn);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = myCmd;
                    adapter.Fill(myDs);
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can not open connection ! ");
                }


                con.Open();
                string insertQuery = string.Empty;

                //string query = "Insert into tblStudent (student_name, student_age,student_gender) values(@studname, @studage , @gender)";
                //SqlCommand cmd = new SqlCommand(query, con);
                //cmd.Parameters.AddWithValue("@studname", strStudentName);
                //cmd.Parameters.AddWithValue("@studage", intAge);
                //cmd.Parameters.AddWithValue("@gender", strGender);
                //return cmd.ExecuteNonQuery();

                using (SqlBulkCopy sqlbc = new SqlBulkCopy(con))
                {
                    DataTable newProducts = sourceData.Tables[0];
                    sqlbc.DestinationTableName = destinationTable;

                    try
                    {
                        sqlbc.WriteToServer(newProducts);
                        MessageBox.Show("Bulk data stored successfully");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Bulk data storimg failed!");
                    }
                   
                }

            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Dictionary<string, Type> types = new Dictionary<string, Type>();
            //types.Add("int", typeof(System.Int32));
            //types.Add("decimal", typeof(System.Decimal));
            //types.Add("bool", typeof(System.Boolean));
            //combodataTypes.Items.AddRange(types.Keys.ToArray());

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

            using (var dbContext = new mydbEntities())
            {
                var metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;

                var tables = metadata.GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .Single()
                    .BaseEntitySets
                    .OfType<EntitySet>()
                    .Where(s => !s.MetadataProperties.Contains("Type")
                    || s.MetadataProperties["Type"].ToString() == "Tables");

                foreach (var table in tables)
                {
                    var tableName = table.MetadataProperties.Contains("Table")
                        && table.MetadataProperties["Table"].Value != null
                        ? table.MetadataProperties["Table"].Value.ToString()
                        : table.Name;

                    comboDestTables.Items.Add(tableName);
                    comboSourcetable.Items.Add(tableName);
                    var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                    Console.WriteLine(tableSchema + "." + tableName);
                }
            }
        }


        private void comboSourcetable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataTable dt = new DataTable();
            string selectedTable = (string)comboBox.SelectedItem;
            var currentEntity = string.Format("{0}.{1},{2}", "CFWDatamigrationApp", selectedTable, "CFWDatamigrationApp");
            Type type = Type.GetType(currentEntity);

            var names = type.GetProperties()
                        .Select(property => new TableColumn() { ColumnName = property.Name, columnType = property.PropertyType })
                        .ToArray();
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
            using (var dbContext = new mydbEntities())
            {
                switch (selectedTable.ToLower())
                {
                    case "claim":

                        foreach (var item in dbContext.Claims.ToList())
                        {
                            dt.Rows.Add();
                        }
                        break;
                    case "claimdetail":
                        foreach (var item in dbContext.VolvoPrograms.ToList())
                        {
                            dt.Rows.Add(item.VolvoProgramId, item.Programname, item.EnableDate);
                        }
                        break;
                }

                dataGridView1.DataSource = dt;
            }

            comboDestcolumns.Items.Clear();
            comboDestcolumns.Items.AddRange(names);

            dtSourceColumns.Rows.Clear();
        }

        private void comboDestTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            string selectedTable = (string)comboBox.SelectedItem;
            var currentEntity = string.Format("{0}.{1},{2}", "CFWDatamigrationApp", selectedTable, "CFWDatamigrationApp");
            Type type = Type.GetType(currentEntity);
            var names = type.GetProperties()
                        .Select(property => new TableColumn() { ColumnName = property.Name, columnType = property.PropertyType })
                        .ToArray();
            destSolumns = names;
            comboDestcolumns.Items.Clear();
            foreach (var item in names)
            {
                comboDestcolumns.Items.Add(item.ColumnName);
            }
            DataTable dt = new DataTable();
            foreach (var item in names)
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

        private DataTable MakeTable()
        {
            DataTable newProducts = sourceData.Tables[0];


            // Return the new DataTable. 
            return newProducts;
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
                dt.Columns.Add(new DataColumn(row.ItemArray[3].ToString(), sourceSolumns.FirstOrDefault(c => c.ColumnName == row.ItemArray[1].ToString()).GetType()));
                query.Append(row.ItemArray[0] + "." + row.ItemArray[1] + " as " + row.ItemArray[3].ToString() + ",");
            }

            query.Remove(query.Length - 1, 1);
            query.Append(" FROM " + tableNmae);

            cnn = new SqlConnection(connetionString);

            try
            {
                cnn.Open();
                SqlCommand myCmd = new SqlCommand(query.ToString(), cnn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = myCmd;
                sourceData.Clear();
                adapter.Fill(sourceData);
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

            dataGridView1.DataSource = sourceData.Tables[0];


            //string con = "Provider=OraOLEDB.Oracle;dbq=vc;Database=testconnection;User Id=system;Password=123;";
            //try
            //{
            //    using (var connection = new OleDbConnection(con))
            //    {
            //        connection.Open();
            //        OleDbCommand cmd = new OleDbCommand();
            //        cmd.Connection = connection;

            //        cmd.CommandType = CommandType.Text;
            //        cmd.CommandText = "select * from EMPloyeetest ";
            //        OleDbDataReader dr = cmd.ExecuteReader();

            //        if (dr.HasRows)
            //        {

            //            while (dr.Read())
            //            {
            //                Console.WriteLine("\n" + dr["table_name"].ToString());
            //            }
            //            Console.Write("</table>");
            //        }
            //        else
            //        {
            //            Console.Write("No Data In DataBase");
            //        }
            //        connection.Close();

            //        //return "connected";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //  return ex.Message.ToString();

            //}
            //finally
            //{
            //    // Console.ReadKey();
            //}

        }

        private void btnInsertData_Click(object sender, EventArgs e)
        {
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
               // MessageBox.Show("Connection Open ! ");
                foreach (DataRow row in dtSourceColumns.Rows)
                {
                    destinationTable = row.ItemArray[2].ToString();
                }

                using (SqlConnection con = new SqlConnection(connetionString))
                {
                    con.Open();

                    using (SqlBulkCopy sqlbc = new SqlBulkCopy(con))
                    {
                        DataTable newProducts = sourceData.Tables[0];
                        sqlbc.DestinationTableName = destinationTable;
                        foreach (DataColumn item in sourceData.Tables[0].Columns)
                        {
                            sqlbc.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                        }

                        try
                        {
                            sqlbc.WriteToServer(newProducts);
                            MessageBox.Show("Bulk data stored successfully");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Bulk data storimg failed!");
                        }

                    }
                    cnn.Close();
                }
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
}
