﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PostgreConsoleInteractorCS
{
    public class DatabaseModel
    {
        NpgsqlConnection connection;
        public DatabaseModel(string connection_string)
        {
            connection = new NpgsqlConnection(connection_string);
            connection.Open();
        }

        public void Insert()
        {
            List<string> tables = GetTables("insert");
            int selected_index = int.Parse(Console.ReadLine()) - 1;
            List<string> columns = GetColumns(tables[selected_index]);
            List<string> values = new List<string>();
            foreach (string column in columns)
            {
                Console.Write("Enter value for column - " + column + " - ");
                values.Add(Console.ReadLine());
            }

            string insert_query = "INSERT INTO " + tables[selected_index] + " (" + ListToString(columns, false) + ") VALUES (" + ListToString(values, true) + ")";
            NpgsqlCommand insert_command = new NpgsqlCommand(insert_query, connection);
            Console.WriteLine(insert_query);
            insert_command.ExecuteNonQuery();
        }

        public void Delete()
        {
            List<string> tables = GetTables("delete");
            int selected_index = int.Parse(Console.ReadLine()) - 1;
            Console.WriteLine("Input row id for delete");
            int row_id = int.Parse(Console.ReadLine());
            NpgsqlCommand delete_command = new NpgsqlCommand("DELETE FROM " + tables[selected_index] + " WHERE " + tables[selected_index] + "_id = " + row_id, connection);
            delete_command.ExecuteNonQuery();
        }

        public void Update()
        {
            List<string> tables = GetTables("update");
            int selected_index = int.Parse(Console.ReadLine()) - 1;
            List<string> columns = GetColumns(tables[selected_index]);
            List<string> values = new List<string>();
            Console.WriteLine("Input row id for update");
            int row_id = int.Parse(Console.ReadLine());
            foreach (string column in columns)
            {
                Console.Write("Enter new value for column - " + column + " - ");
                values.Add(Console.ReadLine());
            }
            string update_query = "UPDATE " + tables[selected_index] + " SET " + UpdatePartialString(columns, values) + " WHERE " + tables[selected_index] + "_id = " + row_id;
            NpgsqlCommand update_command = new NpgsqlCommand(update_query, connection);
            update_command.ExecuteNonQuery();
            Console.WriteLine(update_query);
        }

        public void Generate()
        {
            List<string> tables = GetTables("generate");
            int selected_index = int.Parse(Console.ReadLine()) - 1;
            Console.WriteLine("Input rows count for generate");
            int rows_count = int.Parse(Console.ReadLine());
            List<string> columns = GetColumns(tables[selected_index]);
            Random random = new Random();
            for (int i = 0; i < rows_count; i++)
            {
                while (true)
                {
                    try
                    {
                        List<string> values = new List<string>();
                        foreach (string column in columns)
                        {
                            values.Add(random.Next(0, 150).ToString());
                        }
                        //values[values.Count - 1] = "1";
                        string insert_query = "INSERT INTO " + tables[selected_index] + " (" + ListToString(columns, false) + ") VALUES (" + ListToString(values, true) + ")";
                        NpgsqlCommand insert_command = new NpgsqlCommand(insert_query, connection);
                        insert_command.ExecuteNonQuery();
                        break;
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void Search()
        {
            Console.WriteLine("Choose what to search:");
            Console.WriteLine("1 - select name by company_id");
            Console.WriteLine("2 - select items by company_id");
            Console.WriteLine("3 - select items by category_id");
            Console.Write("Your choice - ");
            int search = int.Parse(Console.ReadLine());
            switch (search)
            {
                case 1:
                    Console.Write("Input company_id - ");
                    string trademark_id = Console.ReadLine();
                    NpgsqlCommand command1 = new NpgsqlCommand("SELECT name FROM company WHERE company_id = " + trademark_id, connection);
                    NpgsqlDataReader reader1 = command1.ExecuteReader();
                    List<string> data1 = new List<string>();
                    while (reader1.Read())
                    {
                        data1.Add(reader1.GetValue(0).ToString());
                    }
                    PrintRow(data1);
                    reader1.Close();
                    break;
                case 2:
                    Console.Write("Input company_id - ");
                    string category_id = Console.ReadLine();
                    NpgsqlCommand command2 = new NpgsqlCommand("SELECT title FROM items WHERE company_id = " + category_id, connection);
                    NpgsqlDataReader reader2 = command2.ExecuteReader();
                    List<string> data2 = new List<string>();
                    while (reader2.Read())
                    {
                        data2.Add(reader2.GetValue(0).ToString());
                    }
                    PrintRow(data2);
                    reader2.Close();
                    break;
                case 3:
                    Console.Write("Input category_id - ");
                    string category_id3 = Console.ReadLine();
                    NpgsqlCommand command3 = new NpgsqlCommand("SELECT title FROM items WHERE category_id = " + category_id3, connection);
                    NpgsqlDataReader reader3 = command3.ExecuteReader();
                    List<string> data3 = new List<string>();
                    while (reader3.Read())
                    {
                        data3.Add(reader3.GetValue(0).ToString());
                    }
                    PrintRow(data3);
                    reader3.Close();
                    break;
            }
        }

        public void Print()
        {
            List<string> tables = GetTables("print");
            int selected_index = int.Parse(Console.ReadLine()) - 1;
            List<string> columns = GetColumns(tables[selected_index]);
            PrintRow(columns);
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM " + tables[selected_index], connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<string> data = new List<string>();
            while (reader.Read())
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    data.Add(reader.GetValue(i).ToString());
                }
                PrintRow(data);
                data.Clear();
            }
            reader.Close();
        }

        private List<string> GetTables(string operation)
        {
            Console.WriteLine("Choose table to " + operation + " data");
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM information_schema.tables WHERE table_schema = 'public';", connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<string> tables = new List<string>();
            while (reader.Read())
            {
                tables.Add(reader.GetString(2));
            }
            reader.Close();
            int index = 1;
            foreach (string table in tables)
            {
                Console.WriteLine(index.ToString() + '.' + table);
                index++;
            }
            return tables;
        }

        private List<string> GetColumns(string table)
        {
            string query = "SELECT * FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '" + table + "'";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<string> columns = new List<string>();
            while (reader.Read())
            {
                columns.Add(reader.GetString(3));
            }
            reader.Close();
            return columns;
        }

        private string ListToString(List<string> list, bool is_value)
        {
            string result = string.Empty;
            for (int i = 1; i < list.Count; i++)
            {
                if (is_value) result += "'" + list[i] + "',";
                else result += list[i] + ',';
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        private void PrintRow(List<string> list)
        {
            foreach (string s in list)
            {
                Console.Write(s + "      ");
            }
            Console.Write('\n');
        }

        private string UpdatePartialString(List<string> columns, List<string> values)
        {
            string partial = string.Empty;
            for (int i = 1; i < columns.Count; i++)
            {
                partial += columns[i] + " = '" + values[i] + "'";
                if (i < columns.Count - 1) partial += ", ";
            }
            return partial;
        }
    }
}
