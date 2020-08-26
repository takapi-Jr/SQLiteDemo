using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteDemo.Models
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
