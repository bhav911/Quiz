﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizComputation_490_Model.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QuizComputation_490Entities : DbContext
    {
        public QuizComputation_490Entities()
            : base("name=QuizComputation_490Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Options> Options { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Quizzes> Quizzes { get; set; }
        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<UserAnswers> UserAnswers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}