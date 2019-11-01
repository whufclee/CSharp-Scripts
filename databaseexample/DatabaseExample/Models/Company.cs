using System;
using SQLite;
namespace DatabaseExample.Models
{
    public class Company
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return this.Name + " (" + this.Address + ")";
        }
    }
}
