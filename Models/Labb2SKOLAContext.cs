using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Labb3DB.Models;

public partial class Labb2SkolaContext : DbContext
{
    public Labb2SkolaContext()
    {
    }

    public Labb2SkolaContext(DbContextOptions<Labb2SkolaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassStudent> ClassStudents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeProfession> EmployeeProfessions { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = LAPTOP-VLADGQVE;Initial Catalog=Labb2SKOLA;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK_Klasser");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(20);
        });

        modelBuilder.Entity<ClassStudent>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FkclassId).HasColumnName("FKClassID");
            entity.Property(e => e.FkstudentId).HasColumnName("FKStudentID");

            entity.HasOne(d => d.Fkclass).WithMany()
                .HasForeignKey(d => d.FkclassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassStudents_Classes");

            entity.HasOne(d => d.Fkstudent).WithMany()
                .HasForeignKey(d => d.FkstudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassStudents_Students");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeStartDate).HasColumnType("date");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("money");
        });

        modelBuilder.Entity<EmployeeProfession>(entity =>
        {
            entity.HasKey(e => new { e.FkemployeeId, e.FkprofessionTitleId });
            entity.ToTable("EmployeeProfessions");

            entity.Property(e => e.FkemployeeId).HasColumnName("FKEmployeeID");
            entity.Property(e => e.FkprofessionTitleId).HasColumnName("FKProfessionTitleID");

            entity.HasOne(d => d.Fkemployee).WithMany()
                .HasForeignKey(d => d.FkemployeeId)
                .HasConstraintName("FK_EmployeeProfessions_Employees");

            entity.HasOne(d => d.FkprofessionTitle).WithMany()
                .HasForeignKey(d => d.FkprofessionTitleId)
                .HasConstraintName("FK_EmployeeProfessions_Professions");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.FkemployeeId).HasColumnName("FKEmployeeID");
            entity.Property(e => e.FkstudentId).HasColumnName("FKStudentID");
            entity.Property(e => e.FksubjectId).HasColumnName("FKSubjectID");
            entity.Property(e => e.Grade)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Fkemployee).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.FkemployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Employees");

            entity.HasOne(d => d.Fkstudent).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.FkstudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Students");

            entity.HasOne(d => d.Fksubject).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.FksubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Subjects");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionTitleId).HasName("PK_Roller");

            entity.Property(e => e.ProfessionTitleId).HasColumnName("ProfessionTitleID");
            entity.Property(e => e.ProfessionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_Elever");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.FkemployeeId).HasColumnName("FKEmployeeID");
            entity.Property(e => e.SubjectName).HasMaxLength(30);

            entity.HasOne(d => d.Fkemployee).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.FkemployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subjects_Employees");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
