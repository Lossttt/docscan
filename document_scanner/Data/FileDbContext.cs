using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using document_scanner.Models;
using Microsoft.EntityFrameworkCore;

namespace document_scanner.Data
{
    public class FileDbContext : DbContext
    {
        // The constructor of the FileDbContext class accepts an instance of the DbContextOptions class as a parameter.
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // The HasKey method is used to specify the primary key for the Document entity.
            // The primary key is a unique identifier for each row in the table.
            // In this case, the primary key is the Id property of the Document entity.
            modelBuilder.Entity<Document>().HasKey(k => k.Id);
        }

        // The DbSet<Document> property is a collection of Document objects 
        // that will be used to interact with the table in the database.
        public DbSet<Document> Documents { get; set; }

    }
}