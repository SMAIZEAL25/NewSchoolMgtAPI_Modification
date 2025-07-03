using New_School_Management_API.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_School_Management_API.Entities.Student_Transaction
{
    public class TransactionDetails
    {
        [Key]
        public int TransactionId { get; set; } // Primary Key

        public int StudentId { get; set; } // Foreign Key

        public string FeeType { get; set; }

        [Column(TypeName = "decimal(18, 2)")] // Precision: 18, Scale: 2
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18, 2)")] // Precision: 18, Scale: 2
        public decimal VAT { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsSuccessful { get; set; }

        public string TransactionReference { get; set; }

        [ForeignKey(nameof(StudentId))]
        public StudentRecord Student { get; set; } // Navigation Property to the StudentRecord class
    }
}