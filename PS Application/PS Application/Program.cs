using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

SQLiteConnection sqlite_conn;
sqlite_conn = CreateConnection();
CreateTable(sqlite_conn);
InsertData(sqlite_conn);
ReadData(sqlite_conn);